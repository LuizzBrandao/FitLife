using Microsoft.EntityFrameworkCore;
using FitLifeAPI.Dados;
using FitLifeAPI.Modelos;
using FitLifeAPI.DTOs;

namespace FitLifeAPI.Servicos
{
    public class ServicoTreino : IServicoTreino
    {
        private readonly FitLifeContexto _contexto;

        public ServicoTreino(FitLifeContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<TreinoDTO>> ObterTodosPorUsuarioAsync(int usuarioId)
        {
            var treinos = new List<TreinoDTO>();

            // Buscar treinos de Cardio
            var cardio = await _contexto.TreinosCardio
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();

            treinos.AddRange(cardio.Select(t => new TreinoDTO
            {
                Id = t.Id,
                Nome = t.Nome,
                Descricao = t.Descricao,
                DuracaoMinutos = t.DuracaoMinutos,
                Data = t.Data,
                Dificuldade = t.Dificuldade,
                CaloriasQueimadas = t.CalcularCaloriasQueimadas(),
                Resumo = t.ObterResumo(),
                TipoTreino = "Cardio"
            }));

            // Buscar treinos de Força
            var forca = await _contexto.TreinosForca
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();

            treinos.AddRange(forca.Select(t => new TreinoDTO
            {
                Id = t.Id,
                Nome = t.Nome,
                Descricao = t.Descricao,
                DuracaoMinutos = t.DuracaoMinutos,
                Data = t.Data,
                Dificuldade = t.Dificuldade,
                CaloriasQueimadas = t.CalcularCaloriasQueimadas(),
                Resumo = t.ObterResumo(),
                TipoTreino = "Força"
            }));

            return treinos.OrderByDescending(t => t.Data).ToList();
        }

        public async Task<TreinoDTO> CriarTreinoCardioAsync(CriarTreinoCardioDTO dto)
        {
            var treino = new TreinoCardio
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                DuracaoMinutos = dto.DuracaoMinutos,
                Dificuldade = dto.Dificuldade,
                DistanciaKm = dto.DistanciaKm,
                FrequenciaCardiacaMedia = dto.FrequenciaCardiacaMedia,
                TipoCardio = dto.TipoCardio,
                UsuarioId = dto.UsuarioId,
                Data = DateTime.Now
            };

            _contexto.TreinosCardio.Add(treino);
            await _contexto.SaveChangesAsync();

            return new TreinoDTO
            {
                Id = treino.Id,
                Nome = treino.Nome,
                Descricao = treino.Descricao,
                DuracaoMinutos = treino.DuracaoMinutos,
                Data = treino.Data,
                Dificuldade = treino.Dificuldade,
                CaloriasQueimadas = treino.CalcularCaloriasQueimadas(),
                Resumo = treino.ObterResumo(),
                TipoTreino = "Cardio"
            };
        }

        public async Task<TreinoDTO> CriarTreinoForcaAsync(CriarTreinoForcaDTO dto)
        {
            var treino = new TreinoForca
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                DuracaoMinutos = dto.DuracaoMinutos,
                Dificuldade = dto.Dificuldade,
                Series = dto.Series,
                Repeticoes = dto.Repeticoes,
                PesoKg = dto.PesoKg,
                GrupoMuscular = dto.GrupoMuscular,
                UsuarioId = dto.UsuarioId,
                Data = DateTime.Now
            };

            _contexto.TreinosForca.Add(treino);
            await _contexto.SaveChangesAsync();

            return new TreinoDTO
            {
                Id = treino.Id,
                Nome = treino.Nome,
                Descricao = treino.Descricao,
                DuracaoMinutos = treino.DuracaoMinutos,
                Data = treino.Data,
                Dificuldade = treino.Dificuldade,
                CaloriasQueimadas = treino.CalcularCaloriasQueimadas(),
                Resumo = treino.ObterResumo(),
                TipoTreino = "Força"
            };
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var cardio = await _contexto.TreinosCardio.FindAsync(id);
            if (cardio != null)
            {
                _contexto.TreinosCardio.Remove(cardio);
                await _contexto.SaveChangesAsync();
                return true;
            }

            var forca = await _contexto.TreinosForca.FindAsync(id);
            if (forca != null)
            {
                _contexto.TreinosForca.Remove(forca);
                await _contexto.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}