namespace FitLifeAPI.Modelos
{
    public class Progresso
    {
        public int Id { get; set; }
        public double Peso { get; set; }
        public double PercentualGordura { get; set; }
        public double MassaMuscularKg { get; set; }
        public DateTime RegistradoEm { get; set; } = DateTime.Now;
        public string Observacoes { get; set; } = string.Empty;

        // Chave estrangeira
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Propriedade calculada - IMC
        public double IMC { get; set; }

        // Método para calcular variação
        public double CalcularVariacaoPeso(double pesoAnterior)
        {
            return Peso - pesoAnterior;
        }
    }
}