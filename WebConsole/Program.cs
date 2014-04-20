using Owin;
using Microsoft.Owin.Host;
using Microsoft.Owin.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace WebConsole
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using System.IO;
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8081";
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Starting web");
                Console.ReadKey();
                Console.WriteLine("Ending web");
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async (env, next) =>
            {
                Console.WriteLine("Requesting: " + env.Request.Path);
                await next();
                Console.WriteLine("ResponseCode: " + env.Response.StatusCode);
            });

            //app.UseWelcomePage();

            app.Run(ctx =>
            {
                return ctx.Response.WriteAsync("Hello JaxArcSig!");
            });

            //app.Use<JaxArcSigComponent>();

            //app.Use(async (env, next) =>
            //{
            //    await env.Response.WriteAsync("Hello there Jacksonville!");
            //    await next();
            //});

            //app.UseHandlerAsync((req, res) =>
            //{
            //    res.ContentType = "text/plain";
            //    return res.WriteAsync("HelloWorld!");
            //});
            
            //app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
            //{
            //    Console.WriteLine("Begin Request");
            //    await next.Invoke(env);
            //    Console.WriteLine("End Request");
            //})));
        }
    }

    public class JaxArcSigComponent
    {
        AppFunc _next;
        public JaxArcSigComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var response = environment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                await writer.WriteAsync("Hello Jacksonville!");
            }
            await _next.Invoke(environment);
        }
    }
}
