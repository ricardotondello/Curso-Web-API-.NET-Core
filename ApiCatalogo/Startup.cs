using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using ApiCatalogo.Services;
using ApiCatalogo.Filters;
using ApiCatalogo.Extensions;
using ApiCatalogo.Logging;
using ApiCatalogo.Repository;
using AutoMapper;
using ApiCatalogo.DTOs.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace ApiCatalogo
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

      //CORS
      services.AddCors(c =>
                        {
                          c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
                        });

      //auto mapper
      var mappingConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new MappingProfile());
      });
      IMapper mapper = mappingConfig.CreateMapper();
      services.AddSingleton(mapper);
      //fim auto mapper

      services.AddScoped<ApiLoggingFilter>();

      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddDbContext<AppDbContext>(
          options => options.UseSqlite(
              Configuration.GetConnectionString("DefaultConnection")
          )
      );

      //Identity
      services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
      //fim Identity

      //validacao token JWT
      services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
         options.TokenValidationParameters = new TokenValidationParameters
         {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidIssuer = Configuration["TokenConfiguration:Issuer"],
           ValidAudience = Configuration["TokenConfiguration:Audience"],
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
         });
      //fim validacao token JWT

      services.AddTransient<IMeuServico, MeuServico>();

      services.AddControllers()
          .AddNewtonsoftJson(options =>
          {
            options.SerializerSettings.ReferenceLoopHandling =
                      Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          });

      services.AddApiVersioning(opt =>
      {
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
      ILoggerFactory loggerFactory)
    {

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      //Tratamento de logging
      loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
      {
        LogLevel = LogLevel.Information
      })); ;

      //midlware tratamento erro
      app.ConfigureExceptionHandler();

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseCors(opt => opt.WithOrigins("https://www.apirequest.io"));
      //app.UseCors();
      //app.UseCors(options => options.AllowAnyOrigin());

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
