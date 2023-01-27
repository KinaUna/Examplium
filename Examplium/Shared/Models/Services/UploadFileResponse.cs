namespace Examplium.Shared.Models.Services
{
    public class UploadFileResponse
    {
        public bool Success { get; set; }
        public string? FileName { get; set; }
        public string? SavedFileName { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
