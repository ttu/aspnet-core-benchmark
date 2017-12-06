using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCoreBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Select correct Startup
            // StartupMinimal
            // StartupMap
            // StartupController
            
            var host = new WebHostBuilder()
             .UseKestrel()
             .UseStartup<StartupMinimal>()
             .Build();

            host.Run();
        }
    }

    public class StartupMinimal
    {
        private static string response = "Hello World!";

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(response);

                // Routing can be done based on Request's path
                // switch (context.Request.Path)
                // {
                //     case "/":
                //         await context.Response.WriteAsync(response);
                //         break;
                //     case "/time":
                //         await context.Response.WriteAsync($"The Time is: {DateTime.Now.ToString("hh:mm:ss tt")}");
                //         break;
                // }
            });
        }
    }

    public class StartupMap
    {
        private static string response = "Hello World!";

        public void Configure(IApplicationBuilder app)
        {
            app.Map("", (builder) =>
            {
                builder.Run(async (context) =>
                {
                    await context.Response.WriteAsync(response);
                });
            });

            app.Map("/time", (builder) => 
            {
                builder.Run(async (context) =>
                {
                    await context.Response.WriteAsync($"The Time is: {DateTime.Now.ToString("hh:mm:ss tt")}");
                });
            });
        }
    }

    public class StartupController
    {
        public void ConfigureServices(IServiceCollection services) => services.AddMvc();

        public void Configure(IApplicationBuilder app) => app.UseMvc();
    }


    [Route("/")]
    public class SimpleController : Controller
    {
        private static string response = "Hello World!";
        
        [HttpGet]
        public IActionResult Hello()
        {
            return Ok(response);
        }

        [HttpGet("time")]
        public IActionResult GetTime()
        {
            return Ok($"The Time is: {DateTime.Now.ToString("hh:mm:ss tt")}");
        }
    }
}
