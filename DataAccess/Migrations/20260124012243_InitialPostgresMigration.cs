using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HABERLER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BASLIK = table.Column<string>(type: "text", nullable: false),
                    EKLEME_TARIHI = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    YAZAR_ID = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_ID = table.Column<int>(type: "integer", nullable: false),
                    ICERIK = table.Column<string>(type: "text", nullable: false),
                    RESIM = table.Column<string>(type: "text", nullable: false),
                    VIDEO = table.Column<string>(type: "text", nullable: false),
                    GOSTERIM_SAYISI = table.Column<int>(type: "integer", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABERLER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KATEGORILER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ACIKLAMA = table.Column<string>(type: "text", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KATEGORILER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SLAYTLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BASLIK = table.Column<string>(type: "text", nullable: false),
                    ICERIK = table.Column<string>(type: "text", nullable: false),
                    HABER_ID = table.Column<int>(type: "integer", nullable: false),
                    RESIM = table.Column<string>(type: "text", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SLAYTLAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "YAZARLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AD = table.Column<string>(type: "text", nullable: false),
                    SOYAD = table.Column<string>(type: "text", nullable: false),
                    EPOSTA = table.Column<string>(type: "text", nullable: false),
                    SIFRE = table.Column<string>(type: "text", nullable: false),
                    RESIM = table.Column<string>(type: "text", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YAZARLAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "YORUMLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AD = table.Column<string>(type: "text", nullable: false),
                    SOYAD = table.Column<string>(type: "text", nullable: false),
                    EPOSTA = table.Column<string>(type: "text", nullable: false),
                    BASLIK = table.Column<string>(type: "text", nullable: false),
                    ICERIK = table.Column<string>(type: "text", nullable: false),
                    HABER_ID = table.Column<int>(type: "integer", nullable: false),
                    EKLEME_TARIHI = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false)
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
