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
        private readonly IServer server;
        private readonly IWebHost host;

        public KookboxHttpServer(IServer server)
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
            app.UseWebSockets();
            app.Use(async (http, next) =>
            {
                // todo: auth?
                if (http.WebSockets.IsWebSocketRequest)
                    await HandleWebSocketRequest(http);
                else
                    await next();
            });
            app.UseMvc();
            app.UseStaticFiles(new StaticFileOptions
            {
                //todo: need a solution to this (serve from resources?)
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "..", "kookbox.http", "wwwroot"))
            });
        }

        private async Task HandleWebSocketRequest(HttpContext http)
        {
            var transport = new WebsocketNetworkTransport(http);
            await server.ConnectUserAsync("jim", transport);

            await transport.RunAsync();
        }
    }
}
