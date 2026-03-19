using System.ComponentModel.DataAnnotations;

namespace IntervencoesAPI.Dtos;

public record UpdateIntervencao
([Required]
    int ProcessoId,

    [Required]
    int TarefaId,

    [Required]
    string Autor,

    [Required]
    byte Visibilidade,

    [Required]
    int Tipo,

    [Required]
    int Estado,

    [Required]
    string HistoricoEstados,

    [Required]
    int Prioridade,

    [Required]
    DateTime DataRegisto,

    [Required]
    DateTime DataLimite,

    [Required]
    DateTime DataConclusao,

    [Required]
    DateTime DataConfirmacao,

    [Required]
    DateTime DataInstalacao,

    [Required]
    string Tema,

    [Required]
    string AccaoRealizada,

    [Required]
    decimal PrevisaoEsforco,

    [Required]
    decimal EsforcoReal,

    [Required]
    string Referencia,

    [Required]
    string Responsavel,

    [Required]
    string Coresponsavel,

    string? Descricao,

    [Required]
    string Notas,

    [Required]
    string Comentarios,

    [Required]
    int IntervencaoPaiId,

    [Required]
    decimal EsforcoACobrar,

    [Required]
    decimal Valor,

    [Required]
    DateTime DataCriacao,

    [Required]
    DateTime DataQualidade,

    [Required]
    string UpdateUser,

    [Required]
    DateTime UpdateDate,

    string? Email,

    string? Codigo,

    DateTime? DataInicio,

    [Required]
    int TarefaAgendadaId,

    [Required]
    DateTime DataInicioPrevista,

    [Required]
    DateTime DataFimPrevista,

    [Required]
    short Alerta,

    [Required]
    string MotivoAlerta
    
    
    );
