using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntervencoesAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEntidade = table.Column<int>(type: "INTEGER", nullable: false),
                    Referencia = table.Column<string>(type: "TEXT", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    NProcesso = table.Column<string>(type: "TEXT", nullable: false),
                    DataDeInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataActualizacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CliCampo1 = table.Column<int>(type: "INTEGER", nullable: false),
                    CliCampo2 = table.Column<int>(type: "INTEGER", nullable: false),
                    CliCampo3 = table.Column<string>(type: "TEXT", nullable: false),
                    CliCampo4 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Referencia = table.Column<string>(type: "TEXT", nullable: false),
                    NomeSocial = table.Column<string>(type: "TEXT", nullable: true),
                    Contribuinte = table.Column<string>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<byte>(type: "INTEGER", nullable: false),
                    Estado = table.Column<short>(type: "INTEGER", nullable: true),
                    Item1 = table.Column<bool>(type: "INTEGER", nullable: false),
                    Item2 = table.Column<int>(type: "INTEGER", nullable: false),
                    Item3 = table.Column<string>(type: "TEXT", nullable: true),
                    DesignacaoComercial = table.Column<string>(type: "TEXT", nullable: true),
                    DataActualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Intervencaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TarefaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Autor = table.Column<string>(type: "TEXT", nullable: false),
                    Visibilidade = table.Column<byte>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    HistoricoEstados = table.Column<string>(type: "TEXT", nullable: false),
                    Prioridade = table.Column<int>(type: "INTEGER", nullable: false),
                    DataRegisto = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataLimite = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataConfirmacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataInstalacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tema = table.Column<string>(type: "TEXT", nullable: false),
                    AccaoRealizada = table.Column<string>(type: "TEXT", nullable: false),
                    PrevisaoEsforco = table.Column<decimal>(type: "TEXT", nullable: false),
                    EsforcoReal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Referencia = table.Column<string>(type: "TEXT", nullable: false),
                    Responsavel = table.Column<string>(type: "TEXT", nullable: false),
                    Coresponsavel = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Notas = table.Column<string>(type: "TEXT", nullable: false),
                    Comentarios = table.Column<string>(type: "TEXT", nullable: false),
                    IntervencaoPaiId = table.Column<int>(type: "INTEGER", nullable: false),
                    EsforcoACobrar = table.Column<decimal>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataQualidade = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateUser = table.Column<string>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TarefaAgendadaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicioPrevista = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFimPrevista = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Alerta = table.Column<short>(type: "INTEGER", nullable: false),
                    MotivoAlerta = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervencaos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessoProjectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumArquivo = table.Column<string>(type: "TEXT", nullable: false),
                    Referencia = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataPrevistaConclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EsforcoPrevisto = table.Column<decimal>(type: "TEXT", nullable: false),
                    EsforcoReal = table.Column<decimal>(type: "TEXT", nullable: false),
                    ProcessoPaiId = table.Column<int>(type: "INTEGER", nullable: false),
                    AvencaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    FornecedorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Fornecedores = table.Column<string>(type: "TEXT", nullable: false),
                    Responsavel = table.Column<string>(type: "TEXT", nullable: false),
                    IdProposta = table.Column<int>(type: "INTEGER", nullable: false),
                    IdContracto = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessoProjectos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Entidades");

            migrationBuilder.DropTable(
                name: "Intervencaos");

            migrationBuilder.DropTable(
                name: "ProcessoProjectos");
        }
    }
}
