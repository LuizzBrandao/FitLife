using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitLifeAPI.Dados;
using FitLifeAPI.Modelos;

namespace FitLifeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreinosController : ControllerBase
    {
        private readonly FitLifeContexto _context;

        public TreinosController(FitLifeContexto context)
        {
            _context = context;
        }

        // GET: api/treinos/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var treinos = await _context.TreinosForca
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();

            return Ok(treinos);
        }

        // GET: api/treinos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var treino = await _context.TreinosForca.FindAsync(id);
            if (treino == null)
                return NotFound(new { Message = $"Treino com Id={id} não encontrado." });

            return Ok(treino);
        }

        // POST: api/treinos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TreinoForca newTreino)
        {
            if (newTreino == null)
                return BadRequest("O corpo da requisição é inválido.");

            var usuario = await _context.Usuarios.FindAsync(newTreino.UsuarioId);
            if (usuario == null)
                return BadRequest($"Usuário com Id={newTreino.UsuarioId} não encontrado.");

            _context.TreinosForca.Add(newTreino);
            await _context.SaveChangesAsync();

            var created = await _context.TreinosForca.FindAsync(newTreino.Id);
            return CreatedAtAction(nameof(GetById), new { id = newTreino.Id }, created);
        }

        // PUT: api/treinos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TreinoForca updatedTreino)
        {
            if (updatedTreino == null)
                return BadRequest("O corpo da requisição é inválido.");

            var existing = await _context.TreinosForca.FindAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Treino com Id={id} não encontrado." });

            // Atualiza campos básicos
            existing.Nome = updatedTreino.Nome;
            existing.DuracaoMinutos = updatedTreino.DuracaoMinutos;
            existing.Series = updatedTreino.Series;
            existing.Repeticoes = updatedTreino.Repeticoes;
            existing.PesoKg = updatedTreino.PesoKg;
            existing.GrupoMuscular = updatedTreino.GrupoMuscular;

            await _context.SaveChangesAsync();

            var updated = await _context.TreinosForca.FindAsync(id);
            return Ok(new { Message = "Treino atualizado com sucesso.", Updated = updated });
        }

        // PATCH: api/treinos/5/complete
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> PatchComplete(int id)
        {
            var treino = await _context.TreinosForca.FindAsync(id);
            if (treino == null)
                return NotFound(new { Message = $"Treino com Id={id} não encontrado." });

            treino.IsComplete = true;
            treino.CompletedEm = DateTime.Now;
            _context.TreinosForca.Update(treino);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Treino '{treino.Nome}' marcado como completo.", Treino = treino });
        }

        // DELETE: api/treinos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var treino = await _context.TreinosForca.FindAsync(id);
            if (treino == null)
                return NotFound(new { Message = $"Treino com Id={id} não encontrado." });

            _context.TreinosForca.Remove(treino);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Treino '{treino.Nome}' removido com sucesso." });
        }
    }
}