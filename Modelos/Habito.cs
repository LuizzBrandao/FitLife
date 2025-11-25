namespace FitLifeAPI.Modelos
{
    public class Habito
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Frequencia { get; set; } = "Diário"; // Diário, Semanal
        public bool Completado { get; set; } = false;
        public DateTime CriadoEm { get; set; } = DateTime.Now;
        public DateTime? CompletadoEm { get; set; }
        public int SequenciaDias { get; set; } = 0; // Streak

        // Chave estrangeira
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Método para marcar como completo
        public void MarcarComoCompleto()
        {
            Completado = true;
            CompletadoEm = DateTime.Now;
            SequenciaDias++;
        }

        // Método para resetar
        public void Resetar()
        {
            Completado = false;
            CompletadoEm = null;
        }
    }
}