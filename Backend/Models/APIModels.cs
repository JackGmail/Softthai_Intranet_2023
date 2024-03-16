using Backend.Enum;

namespace Backend.Models
{
    public class ResultAPI
    {
        public int nStatusCode { get; set; } = (int)APIStatusCode.Success;
        public string? sMessage { get; set; }
        public object? objResult { get; set; }
        public int nPermission { get; set; }
        public string? sID { get; set; }
        public string? sRole { get; set; }
    }
}
