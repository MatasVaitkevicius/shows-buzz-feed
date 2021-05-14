using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shows_buzz_feed.Data;
using shows_buzz_feed.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace shows_buzz_feed
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("connectionString")));
            services.AddRazorPages();
            services.AddServerSideBlazor();
            try
            {
                services.AddSingleton<FilmService>();
                services.AddSingleton<TVShowsService>();
                services.AddSingleton<AnswerService>();
                services.AddSingleton<QuizService>();
                services.AddSingleton<RatingService>();
                services.AddSingleton<UserService>();
                services.AddSingleton<UserSeenFilmService>();
                services.AddSingleton<QuestionService>();
                services.AddSingleton<RecommendedFilmService>();
                services.AddSingleton<UserSeenTvShowService>();
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;

                throw;
            }
            services.AddBlazoredModal();
            services.AddBlazoredToast();
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<HttpClient>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
