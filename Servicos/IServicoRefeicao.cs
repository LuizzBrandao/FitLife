using FitLifeAPI.DTOs;

namespace FitLifeAPI.Servicos
{
    public interface IServicoRefeicao
    {
        Task<List<RefeicaoDTO>> ObterTodasPorUsuarioAsync(int usuarioId);
        Task<RefeicaoDTO> CriarAsync(CriarRefeicaoDTO dto);
        Task<bool> DeletarAsync(int id);
    }
}