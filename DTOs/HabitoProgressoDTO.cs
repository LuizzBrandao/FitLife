namespace FitLifeAPI.DTOs
{
    // ============================================
    // DTOs de Hábito
    // ============================================
    public class HabitoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Frequencia { get; set; } = string.Empty;
        public bool Completado { get; set; }
        public int SequenciaDias { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? CompletadoEm { get; set; }
    }

    public class CriarHabitoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Frequencia { get; set; } = "Diário";
        public int UsuarioId { get; set; }
    }

    // ============================================
    // DTOs de Progresso
    // ============================================
    public class ProgressoDTO
    {
        public int Id { get; set; }
        public double Peso { get; set; }
        public double PercentualGordura { get; set; }
        public double MassaMuscularKg { get; set; }
        public double IMC { get; set; }
        public DateTime RegistradoEm { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public double VariacaoPeso { get; set; } // Em relação ao último registro
    }

    public class CriarProgressoDTO
    {
        public double Peso { get; set; }
        public double PercentualGordura { get; set; }
        public double MassaMuscularKg { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }

    // ============================================
    // DTO com LINQ - Ranking de Usuários
    // ============================================
    public class RankingUsuarioDTO
    {
        public int Posicao { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TotalTreinos { get; set; }
        public int TotalCaloriasQueimadas { get; set; }
        public int SequenciaDias { get; set; }
    }

    // ============================================
    // DTO com LINQ - Histórico Completo
    // ============================================
    public class HistoricoUsuarioDTO
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public List<TreinoDTO> TreinosRecentes { get; set; } = new List<TreinoDTO>();
        public List<RefeicaoDTO> RefeicoesRecentes { get; set; } = new List<RefeicaoDTO>();
        public List<ProgressoDTO> EvoluçãoPeso { get; set; } = new List<ProgressoDTO>();
        public List<HabitoDTO> HabitosAtivos { get; set; } = new List<HabitoDTO>();
        public EstatisticasDTO Estatisticas { get; set; } = new EstatisticasDTO();
    }

    public class EstatisticasDTO
    {
        public int TotalTreinos { get; set; }
        public int TotalCaloriasQueimadas { get; set; }
        public int TotalCaloriasConsumidas { get; set; }
        public int CaloriasLiquidas { get; set; }
        public double MediaPesoUltimos7Dias { get; set; }
        public int HabitosCompletadosHoje { get; set; }
        public int MaiorSequencia { get; set; }
    }
}