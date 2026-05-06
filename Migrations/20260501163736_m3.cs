using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyAMIS.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "costoCompra",
                table: "Activo",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "EstadoFalla",
                columns: table => new
                {
                    idEstadoFalla = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoFalla", x => x.idEstadoFalla);
                });

            migrationBuilder.CreateTable(
                name: "EstadoMantenimiento",
                columns: table => new
                {
                    idEstadoMantenimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoMantenimiento", x => x.idEstadoMantenimiento);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoActivo",
                columns: table => new
                {
                    idMovimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    fechaMovimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    motivo = table.Column<string>(type: "text", nullable: false),
                    activoId = table.Column<int>(type: "integer", nullable: false),
                    areaOrigenId = table.Column<int>(type: "integer", nullable: false),
                    areaDestinoId = table.Column<int>(type: "integer", nullable: false),
                    responsableId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoActivo", x => x.idMovimiento);
                    table.ForeignKey(
                        name: "FK_MovimientoActivo_Activo_activoId",
                        column: x => x.activoId,
                        principalTable: "Activo",
                        principalColumn: "idActivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrioridadFalla",
                columns: table => new
                {
                    idPrioridadFalla = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrioridadFalla", x => x.idPrioridadFalla);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    idTipoDocumento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.idTipoDocumento);
                });

            migrationBuilder.CreateTable(
                name: "TipoMantenimiento",
                columns: table => new
                {
                    idTipoMantenimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMantenimiento", x => x.idTipoMantenimiento);
                });

            migrationBuilder.CreateTable(
                name: "Falla",
                columns: table => new
                {
                    idFalla = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    fechaReporte = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    activoId = table.Column<int>(type: "integer", nullable: false),
                    prioridadFallaId = table.Column<int>(type: "integer", nullable: false),
                    estadoFallaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Falla", x => x.idFalla);
                    table.ForeignKey(
                        name: "FK_Falla_Activo_activoId",
                        column: x => x.activoId,
                        principalTable: "Activo",
                        principalColumn: "idActivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Falla_EstadoFalla_estadoFallaId",
                        column: x => x.estadoFallaId,
                        principalTable: "EstadoFalla",
                        principalColumn: "idEstadoFalla",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Falla_PrioridadFalla_prioridadFallaId",
                        column: x => x.prioridadFallaId,
                        principalTable: "PrioridadFalla",
                        principalColumn: "idPrioridadFalla",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mantenimiento",
                columns: table => new
                {
                    idMantenimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    fechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activoId = table.Column<int>(type: "integer", nullable: false),
                    fallaId = table.Column<int>(type: "integer", nullable: true),
                    tipoMantenimientoId = table.Column<int>(type: "integer", nullable: false),
                    estadoMantenimientoId = table.Column<int>(type: "integer", nullable: false),
                    tecnicoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimiento", x => x.idMantenimiento);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_Activo_activoId",
                        column: x => x.activoId,
                        principalTable: "Activo",
                        principalColumn: "idActivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_EstadoMantenimiento_estadoMantenimientoId",
                        column: x => x.estadoMantenimientoId,
                        principalTable: "EstadoMantenimiento",
                        principalColumn: "idEstadoMantenimiento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_Falla_fallaId",
                        column: x => x.fallaId,
                        principalTable: "Falla",
                        principalColumn: "idFalla");
                    table.ForeignKey(
                        name: "FK_Mantenimiento_TipoMantenimiento_tipoMantenimientoId",
                        column: x => x.tipoMantenimientoId,
                        principalTable: "TipoMantenimiento",
                        principalColumn: "idTipoMantenimiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoActivo",
                columns: table => new
                {
                    idDocumentoActivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    referenciaDocumento = table.Column<string>(type: "text", nullable: false),
                    fechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    activoId = table.Column<int>(type: "integer", nullable: false),
                    tipoDocumentoId = table.Column<int>(type: "integer", nullable: false),
                    mantenimientoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoActivo", x => x.idDocumentoActivo);
                    table.ForeignKey(
                        name: "FK_DocumentoActivo_Activo_activoId",
                        column: x => x.activoId,
                        principalTable: "Activo",
                        principalColumn: "idActivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentoActivo_Mantenimiento_mantenimientoId",
                        column: x => x.mantenimientoId,
                        principalTable: "Mantenimiento",
                        principalColumn: "idMantenimiento");
                    table.ForeignKey(
                        name: "FK_DocumentoActivo_TipoDocumento_tipoDocumentoId",
                        column: x => x.tipoDocumentoId,
                        principalTable: "TipoDocumento",
                        principalColumn: "idTipoDocumento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoActivo_activoId",
                table: "DocumentoActivo",
                column: "activoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoActivo_mantenimientoId",
                table: "DocumentoActivo",
                column: "mantenimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoActivo_tipoDocumentoId",
                table: "DocumentoActivo",
                column: "tipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Falla_activoId",
                table: "Falla",
                column: "activoId");

            migrationBuilder.CreateIndex(
                name: "IX_Falla_estadoFallaId",
                table: "Falla",
                column: "estadoFallaId");

            migrationBuilder.CreateIndex(
                name: "IX_Falla_prioridadFallaId",
                table: "Falla",
                column: "prioridadFallaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_activoId",
                table: "Mantenimiento",
                column: "activoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_estadoMantenimientoId",
                table: "Mantenimiento",
                column: "estadoMantenimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_fallaId",
                table: "Mantenimiento",
                column: "fallaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_tipoMantenimientoId",
                table: "Mantenimiento",
                column: "tipoMantenimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoActivo_activoId",
                table: "MovimientoActivo",
                column: "activoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoActivo");

            migrationBuilder.DropTable(
                name: "MovimientoActivo");

            migrationBuilder.DropTable(
                name: "Mantenimiento");

            migrationBuilder.DropTable(
                name: "TipoDocumento");

            migrationBuilder.DropTable(
                name: "EstadoMantenimiento");

            migrationBuilder.DropTable(
                name: "Falla");

            migrationBuilder.DropTable(
                name: "TipoMantenimiento");

            migrationBuilder.DropTable(
                name: "EstadoFalla");

            migrationBuilder.DropTable(
                name: "PrioridadFalla");

            migrationBuilder.DropColumn(
                name: "costoCompra",
                table: "Activo");
        }
    }
}
