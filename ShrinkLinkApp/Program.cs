using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using ShrinkLinkApp.Helpers;
using ShrinkLinkBusiness.ServicesImplementations;
using ShrinkLinkCore.Abstractions;
using ShrinkLinkCQS.Links.Queries;
using ShrinkLinkDb;

namespace ShrinkLinkApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string LogFilePath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Log\mvc_data.log");
            var path = builder.Configuration ["Serilog:LogFilePath"];
            if (path!=null && path!=string.Empty)
                LogFilePath = path;

            builder.Host.UseSerilog((ctx, lc) =>
              lc.WriteTo.File(
                  LogFilePath,
                  LogEventLevel.Information,
                   rollingInterval: RollingInterval.Day,
                   retainedFileCountLimit: null,
                   rollOnFileSizeLimit: true,
                   fileSizeLimitBytes: 4_194_304)
                  .WriteTo.Console(LogEventLevel.Verbose));

            builder.Services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.ExpireTimeSpan = TimeSpan.FromHours(1);
                   options.LoginPath = new PathString(@"/Account/Login");
                   options.LogoutPath = new PathString(@"/Account/Logout");
                   options.AccessDeniedPath = new PathString(@"/Account/RestrictedLogin");
               });
          
            builder.Services.AddAuthorization();
            //builder.Services.AddAuthorization(opts => {
            //    opts.AddPolicy("FullBlocked", policy => {
            //        policy.RequireClaim("FullBlocked", "false");
            //    });
            //});
            //builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<ShrinkLinkContext>(
                optionsBuilder => optionsBuilder.UseSqlServer(connectionString));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(GetLinkByHashQuery).Assembly));



            builder.Services.AddScoped<ILinkService, LinkService>();
            builder.Services.AddScoped<IAdminPanelService, AdminPanelService>();

            builder.Services.AddScoped<EmailChecker>();
            builder.Services.AddScoped<ModelUserBuilder>();
            builder.Services.AddScoped<BirthDateChecker>();
            builder.Services.AddScoped<ExpDateChecker>();
            builder.Services.AddScoped<URLChecker>();
            builder.Services.AddScoped<MD5>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); // Set HttpContext.User
            app.UseAuthorization();

            app.MapControllerRoute(
              name: "shrink",
              pattern: "{shortid}",
              defaults: new { controller = "Link", action = "RedirectLink"});

            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}