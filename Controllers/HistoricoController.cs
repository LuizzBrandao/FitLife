using Microsoft.AspNetCore.Mvc;
using FitLifeAPI.Servicos;
using FitLifeAPI.DTOs;

namespace FitLifeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoController : ControllerBase
    {
        private readonly IServicoLinq _servico;

        public HistoricoController(IServicoLinq servico)
        {
            _servico = servico;
        }

        // GET: api/historico/ranking
        /// <summary>
        /// Obtém ranking de usuários por treinos e calorias (usa LINQ)
        /// </summary>
        [HttpGet("ranking")]
        public async Task<ActionResult<List<RankingUsuarioDTO>>> ObterRanking()
        {
            var ranking = await _servico.ObterRankingUsuariosAsync();
            return Ok(ranking);
        }

        // GET: api/historico/completo/5
        /// <summary>
        /// Obtém histórico completo do usuário com estatísticas (usa LINQ)
        /// </summary>
        [HttpGet("completo/{usuarioId}")]
        public async Task<ActionResult<HistoricoUsuarioDTO>> ObterHistoricoCompleto(int usuarioId)
        {
            try
            {
                var historico = await _servico.ObterHistoricoCompletoAsync(usuarioId);
                return Ok(historico);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/historico/sugestoes-treino/5
        /// <summary>
        /// Obtém sugestões inteligentes de treino baseadas em IA simples
        /// </summary>
        [HttpGet("sugestoes-treino/{usuarioId}")]
        public async Task<ActionResult<List<string>>> ObterSugestoesTreino(int usuarioId)
        {
            try
            {
                var sugestoes = await _servico.ObterSugestoesInteligentesTreinoAsync(usuarioId);
                return Ok(sugestoes);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/historico/sugestoes-alimentacao/5
        /// <summary>
        /// Obtém sugestões inteligentes de alimentação baseadas em IA simples
        /// </summary>
        [HttpGet("sugestoes-alimentacao/{usuarioId}")]
        public async Task<ActionResult<List<string>>> ObterSugestoesAlimentacao(int usuarioId)
        {
            try
            {
                var sugestoes = await _servico.ObterSugestoesInteligentesAlimentacaoAsync(usuarioId);
                return Ok(sugestoes);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}