using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        builder.Services.AddTransient<IPokeDexService, PokeDexService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<DBConfig>(builder.Configuration.GetSection("MongoConnection"));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}