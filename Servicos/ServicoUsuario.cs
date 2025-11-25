using Microsoft.EntityFrameworkCore;
using FitLifeAPI.Dados;
using FitLifeAPI.Modelos;
using FitLifeAPI.DTOs;

namespace FitLifeAPI.Servicos
{
    public class ServicoUsuario : IServicoUsuario
    {
        private readonly FitLifeContexto _contexto;

        public ServicoUsuario(FitLifeContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<UsuarioDTO>> ObterTodosAsync()
        {
            var usuarios = await _contexto.Usuarios
                .Include(u => u.Treinos)
                .ToListAsync();

            return usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Idade = u.Idade,
                Peso = u.Peso,
                Altura = u.Altura,
                Objetivo = u.Objetivo,
                IMC = u.CalcularIMC(),
                TotalTreinos = u.Treinos.Count
            }).ToList();
        }

        public async Task<UsuarioDTO?> ObterPorIdAsync(int id)
        {
            var usuario = await _contexto.Usuarios
                .Include(u => u.Treinos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Idade = usuario.Idade,
                Peso = usuario.Peso,
                Altura = usuario.Altura,
                Objetivo = usuario.Objetivo,
                IMC = usuario.CalcularIMC(),
                TotalTreinos = usuario.Treinos.Count
            };
        }

        public async Task<UsuarioDTO> CriarAsync(CriarUsuarioDTO dto)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Idade = dto.Idade,
                Peso = dto.Peso,
                Altura = dto.Altura,
                Objetivo = dto.Objetivo,
                CriadoEm = DateTime.Now
            };

            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Idade = usuario.Idade,
                Peso = usuario.Peso,
                Altura = usuario.Altura,
                Objetivo = usuario.Objetivo,
                IMC = usuario.CalcularIMC(),
                TotalTreinos = 0
            };
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();
            return true;
        }
    }
}