using System.Net;
using System.Text.Json;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Extensions.Common.STEmail;

// using Extensions.Common.STEmail;
using Extensions.Common.STFunction;
using ST_API.Interfaces;
using ST_API.Models;

namespace ST_API.Services.ISystemService
{
    public class EmailRequestService : IEmailRequestService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;

        public EmailRequestService(ST_IntranetEntity db, IAuthentication authen)
        {
            _db = db;
            _authen = authen;
        }

        public cEmailRequestResult GetEmail(EmailRequest objEmail)
        {
            cEmailRequestResult result = new cEmailRequestResult();

            try
            {
                var dNow = DateTime.Now;
                string sUrl = "https://softthaiapp.com/Intranet/RegistSuccess?sEmail=" + objEmail.sEmail + "&isConfirm=true" + "&sToken=" + objEmail.sUserID;

                string sSetMessageForm =
                @"<!DOCTYPE html>
                    <html>
                        <head>
                            <title>Page Title</title>
                        </head>
                        <style type='text/css'>
                            .buttonStyle{
                                background-color: green;
                            }
                        </style>
                        <body>
                            <p>for confirm email registeration 
                                <span>
                             
                                        <a href =" + sUrl + @">
                                            click here
                                        </ a >


                                </ span >
                            </ p >
                        </ body >
                    </ html > ";

                var objConfirm = _db.TB_Employee_LineConfirm.FirstOrDefault(w => w.sEmail == objEmail.sEmail);

                if (objConfirm == null)
                {
                    objConfirm = new TB_Employee_LineConfirm();
                    objConfirm.nConfirmEmail_ID = (_db.TB_Employee_LineConfirm.Any() ? _db.TB_Employee_LineConfirm.Max(m => m.nConfirmEmail_ID) : 0) + 1;
                    objConfirm.sEmail = objEmail.sEmail;
                    objConfirm.IsConfirm = false;
                    objConfirm.IsTimeOut = false;
                    objConfirm.tSendEmail = dNow.TimeOfDay;


                    _db.TB_Employee_LineConfirm.Add(objConfirm);
                }

                _db.SaveChanges();

                EmailParameter oEmail = new EmailParameter()
                {
                    // lstFrom = new List<string> { "wanatchaporn.k@softthai.co.th" },
                    lstTo = new List<string> { objEmail.sEmail },
                    //lstCC = new List<string> { objEmail.sEmail },
                    //lstBCC = new List<string> { "wanatchaporn.k@softthai.co.th" },
                    sSubject = "Register Confirmation",
                    sMessage = sSetMessageForm,
                    IsSendtoRealMail = true,
                };

                if (oEmail != null)
                {
                    var resultEmail = STEmail.Send(oEmail);

                    result.sMessage = resultEmail.Message;
                    result.nStatusCode = resultEmail.Status; //StatusCodes.Status200OK;
                }
            }
            catch (Exception e)
            {
                result.sMessage = e.Message;
                result.nStatusCode = StatusCodes.Status500InternalServerError;
            }

            return result;
        }

        public cEmailRequestResult SaveEmailConfirmation(EmailData objEmailConfirm)
        {
            cEmailRequestResult result = new cEmailRequestResult();

            try
            {
                TimeSpan tMinutes = new TimeSpan(0, 0, 30, 0);
                var dNow = DateTime.Now;
                TimeSpan tNow = dNow.TimeOfDay;

                var objConfirm = _db.TB_Employee_LineConfirm.FirstOrDefault(w => w.sEmail == objEmailConfirm.sEmail);


                if (objConfirm != null)
                {
                    var tSend = objConfirm.tSendEmail;
                    var tMinutesSend = tNow - tSend;

                    objConfirm.IsConfirm = objEmailConfirm.isConfirm == "true" ? true : false;
                    objConfirm.IsTimeOut = tMinutesSend > tMinutes ? true : false;

                    _db.TB_Employee_LineConfirm.Update(objConfirm);
                }

                if (objConfirm.IsTimeOut == false)
                {
                    var objEmPloyee = _db.TB_Employee.FirstOrDefault(w => w.sEmail == objConfirm.sEmail);
                    if (objEmPloyee != null)
                    {
                        var objLineAcc = _db.TB_Employee_LineAccount.FirstOrDefault(w => w.nEmployeeID == objEmPloyee.nEmployeeID && w.sEmail == objConfirm.sEmail);

                        if (objLineAcc == null)
                        {
                            objLineAcc = new TB_Employee_LineAccount();
                            objLineAcc.nEmployeeID = objEmPloyee.nEmployeeID;
                            objLineAcc.sEmail = objConfirm.sEmail;

                            _db.TB_Employee_LineAccount.Add(objLineAcc);
                        }
                        objLineAcc.sTokenID = objEmailConfirm.sUserID;
                        objLineAcc.sNameAccount = objEmailConfirm.sUserID;
                        objLineAcc.IsDelete = false;
                        SendToLine(objEmailConfirm.sUserID);
                    }
                }

                _db.SaveChanges();

                result.sMessage = result.sMessage;
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.sMessage = e.Message;
                result.nStatusCode = StatusCodes.Status500InternalServerError;
            }

            return result;
        }
        public ResultAPI SendToLine(string sUserID)
        {
            ResultAPI objReturn = new ResultAPI();

            using (var client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
            SecurityProtocolType.Tls11 |
            SecurityProtocolType.Tls;


                client.BaseAddress = new Uri(STFunction.sBaseAdrees);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + STFunction.sBearer);
                string sPathAPI = $"/v2/bot/user/{sUserID}/richmenu/richmenu-d5297787dc4146292e355a75b2d65ee5";
                var response = client.PostAsync(sPathAPI, null).GetAwaiter().GetResult();

                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                cResult? objResult = JsonSerializer.Deserialize<cResult>(content) as cResult;
                if (objResult != null)
                {
                    if (!string.IsNullOrEmpty(objResult.sMssage))
                    {
                        objReturn.sMessage = objResult.sMssage;
                        objReturn.nStatusCode = StatusCodes.Status500InternalServerError;
                    }
                }



            }


            return objReturn;
        }
    }
}