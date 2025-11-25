namespace FitLifeAPI.DTOs
{
    public class RefeicaoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Calorias { get; set; }
        public double ProteinaGramas { get; set; }
        public double CarboidratosGramas { get; set; }
        public double GordurasGramas { get; set; }
        public DateTime HorarioRefeicao { get; set; }
        public string TipoRefeicao { get; set; } = string.Empty;
        public bool EstaBalanceada { get; set; }
    }

    public class CriarRefeicaoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Calorias { get; set; }
        public double ProteinaGramas { get; set; }
        public double CarboidratosGramas { get; set; }
        public double GordurasGramas { get; set; }
        public string TipoRefeicao { get; set; } = "Almoço";
        public int UsuarioId { get; set; }
    }
}