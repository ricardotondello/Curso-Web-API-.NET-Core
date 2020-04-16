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
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.AspNet.OData.Extensions;

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
      services.AddAuthentication(option =>
            {
              option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
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

      //ODATA
      services.AddOData();
      //FIM ODATA

      //swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "APICatalogo",
          Description = "Catalogo de Produtos e Categorias",
          TermsOfService = new Uri("http://google.com"),
          Contact = new OpenApiContact
          {
            Name = "Ricardo Tondello",
            Email = "rkdtondello@gmail.com",
            Url = new Uri("http://google.com"),
          },
          License = new OpenApiLicense
          {
            Name = "Usar sobre LICX",
            Url = new Uri("http://google.com"),
          }
        });


        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        //security para testes no swagger api

        c.AddSecurityDefinition(
          "Bearer", new OpenApiSecurityScheme
          {
            Scheme = "Bearer",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
          });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
                }
                },
                new string[] {}
            }
          });
      });
      //fim swagger

      services.AddControllers(mvcOpt => 
           mvcOpt.EnableEndpointRouting = false)
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

      app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
      //app.UseCors();
      //app.UseCors(options => options.AllowAnyOrigin());

      app.UseAuthentication();

      app.UseAuthorization();

      //swagger
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICatalogo");
      });
      //fim swagger

      // app.UseEndpoints(endpoints =>
      // {
      //   endpoints.MapControllers();
      // });

      app.UseMvc(option =>
            {
              option.EnableDependencyInjection();
              option.Expand().Select().Count().OrderBy().Filter();
            });

    }
  }
}
