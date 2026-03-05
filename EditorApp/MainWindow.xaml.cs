using EditorApp.Models;
using EditorApp.Services;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace EditorApp
{
    
    public partial class MainWindow : Window
    {
        private readonly ConfigService _service;
        private ConfigModel _model;
        public MainWindow()
        {
            InitializeComponent();

            var path = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "config.xml");

            _service = new ConfigService(path);
            _model = _service.Load();
            DataContext = _model;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _service.Save(_model);
            MessageBox.Show("Saved Successfully");
        }

        private void GeneratePasswords_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccounts = AccountsDataGrid.SelectedItems
                .Cast<Account>()
                .ToList();

            if (!selectedAccounts.Any())
            {
                MessageBox.Show("Select at least one user.");
                return;
            }

            var usernames = string.Join(" ",
                selectedAccounts.Select(a => a.UserName));

            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Git\bin\bash.exe",
                Arguments = $"generate_passwords.sh {usernames}",
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            Process.Start(psi)?.WaitForExit();

            var lines = File.ReadAllLines("new_passwords.txt");

            foreach (var line in lines)
            {
                var parts = line.Split(':');
                var user = parts[0];
                var pass = parts[1];

                var account = _model.Accounts
                    .First(a => a.UserName == user);

                account.Password = pass;
                account.PasswordType = "plaintext";
            }

            File.Delete("new_passwords.txt");

            AccountsDataGrid.Items.Refresh();

            MessageBox.Show("Passwords updated.");
        }
    }
}

