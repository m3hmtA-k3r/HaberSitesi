<div align="center">

# 🎭 MASKER - Modular Application System for Knowledge, Enterprise & Resources

### Cok Yonlu, Olceklenebilir ve Genisleyebilir Platform

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green.svg)](https://docs.microsoft.com/en-us/ef/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue.svg)](https://www.postgresql.org/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-orange.svg)](https://jwt.io/)
[![License](https://img.shields.io/badge/License-Proprietary-red.svg)](LICENSE)

**Icerik + Ticaret + Kullanici Yonetimini Birlestiren Kurumsal Platform**

[Kurulum](#-kurulum-ve-calistirma) • [Mimari](#️-proje-mimarisi) • [Guvenlik](#-guvenlik-ve-yetkilendirme) • [API](#-api-endpoints)

</div>

---

## 📖 Icindekiler

- [Proje Hakkinda](#-proje-hakkinda)
- [Platform Vizyonu](#-platform-vizyonu)
- [Temel Ozellikler](#-temel-ozellikler)
- [Proje Mimarisi](#️-proje-mimarisi)
- [Guvenlik ve Yetkilendirme](#-guvenlik-ve-yetkilendirme)
- [Admin Panel ve UI Kararlari](#-admin-panel-ve-ui-kararlari)
- [Teknolojiler](#️-kullanilan-teknolojiler)
- [Kurulum](#-kurulum-ve-calistirma)
- [Veritabani](#-veritabani-yapisi)
- [API](#-api-endpoints)
- [Lisans](#-lisans)

---

## 🎯 Proje Hakkinda

**MASKER (Modular Application System for Knowledge, Enterprise & Resources)**, ASP.NET Core 8.0 ile gelistirilmis, modern ve olceklenebilir bir kurumsal platformdur. Proje, **N-Katmanli Mimari (N-Tier Architecture)** ve **Clean Architecture** prensiplerine sadik kalarak, kurumsal duzeyde yazilim gelistirme standartlarina uygun olarak tasarlanmistir.

---

## 🚀 Platform Vizyonu

MASKER;

- **Tek amacli bir haber sitesi degil**
- **Moduler, olceklenebilir ve genisleyebilir**
- **Icerik + Ticaret + Kullanici yonetimini birlestiren**

**cok yonlu bir platform** olarak tasarlanmaktadir.

### Neden MASKER?

| Ozellik | Aciklama |
|---------|----------|
| **Moduler Yapi** | Her modul bagimsiz gelistirilebilir ve deploy edilebilir |
| **Olceklenebilirlik** | Yatay ve dikey olcekleme icin hazir altyapi |
| **Genisleyebilirlik** | Yeni moduller kolayca eklenebilir |
| **Kurumsal Hazirlik** | Rol bazli yetkilendirme ve audit log destegi |

Bu platform, sadece bir haber sitesi olmanin otesinde, icerik yonetimi, kullanici etkilesimi ve medya yonetimi icin kapsamli bir cozum sunar. Moduler yapisi sayesinde farkli icerik turlerine kolayca adapte edilebilir.

### 👨‍💻 Gelistirici

**Mehmet Asker**
- 🔗 GitHub: [@m3hmtA-k3r](https://github.com/m3hmtA-k3r)
- 📧 Proje Sahibi & Bas Gelistirici
- 📅 Gelistirme Baslangici: 2025

> *Bu proje bastan sona Mehmet Asker tarafindan tasarlanmis ve gelistirilmistir.*

---

## 🔐 Guvenlik ve Yetkilendirme

MASKER, kurumsal duzeyde guvenlik standartlarina uygun olarak tasarlanmistir.

### Mevcut Durum (Aktif)

| Ozellik | Teknoloji | Durum |
|---------|-----------|-------|
| **Sifre Hashleme** | BCrypt (WorkFactor: 12) | ✅ Aktif |
| **Token Tabanli Kimlik Dogrulama** | JWT (JSON Web Token) | ✅ Aktif |
| **Rol Bazli Yetkilendirme** | Claims-Based Authorization | ✅ Aktif |
| **Oturum Yonetimi** | Cookie Authentication + Session | ✅ Aktif |

### Yetkilendirme Mimarisi

```
┌─────────────────────────────────────────────────────────────┐
│                    KULLANICI GIRISI                         │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              1. Yeni Sistem (KULLANICILAR)                  │
│              - BCrypt sifre dogrulama                       │
│              - JWT token uretimi                            │
│              - Rol bilgisi claims'e eklenir                 │
└─────────────────────────────────────────────────────────────┘
                            │
                   Basarisiz ↓
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              2. Fallback (YAZARLAR - Legacy)                │
│              - Geriye donuk uyumluluk                       │
│              - Mevcut yazar hesaplari                       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              Cookie Authentication                          │
│              - HttpOnly cookie                              │
│              - 30 dakika timeout                            │
│              - Sliding expiration                           │
└─────────────────────────────────────────────────────────────┘
```

### Veritabani Tablolari

| Tablo | Aciklama |
|-------|----------|
| `KULLANICILAR` | Kullanici bilgileri (BCrypt hash ile) |
| `ROLLER` | Sistem rolleri (Admin, Editor, vb.) |
| `KULLANICI_ROLLER` | Kullanici-Rol iliskisi (Many-to-Many) |

### Gelecek Yetkilendirme Plani

Su an **rol bazli yetkilendirme aktiftir**. Ilerleyen asamalarda:

| Ozellik | Aciklama | Oncelik |
|---------|----------|---------|
| **Modul Bazli Yetkiler** | Her modul icin ayri yetki tanimlari | Yuksek |
| **Permission (Izin) Sistemi** | Granular izin kontrolu | Yuksek |
| **Rol-Modul-Aksiyon Matrisi** | Detayli erisim kontrol matrisi | Orta |
| **Audit Log** | Tum islemlerin kaydi | Orta |
| **IP Bazli Erisim** | Beyaz/kara liste yonetimi | Dusuk |

Bu yapi sayesinde sistem:
- ✅ Yeni modullerle sorunsuz genisleyebilir
- ✅ Kurumsal erisim kontrolu saglayabilir
- ✅ Fine-grained yetkilendirmeye hazir hale gelir

---

## 🧩 Admin Panel ve UI Kararlari

Admin panel tasariminda asagidaki prensipler benimsenmistir:

### Menu Yapisi

| Karar | Aciklama |
|-------|----------|
| **Kullanici Yonetimi** | Sol menuden kaldirildi, sag ustte "Yonetim" dropdown altina tasindi |
| **Profil Islemleri** | Sag ust dropdown menusunde |
| **Icerik Modulleri** | Sol menude (Haber, Slayt, Kategori) |
| **Kullanici Modulleri** | Sol menude (Yorum, Editor Yonetimi) |

### Rol Bazli Menu Gorunurlugu

```csharp
// Sadece Admin rolu gorebilir
@if (User.IsInRole("Admin"))
{
    <a href="/Kullanici">Kullanici Yonetimi</a>
}
```

### UI Kararlari

Bu kararlar:
- ✅ Panel karmasikligini azaltir
- ✅ Yetki bazli UI kontrolunu kolaylastirir
- ✅ Ileride eklenecek modullerle cakismayi onler
- ✅ Kullanici deneyimini iyilestirir

### Toast Bildirim Sistemi

Kullanici islemlerinde gorsel geri bildirim:
- 🟢 **Success**: Basarili islemler
- 🔴 **Error**: Hata durumlari
- 🟡 **Warning**: Uyari mesajlari
- 🔵 **Info**: Bilgilendirme

---

## ✨ Temel Ozellikler

### 📰 Haber Yönetimi
- ✅ **CRUD İşlemleri**: Haber ekleme, düzenleme, silme ve listeleme
- ✅ **Kategori Sistemi**: Çoklu kategori desteği ve hiyerarşik yapı
- ✅ **Görsel Yönetimi**: Haber görselleri yükleme ve düzenleme
- ✅ **İçerik Editörü**: Zengin metin editörü desteği
- ✅ **Yayın Durumu**: Taslak, yayında, arşiv durumları

### 👥 Kullanıcı Yönetimi
- ✅ **Yazar Profilleri**: Detaylı yazar profilleri ve biyografileri
- ✅ **Yetki Yönetimi**: Rol tabanlı erişim kontrolü
- ✅ **Yorum Sistemi**: Kullanıcı yorumları ve moderasyon
- ✅ **Admin Paneli**: Kapsamlı yönetim arayüzü

### 🎨 Medya ve Görsellik
- ✅ **Slider/Slayt Yönetimi**: Ana sayfa görsel slider'ı
- ✅ **Galeri Sistemi**: Çoklu görsel yükleme
- ✅ **Dosya Yönetimi**: Organize edilmiş medya kütüphanesi

### 🔧 Teknik Özellikler
- ✅ **RESTful API**: Eksiksiz API desteği
- ✅ **Repository Pattern**: Veri erişim katmanı soyutlaması
- ✅ **Dependency Injection**: IoC Container kullanımı
- ✅ **Entity Framework Core**: Code-First yaklaşımı
- ✅ **Migration Desteği**: Veritabanı versiyon kontrolü
- ✅ **DTO Pattern**: Veri transfer nesneleri
- ✅ **Responsive Tasarım**: Tüm cihazlarda uyumlu

---

## 🏗️ Proje Mimarisi

Proje, **N-Tier (Katmanli) Mimari** ve **Clean Architecture** prensiplerine gore yapilandirilmistir. Her katman, **SOLID** prensiplerine uygun olarak bagimsiz ve test edilebilir sekilde tasarlanmistir.

```
┌─────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                       │
│              (AdminUI, WebUI, ApiUI)                        │
│         Controllers, Views, ViewModels                      │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                    API ACCESS LAYER                         │
│                      (ApiAccess)                            │
│            HTTP Client, API Requests                        │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                    BUSINESS LAYER                           │
│                      (Business)                             │
│         Services, Managers, Business Rules                  │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                  INFRASTRUCTURE LAYER                       │
│                   (Infrastructure)                          │
│     Security (BCrypt), Identity (JWT), Caching, Storage     │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                   DATA ACCESS LAYER                         │
│                     (DataAccess)                            │
│         Repositories, DbContext, Migrations                 │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                     DOMAIN LAYER                            │
│                (Domain + Shared)                            │
│         Entities, DTOs, Interfaces, Helpers                 │
└─────────────────────────────────────────────────────────────┘
```

### 📦 Katman Detayları

#### 1️⃣ **Shared** - Ortak Katman
**Sorumluluk**: Tüm katmanlar tarafından kullanılan ortak yapılar

```
Shared/
├── Entities/          # Domain modelleri (Haberler, Kategoriler, vb.)
├── Dtos/             # Data Transfer Objects
└── Helpers/          # Utility sınıfları ve extension metodları
```

**İçerik**:
- `Haberler.cs` - Haber entity'si
- `Kategoriler.cs` - Kategori entity'si
- `Yazarlar.cs` - Yazar entity'si
- `Yorumlar.cs` - Yorum entity'si
- `Slaytlar.cs` - Slayt/slider entity'si

#### 2️⃣ **DataAccess** - Veri Erişim Katmanı
**Sorumluluk**: Veritabanı işlemleri ve veri kalıcılığı

```
DataAccess/
├── Context/
│   ├── HaberContext.cs        # DbContext sınıfı
│   └── HaberContextFactory.cs # Design-time factory
├── Abstract/
│   └── Repository/            # Repository interface'leri
├── Base/
│   └── Repository/            # Repository implementasyonları
└── Migrations/                # EF Core migration dosyaları
```

**Kullanılan Pattern'ler**:
- Repository Pattern
- Unit of Work Pattern
- Generic Repository

#### 3️⃣ **Business** - Is Katmani
**Sorumluluk**: Is mantigi ve business kurallari

```
Business/
├── Abstract/
│   ├── IHaberService.cs
│   ├── IKategoriService.cs
│   ├── IYazarService.cs
│   ├── IYorumService.cs
│   ├── ISlaytService.cs
│   ├── IAuthService.cs        # Kimlik dogrulama
│   ├── IKullaniciService.cs   # Kullanici yonetimi
│   └── IRolService.cs         # Rol yonetimi
└── Base/
    ├── HaberManager.cs
    ├── KategoriManager.cs
    ├── YazarManager.cs
    ├── YorumManager.cs
    ├── SlaytManager.cs
    ├── AuthManager.cs          # Login, profil, sifre islemleri
    ├── KullaniciManager.cs     # CRUD islemleri
    └── RolManager.cs           # Rol CRUD
```

**Ozellikler**:
- Veri validasyonu
- Is kurallarinin uygulanmasi
- Transaction yonetimi
- Loglama

#### 3.5️⃣ **Infrastructure** - Altyapi Katmani (YENİ)
**Sorumluluk**: Cross-cutting concerns ve teknik altyapi

```
Infrastructure/
├── Security/
│   ├── IPasswordHasher.cs      # Sifre hashleme interface
│   └── BCryptPasswordHasher.cs # BCrypt implementasyonu
├── Identity/
│   ├── IJwtTokenService.cs     # JWT interface
│   └── JwtTokenService.cs      # JWT token uretimi/dogrulama
├── Caching/
│   ├── ICacheService.cs        # Cache interface
│   └── InMemoryCacheService.cs # In-memory cache
└── Storage/
    ├── IFileStorageService.cs  # Dosya storage interface
    └── LocalFileStorageService.cs # Yerel dosya sistemi
```

**Ozellikler**:
- **BCrypt**: Guvenli sifre hashleme (WorkFactor: 12)
- **JWT**: Token tabanli kimlik dogrulama
- **Cache**: Performans optimizasyonu
- **Storage**: Dosya yukleme/indirme

#### 4️⃣ **ApiAccess** - API İstemci Katmanı
**Sorumluluk**: API'lere erişim için hazır servisler

```
ApiAccess/
├── Abstract/
│   ├── ICommonApiRequest.cs
│   ├── IHaberApiRequest.cs
│   └── ...
└── Base/
    ├── CommonApiRequest.cs
    ├── HaberApiRequest.cs
    └── ...
```

**Özellikler**:
- HTTP client wrapper
- API endpoint yönetimi
- Error handling
- Response deserialization

#### 5️⃣ **ApiUI** - RESTful API Katmanı
**Sorumluluk**: HTTP API endpoint'leri

```
ApiUI/
├── Controllers/
│   ├── HaberController.cs
│   ├── KategoriController.cs
│   ├── YazarController.cs
│   ├── YorumController.cs
│   └── SlaytController.cs
├── wwwroot/
│   └── Uploads/              # Yüklenen dosyalar
├── appsettings.json
└── Program.cs
```

**Özellikler**:
- RESTful API tasarımı
- Swagger/OpenAPI dokümantasyonu
- JWT Authentication (opsiyonel)
- CORS policy

#### 6️⃣ **AdminUI** - Yönetim Paneli
**Sorumluluk**: Admin kullanıcıları için yönetim arayüzü

```
AdminUI/
├── Controllers/
│   ├── AccountController.cs    # Giriş/çıkış
│   ├── HaberController.cs
│   ├── KategoriController.cs
│   ├── YazarController.cs
│   ├── YorumController.cs
│   └── SlaytController.cs
├── Models/
│   ├── LoginViewModel.cs
│   ├── HaberViewModel.cs
│   └── ...
├── Views/
│   ├── Shared/
│   ├── Haber/
│   ├── Kategori/
│   └── ...
└── wwwroot/
    ├── assets/
    └── lib/
```

**Özellikler**:
- MVC yapısı
- Authentication & Authorization
- Rich form validations
- AJAX işlemleri

#### 7️⃣ **WebUI** - Kullanıcı Arayüzü
**Sorumluluk**: Son kullanıcılar için web arayüzü

```
WebUI/
├── Controllers/
├── Models/
├── Views/
└── wwwroot/
```

**Özellikler**:
- Responsive tasarım
- SEO uyumlu
- Hızlı sayfa yükleme
- Kullanıcı dostu arayüz

---

## 🛠️ Kullanilan Teknolojiler

### Backend Framework & Libraries

| Teknoloji | Versiyon | Amac |
|-----------|----------|------|
| **ASP.NET Core** | 8.0 | Web framework |
| **C#** | 12.0 | Programlama dili |
| **Entity Framework Core** | 8.0 | ORM |
| **PostgreSQL** | 16 | Veritabani |
| **Razor Pages** | 8.0 | View engine |

### Guvenlik & Kimlik Dogrulama

| Teknoloji | Amac |
|-----------|------|
| **BCrypt.Net** | Sifre hashleme |
| **JWT (JSON Web Token)** | Token tabanli auth |
| **Cookie Authentication** | Oturum yonetimi |
| **Claims-Based Authorization** | Rol bazli yetkilendirme |

### Frontend Technologies

| Teknoloji | Amaç |
|-----------|------|
| **Bootstrap 5** | CSS framework |
| **jQuery** | JavaScript library |
| **Font Awesome** | İkonlar |
| **AJAX** | Asenkron işlemler |

### Development Tools

| Araç | Kullanım |
|------|----------|
| **Visual Studio 2022** | IDE |
| **VS Code** | Lightweight editor |
| **Git** | Version control |
| **Docker** | Containerization |
| **SQL Server Management Studio** | DB yönetimi |
| **Postman** | API testing |

### Design Patterns

- ✅ **Repository Pattern** - Veri erişim soyutlaması
- ✅ **Dependency Injection** - IoC Container
- ✅ **Factory Pattern** - Nesne oluşturma
- ✅ **DTO Pattern** - Veri transfer
- ✅ **MVC Pattern** - UI katmanı
- ✅ **Service Layer Pattern** - İş mantığı

---

## 🚀 Kurulum ve Çalıştırma

### 📋 Ön Gereksinimler

Projeyi çalıştırmak için sisteminizde aşağıdakiler kurulu olmalıdır:

- **.NET 8.0 SDK** veya üzeri ([İndir](https://dotnet.microsoft.com/download))
- **SQL Server 2022** (Express, LocalDB veya Developer Edition) ([İndir](https://www.microsoft.com/sql-server/sql-server-downloads))
- **Visual Studio 2022** veya **VS Code** ([İndir](https://visualstudio.microsoft.com/))
- **Git** ([İndir](https://git-scm.com/))

### 📥 Adım 1: Projeyi İndirin

```bash
# Projeyi klonlayın
git clone https://github.com/m3hmtA-k3r/HaberSitesi.git

# Proje dizinine gidin
cd HaberSitesi
```

### 🗄️ Adım 2: Veritabanını Oluşturun

**Yöntem 1: SQL Script ile (Önerilen)**

1. SQL Server Management Studio'yu açın
2. `database-scripts.txt` dosyasını açın
3. Scriptleri sırayla çalıştırın

**Yöntem 2: Entity Framework Migration ile**

```bash
# DataAccess projesine gidin
cd DataAccess

# Migration'ları uygulayın
dotnet ef database update

# Ana dizine dönün
cd ..
```

### ⚙️ Adım 3: Yapılandırma

Her projenin `appsettings.json` dosyasındaki connection string'i güncelleyin:

**AdminUI/appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HaberSitesi;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**ApiUI/appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HaberSitesi;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**WebUI/appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HaberSitesi;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> 💡 **Not**: `Server` parametresini kendi SQL Server instance'ınıza göre değiştirin:
> - LocalDB: `Server=(localdb)\\mssqllocaldb`
> - Lokal Server: `Server=localhost` veya `Server=.`
> - Uzak Server: `Server=192.168.1.100`

### 🔨 Adım 4: Projeyi Derleyin

```bash
# Tüm bağımlılıkları yükleyin
dotnet restore

# Solution'ı derleyin
dotnet build HaberSitesi.sln
```

### ▶️ Adım 5: Projeleri Çalıştırın

#### API Projesini Çalıştırma

```bash
cd ApiUI
dotnet run
```

🌐 API şu adreste çalışacaktır: `https://localhost:5001` (veya konsolda görünen port)

#### Admin Panelini Çalıştırma

```bash
cd AdminUI
dotnet run
```

🌐 Admin paneli şu adreste: `https://localhost:5002` (veya konsolda görünen port)

#### Kullanıcı Arayüzünü Çalıştırma

```bash
cd WebUI
dotnet run
```

🌐 Web sitesi şu adreste: `https://localhost:5003` (veya konsolda görünen port)

### 🐳 Docker ile Çalıştırma (Opsiyonel)

Proje, Dev Container desteğine sahiptir:

1. VS Code'da projeyi açın
2. **F1** > **Dev Containers: Reopen in Container** seçin
3. Container içinde proje otomatik olarak yapılandırılacaktır

---

## 💾 Veritabanı Yapısı

### 📊 Entity Relationship Diagram (ERD)

```
┌──────────────┐         ┌──────────────┐         ┌──────────────┐
│  Kategoriler │◄────┐   │   Haberler   │         │   Yazarlar   │
├──────────────┤     │   ├──────────────┤         ├──────────────┤
│ Id (PK)      │     └───┤ Id (PK)      │    ┌────┤ Id (PK)      │
│ Adi          │         │ Baslik       │    │    │ AdSoyad      │
│ Aciklama     │         │ Icerik       │◄───┘    │ Email        │
│ Aktif        │         │ KategoriId(FK)│        │ Telefon      │
│ ...          │         │ YazarId (FK) │         │ Biyografi    │
└──────────────┘         │ GorselUrl    │         │ ...          │
                         │ GoruntuSayisi│         └──────────────┘
                         │ Aktif        │
                         │ OlusturmaTrh │              │
                         │ ...          │              │
                         └──────────────┘              │
                                │                      │
                                │                      │
                                ▼                      │
                         ┌──────────────┐              │
                         │   Yorumlar   │              │
                         ├──────────────┤              │
                         │ Id (PK)      │              │
                         │ HaberId (FK) │──────────────┘
                         │ AdSoyad      │
                         │ Email        │
                         │ Yorum        │
                         │ Onaylandi    │
                         │ OlusturmaTrh │
                         │ ...          │
                         └──────────────┘

                         ┌──────────────┐
                         │   Slaytlar   │
                         ├──────────────┤
                         │ Id (PK)      │
                         │ Baslik       │
                         │ Aciklama     │
                         │ GorselUrl    │
                         │ Link         │
                         │ Sira         │
                         │ Aktif        │
                         │ ...          │
                         └──────────────┘
```

### 🗂️ Ana Tablolar

#### 📄 Haberler
Haber içeriklerini saklar.

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | int | Primary Key |
| Baslik | nvarchar(200) | Haber başlığı |
| Icerik | nvarchar(MAX) | Haber içeriği |
| Ozet | nvarchar(500) | Haber özeti |
| GorselUrl | nvarchar(500) | Görsel yolu |
| KategoriId | int | Foreign Key (Kategoriler) |
| YazarId | int | Foreign Key (Yazarlar) |
| GoruntuSayisi | int | Görüntülenme sayısı |
| Aktif | bit | Yayın durumu |
| OlusturmaTarihi | datetime | Oluşturulma tarihi |
| GuncellenmeTarihi | datetime | Güncellenme tarihi |

#### 📁 Kategoriler
Haber kategorilerini yönetir.

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | int | Primary Key |
| Adi | nvarchar(100) | Kategori adı |
| Aciklama | nvarchar(500) | Açıklama |
| Aktif | bit | Aktif/pasif durumu |
| Sira | int | Sıralama |

#### 👤 Yazarlar
Yazar profillerini saklar.

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | int | Primary Key |
| AdSoyad | nvarchar(100) | Ad soyad |
| Email | nvarchar(100) | E-posta |
| Telefon | nvarchar(20) | Telefon |
| Biyografi | nvarchar(1000) | Yazar biyografisi |
| ProfilFoto | nvarchar(500) | Profil fotoğrafı |
| Aktif | bit | Aktif/pasif durumu |

#### 💬 Yorumlar
Kullanıcı yorumlarını tutar.

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | int | Primary Key |
| HaberId | int | Foreign Key (Haberler) |
| AdSoyad | nvarchar(100) | Yorum yapan |
| Email | nvarchar(100) | E-posta |
| Yorum | nvarchar(1000) | Yorum içeriği |
| Onaylandi | bit | Onay durumu |
| OlusturmaTarihi | datetime | Oluşturulma tarihi |

#### 🖼️ Slaytlar
Ana sayfa slider gorsellerini yonetir.

| Kolon | Tip | Aciklama |
|-------|-----|----------|
| Id | int | Primary Key |
| Baslik | nvarchar(200) | Slayt basligi |
| Aciklama | nvarchar(500) | Aciklama |
| GorselUrl | nvarchar(500) | Gorsel yolu |
| Link | nvarchar(500) | Yonlendirilecek link |
| Sira | int | Gosterim sirasi |
| Aktif | bit | Aktif/pasif durumu |

### 🔐 Kullanici Yonetimi Tablolari (YENİ)

#### 👤 KULLANICILAR
Sistem kullanicilarini saklar (BCrypt sifre hash ile).

| Kolon | Tip | Aciklama |
|-------|-----|----------|
| ID | int | Primary Key |
| AD | varchar(100) | Kullanici adi |
| SOYAD | varchar(100) | Kullanici soyadi |
| EPOSTA | varchar(255) | E-posta (unique) |
| SIFRE_HASH | text | BCrypt hash |
| RESIM | varchar(500) | Profil resmi |
| AKTIF_MI | boolean | Aktif/pasif durumu |
| OLUSTURMA_TARIHI | timestamp | Olusturulma tarihi |
| SON_GIRIS_TARIHI | timestamp | Son giris tarihi |

#### 🎭 ROLLER
Sistem rollerini tanimlar.

| Kolon | Tip | Aciklama |
|-------|-----|----------|
| ID | int | Primary Key |
| ROL_ADI | varchar(50) | Rol adi (Admin, Editor, vb.) |
| ACIKLAMA | varchar(255) | Rol aciklamasi |
| AKTIF_MI | boolean | Aktif/pasif durumu |

#### 🔗 KULLANICI_ROLLER
Kullanici-Rol iliskisi (Many-to-Many).

| Kolon | Tip | Aciklama |
|-------|-----|----------|
| ID | int | Primary Key |
| KULLANICI_ID | int | Foreign Key (KULLANICILAR) |
| ROL_ID | int | Foreign Key (ROLLER) |
| ATANMA_TARIHI | timestamp | Rol atanma tarihi |

---

## 📡 API Endpoints

### 🔍 Swagger Dokümantasyonu

API'yi çalıştırdıktan sonra Swagger UI'ya erişin:

```
https://localhost:5001/swagger
```

### 📋 Endpoint Listesi

#### 🗞️ Haber API

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Haber` | Tüm haberleri listele | ❌ |
| GET | `/api/Haber/{id}` | ID'ye göre haber getir | ❌ |
| GET | `/api/Haber/Kategori/{kategoriId}` | Kategoriye göre haberler | ❌ |
| GET | `/api/Haber/Yazar/{yazarId}` | Yazara göre haberler | ❌ |
| POST | `/api/Haber` | Yeni haber ekle | ✅ |
| PUT | `/api/Haber/{id}` | Haber güncelle | ✅ |
| DELETE | `/api/Haber/{id}` | Haber sil | ✅ |

#### 📁 Kategori API

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Kategori` | Tüm kategorileri listele | ❌ |
| GET | `/api/Kategori/{id}` | ID'ye göre kategori getir | ❌ |
| POST | `/api/Kategori` | Yeni kategori ekle | ✅ |
| PUT | `/api/Kategori/{id}` | Kategori güncelle | ✅ |
| DELETE | `/api/Kategori/{id}` | Kategori sil | ✅ |

#### 👤 Yazar API

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Yazar` | Tüm yazarları listele | ❌ |
| GET | `/api/Yazar/{id}` | ID'ye göre yazar getir | ❌ |
| POST | `/api/Yazar` | Yeni yazar ekle | ✅ |
| PUT | `/api/Yazar/{id}` | Yazar güncelle | ✅ |
| DELETE | `/api/Yazar/{id}` | Yazar sil | ✅ |

#### 💬 Yorum API

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Yorum` | Tüm yorumları listele | ✅ |
| GET | `/api/Yorum/{id}` | ID'ye göre yorum getir | ✅ |
| GET | `/api/Yorum/Haber/{haberId}` | Habere ait yorumlar | ❌ |
| POST | `/api/Yorum` | Yeni yorum ekle | ❌ |
| PUT | `/api/Yorum/{id}` | Yorum güncelle | ✅ |
| DELETE | `/api/Yorum/{id}` | Yorum sil | ✅ |

#### 🖼️ Slayt API

| Method | Endpoint | Aciklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Slayt` | Tum slaytlari listele | ❌ |
| GET | `/api/Slayt/{id}` | ID'ye gore slayt getir | ❌ |
| POST | `/api/Slayt` | Yeni slayt ekle | ✅ |
| PUT | `/api/Slayt/{id}` | Slayt guncelle | ✅ |
| DELETE | `/api/Slayt/{id}` | Slayt sil | ✅ |

#### 🔐 Auth API (YENİ)

| Method | Endpoint | Aciklama | Auth |
|--------|----------|----------|------|
| POST | `/api/Auth/login` | Kullanici girisi, JWT token doner | ❌ |
| GET | `/api/Auth/profil` | Mevcut kullanici profili | ✅ |
| PUT | `/api/Auth/profil` | Profil guncelle | ✅ |
| POST | `/api/Auth/sifre-degistir` | Sifre degistir | ✅ |

#### 👥 Kullanici API (YENİ)

| Method | Endpoint | Aciklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Kullanici` | Tum kullanicilari listele | ✅ Admin |
| GET | `/api/Kullanici/{id}` | ID'ye gore kullanici getir | ✅ Admin |
| POST | `/api/Kullanici` | Yeni kullanici ekle | ✅ Admin |
| PUT | `/api/Kullanici/{id}` | Kullanici guncelle | ✅ Admin |
| DELETE | `/api/Kullanici/{id}` | Kullanici sil | ✅ Admin |
| POST | `/api/Kullanici/{id}/roller` | Kullaniciya rol ata | ✅ Admin |

#### 🎭 Rol API (YENİ)

| Method | Endpoint | Aciklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Rol` | Tum rolleri listele | ✅ Admin |
| GET | `/api/Rol/{id}` | ID'ye gore rol getir | ✅ Admin |
| POST | `/api/Rol` | Yeni rol ekle | ✅ Admin |
| PUT | `/api/Rol/{id}` | Rol guncelle | ✅ Admin |
| DELETE | `/api/Rol/{id}` | Rol sil | ✅ Admin |

### 📝 Örnek API Kullanımı

#### Tüm Haberleri Getir

```http
GET https://localhost:5001/api/Haber
Content-Type: application/json
```

**Response:**
```json
[
  {
    "id": 1,
    "baslik": "Örnek Haber Başlığı",
    "icerik": "Haber içeriği...",
    "gorselUrl": "/uploads/haber1.jpg",
    "kategoriAdi": "Teknoloji",
    "yazarAdi": "Mehmet Asker",
    "olusturmaTarihi": "2025-12-06T10:30:00"
  }
]
```

#### Yeni Haber Ekle

```http
POST https://localhost:5001/api/Haber
Content-Type: application/json
Authorization: Bearer {token}

{
  "baslik": "Yeni Haber Başlığı",
  "icerik": "Haber içeriği burada...",
  "ozet": "Kısa özet",
  "kategoriId": 1,
  "yazarId": 1,
  "aktif": true
}
```

---

## 🖼️ Ekran Görüntüleri

### 🏠 Ana Sayfa
> Modern ve kullanıcı dostu arayüz

### 📰 Haber Detay
> Zengin içerik gösterimi ve yorum sistemi

### 🔐 Admin Paneli
> Kapsamlı yönetim özellikleri

### 📊 Dashboard
> İstatistikler ve hızlı erişim

---

## 🤝 Katkıda Bulunma

Bu proje şu anda kişisel bir çalışmadır. Ancak katkılarınızı memnuniyetle karşılarım!

### Nasıl Katkıda Bulunabilirsiniz?

1. **Fork** edin (`https://github.com/m3hmtA-k3r/HaberSitesi/fork`)
2. Feature branch oluşturun (`git checkout -b feature/harika-ozellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: Harika özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/harika-ozellik`)
5. **Pull Request** açın

### Commit Mesaj Formatı

```
<tip>: <açıklama>

[opsiyonel gövde]

[opsiyonel footer]
```

**Tipler:**
- `feat`: Yeni özellik
- `fix`: Hata düzeltme
- `docs`: Dokümantasyon
- `style`: Kod formatı
- `refactor`: Kod yeniden yapılandırma
- `test`: Test ekleme/düzeltme
- `chore`: Genel bakım

---

## 🐛 Hata Bildirimi

Bir hata bulduğunuzda lütfen [GitHub Issues](https://github.com/m3hmtA-k3r/HaberSitesi/issues) üzerinden bildirin:

1. Hatanın detaylı açıklaması
2. Adım adım nasıl tekrar üretilebileceği
3. Beklenen davranış
4. Ekran görüntüleri (varsa)
5. Sistem bilgileri (OS, .NET version, vb.)

---

## 📚 Ek Kaynaklar

### 📖 Dokümantasyon
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core)
- [C# Documentation](https://docs.microsoft.com/dotnet/csharp)

### 🎓 Öğrenme Kaynakları
- [Microsoft Learn](https://docs.microsoft.com/learn)
- [.NET Blog](https://devblogs.microsoft.com/dotnet)

---

## 📄 Lisans

**© 2025 Mehmet Asker - Tüm Hakları Saklıdır**

Bu proje ve içeriği Mehmet Asker'e aittir. Ticari veya kişisel kullanım için izin alınması gerekmektedir.

---

## 📧 İletişim

**Mehmet Asker**

- 🔗 GitHub: [@m3hmtA-k3r](https://github.com/m3hmtA-k3r)
- 📧 E-posta: Proje sayfası üzerinden ulaşabilirsiniz
- 💼 LinkedIn: *Eklenecek*

---

## 🌟 Proje Durumu

![Status](https://img.shields.io/badge/Status-Aktif%20Gelistirme-success.svg)
![Maintenance](https://img.shields.io/badge/Maintenance-Evet-green.svg)
![Version](https://img.shields.io/badge/Version-2.0.0-blue.svg)
![Auth](https://img.shields.io/badge/Auth-JWT%20%2B%20BCrypt-orange.svg)

**Son Guncelleme:** 25 Ocak 2026

### Versiyon Gecmisi

| Versiyon | Tarih | Degisiklikler |
|----------|-------|---------------|
| **2.0.0** | 25 Ocak 2026 | JWT Auth, BCrypt, Rol sistemi, PostgreSQL |
| **1.5.0** | 20 Ocak 2026 | Admin panel modernizasyonu |
| **1.0.0** | 6 Aralik 2025 | Ilk surum |

---

## 🎯 Gelecek Planlari

### Yakin Gelecek (Q1 2026)
- [x] ~~JWT Authentication~~ ✅ Tamamlandi
- [x] ~~BCrypt sifre hashleme~~ ✅ Tamamlandi
- [x] ~~Rol bazli yetkilendirme~~ ✅ Tamamlandi
- [x] ~~PostgreSQL entegrasyonu~~ ✅ Tamamlandi
- [ ] Modul bazli yetkiler (Permission sistemi)
- [ ] Rol-Modul-Aksiyon matrisi
- [ ] Audit log sistemi
- [ ] Redis cache destegi

### Orta Vadeli (Q2 2026)
- [ ] Elasticsearch entegrasyonu
- [ ] SignalR ile canli bildirimler
- [ ] Advanced analytics dashboard
- [ ] Multi-language support
- [ ] E-ticaret modulu entegrasyonu
- [ ] SEO optimizasyonlari

### Uzun Vadeli
- [ ] Microservices mimarisi
- [ ] Cloud deployment (Azure/AWS)
- [ ] AI-powered icerik onerileri
- [ ] Mobile app (React Native / Flutter)
- [ ] Real-time collaboration tools
- [ ] Multi-tenant yapi

---

<div align="center">

### ⭐ Bu projeyi begendinizse yildiz vermeyi unutmayin!

**Made with ❤️ by [Mehmet Asker](https://github.com/m3hmtA-k3r)**

---

**MASKER** - Modular Application System for Knowledge, Enterprise & Resources

*Bu README, alinan mimari kararlarin ve yetkilendirme stratejisinin*
*bilincli, surdurulebilir ve muhendislik temelli oldugunu belgelemek amaciyla hazirlanmistir.*

---

[⬆ Basa Don](#-masker---modular-application-system-for-knowledge-enterprise--resources)

</div>