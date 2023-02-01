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
                    //.WithOrigins("https://localhost:44381","https://localhost:44344","http://localhost:44344/consumer", "https://localhost:44381/professional", "https://portal.mypropertyviewings.com", "https://portal.mypropertyviewings.co.uk")
                    .WithOrigins("https://localhost:5001", "https://localhost:5002", "http://localhost:44344", "https://localhost:44381", "https://portal.mypropertyviewings.com", "https://portal.mypropertyviewings.co.uk")
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

            app.UseHttpsRedirection();
            
            app.MapWhen(ctx => (ctx.Request.Host.Equals("mypropertyviewings.com") || ctx.Request.Path.StartsWithSegments("/consumer")), first =>
            {
                //first.Use((ctx, nxt) =>
                //{
                //    ctx.Request.Path = "/consumer" + ctx.Request.Path;
                //    return nxt();
                //});
                first.UseBlazorFrameworkFiles("/consumer");
                first.UseStaticFiles();
                first.UseStaticFiles("/consumer");
                first.UseRouting();
                first.UseAuthentication(); 
                first.UseAuthorization(); 
                first.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("/consumer/{*path:nonfile}", "/consumer/index.html");
                });
            });

            app.MapWhen(ctx => (ctx.Request.Host.Equals("https://portal.mypropertyviewings.co.uk/") || ctx.Request.Path.StartsWithSegments("/professional")), second =>
            {

                //second.Use((ctx, nxt) =>
                //{
                //    ctx.Request.Path = "/professional" + ctx.Request.Path;
                //    return nxt();
                //});

                second.UseBlazorFrameworkFiles("/professional");
                second.UseStaticFiles();
                second.UseStaticFiles("/professional");
                second.UseRouting();
                //second.UseAuthentication();
                //second.UseAuthorization();
                second.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("/professional/{*path:nonfile}", "/professional/index.html");
                });
            });

        }
    }
}
