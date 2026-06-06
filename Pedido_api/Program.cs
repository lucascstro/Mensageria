using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Pedido_api.Producer;

namespace Pedido_api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pedidos Api", Version = "v1" });
        });

        builder.Services.AddSingleton<ProducerBroker>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
