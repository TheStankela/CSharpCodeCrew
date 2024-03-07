using CSharpCodeCrew.HttpClients;
using CSharpCodeCrew.Interfaces;
using CSharpCodeCrew.Services;
using CSharpCodeCrew.Settings;
using Microsoft.Extensions.Options;

namespace CSharpCodeCrew
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.Configure<RemoteApiSettings>(
                builder.Configuration.GetSection(nameof(RemoteApiSettings)));

            builder.Services.AddHttpClient<RCVaultClient>();
            builder.Services.AddSingleton<IRCVaultClient, RCVaultClient>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
