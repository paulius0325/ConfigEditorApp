using EditorApp.Models;
using System.Xml.Linq;

namespace EditorApp.Services
{
    public class ConfigService
    {
        private readonly string _filePath;

            public ConfigService(string filePath)
            {
                _filePath = filePath;
            }

        public ConfigModel Load()
        {
            var doc = XDocument.Load(_filePath);

            var config = new ConfigModel
            {
                Version = doc.Root?.Element("version")?.Value ?? "",
                UpTime = long.Parse(doc.Root?.Element("uptime")?.Value ?? "0")
            };

            var accounts = doc.Root?
                .Element("person")?
                .Elements("acc")
                .Select(x => new Account
                {
                    Id = int.Parse(x.Attribute("id")?.Value ?? "0"),
                    Enabled = x.Attribute("enabled")?.Value == "1",
                    UserName = x.Element("user_name")?.Value ?? "",
                    Password = x.Element("password")?.Value ?? "",
                    PasswordType = x.Element("password")?.Attribute("type")?.Value ?? ""
                }).ToList() ?? new List<Account>();

            config.Accounts = accounts;

            return config;
        }
        public void Save(ConfigModel model)
        {
            var doc = new XDocument(
                new XElement("config",
                new XElement("version",
                new XAttribute("type", "str"),
                model.Version),
                new XElement("person",
                model.Accounts.Select(a =>
                new XElement("acc",
                new XAttribute("id", a.Id),
                new XAttribute("enabled", a.Enabled ? "1" : "0"),
                new XElement("user_name", a.UserName),
                new XElement("password",
                new XAttribute("type", a.PasswordType),
                a.Password)))),
                new XElement("uptime",
                new XAttribute("type", "int"),
                model.UpTime))
            );

            doc.Save(_filePath);
        }
    }
}
