using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MultipleBlazorApps.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                    .WithOrigins("http://localhost:44344/FirstApp", "https://localhost:44344/SecondApp")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseBlazorFrameworkFiles();
            //app.UseStaticFiles();

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //    endpoints.MapControllers();
            //    endpoints.MapFallbackToFile("index.html");
            //});

            app.UseHttpsRedirection();

            //app.MapWhen(ctx => ctx.Request.Host.Port == 5001 ||
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/FirstApp"), first =>
            {
                //first.Use((ctx, nxt) =>
                //{
                //    ctx.Request.Path = "/FirstApp" + ctx.Request.Path;
                //    return nxt();
                //});

                first.UseBlazorFrameworkFiles("/FirstApp");
                first.UseStaticFiles();

                first.UseRouting();
                first.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("FirstApp/{*path:nonfile}", "FirstApp/index.html");
                });
            });

            //app.MapWhen(ctx => ctx.Request.Host.Port == 5002 ||
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/SecondApp"), second =>
            {

                //second.Use((ctx, nxt) =>
                //{
                //    ctx.Request.Path = "/SecondApp" + ctx.Request.Path;
                //    return nxt();
                //});

                second.UseBlazorFrameworkFiles("/SecondApp");
                second.UseStaticFiles();

                second.UseRouting();
                second.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("SecondApp/{*path:nonfile}", "SecondApp/index.html");
                });
            });

        }
    }
}
