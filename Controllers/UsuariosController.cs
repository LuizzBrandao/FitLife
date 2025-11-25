using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitLifeAPI.Dados;
using FitLifeAPI.Modelos;
using FitLifeAPI.DTOs;

namespace FitLifeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly FitLifeContexto _context;

        public UsuariosController(FitLifeContexto context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Treinos)
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Treinos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound(new { Message = $"Usuário com Id={id} não encontrado." });

            return Ok(usuario);
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CriarUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Idade = dto.Idade,
                Peso = dto.Peso,
                Altura = dto.Altura,
                Objetivo = dto.Objetivo
            };

            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            var created = await _context.Usuarios
                .Include(u => u.Treinos)
                .FirstOrDefaultAsync(u => u.Id == newUser.Id);

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, created);
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AtualizarUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
                return NotFound(new { Message = $"Usuário com Id={id} não encontrado." });

            // Atualiza campos simples
            existing.Nome = dto.Nome;
            existing.Email = dto.Email;
            existing.Idade = dto.Idade;
            existing.Peso = dto.Peso;
            existing.Altura = dto.Altura;
            existing.Objetivo = dto.Objetivo;

            await _context.SaveChangesAsync();

            var updated = await _context.Usuarios
                .Include(u => u.Treinos)
                .FirstOrDefaultAsync(u => u.Id == id);

            return Ok(new { Message = "Usuário atualizado com sucesso.", Updated = updated });
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
                return NotFound(new { Message = $"Usuário com Id={id} não encontrado." });

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Usuário '{usuario.Nome}' removido com sucesso." });
        }
    }
}