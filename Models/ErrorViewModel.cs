namespace SAMS.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } = null!;

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Message { get; set; } = null!;
    }
}