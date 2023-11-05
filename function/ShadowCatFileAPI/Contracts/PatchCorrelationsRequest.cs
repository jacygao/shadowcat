namespace ShadowCatFileAPI.Contracts
{
    public record PatchCorrelationsRequest
    {
        public string Region { get; set; }

        public string Description { get; set; }
    }
}
