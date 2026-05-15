using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntervencoesAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF COL_LENGTH(N'dbo.Clientes', N'nome') IS NULL
                BEGIN
                    ALTER TABLE [dbo].[Clientes]
                    ADD [nome] nvarchar(max) NOT NULL
                        CONSTRAINT [DF_Clientes_nome] DEFAULT N'';
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF COL_LENGTH(N'dbo.Clientes', N'nome') IS NOT NULL
                BEGIN
                    DECLARE @df sysname;

                    SELECT @df = dc.name
                    FROM sys.default_constraints dc
                    INNER JOIN sys.columns c
                        ON c.default_object_id = dc.object_id
                    INNER JOIN sys.tables t
                        ON t.object_id = c.object_id
                    INNER JOIN sys.schemas s
                        ON s.object_id = t.schema_id
                    WHERE s.name = N'dbo'
                        AND t.name = N'Clientes'
                        AND c.name = N'nome';

                    IF @df IS NOT NULL
                        EXEC(N'ALTER TABLE [dbo].[Clientes] DROP CONSTRAINT [' + @df + ']');

                    ALTER TABLE [dbo].[Clientes] DROP COLUMN [nome];
                END
                """);
        }
    }
}
