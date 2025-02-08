using System.Net;

namespace NZ_Walk.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //we make this if any thing happened during the call will catch any exception
                //happens will catch inside catch block
                await next(httpContext);
            }
            catch (Exception ex)
            {

                var errorId = Guid.NewGuid();

                //log this exceptions
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                //return custom response back
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //here we want to assign content type
                httpContext.Response.ContentType = "application/json";

                //now we want to retrieve response 
                var error = new
                {
                    Id = errorId,
                    //we don't want to send the meassge we recieved => ex.Message because maybe contain
                    //information inside it we will send any generic message
                    ErrorMessage = "something went wrong! we are looking to resolve this"
                };

                //we don't have to convert object error to JSON we have a method 
                //auto make that for us 
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
