using System;
using System.IO;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace kookbox.http
{
    public class KookboxHttpServer : IStartup
    {
        private readonly IMusicServer server;
        private readonly IWebHost host;

        public KookboxHttpServer(IMusicServer server)
        {
            this.server = server;

            host = new WebHostBuilder()
                .ConfigureServices(services => services.AddSingleton<IStartup>(this))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .Build();
        }

        public void Start()
        {
            host.Start();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                //todo: need a solution to this (serve from resources?)
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "..", "kookbox.http", "wwwroot"))
            });
            app.UseMvc();
            app.UseWebSockets();
            app.Use(async (http, next) =>
            {
                // todo: auth?
                if (http.WebSockets.IsWebSocketRequest)
                    await HandleWebSocketRequest(http);
                else
                    await next();
            });
            // todo: remove this once we're cooking on gas
            app.Run(async context =>
            {
                await context.Response.WriteAsync(
                    "Hello World. The Time is: " +
                    DateTime.Now.ToString("hh:mm:ss tt"));

            });
        }

        private async Task HandleWebSocketRequest(HttpContext http)
        {
            var socket = await http.WebSockets.AcceptWebSocketAsync();
            await server.ConnectListenerAsync(http.User.Identity.Name, new WebsocketNetworkTransport(socket));
        }
    }
}
