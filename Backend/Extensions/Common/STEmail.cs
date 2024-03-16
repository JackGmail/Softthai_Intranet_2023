
using System.Net.Mail;
using Backend.EF.ST_Intranet;
using Extensions.Common.STResultAPI;
using HtmlAgilityPack;
using static Backend.Enum.EnumConfig;

namespace Extensions.Common.STEmail
{
    /// <summary>
    /// </summary>
    public class STEmail
    {
        /// <summary>
        /// </summary>
        public static ResultAPI Send(EmailParameter o)
        {
            ResultAPI result = new ResultAPI();
            ST_IntranetEntity _db = new ST_IntranetEntity();
            List<TM_Config> lstConfigAll = _db.TM_Config.ToList();
            bool IsSendMail = false;
            bool IsTestMode = true;
            string sSMTP_Mail = "";
            string sSystemMail = "";
            string sDemoMailSend = "";
            string sMailDisplayName = "";
            TM_Config? objTestMode = lstConfigAll.FirstOrDefault(w => w.nConfigID == (int)EmailConfig.IsMailDemo);
            if (objTestMode != null)
            {
                IsSendMail = o.IsSendtoRealMail ? true : objTestMode.sValue != "3";
                IsTestMode = o.IsSendtoRealMail ? false : objTestMode.sValue == "2";
            }

            TM_Config? objSystemMail = lstConfigAll.FirstOrDefault(w => w.nConfigID == (int)EmailConfig.sSystemMail);
            if (objSystemMail != null)
            {
                sSystemMail = objSystemMail.sValue ?? "";
            }

            TM_Config? objSendMailDemo = lstConfigAll.FirstOrDefault(w => w.nConfigID == (int)EmailConfig.sSendMailDemo);
            if (objSendMailDemo != null)
            {
                sDemoMailSend = objSendMailDemo.sValue ?? "";
            }

            TM_Config? objSMTP = lstConfigAll.FirstOrDefault(w => w.nConfigID == (int)EmailConfig.sSMTPMail);
            if (objSMTP != null)
            {
                sSMTP_Mail = objSMTP.sValue ?? "";
            }

            TM_Config? objDisplayName = lstConfigAll.FirstOrDefault(w => w.nConfigID == (int)EmailConfig.sMailDisplayName);
            if (objDisplayName != null)
            {
                sMailDisplayName = objDisplayName.sValue ?? "";
            }

            string sFrom = !string.IsNullOrEmpty(sSystemMail) ? sSystemMail : "";
            System.Net.Mail.MailMessage oEmail = new System.Net.Mail.MailMessage();
            string sException = "";
            bool isSendCompleted = false;
            List<string> lstEmailTo = new List<string>();
            List<string> lstEmailCC = new List<string>();
            List<string> lstEmailBCC = new List<string>();
            TB_LogEmail oLog = new TB_LogEmail();

            //Test Mode
            List<string> lstEmailTestTo = new List<string>();
            //Final
            string sSubject = "";
            string sContent = "";


            try
            {
                //Test Mode
                if (IsTestMode && !string.IsNullOrEmpty(sDemoMailSend))
                {
                    string[] ArrToTemp = sDemoMailSend.Split(',');
                    foreach (var iE in ArrToTemp)
                    {
                        if (!string.IsNullOrEmpty(iE))
                        {
                            lstEmailTestTo.Add(iE);
                        }
                    }
                }

                if (o.lstTo != null && o.lstTo.Any()) lstEmailTo.AddRange(o.lstTo);
                if (o.lstCC != null && o.lstCC.Any()) lstEmailCC.AddRange(o.lstCC);
                if (o.lstBCC != null && o.lstBCC.Any()) lstEmailBCC.AddRange(o.lstBCC);

                oLog.sTo = string.Join(",", lstEmailTo);
                oLog.sCc = string.Join(",", lstEmailCC);
                oLog.sBcc = string.Join(",", lstEmailBCC);

                if (!string.IsNullOrEmpty(o.sSubject) && !string.IsNullOrEmpty(o.sMessage))
                {
                    sSubject = o.sSubject;
                    sContent = o.sMessage;

                    if (IsTestMode)
                    {
                        sContent += @"<br><br><br>------------------------------- DebugMode --------------------------------";
                        sContent += @"<br>ถึง : " + string.Join(",", lstEmailTo) + "<br>";
                        sContent += @"<br>cc : " + string.Join(",", lstEmailCC) + "<br>";
                        sContent += @"<br>Bcc : " + string.Join(",", lstEmailBCC) + "<br>";
                        lstEmailTo = lstEmailTestTo;
                        lstEmailCC = new List<string>();
                        lstEmailBCC = new List<string>();
                    }

                    if (!string.IsNullOrEmpty(sFrom))
                    {
                        oEmail.From = new System.Net.Mail.MailAddress(sFrom, sMailDisplayName);
                    }

                    // TODO: Replace with recipient e-mail address.
                    if (lstEmailTo.Count > 0)
                    {
                        lstEmailTo.ForEach(f =>
                        {
                            oEmail.To.Add(f);
                        });
                    }
                    else
                    {
                        result.Message = "Email's Receiver is Empty";
                        result.Status = StatusCodes.Status500InternalServerError;
                        return result;
                    }
                    lstEmailCC.ForEach(f =>
                    {
                        oEmail.CC.Add(f);
                    });
                    lstEmailBCC.ForEach(f =>
                    {
                        oEmail.Bcc.Add(f);
                    });
                    oEmail.Subject = sSubject.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace("\t", String.Empty);
                    oEmail.IsBodyHtml = true;

                    try
                    {
                        string sSetFont = @"<style type='text/css'>
                        body{
                                    font-family:Angsana New;
                                    font-size:16pt;
                            }
                        </style>";
                        string sIconLink = "<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css'>";

                        HtmlDocument doc = new HtmlDocument();
                        sContent = "<HTML><head>" + sIconLink + sSetFont + "</head><BODY>" + sContent + "</BODY></HTML>";
                        doc.LoadHtml(sContent);
                        var myResources = new List<System.Net.Mail.LinkedResource>();

                        AlternateView av2 = AlternateView.CreateAlternateViewFromString(sContent, null, System.Net.Mime.MediaTypeNames.Text.Html);
                        foreach (LinkedResource linkedResource in myResources)
                        {
                            av2.LinkedResources.Add(linkedResource);
                        }

                        // ADD AN ATTACHMENT.
                        //TODO: Replace with path to attachment.
                        if (o.lstFile != null)
                        {
                            if (o.lstFile.Any())
                                o.lstFile.Select(s => new Attachment(s)).ToList().ForEach(oEmail.Attachments.Add);
                        }

                        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient()
                        {
                            Host = sSMTP_Mail,
                            // Timeout = 2000,
                            // Port = 25,
                            EnableSsl = false // ตอนแสกนต้องส่งเป็น true
                        };
                        oEmail.AlternateViews.Add(av2);
                        oEmail.Body = sContent;
                        oEmail.BodyEncoding = System.Text.Encoding.UTF8;

                        if (IsSendMail)
                        {
                            smtp.Send(oEmail);
                        }

                        isSendCompleted = true;
                    }
                    catch (Exception ex)
                    {
                        isSendCompleted = false;
                        result.Status = StatusCodes.Status500InternalServerError;
                        result.Message = ex.Message.ToString();
                        sException = ex.Message;
                    }
                }
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message.ToString();
                sException = e.Message.ToString();
                isSendCompleted = false;
            }

            //Save Log
            oLog.sFrom = sFrom;
            oLog.sSubject = sSubject;
            oLog.sMessage = sContent;
            oLog.IsSuccess = isSendCompleted;
            oLog.dSend = DateTime.Now;
            oLog.IsMailTest = IsTestMode;
            oLog.sMessage_Error = sException;
            _db.TB_LogEmail.Add(oLog);
            _db.SaveChanges();

            oEmail = new System.Net.Mail.MailMessage();
            result.Message = sException;
            result.Status = isSendCompleted ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
            return result;
        }
    }

    public class EmailParameter
    {
        public List<string>? lstFrom { get; set; }
        public List<string>? lstTo { get; set; }
        public List<string>? lstCC { get; set; }
        public List<string>? lstBCC { get; set; }
        public string? sSubject { get; set; }
        public string? sMessage { get; set; }
        public string? sRefer { get; set; }
        public int? nRefID1 { get; set; }
        public List<string>? lstFile { get; set; }
        public bool IsSendtoRealMail { get; set; }
    }
}
