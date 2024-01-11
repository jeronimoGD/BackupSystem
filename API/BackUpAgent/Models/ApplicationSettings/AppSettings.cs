namespace BackUpAgent.Models.ApplicationSettings
{
    public class AppSettings
    {
        public string AgentConnectionKey { get; set; }
        public string AgentManagerApiUrl {  get; set; }
        public string DefaultDateFormat { get; set; }
        public BackUpSettings BackUpSettings { get; set; }
        public LoggingCredentials LoggingCredentials { get; set; }
    }
}
