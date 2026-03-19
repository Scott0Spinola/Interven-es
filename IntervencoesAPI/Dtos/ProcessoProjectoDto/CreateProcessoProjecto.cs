using System.ComponentModel.DataAnnotations;

namespace IntervencoesAPI.Dtos;

public record CreateProcessoProjecto(
    [Required]
    string NumArquivo,

    [Required]
    string Referencia,

    [Required]
    int Estado,

    [Required]
    DateTime DataInicio,

    [Required]
    DateTime DataPrevistaConclusao,



    [Required]
    decimal EsforcoPrevisto,

    [Required]
    decimal EsforcoReal,

    [Required]
    int ProcessoPaiId,

    [Required]
    int AvencaId,

    [Required]
    int ClienteId,

    [Required]
    int FornecedorId,

    [Required]
    string Descricao,

    [Required]
    string Fornecedores,

    [Required]
    string Responsavel,

    [Required]
    int IdProposta,

    [Required]
    int IdContracto
);
