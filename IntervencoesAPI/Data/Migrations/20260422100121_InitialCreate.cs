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
                name: "Entidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contribuinte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<byte>(type: "tinyint", nullable: false),
                    Estado = table.Column<short>(type: "smallint", nullable: true),
                    Item1 = table.Column<bool>(type: "bit", nullable: false),
                    Item2 = table.Column<int>(type: "int", nullable: false),
                    Item3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignacaoComercial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataActualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEntidade = table.Column<int>(type: "int", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    NProcesso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDeInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataActualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CliCampo1 = table.Column<int>(type: "int", nullable: false),
                    CliCampo2 = table.Column<int>(type: "int", nullable: false),
                    CliCampo3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CliCampo4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Entidades_IdEntidade",
                        column: x => x.IdEntidade,
                        principalTable: "Entidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessoProjectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumArquivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPrevistaConclusao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EsforcoPrevisto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EsforcoReal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProcessoPaiId = table.Column<int>(type: "int", nullable: false),
                    AvencaId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fornecedores = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProposta = table.Column<int>(type: "int", nullable: false),
                    IdContracto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessoProjectos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessoProjectos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Intervencaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessoId = table.Column<int>(type: "int", nullable: false),
                    TarefaId = table.Column<int>(type: "int", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visibilidade = table.Column<byte>(type: "tinyint", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    HistoricoEstados = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prioridade = table.Column<int>(type: "int", nullable: false),
                    DataRegisto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConfirmacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataInstalacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccaoRealizada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrevisaoEsforco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EsforcoReal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coresponsavel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntervencaoPaiId = table.Column<int>(type: "int", nullable: false),
                    EsforcoACobrar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataQualidade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarefaAgendadaId = table.Column<int>(type: "int", nullable: false),
                    DataInicioPrevista = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFimPrevista = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alerta = table.Column<short>(type: "smallint", nullable: false),
                    MotivoAlerta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervencaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Intervencaos_ProcessoProjectos_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "ProcessoProjectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdEntidade",
                table: "Clientes",
                column: "IdEntidade");

            migrationBuilder.CreateIndex(
                name: "IX_Intervencaos_ProcessoId",
                table: "Intervencaos",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessoProjectos_ClienteId",
                table: "ProcessoProjectos",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Intervencaos");

            migrationBuilder.DropTable(
                name: "ProcessoProjectos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Entidades");
        }
    }
}
