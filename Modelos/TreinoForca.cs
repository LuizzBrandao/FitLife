namespace FitLifeAPI.Modelos
{
    // Herda de Treino (Herança)
    public class TreinoForca : Treino
    {
        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public double PesoKg { get; set; }
        public string GrupoMuscular { get; set; } = "Corpo Todo";

        // Polimorfismo - Implementação específica
        public override int CalcularCaloriasQueimadas()
        {
            return (int)(Series * Repeticoes * PesoKg * 0.5);
        }

        public override string ObterResumo()
        {
            return $"{Nome} ({GrupoMuscular}) - {Series}x{Repeticoes} @ {PesoKg}kg ({CalcularCaloriasQueimadas()} cal)";
        }
    }
}