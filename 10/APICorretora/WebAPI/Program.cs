using ControleUsuario.Infra;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPI.Application.Services;
using WebAPI.Domain;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Adiciona os serviços necessários para o Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<UsuarioService>();
        builder.Services.AddScoped<CorretoraService>();

        builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("Interno"));

        var app = builder.Build();

        var jsonUsuarios = File.ReadAllText("Infra/Data/usuarios.json", Encoding.UTF8);
        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonUsuarios, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var jsonAtivos = File.ReadAllText("Infra/Data/ativos.json", Encoding.UTF8);
        var ativos = JsonSerializer.Deserialize<List<Ativo>>(jsonAtivos, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var jsonCotacoes = File.ReadAllText("Infra/Data/cotacoes.json", Encoding.UTF8);
        var cotacoes = JsonSerializer.Deserialize<List<Cotacao>>(jsonCotacoes, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var jsonOperacoes = File.ReadAllText("Infra/Data/operacoes.json", Encoding.UTF8);
        var operacoes = JsonSerializer.Deserialize<List<Operacao>>(jsonOperacoes, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } });

        var jsonPosicoes = File.ReadAllText("Infra/Data/posicoes.json", Encoding.UTF8);
        var posicoes = JsonSerializer.Deserialize<List<Posicao>>(jsonPosicoes, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (usuarios is not null && !context.Usuarios.Any())
                context.Usuarios.AddRange(usuarios);

            if (ativos is not null && !context.Ativos.Any())
                context.Ativos.AddRange(ativos);

            if (cotacoes is not null && !context.Cotacoes.Any())
                context.Cotacoes.AddRange(cotacoes);

            if (operacoes is not null && !context.Operacoes.Any())
                context.Operacoes.AddRange(operacoes);

            if (posicoes is not null && !context.Posicoes.Any())
                context.Posicoes.AddRange(posicoes);

            context.SaveChanges();
        }

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Corretora");
        });

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
