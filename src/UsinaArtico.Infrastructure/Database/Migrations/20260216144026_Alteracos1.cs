using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsinaArtico.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alteracos1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unidade_consumidora",
                schema: "public",
                table: "enderecos");

            migrationBuilder.DropColumn(
                name: "data_vencimento",
                schema: "public",
                table: "contratos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "unidade_consumidora",
                schema: "public",
                table: "enderecos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "data_vencimento",
                schema: "public",
                table: "contratos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
