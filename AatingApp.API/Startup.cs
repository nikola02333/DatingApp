using AatingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using AatingApp.API.Helpers;
using AutoMapper;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext> (x => x.UseSqlite( Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                        .AddJsonOptions( opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
                        
            services.AddCors();
            services.AddAutoMapper();
            
            services.AddTransient<Seed>();

            services.AddScoped<IAuthRepository, AuthRepository>(); // servis je kreiran za svaki poziv, slicno kao Singleton
            services.AddScoped<IDatingRepository,DatingRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(options =>{
                         options.TokenValidationParameters = new TokenValidationParameters{

                             ValidateIssuer=false,
                             IssuerSigningKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes(Configuration.GetSection("appSettings:Token").Value)),
                             
                             
                             ValidateAudience=false
                         };
                     });   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => 
                        builder.Run( async _context => {
                            _context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                            var error = _context.Features.Get<IExceptionHandlerFeature>();
                            if(error != null)
                            
                            { 
                                _context.Response.AddApplicationError(error.Error.Message);
                                await _context.Response.WriteAsync(error.Error.Message);
                            }
                        } )); // Midlware za greske
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               // app.UseHsts();
            }

            //app.UseHttpsRedirection();

            //seeder.SeedUsers(); // Popunjaca Db
            app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc(); // midlware , izmedju klijenta i API(servera)
        }
    }
}
