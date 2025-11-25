using Microsoft.EntityFrameworkCore;
using FitLifeAPI.Dados;
using FitLifeAPI.DTOs;

namespace FitLifeAPI.Servicos
{
    public interface IServicoLinq
    {
        Task<List<RankingUsuarioDTO>> ObterRankingUsuariosAsync();
        Task<HistoricoUsuarioDTO> ObterHistoricoCompletoAsync(int usuarioId);
        Task<List<string>> ObterSugestoesInteligentesTreinoAsync(int usuarioId);
        Task<List<string>> ObterSugestoesInteligentesAlimentacaoAsync(int usuarioId);
    }

    public class ServicoLinq : IServicoLinq
    {
        private readonly FitLifeContexto _contexto;

        public ServicoLinq(FitLifeContexto contexto)
        {
            _contexto = contexto;
        }

        // ============================================
        // LINQ - Ranking de Usuários por Treinos
        // ============================================
        public async Task<List<RankingUsuarioDTO>> ObterRankingUsuariosAsync()
        {
            var ranking = await _contexto.Usuarios
                .Select(u => new
                {
                    Usuario = u,
                    TotalTreinos = u.Treinos.Count,
                    TotalCalorias = u.Treinos.OfType<Modelos.TreinoCardio>()
                        .Sum(t => t.DuracaoMinutos * 10) +
                        u.Treinos.OfType<Modelos.TreinoForca>()
                        .Sum(t => (int)(t.Series * t.Repeticoes * t.PesoKg * 0.5)),
                    MaiorSequencia = u.Habitos.Max(h => (int?)h.SequenciaDias) ?? 0
                })
                .OrderByDescending(x => x.TotalCalorias) // Ordenar por calorias queimadas
                .ThenByDescending(x => x.TotalTreinos)
                .ToListAsync();

            return ranking.Select((item, index) => new RankingUsuarioDTO
            {
                Posicao = index + 1,
                Nome = item.Usuario.Nome,
                TotalTreinos = item.TotalTreinos,
                TotalCaloriasQueimadas = item.TotalCalorias,
                SequenciaDias = item.MaiorSequencia
            }).ToList();
        }

        // ============================================
        // LINQ - Histórico Completo do Usuário
        // ============================================
        public async Task<HistoricoUsuarioDTO> ObterHistoricoCompletoAsync(int usuarioId)
        {
            var usuario = await _contexto.Usuarios
                .Include(u => u.Treinos)
                .Include(u => u.Refeicoes)
                .Include(u => u.Habitos)
                .Include(u => u.Progressos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            // LINQ - Treinos dos últimos 30 dias
            var treinosRecentes = usuario.Treinos
                .Where(t => t.Data >= DateTime.Now.AddDays(-30))
                .OrderByDescending(t => t.Data)
                .Select(t => new TreinoDTO
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Data = t.Data,
                    DuracaoMinutos = t.DuracaoMinutos,
                    CaloriasQueimadas = t.CalcularCaloriasQueimadas(),
                    Resumo = t.ObterResumo()
                })
                .ToList();

            // LINQ - Refeições dos últimos 7 dias
            var refeicoesRecentes = usuario.Refeicoes
                .Where(r => r.HorarioRefeicao >= DateTime.Now.AddDays(-7))
                .OrderByDescending(r => r.HorarioRefeicao)
                .Select(r => new RefeicaoDTO
                {
                    Id = r.Id,
                    Nome = r.Nome,
                    Calorias = r.Calorias,
                    HorarioRefeicao = r.HorarioRefeicao,
                    TipoRefeicao = r.TipoRefeicao,
                    EstaBalanceada = r.EstaBalanceada()
                })
                .ToList();

            // LINQ - Evolução de peso
            var evolucaoPeso = usuario.Progressos
                .OrderByDescending(p => p.RegistradoEm)
                .Take(10)
                .Select(p => new ProgressoDTO
                {
                    Id = p.Id,
                    Peso = p.Peso,
                    PercentualGordura = p.PercentualGordura,
                    MassaMuscularKg = p.MassaMuscularKg,
                    IMC = p.IMC,
                    RegistradoEm = p.RegistradoEm,
                    Observacoes = p.Observacoes
                })
                .ToList();

            // LINQ - Hábitos ativos
            var habitosAtivos = usuario.Habitos
                .Where(h => h.Completado || h.CriadoEm >= DateTime.Now.AddDays(-30))
                .OrderByDescending(h => h.SequenciaDias)
                .Select(h => new HabitoDTO
                {
                    Id = h.Id,
                    Nome = h.Nome,
                    Completado = h.Completado,
                    SequenciaDias = h.SequenciaDias,
                    Frequencia = h.Frequencia
                })
                .ToList();

            // LINQ - Estatísticas calculadas
            var estatisticas = new EstatisticasDTO
            {
                TotalTreinos = usuario.Treinos.Count,
                TotalCaloriasQueimadas = usuario.Treinos.Sum(t => t.CalcularCaloriasQueimadas()),
                TotalCaloriasConsumidas = usuario.Refeicoes
                    .Where(r => r.HorarioRefeicao.Date == DateTime.Today)
                    .Sum(r => r.Calorias),
                MediaPesoUltimos7Dias = usuario.Progressos
                    .Where(p => p.RegistradoEm >= DateTime.Now.AddDays(-7))
                    .Average(p => (double?)p.Peso) ?? usuario.Peso,
                HabitosCompletadosHoje = usuario.Habitos
                    .Count(h => h.Completado && h.CompletadoEm?.Date == DateTime.Today),
                MaiorSequencia = usuario.Habitos.Max(h => (int?)h.SequenciaDias) ?? 0
            };

            estatisticas.CaloriasLiquidas = estatisticas.TotalCaloriasConsumidas -
                estatisticas.TotalCaloriasQueimadas;

            return new HistoricoUsuarioDTO
            {
                NomeUsuario = usuario.Nome,
                TreinosRecentes = treinosRecentes,
                RefeicoesRecentes = refeicoesRecentes,
                EvoluçãoPeso = evolucaoPeso,
                HabitosAtivos = habitosAtivos,
                Estatisticas = estatisticas
            };
        }

