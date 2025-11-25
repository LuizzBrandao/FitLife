using FitLifeAPI.DTOs;

namespace FitLifeAPI.Servicos
{
    public interface IServicoTreino
    {
        Task<List<TreinoDTO>> ObterTodosPorUsuarioAsync(int usuarioId);
        Task<TreinoDTO> CriarTreinoCardioAsync(CriarTreinoCardioDTO dto);
        Task<TreinoDTO> CriarTreinoForcaAsync(CriarTreinoForcaDTO dto);
        Task<bool> DeletarAsync(int id);
    }
}