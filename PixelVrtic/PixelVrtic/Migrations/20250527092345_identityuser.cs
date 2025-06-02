using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelVrtic.Migrations
{
    /// <inheritdoc />
    public partial class identityuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aktivnost_Korisnik_idKorisnika",
                table: "Aktivnost");

            migrationBuilder.DropForeignKey(
                name: "FK_Dijete_Korisnik_roditeljId",
                table: "Dijete");

            migrationBuilder.DropForeignKey(
                name: "FK_Grupa_Korisnik_idKorisnika",
                table: "Grupa");

            migrationBuilder.DropForeignKey(
                name: "FK_Obavijest_Korisnik_idAutora",
                table: "Obavijest");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.AlterColumn<string>(
                name: "idAutora",
                table: "Obavijest",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "idKorisnika",
                table: "Grupa",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "roditeljId",
                table: "Dijete",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "adresa",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "brojTelefona",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "datumRodjenja",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "diploma",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fotografija",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "grad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idGrupe",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "krajObrazovanja",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mjestoRodjenja",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "naknada",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pocetakObrazovanja",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "uloga",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "univerzitet",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "uplaceno",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "idKorisnika",
                table: "Aktivnost",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Aktivnost_AspNetUsers_idKorisnika",
                table: "Aktivnost",
                column: "idKorisnika",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dijete_AspNetUsers_roditeljId",
                table: "Dijete",
                column: "roditeljId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grupa_AspNetUsers_idKorisnika",
                table: "Grupa",
                column: "idKorisnika",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Obavijest_AspNetUsers_idAutora",
                table: "Obavijest",
                column: "idAutora",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aktivnost_AspNetUsers_idKorisnika",
                table: "Aktivnost");

            migrationBuilder.DropForeignKey(
                name: "FK_Dijete_AspNetUsers_roditeljId",
                table: "Dijete");

            migrationBuilder.DropForeignKey(
                name: "FK_Grupa_AspNetUsers_idKorisnika",
                table: "Grupa");

            migrationBuilder.DropForeignKey(
                name: "FK_Obavijest_AspNetUsers_idAutora",
                table: "Obavijest");

            migrationBuilder.DropColumn(
                name: "adresa",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "brojTelefona",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "datumRodjenja",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "diploma",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "fotografija",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "grad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "idGrupe",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "krajObrazovanja",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "mjestoRodjenja",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "naknada",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "pocetakObrazovanja",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "prezime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "uloga",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "univerzitet",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "uplaceno",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "idAutora",
                table: "Obavijest",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "idKorisnika",
                table: "Grupa",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "roditeljId",
                table: "Dijete",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "idKorisnika",
                table: "Aktivnost",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    diploma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fotografija = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    grad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idGrupe = table.Column<int>(type: "int", nullable: true),
                    ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    krajObrazovanja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mjestoRodjenja = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    naknada = table.Column<double>(type: "float", nullable: true),
                    pocetakObrazovanja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uloga = table.Column<int>(type: "int", nullable: false),
                    univerzitet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uplaceno = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Aktivnost_Korisnik_idKorisnika",
                table: "Aktivnost",
                column: "idKorisnika",
                principalTable: "Korisnik",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dijete_Korisnik_roditeljId",
                table: "Dijete",
                column: "roditeljId",
                principalTable: "Korisnik",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grupa_Korisnik_idKorisnika",
                table: "Grupa",
                column: "idKorisnika",
                principalTable: "Korisnik",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Obavijest_Korisnik_idAutora",
                table: "Obavijest",
                column: "idAutora",
                principalTable: "Korisnik",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
