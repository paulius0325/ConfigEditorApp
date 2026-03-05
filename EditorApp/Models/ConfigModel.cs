namespace EditorApp.Models
{
    public class ConfigModel
    {
        public string Version { get; set; } = string.Empty;
        public long UpTime { get; set; }
        public List<Account> Accounts { get; set; } = new();
    }
}
