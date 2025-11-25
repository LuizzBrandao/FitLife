namespace FitLifeAPI.DTOs
{
    public class TreinoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int DuracaoMinutos { get; set; }
        public DateTime Data { get; set; }
        public string Dificuldade { get; set; } = string.Empty;
        public int CaloriasQueimadas { get; set; }
        public string Resumo { get; set; } = string.Empty;
        public string TipoTreino { get; set; } = string.Empty;
    }

    public class CriarTreinoCardioDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int DuracaoMinutos { get; set; }
        public string Dificuldade { get; set; } = "Médio";
        public double DistanciaKm { get; set; }
        public int FrequenciaCardiacaMedia { get; set; }
        public string TipoCardio { get; set; } = "Corrida";
        public int UsuarioId { get; set; }
    }

    public class CriarTreinoForcaDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int DuracaoMinutos { get; set; }
        public string Dificuldade { get; set; } = "Médio";
        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public double PesoKg { get; set; }
        public string GrupoMuscular { get; set; } = "Corpo Todo";
        public int UsuarioId { get; set; }
    }
}