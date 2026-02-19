using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsinaArtico.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Alteracoes5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_todo_items_asp_net_users_user_id",
                schema: "public",
                table: "todo_items");

            migrationBuilder.DropIndex(
                name: "ix_todo_items_user_id",
                schema: "public",
                table: "todo_items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_todo_items_user_id",
                schema: "public",
                table: "todo_items",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_todo_items_asp_net_users_user_id",
                schema: "public",
                table: "todo_items",
                column: "user_id",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
