-- ============================================
-- MASKER - Menu Yapısı Kurulum Script'i
-- ============================================
-- Bu script menü tablolarını oluşturur ve
-- modüler menü yapısını veritabanına ekler
-- ============================================

-- 1. MENULER Tablosu - Menü Grupları
CREATE TABLE IF NOT EXISTS "MENULER" (
    "ID" SERIAL PRIMARY KEY,
    "ADI" VARCHAR(100) NOT NULL,
    "IKON" VARCHAR(100),
    "SIRA" INTEGER NOT NULL DEFAULT 0,
    "AKTIF_MI" BOOLEAN NOT NULL DEFAULT true,
    "COLLAPSE_ID" VARCHAR(50)
);

-- 2. MENU_OGELERI Tablosu - Menü Öğeleri
CREATE TABLE IF NOT EXISTS "MENU_OGELERI" (
    "ID" SERIAL PRIMARY KEY,
    "MENU_ID" INTEGER,
    "ADI" VARCHAR(100) NOT NULL,
    "URL" VARCHAR(255),
    "IKON" VARCHAR(100),
    "SIRA" INTEGER NOT NULL DEFAULT 0,
    "AKTIF_MI" BOOLEAN NOT NULL DEFAULT true,
    FOREIGN KEY ("MENU_ID") REFERENCES "MENULER"("ID") ON DELETE CASCADE
);

-- 3. MENU_ROLLER Junction Table - Menü-Rol İlişkisi
CREATE TABLE IF NOT EXISTS "MENU_ROLLER" (
    "ID" SERIAL PRIMARY KEY,
    "MENU_ID" INTEGER NOT NULL,
    "ROL_ID" INTEGER NOT NULL,
    FOREIGN KEY ("MENU_ID") REFERENCES "MENULER"("ID") ON DELETE CASCADE,
    FOREIGN KEY ("ROL_ID") REFERENCES "ROLLER"("ID") ON DELETE CASCADE,
    UNIQUE ("MENU_ID", "ROL_ID")
);

-- 4. MENU_OGELERI_ROLLER Junction Table - Menü Öğesi-Rol İlişkisi
CREATE TABLE IF NOT EXISTS "MENU_OGELERI_ROLLER" (
    "ID" SERIAL PRIMARY KEY,
    "MENU_OGE_ID" INTEGER NOT NULL,
    "ROL_ID" INTEGER NOT NULL,
    FOREIGN KEY ("MENU_OGE_ID") REFERENCES "MENU_OGELERI"("ID") ON DELETE CASCADE,
    FOREIGN KEY ("ROL_ID") REFERENCES "ROLLER"("ID") ON DELETE CASCADE,
    UNIQUE ("MENU_OGE_ID", "ROL_ID")
);

-- ============================================
-- Mevcut verileri temizle (varsa)
-- ============================================
TRUNCATE TABLE "MENU_OGELERI_ROLLER" CASCADE;
TRUNCATE TABLE "MENU_ROLLER" CASCADE;
TRUNCATE TABLE "MENU_OGELERI" CASCADE;
TRUNCATE TABLE "MENULER" CASCADE;

-- Sequence'leri sıfırla
ALTER SEQUENCE "MENULER_ID_seq" RESTART WITH 1;
ALTER SEQUENCE "MENU_OGELERI_ID_seq" RESTART WITH 1;

-- ============================================
-- Admin ve Editor Rol ID'lerini al
-- ============================================
DO $$
DECLARE
    admin_rol_id INTEGER;
    editor_rol_id INTEGER;