        // ============================================
        // IA SIMPLES - Sugestões de Treino
        // ============================================
        public async Task<List<string>> ObterSugestoesInteligentesTreinoAsync(int usuarioId)
        {
            var usuario = await _contexto.Usuarios
                .Include(u => u.Treinos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            var sugestoes = new List<string>();

            // LINQ - Analisar padrões de treino
            var treinosUltimos7Dias = usuario.Treinos
                .Where(t => t.Data >= DateTime.Now.AddDays(-7))
                .ToList();

            var totalCardio = treinosUltimos7Dias.OfType<Modelos.TreinoCardio>().Count();
            var totalForca = treinosUltimos7Dias.OfType<Modelos.TreinoForca>().Count();

            // Sugestão baseada em objetivo
            if (usuario.Objetivo == "Perder Peso" && totalCardio < 3)
            {
                sugestoes.Add("💡 Para perder peso, recomendamos 3-4 treinos de cardio por semana. Que tal uma corrida hoje?");
            }

            if (usuario.Objetivo == "Ganhar Massa" && totalForca < 4)
            {
                sugestoes.Add("💪 Para ganhar massa muscular, faça 4-5 treinos de força por semana. Foque em grupos musculares diferentes!");
            }

            // Sugestão baseada em frequência
            if (treinosUltimos7Dias.Count == 0)
            {
                sugestoes.Add("🏃 Você não treinou nos últimos 7 dias. Que tal começar com um treino leve de 20 minutos?");
            }

            // LINQ - Identificar grupo muscular menos treinado
            var gruposMusculares = usuario.Treinos
                .OfType<Modelos.TreinoForca>()
                .Where(t => t.Data >= DateTime.Now.AddDays(-30))
                .GroupBy(t => t.GrupoMuscular)
                .Select(g => new { Grupo = g.Key, Total = g.Count() })
                .OrderBy(x => x.Total)
                .ToList();

            if (gruposMusculares.Any())
            {
                var menosTreinado = gruposMusculares.First().Grupo;
                sugestoes.Add($"🎯 Você treinou pouco '{menosTreinado}' este mês. Considere adicionar exercícios para esse grupo!");
            }

            // LINQ - Calcular IMC e sugerir
            var imc = usuario.CalcularIMC();
            if (imc > 25 && totalCardio < 3)
            {
                sugestoes.Add("🏃 Seu IMC está acima do ideal. Aumente a frequência de treinos aeróbicos para 4-5x por semana.");
            }

            if (sugestoes.Count == 0)
            {
                sugestoes.Add("✅ Parabéns! Você está com uma rotina de treinos equilibrada. Continue assim!");
            }

            return sugestoes;
        }

        // ============================================
        // IA SIMPLES - Sugestões de Alimentação
        // ============================================
        public async Task<List<string>> ObterSugestoesInteligentesAlimentacaoAsync(int usuarioId)
        {
            var usuario = await _contexto.Usuarios
                .Include(u => u.Refeicoes)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            var sugestoes = new List<string>();

            // LINQ - Analisar refeições de hoje
            var refeicoesHoje = usuario.Refeicoes
                .Where(r => r.HorarioRefeicao.Date == DateTime.Today)
                .ToList();

            var totalCaloriasHoje = refeicoesHoje.Sum(r => r.Calorias);
            var totalProteinasHoje = refeicoesHoje.Sum(r => r.ProteinaGramas);

            // Sugestão baseada em objetivo e calorias
            if (usuario.Objetivo == "Perder Peso" && totalCaloriasHoje > 2000)
            {
                sugestoes.Add("⚠️ Você já consumiu muitas calorias hoje. Tente fazer refeições mais leves à noite.");
            }

            if (usuario.Objetivo == "Ganhar Massa" && totalProteinasHoje < 100)
            {
                sugestoes.Add("🥩 Para ganhar massa, consuma pelo menos 1,6g de proteína por kg de peso. Adicione mais proteínas!");
            }

            // LINQ - Verificar refeições balanceadas
            var refeicoesDesbalanceadas = refeicoesHoje
                .Where(r => !r.EstaBalanceada())
                .Count();

            if (refeicoesDesbalanceadas > 2)
            {
                sugestoes.Add("🥗 Suas refeições estão desbalanceadas. Equilibre melhor proteínas, carboidratos e gorduras.");
            }

            // Verificar se comeu hoje
            if (refeicoesHoje.Count == 0)
            {
                sugestoes.Add("🍽️ Você ainda não registrou nenhuma refeição hoje. Não esqueça de se alimentar bem!");
            }

            // LINQ - Média de calorias últimos 7 dias
            var mediaCaloriasSemanais = usuario.Refeicoes
                .Where(r => r.HorarioRefeicao >= DateTime.Now.AddDays(-7))
                .GroupBy(r => r.HorarioRefeicao.Date)
                .Average(g => (double?)g.Sum(r => r.Calorias)) ?? 0;

            if (mediaCaloriasSemanais < 1500)
            {
                sugestoes.Add("⚠️ Sua média de calorias está muito baixa. Isso pode prejudicar seu metabolismo!");
            }

            if (sugestoes.Count == 0)
            {
                sugestoes.Add("✅ Sua alimentação está equilibrada. Continue assim!");
            }

            return sugestoes;
        }
    }
}