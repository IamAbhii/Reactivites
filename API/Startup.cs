using System.Reflection.Emit;
using System.Text;
using System.Buffers;
using Application.Activities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using FluentValidation.AspNetCore;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;

namespace API
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
      //adding datacotext to application
      services.AddDbContext<DataContext>(opt =>
      {
        opt.UseLazyLoadingProxies();
        opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
      });

      //allowing cors form localhost 3000
      services.AddCors(opt =>
      {

        opt.AddPolicy("CorsPolicy", policy =>
         {
           policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
         });
      });

      //Mediator service injection
      services.AddMediatR(typeof(List.Handler).Assembly);
      //Automapper
      services.AddAutoMapper(typeof(List.Handler));

      services.AddControllers(opt=>
      {
        var policy= new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        opt.Filters.Add(new AuthorizeFilter(policy));
      })
      .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Create>());

      //Addint identity core to project with signin manager
      var builder=services.AddIdentityCore<AppUser>();
      var identityBuilder=new IdentityBuilder(builder.UserType,builder.Services);
      identityBuilder.AddEntityFrameworkStores<DataContext>();
      identityBuilder.AddSignInManager<SignInManager<AppUser>>();

      //Adding authorizttion custom policy for host can edit event
      services.AddAuthorization(opt=>
      {
          opt.AddPolicy("IsActivityHost",policy=>
          {
            policy.Requirements.Add(new IsHostRequirement());
          });
      });

      services.AddTransient<IAuthorizationHandler,IsHostRequirementHandler>();

      //Adding scope for the interfaces to class
      services.AddScoped<IJWTGenerator,JwtGenerator>();
      services.AddScoped<IUserAccessor,UserAccessor>();


      var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(opt=> 
      {
          opt.TokenValidationParameters= new TokenValidationParameters
          {
            ValidateIssuerSigningKey=true,
            IssuerSigningKey=key,
            ValidateAudience=false,
            ValidateIssuer=false,
          };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      app.UseMiddleware<ErrorHandlingMiddleware>();
      
      if (env.IsDevelopment())
      {
        // app.UseDeveloperExceptionPage();
      }

      //app.UseHttpsRedirection();
   

      app.UseRouting();
      app.UseCors("CorsPolicy");

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
