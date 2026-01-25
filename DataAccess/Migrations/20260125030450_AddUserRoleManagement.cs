using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KULLANICI_ID",
                table: "YAZARLAR",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EKLEME_TARIHI",
                table: "HABERLER",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "KULLANICI_ROLLER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KULLANICI_ID = table.Column<int>(type: "integer", nullable: false),
                    ROL_ID = table.Column<int>(type: "integer", nullable: false),
                    ATANMA_TARIHI = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KULLANICI_ROLLER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KULLANICILAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SOYAD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EPOSTA = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SIFRE_HASH = table.Column<string>(type: "text", nullable: false),
                    RESIM = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false),
                    OLUSTURMA_TARIHI = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SON_GIRIS_TARIHI = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KULLANICILAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLLER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ROL_ADI = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ACIKLAMA = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLLER", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "ROLLER",
                columns: new[] { "ID", "ACIKLAMA", "AKTIF_MI", "ROL_ADI" },
                values: new object[,]
                {
                    { 1, "Sistem yöneticisi - tüm yetkiler", true, "Admin" },
                    { 2, "İçerik editörü - haber ve kategori yönetimi", true, "Editor" },
                    { 3, "Yazar - kendi haberlerini yönetir", true, "Yazar" },
                    { 4, "Moderatör - yorum moderasyonu", true, "Moderator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_YAZARLAR_KULLANICI_ID",
                table: "YAZARLAR",
                column: "KULLANICI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_KULLANICI_ROLLER_KULLANICI_ID_ROL_ID",
                table: "KULLANICI_ROLLER",
                columns: new[] { "KULLANICI_ID", "ROL_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KULLANICILAR_EPOSTA",
                table: "KULLANICILAR",
                column: "EPOSTA",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KULLANICI_ROLLER");

            migrationBuilder.DropTable(
                name: "KULLANICILAR");

            migrationBuilder.DropTable(
                name: "ROLLER");

            migrationBuilder.DropIndex(
                name: "IX_YAZARLAR_KULLANICI_ID",
                table: "YAZARLAR");

            migrationBuilder.DropColumn(
                name: "KULLANICI_ID",
                table: "YAZARLAR");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EKLEME_TARIHI",
                table: "HABERLER",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
