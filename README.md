<div align="center">

# 🎭 MASKER - Modular Application System for Knowledge, Enterprise & Resources

### Çok Yönlü, Ölçeklenebilir ve Genişleyebilir Platform

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green.svg)](https://docs.microsoft.com/en-us/ef/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue.svg)](https://www.postgresql.org/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-orange.svg)](https://jwt.io/)

**İçerik + Ticaret + Kullanıcı Yönetimini Birleştiren Kurumsal Platform**

[Hızlı Başlangıç](#-hızlı-başlangıç) • [Mimari](#️-proje-mimarisi) • [Özellikler](#-özellikler) • [Kurulum](#-kurulum)

</div>

---

## 📖 İçindekiler

- [Hızlı Başlangıç](#-hızlı-başlangıç)
- [Proje Hakkında](#-proje-hakkında)
- [Özellikler](#-özellikler)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [Erişim Adresleri](#-erişim-adresleri)
- [Proje Mimarisi](#️-proje-mimarisi)
- [Son Güncellemeler](#-son-güncellemeler-2026-01-31)
- [Sorun Giderme](#-sorun-giderme)

---

## ⚡ Hızlı Başlangıç

### Tek Komutla Tüm Servisleri Başlat

```bash
cd /home/ubuntu_user/projects/MASKER
./start-masker.sh
```

**Alternatif: Manuel Başlatma**

```bash
# Docker servisleri
docker-compose up -d

# .NET uygulamaları
export PATH="$HOME/.dotnet:$PATH"
cd ApiUI && nohup dotnet run --launch-profile http > /tmp/apiui.log 2>&1 &
cd ../AdminUI && nohup dotnet run --launch-profile http > /tmp/adminui.log 2>&1 &
cd ../WebUI && nohup dotnet run --urls "http://localhost:5167" > /tmp/webui.log 2>&1 &
```

### Servisleri Durdurma

```bash
./stop-masker.sh
```

---

## 🎯 Proje Hakkında

**MASKER** ASP.NET Core 8.0 ile geliştirilmiş, modern ve ölçeklenebilir bir kurumsal platformdur. **N-Tier Architecture** ve **Clean Architecture** prensiplerine uygun olarak tasarlanmıştır.

### Neden MASKER?

- ✅ **Modüler Yapı** - Her modül bağımsız geliştirilebilir
- ✅ **Ölçeklenebilir** - Yatay ve dikey ölçekleme hazır
- ✅ **Dinamik Menü Sistemi** - Veritabanı odaklı, rol bazlı menü
- ✅ **Güvenli** - BCrypt, JWT, rol bazlı yetkilendirme
- ✅ **Modern UI** - Responsive tasarım, smooth animasyonlar

---

## 🚀 Özellikler

### ✅ Tamamlanan Özellikler

#### 1. **Dinamik Menü Sistemi** ⭐ YENİ!
- Veritabanı odaklı menü yönetimi
- Rol bazlı erişim kontrolü
- Modern CSS stilleri ve animasyonlar
- Aktif sayfa vurgulama
- Accordion davranışı
- Badge desteği

**Menü Yapısı:**
```
🏠 Dashboard

📰 İçerik Yönetimi
   ├─ Haber Yönetimi
   ├─ Kategoriler
   ├─ Slayt Yönetimi
   └─ Yorumlar

✍️ Blog Yönetimi
   ├─ Blog Yazıları
   ├─ Blog Kategorileri
   └─ Blog Yorumları

👥 Kullanıcı İşlemleri
   ├─ Kullanıcı Yönetimi
   ├─ Yazarlar
   └─ Rol Yönetimi

⚙️ Sistem Ayarları
   ├─ Menü Yönetimi
   └─ İletişim Mesajları
```

#### 2. **İçerik Yönetimi**
- ✅ Haber CRUD işlemleri
- ✅ Kategori yönetimi
- ✅ Slayt/Slider yönetimi (Silme düzeltildi ⭐)
- ✅ Yorum moderasyonu
- ✅ Blog sistemi
- ✅ Medya yükleme

#### 3. **Kullanıcı ve Güvenlik**
- ✅ JWT Authentication
- ✅ BCrypt password hashing
- ✅ Rol bazlı yetkilendirme (Admin, Editor, Yazar, Moderator)
- ✅ Cookie-based sessions
- ✅ Kullanıcı yönetimi

#### 4. **Altyapı**
- ✅ PostgreSQL 16 veritabanı
- ✅ Redis cache desteği
- ✅ Docker containerization
- ✅ RESTful API (Swagger dokümantasyonu)
- ✅ Global exception handling
- ✅ Rate limiting

---

## 🛠 Kurulum ve Çalıştırma

### Gereksinimler

- .NET 8.0 SDK
- Docker Desktop
- PostgreSQL 16 (Docker ile gelir)
- Redis 7 (Docker ile gelir - opsiyonel)

### Kurulum Adımları

#### 1. Depoyu Klonlayın

```bash
git clone https://github.com/yourusername/MASKER.git
cd MASKER
```

#### 2. Docker Servislerini Başlatın

```bash
docker-compose up -d
```

Bu komut şunları başlatır:
- PostgreSQL (Port: 5432)
- pgAdmin (Port: 5050)
- Redis (Port: 6379)

#### 3. Veritabanını Oluşturun

Veritabanı otomatik olarak migration'larla oluşturulur. Manuel oluşturmak için:

```bash
cd DataAccess
dotnet ef database update
```

#### 4. Menü Yapısını Yükleyin

```bash
docker exec -i masker_postgres psql -U masker_admin -d MaskerDB < Database/Scripts/01_Menu_Structure_Setup.sql
```

#### 5. Uygulamaları Başlatın

```bash
./start-masker.sh
```

---

## 🌐 Erişim Adresleri

| Servis | Port | URL | Açıklama |
|--------|------|-----|----------|
| **WebUI** | 5167 | http://localhost:5167 | Kullanıcı arayüzü (Frontend) |
| **AdminUI** | 5251 | http://localhost:5251 | Yönetim paneli (Backoffice) |
| **API** | 5100 | http://localhost:5100 | REST API |
| **Swagger** | 5100 | http://localhost:5100/swagger | API dokümantasyonu |
| **pgAdmin** | 5050 | http://localhost:5050 | Veritabanı yönetimi |
| **PostgreSQL** | 5432 | localhost:5432 | Veritabanı sunucusu |
| **Redis** | 6379 | localhost:6379 | Cache sunucusu |

### Varsayılan Kullanıcılar

| Rol | E-posta | Şifre | Yetkiler |
|-----|---------|-------|----------|
| **Admin** | admin@masker.com | Admin123 | Tam yetki (Tüm modüller) |
| **Editor** | editor@masker.com | Editor123 | İçerik yönetimi |

**pgAdmin Girişi:**
- E-posta: admin@masker.com
- Şifre: MaskerAdmin2026!

---

## 🏗️ Proje Mimarisi

```
┌─────────────────────────────────────────────┐
│         PRESENTATION LAYER                  │
│     (AdminUI, WebUI, ApiUI)                 │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│          API ACCESS LAYER                   │
│           (ApiAccess)                       │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│         BUSINESS LAYER                      │
│     (Business + Application)                │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│       INFRASTRUCTURE LAYER                  │
│  (Security, Identity, Cache, Storage)       │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│       DATA ACCESS LAYER                     │
│  (DataAccess - Repositories, DbContext)     │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│          DOMAIN LAYER                       │
│     (Domain + Shared - Entities, DTOs)      │
└─────────────────────────────────────────────┘
```

### Katmanlar

- **Domain/Shared**: Entities, DTOs, Interfaces
- **DataAccess**: EF Core, Repositories, Migrations
- **Infrastructure**: Security, JWT, Cache, File Storage
- **Business**: Business Logic, Managers
- **Application**: CQRS, MediatR (opsiyonel)
- **ApiAccess**: HTTP Client wrappers
- **ApiUI**: REST API Controllers
- **AdminUI**: Backoffice MVC
- **WebUI**: Frontend MVC

---

## 📝 Son Güncellemeler (2026-01-31)

### ✅ Tamamlanan

#### 1. **Dinamik Menü Sistemi** ⭐
- Veritabanı tabloları oluşturuldu (MENULER, MENU_OGELERI, MENU_ROLLER, MENU_OGELERI_ROLLER)
- 4 menü grubu + 14 menü öğesi seed data eklendi
- MenuYonetim sayfası tam çalışır durumda
- Rol bazlı menü erişim kontrolü

#### 2. **UI/UX İyileştirmeleri** 🎨
- Modern CSS stilleri (aktif sayfa vurgusu, hover efektleri)
- JavaScript: Otomatik aktif sayfa algılama
- Accordion davranışı (tek menü açık)
- Badge desteği (bildirimler için hazır)
- Null-safe menü rendering

#### 3. **Bug Düzeltmeleri** 🐛
- ✅ Slayt silme sorunu (GET → DELETE metod düzeltmesi)
- ✅ Slayt güncelleme (POST → PUT metod düzeltmesi)
- ✅ MenuOgeRoller entity tablo adı (`MENU_OGE_ROLLER` → `MENU_OGELERI_ROLLER`)
- ✅ DinamikMenu null reference hatası
- ✅ API port çakışması

#### 4. **Veritabanı** 💾
- Menu sistemi SQL script'i: `Database/Scripts/01_Menu_Structure_Setup.sql`
- Otomatik rol atama
- Veritabanı ilişkileri düzgün çalışıyor

---

## 🔧 Sorun Giderme

### 1. Docker: "command not found"

**WSL2 Kullanıcıları:**
1. Docker Desktop'ı açın (Windows tarafında)
2. Settings > Resources > WSL Integration
3. Ubuntu toggle'ını aktif edin
4. Apply & Restart

### 2. Port Çakışması

```bash
# Port 5100'ü temizle (ApiUI)
lsof -ti:5100 | xargs kill -9

# Port 5251'i temizle (AdminUI)
lsof -ti:5251 | xargs kill -9

# Port 5167'yi temizle (WebUI)
lsof -ti:5167 | xargs kill -9
```

### 3. Menü Görünmüyor

```bash
# API'yi test et
curl http://localhost:5100/api/Menu/GetTumMenuYapisi

# Veritabanını kontrol et
docker exec masker_postgres psql -U masker_admin -d MaskerDB -c "SELECT COUNT(*) FROM \"MENULER\";"
```

Eğer menü verisi yoksa:
```bash
docker exec -i masker_postgres psql -U masker_admin -d MaskerDB < Database/Scripts/01_Menu_Structure_Setup.sql
```

### 4. NullReferenceException Hatası

Razor view cache'ini temizleyin:
```bash
rm -rf AdminUI/obj/Debug/net8.0/Razor
rm -rf AdminUI/bin/Debug/net8.0
./start-masker.sh
```

### 5. Log Dosyalarını İzleme

```bash
# ApiUI logs
tail -f /tmp/apiui.log

# AdminUI logs
tail -f /tmp/adminui.log

# WebUI logs
tail -f /tmp/webui.log
```

---

## 📚 Kullanım Kılavuzu

### Menü Yönetimi

1. Admin olarak giriş yapın: http://localhost:5251
2. Sağ üst "Yönetim" > "Menü Yönetimi"
3. Yeni menü grubu veya öğe ekleyin
4. Rol bazlı erişim ayarlayın
5. Sıralama ve ikonları düzenleyin

### İçerik Yönetimi

**Haber Ekleme:**
1. Sol menü > İçerik Yönetimi > Haber Yönetimi
2. "Yeni Haber" butonu
3. Başlık, içerik, kategori seçimi
4. Görseller yükleyin
5. Aktif/Pasif durumu ayarlayın

**Slayt Yönetimi:**
1. Sol menü > İçerik Yönetimi > Slayt Yönetimi
2. Mevcut slaytları görüntüleyin
3. Düzenle/Sil işlemleri yapın
4. Yeni slayt ekleyin

---

## 🛡️ Güvenlik

### Mevcut Güvenlik Özellikleri

- ✅ **BCrypt Password Hashing** (WorkFactor: 12)
- ✅ **JWT Authentication** (Token-based)
- ✅ **Role-Based Authorization** (4 rol seviyesi)
- ✅ **Cookie Authentication** (HttpOnly, Secure)
- ✅ **CORS Policy** (Yapılandırılabilir)
- ✅ **Rate Limiting** (Spam koruması)
- ✅ **Input Validation** (Model validation)
- ✅ **SQL Injection** koruması (EF Core parametreli sorgular)

### Güvenlik Yapılandırması

**JWT Secret (Gerekli):**
```bash
export MASKER_JWT_SECRET="your-super-secret-key-here-min-32-chars"
export MASKER_JWT_ISSUER="MaskerAPI"
export MASKER_JWT_AUDIENCE="MaskerClients"
```

**CORS:**
```bash
export MASKER_CORS_ORIGINS="http://localhost:5167,https://yourdomain.com"
```

---

## 📊 Veritabanı Yapısı

### Ana Tablolar

| Tablo | Açıklama | Kayıt Sayısı |
|-------|----------|--------------|
| HABERLER | Haber içerikleri | - |
| KATEGORILER | Haber kategorileri | - |
| SLAYTLAR | Ana sayfa slaytları | - |
| YORUMLAR | Haber yorumları | - |
| BLOGLAR | Blog yazıları | - |
| KULLANICILAR | Sistem kullanıcıları | 1 (admin) |
| ROLLER | Kullanıcı rolleri | 4 |
| **MENULER** ⭐ | Menü grupları | 4 |
| **MENU_OGELERI** ⭐ | Menü öğeleri | 14 |
| **MENU_ROLLER** ⭐ | Menü-rol ilişkisi | - |
| **MENU_OGELERI_ROLLER** ⭐ | Öğe-rol ilişkisi | - |

---

## 🎨 Teknolojiler

### Backend
- .NET 8.0
- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL 16
- Redis 7 (Cache)

### Frontend
- Razor Views
- Bootstrap 5
- Font Awesome 5
- jQuery
- Custom CSS/JS

### DevOps
- Docker & Docker Compose
- GitHub Actions (CI/CD hazır)
- Swagger/OpenAPI

---

## 👨‍💻 Geliştirici

**Mehmet Asker**
- GitHub: [@m3hmtA-k3r](https://github.com/m3hmtA-k3r)
- Proje Sahibi & Baş Geliştirici
- Başlangıç: 2025

---

## 📄 Lisans

Bu proje proprietary lisans altındadır. Tüm hakları saklıdır.

---

## 🆘 Destek

Sorun bildirmek veya öneride bulunmak için:
- GitHub Issues: [MASKER Issues](https://github.com/m3hmtA-k3r/MASKER/issues)
- E-posta: [İletişim]

---

<div align="center">

**⭐ MASKER - Kurumsal İçerik Yönetim Platformu**

Son Güncelleme: 31 Ocak 2026

</div>
