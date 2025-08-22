using System.Text.Json.Serialization;

namespace MyApi5.UI.Resources
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<ErrorResponseItem> Errors { get; set; } = new List<ErrorResponseItem>();
    }
    public class ErrorResponseItem
    {
        public string Key { get; set; }
        public string ErrorMessage { get; set; }
    }
}
