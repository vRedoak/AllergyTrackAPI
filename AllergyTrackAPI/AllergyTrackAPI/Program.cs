using AllergyTrackAPI.Extensions;
using AllergyTrackAPI.Middlewares;
using AllergyTrackAPI.TriggerNotificationSending;
using Application.Mappings;
using Application.Providers;
using Application.Providers.Interfaces;
using Application.Queries.User;
using Application.Services;
using Application.Validations.User;
using Domain.Entities;
using Domain.RabbitMqServices;
using FluentValidation.AspNetCore;
using Infrastructure.DAL;
using Infrastructure.RabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Reflection;
using System.Text;
using TPT.Patents.Api.PipelineBehaviours;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });
    options.AddSecurityDefinition("bearerAuth",
    new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[]{}
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<AllergyTrackContext>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(typeof(AuthorizeUserQueryHandler).Assembly);

builder.Services.AddScoped<IRepository<User>, EntityRepository<User>>();
builder.Services.AddScoped<IRepository<NotificationCategory>, EntityRepository<NotificationCategory>>();
builder.Services.AddScoped<IRepository<NotificationType>, EntityRepository<NotificationType>>();
builder.Services.AddScoped<IRepository<Notification>, EntityRepository<Notification>>();

builder.Services.AddScoped<RabbitMQHelper>();

builder.Services.AddTransient<IRabbitMQEmailSenderService, RabbitMQEmailSenderService>();

builder.Services.AddQuartz(q =>
  {
      q.UseMicrosoftDependencyInjectionJobFactory();

      q.AddJobAndTrigger<TriggerNotificationSendingJob>(configuration);
  });
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

builder.Services.AddTransient<ICurrentUserInfoProvider, CurrentUserInfoProvider>();

builder.Services.AddFluentValidation(conf =>
{
    conf.RegisterValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
    conf.AutomaticValidationEnabled = true;
});
builder.Services.AddTransient<CreateUserCommandValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
