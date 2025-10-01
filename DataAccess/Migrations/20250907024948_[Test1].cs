using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HABERLER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BASLIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EKLEME_TARIHI = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YAZAR_ID = table.Column<int>(type: "int", nullable: false),
                    CATEGORY_ID = table.Column<int>(type: "int", nullable: false),
                    ICERIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RESIM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIDEO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GOSTERIM_SAYISI = table.Column<int>(type: "int", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABERLER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KATEGORILER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACIKLAMA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KATEGORILER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SLAYTLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BASLIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ICERIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HABER_ID = table.Column<int>(type: "int", nullable: false),
                    RESIM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SLAYTLAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "YAZARLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SOYAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPOSTA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SIFRE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RESIM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YAZARLAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "YORUMLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SOYAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPOSTA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BASLIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ICERIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HABER_ID = table.Column<int>(type: "int", nullable: false),
                    EKLEME_TARIHI = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YORUMLAR", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HABERLER");

            migrationBuilder.DropTable(
                name: "KATEGORILER");

            migrationBuilder.DropTable(
                name: "SLAYTLAR");

            migrationBuilder.DropTable(
                name: "YAZARLAR");

            migrationBuilder.DropTable(
                name: "YORUMLAR");
        }
    }
}
