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

      services.AddTransient<IMeuServico, MeuServico>();

      services.AddControllers()
          .AddNewtonsoftJson(options =>
          {
            options.SerializerSettings.ReferenceLoopHandling =
                      Newtonsoft.Json.ReferenceLoopHandling.Ignore;
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

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
