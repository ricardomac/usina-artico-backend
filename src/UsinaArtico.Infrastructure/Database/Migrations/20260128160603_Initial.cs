using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsinaArtico.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "clientes",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    codigo_cliente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contratos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    anexo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    valor_kwh = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    quantidade_kwh = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    data_vencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contratos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "estados",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_estados", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    normalized_user_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    normalized_email = table.Column<string>(type: "text", nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cidades",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    estado_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cidades", x => x.id);
                    table.ForeignKey(
                        name: "fk_cidades_estados_estado_id",
                        column: x => x.estado_id,
                        principalSchema: "public",
                        principalTable: "estados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "todo_items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    labels = table.Column<List<string>>(type: "text[]", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_items_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enderecos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    unidade_consumidora = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    codigo_instalacao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    logradouro = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    tipo_ligacao = table.Column<int>(type: "integer", nullable: false),
                    cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cidade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contrato_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_enderecos", x => x.id);
                    table.ForeignKey(
                        name: "fk_enderecos_cidades_cidade_id",
                        column: x => x.cidade_id,
                        principalSchema: "public",
                        principalTable: "cidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_enderecos_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "public",
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_enderecos_contratos_contrato_id",
                        column: x => x.contrato_id,
                        principalSchema: "public",
                        principalTable: "contratos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cidades_estado_id",
                schema: "public",
                table: "cidades",
                column: "estado_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientes_codigo_cliente",
                schema: "public",
                table: "clientes",
                column: "codigo_cliente");

            migrationBuilder.CreateIndex(
                name: "ix_enderecos_cidade_id",
                schema: "public",
                table: "enderecos",
                column: "cidade_id");

            migrationBuilder.CreateIndex(
                name: "ix_enderecos_cliente_id",
                schema: "public",
                table: "enderecos",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_enderecos_contrato_id",
                schema: "public",
                table: "enderecos",
                column: "contrato_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_items_user_id",
                schema: "public",
                table: "todo_items",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                schema: "public",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enderecos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "todo_items",
                schema: "public");

            migrationBuilder.DropTable(
                name: "cidades",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "contratos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "estados",
                schema: "public");
        }
    }
}
