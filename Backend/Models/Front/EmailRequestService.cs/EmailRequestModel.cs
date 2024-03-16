
using Backend.Models;

namespace ST_API.Models
{
    public class cEmailRequestResult : ResultAPI
    {
        // public List<EmailRequest> lstEmail { get; set; }
        public string sEmail { get; set; }
    }

    public class EmailRequest
    {
        public string sEmail { get; set; }
        public string sUserID { get; set; }
    }

    public class EmailData
    {
        public int? nConfirmEmailID { get; set; }
        public string? sEmail { get; set; }
        public string? isConfirm { get; set; }
        // public bool? isConfirm { get; set; }
        public bool? isTimeout { get; set; }
        public DateTime? dSendEmail { get; set; }
        public DateTime? dConfirmEmail { get; set; }
        public string sUserID { get; set; }
    }
}
