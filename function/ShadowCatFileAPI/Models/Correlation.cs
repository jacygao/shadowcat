namespace Models
{
    public record Correlation
    {
        // Container ID
        public string Id { get; set; }

        // Partition Key
        public string Username { get; set; }

        public string RegionCode { get; set; }

        public string Filename { get; set; }

        public string DownloadUrl { get; set; }

        public string Description { get; set; }
    }
}