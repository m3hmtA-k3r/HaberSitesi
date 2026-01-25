# MASKER AdminUI - Oturum Raporu

## Tarih: 2026-01-25

---

## 1. Yapilan Islemler Ozeti

### 1.1 Yazar Login Sorunu Analizi ve Cozumu

**Sorun:**
- `admin@masker.com` / `Admin2026!` ile giris yapilamiyordu
- Login sayfasi refresh olup ayni sayfada kaliyordu

**Yapilan Analiz:**
1. API endpoint'leri test edildi:
   - `Auth/login` → Kullanici var ama sifre uyusmuyor (BCrypt ile "Admin123" kayitli)
   - `Yazar/GetYazarByEmailPassword` → Dogru calisiyor, kullanici donuyor

2. Login akisina debug log eklendi:
   ```
   [LOGIN DEBUG] Auth result: null
   [LOGIN DEBUG] Yazar result: Id=1, Ad=Mehmet
   ```

3. Sonuc: Yazar fallback mekanizmasi duzgun calisiyordu, sorun eski/bozuk servis durumundan kaynaklaniyordu

**Cozum:**
- AdminUI servisi yeniden baslatildi
- Debug loglari temizlendi
- Login artik duzgun calisiyor

---

## 2. Mevcut Sistem Durumu

### 2.1 Calisan Servisler

| Servis | Port | Durum |
|--------|------|-------|
| PostgreSQL (Docker) | 5432 | Calisiyor |
| pgAdmin (Docker) | 5050 | Calisiyor |
| ApiUI (.NET) | 5100 | Calisiyor |
| AdminUI (.NET) | 5251 | Calisiyor |

### 2.2 Kullanici Sistemleri

**Iki ayri kullanici sistemi mevcut:**

| Sistem | Tablo | Sifreleme | Rol Destegi |
|--------|-------|-----------|-------------|
| Yeni Sistem (Kullanici Yonetimi) | KULLANICILAR | BCrypt | Var |
| Eski Sistem (Editor Yonetimi) | YAZARLAR | Plain Text | Yok |

---

## 3. Kullanici Bilgileri

### 3.1 Admin Kullanicisi (Yeni Sistem)
```
E-posta : admin@masker.com
Sifre   : Admin123
Rol     : Admin
```
→ "Kullanici Yonetimi" menusune erisebilir

### 3.2 Yazar Kullanicisi (Eski Sistem)
```
E-posta : admin@masker.com
Sifre   : Admin2026!
Ad/Soyad: Mehmet Masker
```
→ Sadece icerik yonetimi yapabilir, Admin menusune erisemez

---

## 4. Login Akisi

```
+----------------------------------------------------------+
|                    GirisYap POST                          |
+----------------------------------------------------------+
                          |
                          v
+----------------------------------------------------------+
|         1. Auth/login API'yi dene (Yeni Sistem)          |
|         - BCrypt ile sifre kontrolu                       |
|         - JWT token olusturma                             |
+----------------------------------------------------------+
                          |
              Basarili?   |
           +--------------+--------------+
           | EVET                        | HAYIR
           v                             v
+----------------------+    +------------------------------+
| JWT Token kaydet     |    | 2. Yazar API'yi dene         |
| Claims olustur       |    |    (Eski Sistem - Fallback)  |
| Cookie set et        |    |    - Plain text sifre        |
| /Home/Index'e git    |    +------------------------------+
+----------------------+                 |
                                Basarili?|
                             +-----------+-----------+
                             | EVET                  | HAYIR
                             v                       v
                  +------------------+    +------------------+
                  | Claims olustur   |    | Hata mesaji      |
                  | Cookie set et    |    | Login sayfasinda |
                  | /Home/Index'e git|    | kal              |
                  +------------------+    +------------------+
```

---

## 5. Dosya Degisiklikleri

### Bu Oturumda Degistirilen Dosyalar:

| Dosya | Degisiklik |
|-------|------------|
| `AdminUI/Controllers/AccountController.cs` | Debug log eklendi ve temizlendi (net degisiklik yok) |

**Not:** Kalici kod degisikligi yapilmadi. Sadece servis restart ile sorun cozuldu.

---

## 6. Erisim URL'leri

| Sayfa | URL |
|-------|-----|
| Login | http://localhost:5251/Account/Login |
| Ana Sayfa | http://localhost:5251/Home/Index |
| Profil | http://localhost:5251/Account/Profil |
| Kullanici Yonetimi | http://localhost:5251/Kullanici (Sadece Admin) |
| pgAdmin | http://localhost:5050 |

---

## 7. Oneriler

1. **Kullanici Sistemlerinin Birlestirilmesi:** Iki ayri kullanici sistemi (Yazar/Kullanici) karisikliga neden olabilir. Ileride tek sisteme gecis dusunulebilir.

2. **Yazar Sifreleri:** YAZARLAR tablosunda sifreler plain text olarak saklaniyor. Guvenlik icin BCrypt'e gecis onerilir.

3. **Session Yonetimi:** Servis restart sonrasi tum oturumlar sonlaniyor. Production ortaminda distributed cache (Redis) kullanilabilir.

---

**Rapor Sonu**
