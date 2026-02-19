using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsinaArtico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Teste12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_inicio",
                schema: "public",
                table: "contratos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_inicio",
                schema: "public",
                table: "contratos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
