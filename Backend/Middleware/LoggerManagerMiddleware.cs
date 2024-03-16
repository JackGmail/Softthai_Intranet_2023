using Backend.Interfaces;
using System.Net;
using System.Text.Json;

namespace Backend.Middleware
{
    public class LoggerManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        public LoggerManagerMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                _logger.LogError($"Somthing went wrong : {error}");
                await HandleExceptionAsync(context, error);
            }
        }
        /// <summary>
        /// Error Message ที่แจ้งกลับ
        /// </summary>
        /// <param name="context"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            context.Response.ContentType = "application/json";
            //var errorResponse = new ErrorDetails();

            switch (error)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        //errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                        //errorResponse.Message = ex.Message;
                        break;
                    }
                    //errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    //errorResponse.Message = ex.Message;
                    break;
                //ไม่เจอข้อมูล
                case KeyNotFoundException ex:
                    //errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    //errorResponse.Message = ex.Message;
                    break;
                case AccessViolationException e:
                    // if (e.Message.Contains(ResultStatus.Duplicate))
                    // {
                    //     errorResponse.StatusCode = (int)HttpStatusCode.AlreadyReported;
                    //     errorResponse.Message = e.Message;
                    // }
                    // else if (e.Message.Contains(ResultStatus.Success))
                    // {
                    //     errorResponse.StatusCode = (int)HttpStatusCode.OK;
                    //     errorResponse.Message = "OK This is Error Exception";
                    // }
                    break;
                default:
                    //errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    //errorResponse.Message = "Internal Server Error Check Log";
                    break;
            }
            _logger.LogError(error.Message);
            //var result = JsonSerializer.Serialize(errorResponse);
            var result = JsonSerializer.Serialize("");

            #region Save To DB

            #endregion

            await context.Response.WriteAsync(result);

        }
    }
}
