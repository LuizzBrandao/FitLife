using Microsoft.AspNetCore.Mvc;
using FitLifeAPI.Servicos;
using FitLifeAPI.DTOs;

namespace FitLifeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefeicoesController : ControllerBase
    {
        private readonly IServicoRefeicao _servico;

        public RefeicoesController(IServicoRefeicao servico)
        {
            _servico = servico;
        }

        // GET: api/refeicoes/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<List<RefeicaoDTO>>> ObterPorUsuario(int usuarioId)
        {
            var refeicoes = await _servico.ObterTodasPorUsuarioAsync(usuarioId);
            return Ok(refeicoes);
        }

        // POST: api/refeicoes
        [HttpPost]
        public async Task<ActionResult<RefeicaoDTO>> Criar([FromBody] CriarRefeicaoDTO dto)
        {
            var refeicao = await _servico.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorUsuario), new { usuarioId = dto.UsuarioId }, refeicao);
        }

        // DELETE: api/refeicoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var sucesso = await _servico.DeletarAsync(id);
            if (!sucesso)
                return NotFound($"Refeição com ID {id} não encontrada.");

            return NoContent();
        }
    }
}