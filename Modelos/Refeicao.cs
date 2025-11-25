namespace FitLifeAPI.Modelos
{
    public class Refeicao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Calorias { get; set; }
        public double ProteinaGramas { get; set; }
        public double CarboidratosGramas { get; set; }
        public double GordurasGramas { get; set; }
        public DateTime HorarioRefeicao { get; set; } = DateTime.Now;
        public string TipoRefeicao { get; set; } = "Almoço";

        // Chave estrangeira
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Método para verificar se é balanceada
        public bool EstaBalanceada()
        {
            if (Calorias == 0) return false;
            double percentualProteina = (ProteinaGramas * 4 / Calorias) * 100;
            return percentualProteina >= 20 && percentualProteina <= 35;
        }
    }
}