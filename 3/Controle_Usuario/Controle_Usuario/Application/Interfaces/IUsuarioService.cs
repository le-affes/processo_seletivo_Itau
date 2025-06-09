using ControleUsuario.Models;

public interface IUsuarioService
{
    Task<Dictionary<string, decimal>> GetInvestidoPorAtivoAsync(int idUsuario);
    Task<Dictionary<string, decimal>> GetPosicoesPorUsuarioAsync(int idUsuario);
    Task<decimal> GetPLTotalAsync(int idUsuario);
    Task<decimal> GetTotalCorretagemAsync(int idUsuario);
    Task<List<Usuario>> GetUsuariosAsync();
}