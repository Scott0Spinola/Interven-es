using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntervencoesAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipsAndNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HistoricoEstados",
                table: "Intervencaos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Autor",
                table: "Intervencaos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessoProjectos_ClienteId",
                table: "ProcessoProjectos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Intervencaos_ProcessoId",
                table: "Intervencaos",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdEntidade",
                table: "Clientes",
                column: "IdEntidade");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Entidades_IdEntidade",
                table: "Clientes",
                column: "IdEntidade",
                principalTable: "Entidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervencaos_ProcessoProjectos_ProcessoId",
                table: "Intervencaos",
                column: "ProcessoId",
                principalTable: "ProcessoProjectos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessoProjectos_Clientes_ClienteId",
                table: "ProcessoProjectos",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Entidades_IdEntidade",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervencaos_ProcessoProjectos_ProcessoId",
                table: "Intervencaos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessoProjectos_Clientes_ClienteId",
                table: "ProcessoProjectos");

            migrationBuilder.DropIndex(
                name: "IX_ProcessoProjectos_ClienteId",
                table: "ProcessoProjectos");

            migrationBuilder.DropIndex(
                name: "IX_Intervencaos_ProcessoId",
                table: "Intervencaos");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_IdEntidade",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "HistoricoEstados",
                table: "Intervencaos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Autor",
                table: "Intervencaos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
