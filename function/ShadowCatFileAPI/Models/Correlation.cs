using System;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public class Correlation
    {
        // Container ID
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        // Partition Key
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "regionCode")]
        public string RegionCode { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "downloadUrl")]
        public string DownloadUrl { get; set; }
    }
}