using concessionnaireVoituesGrA.Data;
using concessionnaireVoituesGrA.Services;

namespace concessionnaireVoituesGrA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddScoped<InterfaceDbFactory, SqlServerDBFactory>();
            builder.Services.AddScoped<VoituresDao>();
            builder.Services.AddScoped<ClientsDao>();
            
            builder.Services.AddScoped<ComptesDao>();
            builder.Services.AddScoped<DemandesLocationDao>();

            builder.Services.AddScoped<InterfaceClients, GestionClients>();
            builder.Services.AddScoped<InterfaceVoitures, GestionVoitures>();

            builder.Services.AddScoped<InterfaceComptes, GestionComptes>();
            builder.Services.AddScoped<InterfaceLocations, GestionLocations>();

            builder.Services.AddAuthentication("Cookies").AddCookie(options =>
            {
                options.LoginPath = "/Comptes/Authentifier";       //action pour se connecter
                options.LogoutPath = "/Comptes/Signout"; //action pour se déconnecter
                options.AccessDeniedPath = "/Comptes/AccessDenied"; //action en cas d’accès refusé
            });
            
            builder.Services.AddAuthorization();
           
            //ajout d'une autorisation CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:12054")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Middleware d'authentification et d'autorisation
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
