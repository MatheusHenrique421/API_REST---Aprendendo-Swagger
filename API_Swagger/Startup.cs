using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.IO;
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using System.Threading.Tasks;

namespace API_Swagger
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
      services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
          //Desabilita as validações pelo Model;
          options.SuppressModelStateInvalidFilter = true;
        });

      services.AddSwaggerGen(c =>
      {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = "Please enter into field the word 'Bearer' following by space and JWT",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
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
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            Array.Empty<string>()
          }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "API_REST - Aprendendo Swagger",
          Version = "v1",
          Description = "Introdução e biblioteca Swashbuckle.AspNetCore.Annotations"
        });
      });

      //var secret = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtConfigurations:Secret").Value);
      //services.AddAuthentication(x =>
      //  {
      //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      //  })
      //  .AddJwtBearer(x =>
      //    {
      //      x.RequireHttpsMetadata = false;
      //      x.SaveToken = true;
      //      x.TokenValidationParameters = new TokenValidationParameters
      //      {
      //        ValidateIssuerSigningKey = true,
      //        IssuerSigningKey = new SymmetricSecurityKey(secret),
      //        ValidateIssuer = false,
      //        ValidateAudience = false
      //      };
      //    });

      var key = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtConfigurations:Secret").Value);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        //x.SaveToken = false;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };

        x.Events = new JwtBearerEvents()
        {
          OnForbidden = c =>
          {
            return Task.CompletedTask;
          },
          OnAuthenticationFailed = c =>
          {
            c.Response.WriteAsync(c.Exception.ToString()).Wait();
            return Task.CompletedTask;
          }
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseExceptionHandler(errorApp =>
        {
          errorApp.Run(async context =>
          {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; ;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
            await context.Response.WriteAsync("ERROR!<br><br>\r\n");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
              await context.Response.WriteAsync(
                                        "File error thrown!<br><br>\r\n");
            }

            await context.Response.WriteAsync(
                                          "<a href=\"/\">Home</a><br>\r\n");
            await context.Response.WriteAsync("</body></html>\r\n");
            await context.Response.WriteAsync(new string(' ', 512));
          });
        });
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "application v1");
          //Obriga o swagger abrir na página da API/SWAGGER
          c.RoutePrefix = string.Empty;
          c.DefaultModelsExpandDepth(-1);

        });
      }

      app.UseRouting();

      app.UseAuthorization();
      app.UseAuthentication();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
