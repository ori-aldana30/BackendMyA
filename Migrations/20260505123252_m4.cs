using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyAMIS.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccionAuditoria",
                columns: table => new
                {
                    idAccionAuditoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccionAuditoria", x => x.idAccionAuditoria);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostico",
                columns: table => new
                {
                    idDiagnostico = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    fechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    mantenimientoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostico", x => x.idDiagnostico);
                    table.ForeignKey(
                        name: "FK_Diagnostico_Mantenimiento_mantenimientoId",
                        column: x => x.mantenimientoId,
                        principalTable: "Mantenimiento",
                        principalColumn: "idMantenimiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MantenimientoRepuesto",
                columns: table => new
                {
                    idMantenimientoRepuesto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    estado = table.Column<string>(type: "text", nullable: false),
                    mantenimientoId = table.Column<int>(type: "integer", nullable: false),
                    repuestoId = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientoRepuesto", x => x.idMantenimientoRepuesto);
                    table.ForeignKey(
                        name: "FK_MantenimientoRepuesto_Mantenimiento_mantenimientoId",
                        column: x => x.mantenimientoId,
                        principalTable: "Mantenimiento",
                        principalColumn: "idMantenimiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetodoDepreciacion",
                columns: table => new
                {
                    idMetodoDepreciacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodoDepreciacion", x => x.idMetodoDepreciacion);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaActivo",
                columns: table => new
                {
                    idAuditoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    activoId = table.Column<int>(type: "integer", nullable: false),
                    usuarioId = table.Column<int>(type: "integer", nullable: false),
                    accionAuditoriaId = table.Column<int>(type: "integer", nullable: false),
                    fechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    detalle = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaActivo", x => x.idAuditoria);
                    table.ForeignKey(
                        name: "FK_AuditoriaActivo_AccionAuditoria_accionAuditoriaId",
                        column: x => x.accionAuditoriaId,
                        principalTable: "AccionAuditoria",
                        principalColumn: "idAccionAuditoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditoriaActivo_Activo_activoId",
                        column: x => x.activoId,
                        principalTable: "Activo",
                        principalColumn: "idActivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepreciacionActivo",
                columns: table => new
                {
                    idDepreciacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    activoId = table.Column<int>(type: "integer", nullable: false),
                    metodoDepreciacionId = table.Column<int>(type: "integer", nullable: false),
                    fechaCalculo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    depreciacionAcumulada = table.Column<decimal>(type: "numeric", nullable: false),
                    valorActual = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepreciacionActivo", x => x.idDepreciacion);
                    table.ForeignKey(
                        name: "FK_DepreciacionActivo_Activo_activoId",
                        column: x => x.activoId,
                        principalTable: "Activo",
                        principalColumn: "idActivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepreciacionActivo_MetodoDepreciacion_metodoDepreciacionId",
                        column: x => x.metodoDepreciacionId,
                        principalTable: "MetodoDepreciacion",
                        principalColumn: "idMetodoDepreciacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaActivo_accionAuditoriaId",
                table: "AuditoriaActivo",
                column: "accionAuditoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaActivo_activoId",
                table: "AuditoriaActivo",
                column: "activoId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciacionActivo_activoId",
                table: "DepreciacionActivo",
                column: "activoId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciacionActivo_metodoDepreciacionId",
                table: "DepreciacionActivo",
                column: "metodoDepreciacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostico_mantenimientoId",
                table: "Diagnostico",
                column: "mantenimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_MantenimientoRepuesto_mantenimientoId",
                table: "MantenimientoRepuesto",
                column: "mantenimientoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaActivo");

            migrationBuilder.DropTable(
                name: "DepreciacionActivo");

            migrationBuilder.DropTable(
                name: "Diagnostico");

            migrationBuilder.DropTable(
                name: "MantenimientoRepuesto");

            migrationBuilder.DropTable(
                name: "AccionAuditoria");

            migrationBuilder.DropTable(
                name: "MetodoDepreciacion");
        }
    }
}
