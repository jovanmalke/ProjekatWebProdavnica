using Microsoft.EntityFrameworkCore.Migrations;

namespace PrWeb.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nabavljac",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BrTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifra = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nabavljac", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Proizvod",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvod", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prodavnica",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProdavnicaNabavljacID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavnica", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prodavnica_Nabavljac_ProdavnicaNabavljacID",
                        column: x => x.ProdavnicaNabavljacID,
                        principalTable: "Nabavljac",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dostavljac",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Godine = table.Column<int>(type: "int", nullable: false),
                    BrTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CenaUsluge = table.Column<int>(type: "int", nullable: false),
                    DostavljacProdavnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dostavljac", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dostavljac_Prodavnica_DostavljacProdavnicaID",
                        column: x => x.DostavljacProdavnicaID,
                        principalTable: "Prodavnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdavnicaProizvod",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    ProdavnicaSpoj1ID = table.Column<int>(type: "int", nullable: true),
                    ProizvodSpoj1ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdavnicaProizvod", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProdavnicaProizvod_Prodavnica_ProdavnicaSpoj1ID",
                        column: x => x.ProdavnicaSpoj1ID,
                        principalTable: "Prodavnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProdavnicaProizvod_Proizvod_ProizvodSpoj1ID",
                        column: x => x.ProizvodSpoj1ID,
                        principalTable: "Proizvod",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Narudzbina",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adresa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NarudzbinaProdavnicaID = table.Column<int>(type: "int", nullable: true),
                    NarudzbinaDostavljacID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narudzbina", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Narudzbina_Dostavljac_NarudzbinaDostavljacID",
                        column: x => x.NarudzbinaDostavljacID,
                        principalTable: "Dostavljac",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Narudzbina_Prodavnica_NarudzbinaProdavnicaID",
                        column: x => x.NarudzbinaProdavnicaID,
                        principalTable: "Prodavnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProizvodNarudzbina",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KolicinaProizvodaUNarudzbini = table.Column<int>(type: "int", nullable: false),
                    NarudzbinaSpoj2ID = table.Column<int>(type: "int", nullable: true),
                    ProizvodSpoj2ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProizvodNarudzbina", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProizvodNarudzbina_Narudzbina_NarudzbinaSpoj2ID",
                        column: x => x.NarudzbinaSpoj2ID,
                        principalTable: "Narudzbina",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProizvodNarudzbina_Proizvod_ProizvodSpoj2ID",
                        column: x => x.ProizvodSpoj2ID,
                        principalTable: "Proizvod",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dostavljac_DostavljacProdavnicaID",
                table: "Dostavljac",
                column: "DostavljacProdavnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbina_NarudzbinaDostavljacID",
                table: "Narudzbina",
                column: "NarudzbinaDostavljacID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbina_NarudzbinaProdavnicaID",
                table: "Narudzbina",
                column: "NarudzbinaProdavnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Prodavnica_ProdavnicaNabavljacID",
                table: "Prodavnica",
                column: "ProdavnicaNabavljacID");

            migrationBuilder.CreateIndex(
                name: "IX_ProdavnicaProizvod_ProdavnicaSpoj1ID",
                table: "ProdavnicaProizvod",
                column: "ProdavnicaSpoj1ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProdavnicaProizvod_ProizvodSpoj1ID",
                table: "ProdavnicaProizvod",
                column: "ProizvodSpoj1ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProizvodNarudzbina_NarudzbinaSpoj2ID",
                table: "ProizvodNarudzbina",
                column: "NarudzbinaSpoj2ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProizvodNarudzbina_ProizvodSpoj2ID",
                table: "ProizvodNarudzbina",
                column: "ProizvodSpoj2ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdavnicaProizvod");

            migrationBuilder.DropTable(
                name: "ProizvodNarudzbina");

            migrationBuilder.DropTable(
                name: "Narudzbina");

            migrationBuilder.DropTable(
                name: "Proizvod");

            migrationBuilder.DropTable(
                name: "Dostavljac");

            migrationBuilder.DropTable(
                name: "Prodavnica");

            migrationBuilder.DropTable(
                name: "Nabavljac");
        }
    }
}
