using Microsoft.EntityFrameworkCore;
using Notes.Main.Mapper;
using Notes.Main.Services;

namespace Notes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<NoteService>();

            builder.Services.AddAuthentication("Cookies")
                .AddCookie(options => 
                {
                    options.LoginPath = "/Home/Login";
                    options.ReturnUrlParameter = "/Home";
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                });
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<DataLayer.DataContext>(options =>
            {
                options.UseMySQL(builder.Configuration.GetConnectionString("MySQL"), sql => { });
            });

            var app = builder.Build();
            using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                if (serviceScope != null)
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<Notes.DataLayer.DataContext>();
                    context.Database.Migrate();
                }
            }
            app.UseStaticFiles();
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();


        }
    }
}
