using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IIS.Controllers;

namespace IIS
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapPost("/api/Player/SaveWithXSD", async context =>
                            {
                                var file = context.Request.Form.Files.GetFile("file");

                                var countryController = new PlayerController();
                                if (file == null)
                                {
                                    await context.Response.WriteAsync("Can't open the file, try again.");
                                    return;
                                }
                                countryController.ProcessXmlFileWithXSD(file);


                                context.Response.StatusCode = StatusCodes.Status200OK;
                                await context.Response.WriteAsync("XML file is valid according to the provided XSD schema.");
                            });

                            endpoints.MapPost("/api/Player/SaveWithRNG", async context =>
                            {
                                var file = context.Request.Form.Files.GetFile("file");

                                if (file == null)
                                {
                                    await context.Response.WriteAsync("Can't open the file, try again.");
                                    return;
                                }

                                var countryController = new PlayerController();
                                countryController.ProcessXmlFileWithRNG(file);

                                context.Response.StatusCode = StatusCodes.Status200OK;
                                await context.Response.WriteAsync("XML file is valid according to the provided RNG schema.");
                            });

                        });
                    });
                });
    }
}