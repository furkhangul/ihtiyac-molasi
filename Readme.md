# İhtiyaç Molası — Anlık Mahalle Yardım Ağı

Mahalle sakinlerinin ilaç, market alışverişi, çocuk alma ve ulaşım gibi anlık küçük yardım taleplerini anonim olarak paylaşabildiği; yakın çevredeki gönüllülerin bu talepleri gerçek zamanlı görüp üstlenebildiği bir web platformudur.

---

## İçindekiler

- [Proje Hakkında](#proje-hakkında)
- [Özellikler](#özellikler)
- [Teknoloji Yığını](#teknoloji-yığını)
- [Mimari](#mimari)
- [Klasör Yapısı](#klasör-yapısı)
- [Kurulum](#kurulum)
- [API Referansı](#api-referansı)
- [Veritabanı Şeması](#veritabanı-şeması)
- [Geliştirme Notları](#geliştirme-notları)
- [Lisans](#lisans)

---

## Proje Hakkında

İhtiyaç Molası, Bandırma Onyedi Eylül Üniversitesi Yazılım Mühendisliği Bölümü kapsamında yürütülen Gönüllülük Projesi çerçevesinde geliştirilmiştir. Projenin temel amacı; teknoloji aracılığıyla mahalle dayanışmasını güçlendirmek ve küçük ama kritik anlık yardım taleplerini hızlıca doğru kişiye ulaştırmaktır.

Platform tamamen üyeliksiz çalışır. Kişisel veri toplanmaz, e-posta veya telefon numarası istenmez. Kullanıcılar istedikleri takdirde tamamen anonim kalabilir.

---
<img width="1919" height="1021" alt="Ekran görüntüsü 2026-06-12 112416" src="https://github.com/user-attachments/assets/e7db4bb8-8d62-4fe6-b496-bedc769b27f2" />


<img width="1589" height="887" alt="Ekran görüntüsü 2026-06-12 112426" src="https://github.com/user-attachments/assets/ec318b97-ae02-4fa3-b5eb-dfdd6c7b1c5c" />

## Özellikler

- Üyelik gerektirmeden yardım talebi oluşturma
- Rumuz veya anonim kullanım seçeneği
- Kategori bazlı filtreleme (ilaç, market, çocuk, ulaşım, diğer)
- Talepleri gerçek zamanlı listeleme ve otomatik yenileme (15 saniyede bir)
- Tek tıkla talep üstlenme
- Uygunsuz içerik raporlama sistemi
- Tamamen mobil uyumlu arayüz

---
<img width="1919" height="1021" alt="Ekran görüntüsü 2026-06-12 112416" src="https://github.com/user-attachments/assets/93781e99-0dde-4ee2-b443-a366b530d85d" />

<img width="1046" height="897" alt="Ekran görüntüsü 2026-06-12 112435" src="https://github.com/user-attachments/assets/c4fed66d-9393-4d8a-b57a-2bd052aad8ef" />

## Teknoloji Yığını

| Katman | Teknoloji |
|---|---|
| Frontend | HTML5, CSS3 (özel, framework bağımlılığı yok), Bootstrap 5, Vanilla JavaScript |
| Backend | .NET 8 Web API (C#) |
| ORM | Entity Framework Core 8 |
| Veritabanı | MySQL 8.0 |
| API Belgeleme | Swagger / Swashbuckle |

---

## Mimari

```
Kullanıcı (Tarayıcı)
       |
       | HTTP / HTTPS
       v
   Frontend
   HTML + CSS + JS
   (Statik dosyalar)
       |
       | REST API — JSON
       v
   Backend
   .NET 8 Web API
   TaleplerController
   Entity Framework Core
       |
       | TCP 3306
       v
   Veritabanı
   MySQL 8.0
   talepler / ustlenmeler / raporlar
```

---
<img width="1919" height="1021" alt="Ekran görüntüsü 2026-06-12 112416" src="https://github.com/user-attachments/assets/e7db4bb8-8d62-4fe6-b496-bedc769b27f2" />


<img width="1141" height="918" alt="Ekran görüntüsü 2026-06-12 112512" src="https://github.com/user-attachments/assets/07f14328-4cc7-45c5-8f63-8ec4f9156e82" />


<img width="1141" height="904" alt="Ekran görüntüsü 2026-06-12 112506" src="https://github.com/user-attachments/assets/4e2e53e7-99b3-4d5f-8650-dbd11fc5a682" />

## Klasör Yapısı

```
ihtiyac-molasi/
|
|-- frontend/
|   |-- index.html               Ana sayfa
|   |-- css/
|   |   └── style.css            Tüm stiller (sıfırdan yazılmış)
|   |-- js/
|   |   |-- api.js               Backend iletişim katmanı
|   |   |-- app.js               Ana sayfa mantığı
|   |   |-- talepler.js          Talep listesi ve filtreleme
|   |   └── yeni-talep.js        Form gönderimi
|   └── pages/
|       |-- talepler.html        Talepler listesi sayfası
|       └── yeni-talep.html      Yardım isteme formu
|
|-- backend/
|   |-- IhtiyacMolasi.csproj     Proje ve NuGet bağımlılıkları
|   |-- Program.cs               Uygulama başlangıcı ve yapılandırma
|   |-- appsettings.json         Veritabanı bağlantı ayarları
|   |-- Controllers/
|   |   └── TaleplerController.cs
|   |-- Models/
|   |   |-- Talep.cs
|   |   └── Ustlenme.cs
|   └── Data/
|       └── AppDbContext.cs
|
|-- database/
|   └── schema.sql               Tablo şemaları ve örnek veri
|
└── README.md
```

---

## Kurulum

### Gereksinimler

- .NET SDK 8.0 veya üzeri — https://dotnet.microsoft.com/download
- MySQL Server 8.0 veya üzeri — https://dev.mysql.com/downloads/
- Git — https://git-scm.com

### 1. Repoyu klonla

```bash
git clone https://github.com/KULLANICI_ADIN/ihtiyac-molasi.git
cd ihtiyac-molasi
```

### 2. Veritabanını oluştur

```bash
mysql -u root -p < database/schema.sql
```

MySQL Workbench kullanıyorsan `schema.sql` dosyasını aç ve çalıştır.

### 3. Veritabanı bağlantısını ayarla

`backend/appsettings.json` dosyasını aç ve MySQL şifreni yaz:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=ihtiyac_molasi;User=root;Password=SIFREN;"
  }
}
```

### 4. Backend'i çalıştır

```bash
cd backend
dotnet restore
dotnet run
```

Başarılı çalışma çıktısı:

```
Now listening on: https://localhost:7001
Now listening on: http://localhost:5001
```

Swagger arayüzü: `https://localhost:7001/swagger`

### 5. Frontend'i aç

`frontend/index.html` dosyasını doğrudan tarayıcıda açabilir ya da VS Code Live Server eklentisini kullanabilirsin.

`frontend/js/api.js` dosyasının en üstündeki `API_BASE` satırının backend portunla eşleştiğinden emin ol:

```javascript
const API_BASE = 'https://localhost:7001/api';
```

---

## API Referansı

### Endpoint'ler

```
GET    /api/talepler                Talepleri listele
GET    /api/talepler/{id}           Tek talep getir
POST   /api/talepler                Yeni talep oluştur
POST   /api/talepler/{id}/ustlen    Talebi üstlen
POST   /api/talepler/{id}/rapor     Talebi raporla
DELETE /api/talepler/{id}           Talep sil
```

### Sorgu Parametreleri (GET /api/talepler)

| Parametre | Tip | Açıklama |
|---|---|---|
| `durum` | string | bekliyor, ustlenildi, tamamlandi, iptal |
| `kategori` | string | ilac, market, cocuk, ulasim, diger |
| `mahalle` | string | Mahalle adı (kısmi eşleşme) |
| `limit` | int | Maksimum sonuç sayısı (varsayılan: 50) |

### Yeni Talep Oluşturma

```http
POST /api/talepler
Content-Type: application/json

{
  "baslik":   "İlaç almam lazım acil",
  "aciklama": "Kalp ilacım bitti, eczaneden alınması gerek.",
  "kategori": "ilac",
  "mahalle":  "Bağcılar",
  "rumuz":    "Ayse_H"
}
```

Başarılı yanıt: `201 Created` + oluşturulan talep nesnesi

### Talebi Üstlenme

```http
POST /api/talepler/1/ustlen
Content-Type: application/json

{
  "gonullu": "Mehmet42"
}
```

---

## Veritabanı Şeması

### talepler

| Sütun | Tip | Açıklama |
|---|---|---|
| id | INT AUTO_INCREMENT | Birincil anahtar |
| rumuz | VARCHAR(50) | Kullanıcı rumuzu, varsayılan: Anonim |
| baslik | VARCHAR(120) | Talep başlığı |
| aciklama | TEXT | Detaylı açıklama |
| kategori | ENUM | ilac, market, cocuk, ulasim, diger |
| mahalle | VARCHAR(100) | Mahalle adı |
| durum | ENUM | bekliyor, ustlenildi, tamamlandi, iptal |
| olusturma | DATETIME | Oluşturulma zamanı |
| son_tarih | DATETIME | İsteğe bağlı son tarih |

### ustlenmeler

| Sütun | Tip | Açıklama |
|---|---|---|
| id | INT AUTO_INCREMENT | Birincil anahtar |
| talep_id | INT | talepler tablosuna yabancı anahtar |
| gonullu | VARCHAR(50) | Gönüllü rumuzu |
| zaman | DATETIME | Üstlenme zamanı |

### raporlar

| Sütun | Tip | Açıklama |
|---|---|---|
| id | INT AUTO_INCREMENT | Birincil anahtar |
| talep_id | INT | talepler tablosuna yabancı anahtar |
| sebep | VARCHAR(255) | Raporlama sebebi |
| zaman | DATETIME | Raporlama zamanı |

### Durum Akışı

```
bekliyor --> ustlenildi --> tamamlandi
         \-> iptal
```

---

## Geliştirme Notları

### Bilinen Kısıtlamalar

- Gerçek zamanlı bildirim yok, polling (15 sn) ile simüle ediliyor.
- Kullanıcı doğrulaması yok; admin işlemleri (silme) açık endpoint üzerinden.
- Şu an için mailler arasında coğrafi filtreleme yok, tüm mahalleler tek listede görünüyor.

### Potansiyel İyileştirmeler

- WebSocket ile gerçek anlık bildirim
- Konum bazlı filtreleme (yakın mahalleler önce)
- Admin paneli ve moderasyon arayüzü
- Rate limiting (spam koruması)
- HTTPS zorunluluğu ve CORS kısıtlaması

### Güvenlik Notu

`appsettings.json` içindeki veritabanı şifresini doğrudan commit etme. Üretim ortamında environment variable kullan:

```bash
export ConnectionStrings__DefaultConnection="Server=...;Password=GERCEK_SIFRE;"
```

---

## Lisans

MIT License — dilediğin gibi kullanabilir, fork edebilir, katkıda bulunabilirsin.

---

**Furkan Gül** — Software Engeenering
