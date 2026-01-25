# MASKER Projesi - Durum Raporu
**Tarih:** 2026-01-25

## Servis Durumu

| Servis | Port | Durum |
|--------|------|-------|
| PostgreSQL 16 | 5432 | ✅ Çalışıyor (Docker) |
| pgAdmin | 5050 | ✅ Çalışıyor (Docker) |
| ApiUI | 5100 | ✅ Çalışıyor |
| AdminUI | 5251 | ✅ Çalışıyor |

## Veritabanı Bağlantı Bilgileri

```
Host: localhost
Port: 5432
Database: MaskerDB
User: masker_admin
Password: Masker2026!SecurePass
```

## pgAdmin Erişimi

```
URL: http://localhost:5050
Email: admin@masker.com
Password: MaskerAdmin2026!
```

## AdminUI Giriş Bilgileri

```
URL: http://localhost:5251/Account/Login
E-posta: admin@masker.com
Şifre: Admin123
```

## Veritabanı Tabloları

- KULLANICILAR
- ROLLER
- KULLANICI_ROLLER
- HABERLER
- KATEGORILER
- YAZARLAR
- YORUMLAR
- SLAYTLAR

## Kullanıcılar

| ID | Ad | Soyad | E-posta | Rol | Aktif |
|----|-----|-------|---------|-----|-------|
| 3 | Admin | User | admin@masker.com | Admin | ✅ |

## Roller

| ID | Rol Adı | Açıklama |
|----|---------|----------|
| 1 | Admin | Sistem yöneticisi - tüm yetkiler |
| 2 | Editor | İçerik editörü - haber ve kategori yönetimi |
| 3 | Yazar | Yazar - kendi haberlerini yönetir |
| 4 | Moderator | Moderatör - yorum moderasyonu |

## Çözülen Sorunlar

### 1. Login Sorunu (2026-01-25)
**Problem:** Login başarılı olmasına rağmen kullanıcı ana sayfaya yönlendirilmiyordu.

**Neden:**
- `ClaimsIdentity` authentication type cookie scheme ile eşleşmiyordu
- `RedirectToAction("Index", "Home")` beklendiği gibi çalışmıyordu

**Çözüm:**
1. `AccountController.cs`'de `CookieAuthenticationDefaults.AuthenticationScheme` kullanıldı
2. `SignInAsync` metoduna authentication scheme eklendi
3. `RedirectToAction` yerine `Redirect("/Home/Index")` kullanıldı

## Servisleri Başlatma Komutları

```bash
# .NET PATH ayarı
export PATH="$HOME/.dotnet:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"

# Docker servisleri (PostgreSQL + pgAdmin)
cd /home/ubuntu_user/projects/MASKER
docker-compose up -d

# ApiUI başlatma
cd /home/ubuntu_user/projects/MASKER/ApiUI
nohup dotnet run --launch-profile http > /tmp/apiui.log 2>&1 &

# AdminUI başlatma
cd /home/ubuntu_user/projects/MASKER/AdminUI
nohup dotnet run --launch-profile http > /tmp/adminui.log 2>&1 &
```

## Servisleri Durdurma Komutları

```bash
# .NET uygulamalarını durdur
pkill -f "dotnet.*ApiUI"
pkill -f "dotnet.*AdminUI"

# Docker servislerini durdur
cd /home/ubuntu_user/projects/MASKER
docker-compose down
```

## Proje Yapısı

```
MASKER/
├── AdminUI/          # Admin paneli (MVC)
├── ApiUI/            # REST API
├── ApiAccess/        # API erişim katmanı
├── Business/         # İş mantığı
├── DataAccess/       # Veritabanı erişim katmanı
├── Domain/           # Entity'ler
├── Application/      # DTO'lar
├── Infrastructure/   # Altyapı (Security, Identity)
├── Shared/           # Ortak kod
└── docker-compose.yml
```
