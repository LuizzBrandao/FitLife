using System.ComponentModel.DataAnnotations;

namespace FitLifeAPI.DTOs
{
 public class AtualizarUsuarioDTO
 {
 [Required]
 public string Nome { get; set; } = string.Empty;

 [Required]
 [EmailAddress]
 public string Email { get; set; } = string.Empty;

 [Range(0,150)]
 public int Idade { get; set; }

 [Range(0,500)]
 public double Peso { get; set; }

 [Range(0,3)]
 public double Altura { get; set; }

 public string Objetivo { get; set; } = "Manter Peso";
 }
}