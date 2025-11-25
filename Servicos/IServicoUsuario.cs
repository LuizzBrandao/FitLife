using FitLifeAPI.DTOs;

namespace FitLifeAPI.Servicos
{
    public interface IServicoUsuario
    {
        Task<List<UsuarioDTO>> ObterTodosAsync();
        Task<UsuarioDTO?> ObterPorIdAsync(int id);
        Task<UsuarioDTO> CriarAsync(CriarUsuarioDTO dto);
        Task<bool> DeletarAsync(int id);
    }
}