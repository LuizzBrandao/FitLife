namespace FitLifeAPI.Modelos
{
    // Interface (Polimorfismo)
    public interface ITreino
    {
        int CalcularCaloriasQueimadas();
        string ObterResumo();
    }

    // Classe Base Abstrata (Herança)
    public abstract class Treino : ITreino
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int DuracaoMinutos { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public string Dificuldade { get; set; } = "Médio";

        // Chave estrangeira
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Indica se o treino foi concluído (útil para PATCH)
        public bool IsComplete { get; set; } = false;
        public DateTime? CompletedEm { get; set; }

        // Métodos abstratos (cada classe filha implementa)
        public abstract int CalcularCaloriasQueimadas();
        public abstract string ObterResumo();
    }
}