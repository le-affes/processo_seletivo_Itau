using Microsoft.EntityFrameworkCore;
using WebAPI.Domain;

namespace ControleUsuario.Infra;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Operacao> Operacoes { get; set; }
    public DbSet<Ativo> Ativos { get; set; }
    public DbSet<Posicao> Posicoes { get; set; }
    public DbSet<Cotacao> Cotacoes { get; set; }
}