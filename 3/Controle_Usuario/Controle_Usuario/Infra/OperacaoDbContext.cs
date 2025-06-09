using ControleUsuario.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleUsuario.Infra;

public class OperacaoDbContext : DbContext
{
    public OperacaoDbContext(DbContextOptions<OperacaoDbContext> options) : base(options) { }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Operacao> Operacoes { get; set; }
    public DbSet<Ativo> Ativos { get; set; }
    public DbSet<Posicao> Posicoes { get; set; }
    public DbSet<Cotacao> Cotacoes { get; set; }
}