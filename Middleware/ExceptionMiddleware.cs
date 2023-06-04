namespace Get_Together_Riders.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                Console.WriteLine("Error: " + error.Message);
                Console.WriteLine("Error Details: " + context.Request.Path);

                if (error.InnerException != null)
                    Console.WriteLine("Inner Error: " + error.InnerException.Message);

                // Handle Exceptions

                // Return StatusCode, Message, Details or TraceId etc as restult

                //Return StatusCode, Message, Details as result
                await response.WriteAsync("Error");
            }
        }
    }
}
