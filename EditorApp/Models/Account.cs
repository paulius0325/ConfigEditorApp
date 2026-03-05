namespace EditorApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordType { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}
