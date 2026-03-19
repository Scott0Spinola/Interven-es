namespace IntervencoesAPI.Models
{
    public class ProcessoProjecto
    {
        public int Id { get; set; }

        public string NumArquivo { get; set; } = string.Empty;

        public string Referencia { get; set; } = string.Empty;

        public int Estado { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataPrevistaConclusao { get; set; }

        public DateTime DataConclusao { get; set; }

        public decimal EsforcoPrevisto { get; set; }

        public decimal EsforcoReal { get; set; }

        public int ProcessoPaiId { get; set; }

        public int AvencaId { get; set; }

        public int ClienteId { get; set; }

        public int FornecedorId { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public string Fornecedores { get; set; } = string.Empty;

        public string Responsavel { get; set; } = string.Empty;

        public int IdProposta { get; set; }

        public int IdContracto { get; set; }
    }
}