BEGIN
    -- Rol ID'lerini bul
    SELECT "ID" INTO admin_rol_id FROM "ROLLER" WHERE "ROL_ADI" = 'Admin' LIMIT 1;
    SELECT "ID" INTO editor_rol_id FROM "ROLLER" WHERE "ROL_ADI" = 'Editor' LIMIT 1;

    -- Eğer roller yoksa hata ver
    IF admin_rol_id IS NULL THEN
        RAISE EXCEPTION 'Admin rolü bulunamadı!';
    END IF;

    -- ============================================
    -- BAĞIMSIZ MENÜ ÖĞELERİ (Dashboard)
    -- ============================================

    INSERT INTO "MENU_OGELERI" ("MENU_ID", "ADI", "URL", "IKON", "SIRA", "AKTIF_MI")
    VALUES (NULL, 'Dashboard', '/Home/Index', 'fas fa-tachometer-alt', 1, true);

    -- Dashboard tüm rollere açık
    INSERT INTO "MENU_OGELERI_ROLLER" ("MENU_OGE_ID", "ROL_ID")
    SELECT currval('"MENU_OGELERI_ID_seq"'), "ID" FROM "ROLLER";

    -- ============================================
    -- MENÜ GRUBU 1: İÇERİK YÖNETİMİ
    -- ============================================

    INSERT INTO "MENULER" ("ADI", "IKON", "SIRA", "AKTIF_MI", "COLLAPSE_ID")
    VALUES ('İçerik Yönetimi', 'fas fa-newspaper', 1, true, 'menuIcerik');

    -- İçerik Yönetimi tüm rollere açık
    INSERT INTO "MENU_ROLLER" ("MENU_ID", "ROL_ID")
    SELECT currval('"MENULER_ID_seq"'), "ID" FROM "ROLLER";

    -- Alt menü öğeleri
    INSERT INTO "MENU_OGELERI" ("MENU_ID", "ADI", "URL", "IKON", "SIRA", "AKTIF_MI")
    VALUES
        (currval('"MENULER_ID_seq"'), 'Haber Yönetimi', '/Haber', 'fas fa-file-alt', 1, true),
        (currval('"MENULER_ID_seq"'), 'Kategoriler', '/Kategori', 'fas fa-folder-open', 2, true),
        (currval('"MENULER_ID_seq"'), 'Slayt Yönetimi', '/Slayt', 'fas fa-images', 3, true),
        (currval('"MENULER_ID_seq"'), 'Yorumlar', '/Yorum', 'fas fa-comments', 4, true);

    -- İçerik yönetimi alt öğeleri tüm rollere açık
    INSERT INTO "MENU_OGELERI_ROLLER" ("MENU_OGE_ID", "ROL_ID")
    SELECT mo."ID", r."ID"
    FROM "MENU_OGELERI" mo
    CROSS JOIN "ROLLER" r
    WHERE mo."MENU_ID" = currval('"MENULER_ID_seq"');

    -- ============================================
    -- MENÜ GRUBU 2: BLOG YÖNETİMİ
    -- ============================================

    INSERT INTO "MENULER" ("ADI", "IKON", "SIRA", "AKTIF_MI", "COLLAPSE_ID")
    VALUES ('Blog Yönetimi', 'fas fa-blog', 2, true, 'menuBlog');

    -- Blog Yönetimi tüm rollere açık
    INSERT INTO "MENU_ROLLER" ("MENU_ID", "ROL_ID")
    SELECT currval('"MENULER_ID_seq"'), "ID" FROM "ROLLER";

    -- Alt menü öğeleri
    INSERT INTO "MENU_OGELERI" ("MENU_ID", "ADI", "URL", "IKON", "SIRA", "AKTIF_MI")
    VALUES
        (currval('"MENULER_ID_seq"'), 'Blog Yazıları', '/Blog', 'fas fa-pen-fancy', 1, true),
        (currval('"MENULER_ID_seq"'), 'Blog Kategorileri', '/BlogKategori', 'fas fa-tags', 2, true),
        (currval('"MENULER_ID_seq"'), 'Blog Yorumları', '/BlogYorum', 'fas fa-comment-dots', 3, true);

    -- Blog yönetimi alt öğeleri tüm rollere açık
    INSERT INTO "MENU_OGELERI_ROLLER" ("MENU_OGE_ID", "ROL_ID")
    SELECT mo."ID", r."ID"
    FROM "MENU_OGELERI" mo
    CROSS JOIN "ROLLER" r
    WHERE mo."MENU_ID" = currval('"MENULER_ID_seq"');

    -- ============================================
    -- MENÜ GRUBU 3: KULLANICI İŞLEMLERİ
    -- ============================================

    INSERT INTO "MENULER" ("ADI", "IKON", "SIRA", "AKTIF_MI", "COLLAPSE_ID")
    VALUES ('Kullanıcı İşlemleri', 'fas fa-users', 3, true, 'menuKullanici');

    -- Kullanıcı İşlemleri tüm rollere açık
    INSERT INTO "MENU_ROLLER" ("MENU_ID", "ROL_ID")
    SELECT currval('"MENULER_ID_seq"'), "ID" FROM "ROLLER";

    -- Alt menü öğeleri
    INSERT INTO "MENU_OGELERI" ("MENU_ID", "ADI", "URL", "IKON", "SIRA", "AKTIF_MI")
    VALUES
        (currval('"MENULER_ID_seq"'), 'Kullanıcı Yönetimi', '/Kullanici', 'fas fa-user-friends', 1, true),
        (currval('"MENULER_ID_seq"'), 'Yazarlar', '/Yazar', 'fas fa-user-edit', 2, true),
        (currval('"MENULER_ID_seq"'), 'Rol Yönetimi', '/Rol', 'fas fa-user-shield', 3, true);

    -- Kullanıcı Yönetimi ve Rol Yönetimi sadece Admin'e açık
    INSERT INTO "MENU_OGELERI_ROLLER" ("MENU_OGE_ID", "ROL_ID")
    SELECT mo."ID", admin_rol_id
    FROM "MENU_OGELERI" mo
    WHERE mo."MENU_ID" = currval('"MENULER_ID_seq"')
    AND mo."ADI" IN ('Kullanıcı Yönetimi', 'Rol Yönetimi');

    -- Yazarlar tüm rollere açık
    INSERT INTO "MENU_OGELERI_ROLLER" ("MENU_OGE_ID", "ROL_ID")
    SELECT mo."ID", r."ID"
    FROM "MENU_OGELERI" mo
    CROSS JOIN "ROLLER" r
    WHERE mo."MENU_ID" = currval('"MENULER_ID_seq"')
    AND mo."ADI" = 'Yazarlar';

    -- ============================================
    -- MENÜ GRUBU 4: SİSTEM AYARLARI (Sadece Admin)
    -- ============================================

    INSERT INTO "MENULER" ("ADI", "IKON", "SIRA", "AKTIF_MI", "COLLAPSE_ID")
    VALUES ('Sistem Ayarları', 'fas fa-cogs', 4, true, 'menuSistem');

    -- Sistem Ayarları sadece Admin'e açık
    INSERT INTO "MENU_ROLLER" ("MENU_ID", "ROL_ID")
    VALUES (currval('"MENULER_ID_seq"'), admin_rol_id);

    -- Alt menü öğeleri
    INSERT INTO "MENU_OGELERI" ("MENU_ID", "ADI", "URL", "IKON", "SIRA", "AKTIF_MI")
    VALUES
        (currval('"MENULER_ID_seq"'), 'Menü Yönetimi', '/MenuYonetim', 'fas fa-bars', 1, true),
        (currval('"MENULER_ID_seq"'), 'İletişim Mesajları', '/Iletisim', 'fas fa-envelope', 2, true),
        (currval('"MENULER_ID_seq"'), 'Sistem Logları', '/Logs', 'fas fa-file-medical-alt', 3, false);

    -- Sistem ayarları tüm alt öğeleri sadece Admin'e açık
    INSERT INTO "MENU_OGELERI_ROLLER" ("MENU_OGE_ID", "ROL_ID")
    SELECT mo."ID", admin_rol_id
    FROM "MENU_OGELERI" mo
    WHERE mo."MENU_ID" = currval('"MENULER_ID_seq"');

END $$;

-- ============================================
-- Verification - Oluşturulan yapıyı göster
-- ============================================

SELECT 'MENULER Tablosu:' as "Tablo";
SELECT * FROM "MENULER" ORDER BY "SIRA";

SELECT 'MENU_OGELERI Tablosu:' as "Tablo";
SELECT
    mo."ID",
    COALESCE(m."ADI", 'Bağımsız') as "MENU_GRUBU",
    mo."ADI" as "MENU_ADI",
    mo."URL",
    mo."IKON",
    mo."SIRA",
    mo."AKTIF_MI"
FROM "MENU_OGELERI" mo
LEFT JOIN "MENULER" m ON mo."MENU_ID" = m."ID"
ORDER BY m."SIRA" NULLS FIRST, mo."SIRA";

SELECT '✅ Menü yapısı başarıyla oluşturuldu!' as "Durum";
