using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelVrtic.Migrations
{
    /// <inheritdoc />
    public partial class izvjestaji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Izvjestaj",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DijeteId = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrojDanaPrisustva = table.Column<int>(type: "int", nullable: false),
                    KomentarVaspitaca = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ListaAktivnosti = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izvjestaj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Izvjestaj_Dijete_DijeteId",
                        column: x => x.DijeteId,
                        principalTable: "Dijete",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Izvjestaj_DijeteId_Period",
                table: "Izvjestaj",
                columns: new[] { "DijeteId", "Period" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izvjestaj");
        }
    }
}
