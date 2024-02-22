
namespace Project.Core.Common
{
    public class SeqConfig
    {
        public string? ServerUrl { get; set; }
        public string? ApiKey { get; set; }
        public string? MinimumLevel { get; set; }
        public Dictionary<string, string> LevelOverride { get; set; }
    }
}
