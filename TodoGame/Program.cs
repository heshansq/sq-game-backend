using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TodoGame.Config;
using TodoGame.Events;
using TodoGame.Services;
using TodoGame.Services.Impl;

internal class Program
{
    public Program(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddSingleton<IDBClient, DBClient>();
        //builder.Services.AddSingleton<SignalRGameEventHub>();
        //builder.Services.Configure<DBConfig>(Configuration);
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IPokeDexService, PokeDexService>();
        builder.Services.AddTransient<ITodoService, TodoService>();
        builder.Services.AddTransient<IGameService, GameService>();
        builder.Services.AddTransient<ISignalRMessageService, SignalRMessageService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<DBConfig>(builder.Configuration.GetSection("MongoConnection"));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtKey").ToString())),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });


        /*
         builder.Services.AddResponseCompression(opt =>
        {
            opt.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
        });
         */

        /**
         * 
         builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowAnyOrigin();
            });
        });
         */

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                builder.WithOrigins("https://localhost:3000", "http://localhost:3000", "http://localhost:7214")
                    .AllowCredentials()
                    //.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                    //.SetIsOriginAllowedToAllowWildcardSubdomains());
        });

        builder.Services.AddSignalR();

        var app = builder.Build();
        app.UseStaticFiles();
        /*
         app.UseCors(builder => builder
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()
         .WithOrigins("http://localhost:3000", "http://localhost:7214"));

        */
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseResponseCompression();
        app.UseCors("AllowSpecificOrigin");
        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseAuthentication();

        app.MapControllers();

        app.MapHub<SignalRGameEventHub>("/signalRGameEventHub");

        app.Run();
    }
}