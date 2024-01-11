namespace BackupSystem.ApplicationSettings
{
    public class APISettings
    {
        public JwtAuthorizationFields JwtAuthFields { get; set; }

    }

    public class JwtAuthorizationFields
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

    }
}
