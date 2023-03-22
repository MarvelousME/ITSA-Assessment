namespace VetClinic.Api
{
    public class SmtpConfig
    {
        public string Host { get; set; } = String.Empty;
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string EmailAddress { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
