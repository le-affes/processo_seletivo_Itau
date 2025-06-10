using Microsoft.EntityFrameworkCore;
using WorkerService.Domain;

namespace WorkerService.Infra;

public class CotacaoDbContext : DbContext
{
    public CotacaoDbContext(DbContextOptions<CotacaoDbContext> options) : base(options) { }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Operacao> Operacoes { get; set; }
    public DbSet<Ativo> Ativos { get; set; }
    public DbSet<Posicao> Posicoes { get; set; }
    public DbSet<Cotacao> Cotacoes { get; set; }
}