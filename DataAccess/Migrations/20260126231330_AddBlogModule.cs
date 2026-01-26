using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BLOG_KATEGORILER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ADI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ACIKLAMA = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SIRA = table.Column<int>(type: "integer", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false),
                    OLUSTURMA_TARIHI = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOG_KATEGORILER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BLOGLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BASLIK = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    OZET = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ICERIK = table.Column<string>(type: "text", nullable: false),
                    GORSEL_URL = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ETIKETLER = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    YAYIN_TARIHI = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OLUSTURMA_TARIHI = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GUNCELLENME_TARIHI = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false),
                    KATEGORI_ID = table.Column<int>(type: "integer", nullable: true),
                    YAZAR_ID = table.Column<int>(type: "integer", nullable: true),
                    GORUNTULEME_SAYISI = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOGLAR", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BLOGLAR_BLOG_KATEGORILER_KATEGORI_ID",
                        column: x => x.KATEGORI_ID,
                        principalTable: "BLOG_KATEGORILER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BLOGLAR_KULLANICILAR_YAZAR_ID",
                        column: x => x.YAZAR_ID,
                        principalTable: "KULLANICILAR",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BLOG_YORUMLAR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BLOG_ID = table.Column<int>(type: "integer", nullable: false),
                    AD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SOYAD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EPOSTA = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    YORUM = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ONAYLANDI_MI = table.Column<bool>(type: "boolean", nullable: false),
                    AKTIF_MI = table.Column<bool>(type: "boolean", nullable: false),
                    OLUSTURMA_TARIHI = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOG_YORUMLAR", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BLOG_YORUMLAR_BLOGLAR_BLOG_ID",
                        column: x => x.BLOG_ID,
                        principalTable: "BLOGLAR",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BLOG_YORUMLAR_BLOG_ID",
                table: "BLOG_YORUMLAR",
                column: "BLOG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BLOGLAR_AKTIF_MI",
                table: "BLOGLAR",
                column: "AKTIF_MI");

            migrationBuilder.CreateIndex(
                name: "IX_BLOGLAR_KATEGORI_ID",
                table: "BLOGLAR",
                column: "KATEGORI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BLOGLAR_YAZAR_ID",
                table: "BLOGLAR",
                column: "YAZAR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BLOG_YORUMLAR");

            migrationBuilder.DropTable(
                name: "BLOGLAR");

            migrationBuilder.DropTable(
                name: "BLOG_KATEGORILER");
        }
    }
}
