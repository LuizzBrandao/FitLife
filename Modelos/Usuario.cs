namespace FitLifeAPI.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Idade { get; set; }
        public double Peso { get; set; }
        public double Altura { get; set; }
        public string Objetivo { get; set; } = "Manter Peso";
        public DateTime CriadoEm { get; set; } = DateTime.Now;

        // Relacionamentos
        public List<Treino> Treinos { get; set; } = new List<Treino>();
        public List<Refeicao> Refeicoes { get; set; } = new List<Refeicao>();
        public List<Habito> Habitos { get; set; } = new List<Habito>();
        public List<Progresso> Progressos { get; set; } = new List<Progresso>();

        // Método para calcular IMC
        public double CalcularIMC()
        {
            if (Altura <= 0) return 0;
            return Peso / (Altura * Altura);
        }
    }
}