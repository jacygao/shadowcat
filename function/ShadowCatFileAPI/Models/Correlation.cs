using Newtonsoft.Json;

namespace Models
{
    public class Correlation
    {
        // Container ID
        [JsonProperty("id")]
        public string Id { get; set; }

        // Partition Key
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("regionCode")]
        public string RegionCode { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}