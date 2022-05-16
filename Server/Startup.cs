using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System;

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
            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                    ClockSkew = TimeSpan.Zero
                });



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

            //app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/FirstApp"), first =>
            app.MapWhen(ctx => (ctx.Request.Host.Equals("firstapp.com") || ctx.Request.Path.StartsWithSegments("/FirstApp")), first =>
            {
                //first.Use((ctx, nxt) =>
                //{
                //    ctx.Request.Path = "/FirstApp" + ctx.Request.Path;
                //    return nxt();
                //});

                first.UseBlazorFrameworkFiles("/FirstApp");
                first.UseStaticFiles();

                first.UseRouting();
                first.UseAuthentication(); // very important to keep this order
                first.UseAuthorization(); // very important to keep this order
                first.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("FirstApp/{*path:nonfile}", "FirstApp/index.html");
                });
            });

            //app.UseAuthentication();
            //app.UseAuthorization(); 
            //app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/SecondApp"), second =>
            app.MapWhen(ctx => (ctx.Request.Host.Equals("secondapp.com") || ctx.Request.Path.StartsWithSegments("/SecondApp")), second =>
            {

                //second.Use((ctx, nxt) =>
                //{
                //    ctx.Request.Path = "/SecondApp" + ctx.Request.Path;
                //    return nxt();
                //});

                second.UseBlazorFrameworkFiles("/SecondApp");
                second.UseStaticFiles();

                second.UseRouting();
                second.UseAuthentication(); // very important to keep this order
                second.UseAuthorization(); // very important to keep this order
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
