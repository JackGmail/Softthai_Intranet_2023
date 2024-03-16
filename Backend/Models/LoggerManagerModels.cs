using System.Text.Json;

namespace Backend.Models
{
    public class LoggerManagerModels
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
