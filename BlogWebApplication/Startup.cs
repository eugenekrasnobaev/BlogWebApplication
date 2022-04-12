using System.IO;
using BlogBLL;
using BlogBLL.Interfaces;
using BlogBLL.Services;
using BlogDAL.Entities;
using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using BlogWebApplication.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BlogWebApplication.Logging;
using Microsoft.Extensions.Logging;

namespace BlogWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogContext>(options => options.UseSqlServer(connection));

            services.AddScoped<IBlogContext, BlogContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRepository<Post>, PostRepository>();
            services.AddScoped<IRepository<Comment>, CommentRepository>();

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IAccountService, AccountService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogle("Google", options =>
                {
                    options.ClientId = "598105207110-etk6m0cosop0mlm0djhpbsugph9e2j8g.apps.googleusercontent.com";
                    options.ClientSecret = "RTnPTpFVYRu5zmNqYgZjAkat";
                });

            services.AddAutoMapper(typeof(MapperBllProfile), typeof(MapperPlProfile));

            services.AddScoped<IAuthorizationHandler, OwnerAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OwnerPolicy", policy =>
                    policy.Requirements.Add(new OwnerRequirement()));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BlogContext>();
                DbInitializer.Seed(context);
            }

            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            loggerFactory.CreateLogger("FileLogger");
        }
    }
}
