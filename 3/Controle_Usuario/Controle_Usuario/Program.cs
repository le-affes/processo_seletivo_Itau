using ControleUsuario.Application.Interfaces;
using ControleUsuario.Application.Services;
using ControleUsuario.Infra;
using ControleUsuario.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<OperacaoDbContext>(options => options.UseInMemoryDatabase("Interno"));
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<IAnaliseFinanceiraService, AnaliseFinanceiraService>();

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

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<OperacaoDbContext>();

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

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}