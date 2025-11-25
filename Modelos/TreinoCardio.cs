namespace FitLifeAPI.Modelos
{
    // Herda de Treino (Herança)
    public class TreinoCardio : Treino
    {
        public double DistanciaKm { get; set; }
        public int FrequenciaCardiacaMedia { get; set; }
        public string TipoCardio { get; set; } = "Corrida";

        // Polimorfismo - Implementação específica
        public override int CalcularCaloriasQueimadas()
        {
            double fatorIntensidade = TipoCardio switch
            {
                "Corrida" => 1.2,
                "Ciclismo" => 1.0,
                "Natação" => 1.3,
                _ => 1.0
            };

            return (int)(DuracaoMinutos * 10 * fatorIntensidade);
        }

        public override string ObterResumo()
        {
            return $"{TipoCardio} - {DistanciaKm}km em {DuracaoMinutos}min ({CalcularCaloriasQueimadas()} cal)";
        }
    }
}