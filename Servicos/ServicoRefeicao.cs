using Microsoft.EntityFrameworkCore;
using FitLifeAPI.Dados;
using FitLifeAPI.Modelos;
using FitLifeAPI.DTOs;

namespace FitLifeAPI.Servicos
{
    public class ServicoRefeicao : IServicoRefeicao
    {
        private readonly FitLifeContexto _contexto;

        public ServicoRefeicao(FitLifeContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<RefeicaoDTO>> ObterTodasPorUsuarioAsync(int usuarioId)
        {
            var refeicoes = await _contexto.Refeicoes
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            return refeicoes.Select(r => new RefeicaoDTO
            {
                Id = r.Id,
                Nome = r.Nome,
                Descricao = r.Descricao,
                Calorias = r.Calorias,
                ProteinaGramas = r.ProteinaGramas,
                CarboidratosGramas = r.CarboidratosGramas,
                GordurasGramas = r.GordurasGramas,
                HorarioRefeicao = r.HorarioRefeicao,
                TipoRefeicao = r.TipoRefeicao,
                EstaBalanceada = r.EstaBalanceada()
            }).OrderByDescending(r => r.HorarioRefeicao).ToList();
        }

        public async Task<RefeicaoDTO> CriarAsync(CriarRefeicaoDTO dto)
        {
            var refeicao = new Refeicao
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Calorias = dto.Calorias,
                ProteinaGramas = dto.ProteinaGramas,
                CarboidratosGramas = dto.CarboidratosGramas,
                GordurasGramas = dto.GordurasGramas,
                TipoRefeicao = dto.TipoRefeicao,
                UsuarioId = dto.UsuarioId,
                HorarioRefeicao = DateTime.Now
            };

            _contexto.Refeicoes.Add(refeicao);
            await _contexto.SaveChangesAsync();

            return new RefeicaoDTO
            {
                Id = refeicao.Id,
                Nome = refeicao.Nome,
                Descricao = refeicao.Descricao,
                Calorias = refeicao.Calorias,
                ProteinaGramas = refeicao.ProteinaGramas,
                CarboidratosGramas = refeicao.CarboidratosGramas,
                GordurasGramas = refeicao.GordurasGramas,
                HorarioRefeicao = refeicao.HorarioRefeicao,
                TipoRefeicao = refeicao.TipoRefeicao,
                EstaBalanceada = refeicao.EstaBalanceada()
            };
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var refeicao = await _contexto.Refeicoes.FindAsync(id);
            if (refeicao == null) return false;

            _contexto.Refeicoes.Remove(refeicao);
            await _contexto.SaveChangesAsync();
            return true;
        }
    }
}