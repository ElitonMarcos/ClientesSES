using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clientes.Migrations
{
    public partial class Migracao1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    TipoCliente = table.Column<int>(type: "int", nullable: false),
                    Documento = table.Column<string>(type: "VARCHAR(14)", nullable: false),
                    Cadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Telefone = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_cliente_documento",
                table: "Clientes",
                column: "Documento");

            migrationBuilder.CreateIndex(
                name: "idx_cliente_nome",
                table: "Clientes",
                column: "Nome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
