using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyAMIS.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoActivo",
                columns: table => new
                {
                    idEstadoActivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoActivo", x => x.idEstadoActivo);
                });

            migrationBuilder.CreateTable(
                name: "TipoActivo",
                columns: table => new
                {
                    idTipoActivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoActivo", x => x.idTipoActivo);
                });

            migrationBuilder.CreateTable(
                name: "Activo",
                columns: table => new
                {
                    idActivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    marca = table.Column<string>(type: "text", nullable: false),
                    modelo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    tipoActivold = table.Column<int>(type: "integer", nullable: false),
                    estadoActivold = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activo", x => x.idActivo);
                    table.ForeignKey(
                        name: "FK_Activo_EstadoActivo_estadoActivold",
                        column: x => x.estadoActivold,
                        principalTable: "EstadoActivo",
                        principalColumn: "idEstadoActivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activo_TipoActivo_tipoActivold",
                        column: x => x.tipoActivold,
                        principalTable: "TipoActivo",
                        principalColumn: "idTipoActivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activo_estadoActivold",
                table: "Activo",
                column: "estadoActivold");

            migrationBuilder.CreateIndex(
                name: "IX_Activo_tipoActivold",
                table: "Activo",
                column: "tipoActivold");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activo");

            migrationBuilder.DropTable(
                name: "EstadoActivo");

            migrationBuilder.DropTable(
                name: "TipoActivo");
        }
    }
}
