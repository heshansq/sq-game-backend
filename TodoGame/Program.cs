using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TodoGame.Config;
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

        // Add services to the container.


        builder.Services.AddControllers();
        builder.Services.AddSingleton<IDBClient, DBClient>();
        //builder.Services.Configure<DBConfig>(Configuration);
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IPokeDexService, PokeDexService>();
        builder.Services.AddTransient<ITodoService, TodoService>();
        builder.Services.AddTransient<IGameService, GameService>();
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseAuthentication();

        app.MapControllers();

        app.Run();
    }
}