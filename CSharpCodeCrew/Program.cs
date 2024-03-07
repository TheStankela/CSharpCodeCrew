using CSharpCodeCrew.Application;
using CSharpCodeCrew.Domain.Interfaces;
using CSharpCodeCrew.Services;
using CSharpCodeCrew.Settings;

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

            builder.Services.Configure<LocalApiSettings>(
                builder.Configuration.GetSection(nameof(LocalApiSettings)));

            builder.Services.AddHttpClient<RCVaultClient>();
            builder.Services.AddHttpClient<LocalApiClient>();

            builder.Services.AddSingleton<IRCVaultClient, RCVaultClient>();
            builder.Services.AddSingleton<ILocalApiClient, LocalApiClient>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
