<div align="center">

# 🗞️ Haber Sitesi Yönetim Sistemi

### Modern ve Kapsamlı Haber Platformu

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green.svg)](https://docs.microsoft.com/en-us/ef/core/)
[![License](https://img.shields.io/badge/License-Proprietary-red.svg)](LICENSE)

**Profesyonel bir haber sitesi için eksiksiz çözüm**

[Kurulum](#-kurulum-ve-çalıştırma) • [Mimari](#️-proje-mimarisi) • [Özellikler](#-temel-özellikler) • [Dokümantasyon](#-api-endpoints)

</div>

---

## 📖 İçindekiler

- [Proje Hakkında](#-proje-hakkında)
- [Temel Özellikler](#-temel-özellikler)
- [Proje Mimarisi](#️-proje-mimarisi)
- [Teknolojiler](#️-kullanılan-teknolojiler)
- [Kurulum](#-kurulum-ve-çalıştırma)
- [Veritabanı](#-veritabanı-yapısı)
- [API](#-api-endpoints)
- [Ekran Görüntüleri](#-ekran-görüntüleri)
- [Lisans](#-lisans)

---

## 🎯 Proje Hakkında

**Haber Sitesi Yönetim Sistemi**, ASP.NET Core 8.0 ile geliştirilmiş, modern ve ölçeklenebilir bir haber platformudur. Proje, **N-Katmanlı Mimari (N-Tier Architecture)** prensiplerine sadık kalarak, kurumsal düzeyde yazılım geliştirme standartlarına uygun olarak tasarlanmıştır.

### 💡 Proje Vizyonu

Bu platform, sadece bir haber sitesi olmanın ötesinde, içerik yönetimi, kullanıcı etkileşimi ve medya yönetimi için kapsamlı bir çözüm sunar. Modüler yapısı sayesinde farklı içerik türlerine kolayca adapte edilebilir.

### 👨‍💻 Geliştirici

**Mehmet Asker**
- 🔗 GitHub: [@m3hmtA-k3r](https://github.com/m3hmtA-k3r)
- 📧 Proje Sahibi & Baş Geliştirici
- 📅 Geliştirme Başlangıcı: 2025

> *Bu proje baştan sona Mehmet Asker tarafından tasarlanmış ve geliştirilmiştir.*

---

## ✨ Temel Özellikler

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

Proje, **N-Tier (Katmanlı) Mimari** prensiplerine göre yapılandırılmıştır. Her katman, **SOLID** prensiplerine uygun olarak bağımsız ve test edilebilir şekilde tasarlanmıştır.

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│                 (AdminUI, WebUI, ApiUI)                 │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                    API Access Layer                      │
│                    (ApiAccess)                          │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                    Business Layer                        │
│                    (Business)                           │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                 Data Access Layer                        │
│                   (DataAccess)                          │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                   Shared Layer                           │
│            (Entities, DTOs, Helpers)                    │
└─────────────────────────────────────────────────────────┘
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

#### 3️⃣ **Business** - İş Katmanı
**Sorumluluk**: İş mantığı ve business kuralları

```
Business/
├── Abstract/
│   ├── IHaberService.cs
│   ├── IKategoriService.cs
│   ├── IYazarService.cs
│   ├── IYorumService.cs
│   └── ISlaytService.cs
└── Base/
    ├── HaberManager.cs
    ├── KategoriManager.cs
    ├── YazarManager.cs
    ├── YorumManager.cs
    └── SlaytManager.cs
```

**Özellikler**:
- Veri validasyonu
- İş kurallarının uygulanması
- Transaction yönetimi
- Loglama

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

## 🛠️ Kullanılan Teknolojiler

### Backend Framework & Libraries

| Teknoloji | Versiyon | Amaç |
|-----------|----------|------|
| **ASP.NET Core** | 8.0 | Web framework |
| **C#** | 12.0 | Programlama dili |
| **Entity Framework Core** | 8.0 | ORM |
| **SQL Server** | 2022 | Veritabanı |
| **Razor Pages** | 8.0 | View engine |

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
Ana sayfa slider görsellerini yönetir.

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | int | Primary Key |
| Baslik | nvarchar(200) | Slayt başlığı |
| Aciklama | nvarchar(500) | Açıklama |
| GorselUrl | nvarchar(500) | Görsel yolu |
| Link | nvarchar(500) | Yönlendirilecek link |
| Sira | int | Gösterim sırası |
| Aktif | bit | Aktif/pasif durumu |

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

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Slayt` | Tüm slaytları listele | ❌ |
| GET | `/api/Slayt/{id}` | ID'ye göre slayt getir | ❌ |
| POST | `/api/Slayt` | Yeni slayt ekle | ✅ |
| PUT | `/api/Slayt/{id}` | Slayt güncelle | ✅ |
| DELETE | `/api/Slayt/{id}` | Slayt sil | ✅ |

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

![Status](https://img.shields.io/badge/Status-Aktif%20Geli%C5%9Ftirme-success.svg)
![Maintenance](https://img.shields.io/badge/Maintenance-Evet-green.svg)
![Version](https://img.shields.io/badge/Version-1.0.0-blue.svg)

**Son Güncelleme:** 6 Aralık 2025

---

## 🎯 Gelecek Planları

### Yakın Gelecek (Q1 2026)
- [ ] Elasticsearch entegrasyonu
- [ ] Redis cache desteği
- [ ] JWT Authentication
- [ ] SignalR ile canlı bildirimler

### Orta Vadeli (Q2 2026)
- [ ] Mobile app (React Native)
- [ ] Advanced analytics dashboard
- [ ] Multi-language support
- [ ] SEO optimizasyonları

### Uzun Vadeli
- [ ] Microservices mimarisi
- [ ] Cloud deployment (Azure)
- [ ] AI-powered content recommendations
- [ ] Real-time collaboration tools

---

<div align="center">

### ⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!

**Made with ❤️ by [Mehmet Asker](https://github.com/m3hmtA-k3r)**

---

[⬆ Başa Dön](#️-haber-sitesi-yönetim-sistemi)

</div>