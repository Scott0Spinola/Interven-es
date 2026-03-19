namespace IntervencoesAPI.Models
{
    public class Intervencao
    {
        public int Id { get; set; }

        public int ProcessoId { get; set; }

        public int TarefaId { get; set; }

        public string? Autor { get; set; }

        public byte Visibilidade { get; set; }

        public int Tipo { get; set; }

        public int Estado { get; set; }

        public string? HistoricoEstados { get; set; } 

        public int Prioridade { get; set; }

        public DateTime DataRegisto { get; set; }

        public DateTime DataLimite { get; set; }

        public DateTime DataConclusao { get; set; }

        public DateTime DataConfirmacao { get; set; }

        public DateTime DataInstalacao { get; set; }

        public string Tema { get; set; } = string.Empty;

        public string AccaoRealizada { get; set; } = string.Empty;

        public decimal PrevisaoEsforco { get; set; }

        public decimal EsforcoReal { get; set; }

        public string Referencia { get; set; } = string.Empty;

        public string Responsavel { get; set; } = string.Empty;

        public string Coresponsavel { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public string Notas { get; set; } = string.Empty;

        public string Comentarios { get; set; } = string.Empty;

        public int IntervencaoPaiId { get; set; }

        public decimal EsforcoACobrar { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataQualidade { get; set; }

        public string UpdateUser { get; set; } = string.Empty;

        public DateTime UpdateDate { get; set; }

        public string? Email { get; set; }

        public string? Codigo { get; set; }

        public DateTime? DataInicio { get; set; }

        public int TarefaAgendadaId { get; set; }

        public DateTime DataInicioPrevista { get; set; }

        public DateTime DataFimPrevista { get; set; }

        public short Alerta { get; set; }

        public string MotivoAlerta { get; set; } = string.Empty;

        public ProcessoProjecto? ProcessoProjecto{get; set; }
    }
}
