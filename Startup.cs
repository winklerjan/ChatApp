using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RascalChatApp.Services;
//using Westwind.AspNetCore.LiveReload;


namespace RascalChatApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddLiveReload();
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddDbContext<Database.ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<UserService>();
            services.AddTransient<MessageService>();
            services.AddTransient<ChannelService>();

            //// for ASP.NET Core 3.x and later, add Runtime Razor Compilation if using anything Razor
            //services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddMvc().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseLiveReload();

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
