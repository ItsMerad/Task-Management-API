# Task Management API

Basit ve kullanışlı bir görev yönetim sistemi sunan ASP.NET Core REST API uygulaması.

## 📋 Proje Açıklaması

Bu uygulama, kullanıcıların görevler (tasks) oluşturmasına, okuyabilmesine, güncellemesine ve silebilmesine olanak tanır. Tüm veriler bellekte (in-memory) depolanır ve uygulama yeniden başlatıldığında sıfırlanır.

### Özellikler
- ✅ **CRUD Operasyonları**: Tam CRUD (Create, Read, Update, Delete) desteği
- ✅ **RESTful API**: Standart REST mimarisi
- ✅ **Swagger/OpenAPI**: Otomatik API dokümantasyonu
- ✅ **Input Validasyonu**: Gerekli alanların kontrolü
- ✅ **Hata Yönetimi**: Uygun HTTP status kodları

## 🛠️ Gereksinimler

- **.NET 9.0 SDK** veya daha yüksek sürüm
- **macOS / Windows / Linux**

.NET SDK'nı kontrol etmek için:
```bash
dotnet --version
```

## 🚀 Kurulum ve Çalıştırma

### 1. Projeyi Klonlayın
```bash
cd "/Users/mehmeterad/Desktop/GitHub/Web Based /Midterm/Task Management API"
```

### 2. Uygulamayı Çalıştırın
```bash
dotnet run
```

### 3. Swagger UI'ye Erişin
Uygulamayı başlattıktan sonra tarayıcıda açın:
```
http://localhost:5218/swagger
```

## 📡 API Endpointleri

Tüm endpointler `/api/v1/tasks` base URL'i altında gruplandırılmıştır.

### 1. Tüm Görevleri Al
**GET** `/api/v1/tasks/`

Başarılı Yanıt (200):
```json
[
  {
    "id": 1,
    "title": "Proje tamamla",
    "isCompleted": false,
    "description": "Midterm projesi bitir"
  }
]
```

### 2. Belirli Bir Görevi Al
**GET** `/api/v1/tasks/{id}`

Örnek: `GET /api/v1/tasks/1`

Başarılı Yanıt (200):
```json
{
  "id": 1,
  "title": "Proje tamamla",
  "isCompleted": false,
  "description": "Midterm projesi bitir"
}
```

Hata Yanıtı (404): Görev bulunamazsa

### 3. Yeni Görev Oluştur
**POST** `/api/v1/tasks/`

İstek Gövdesi:
```json
{
  "title": "Yeni görev",
  "isCompleted": false,
  "description": "Görev açıklaması (isteğe bağlı)"
}
```

Başarılı Yanıt (201):
```json
{
  "id": 2,
  "title": "Yeni görev",
  "isCompleted": false,
  "description": "Görev açıklaması"
}
```

Hata Yanıtı (400): Title alanı boş ise

### 4. Görevi Güncelle
**PUT** `/api/v1/tasks/{id}`

Örnek: `PUT /api/v1/tasks/1`

İstek Gövdesi:
```json
{
  "title": "Güncellenmiş başlık",
  "isCompleted": true,
  "description": "Güncellenmiş açıklama"
}
```

Başarılı Yanıt (200):
```json
{
  "id": 1,
  "title": "Güncellenmiş başlık",
  "isCompleted": true,
  "description": "Güncellenmiş açıklama"
}
```

Hata Yanıtları:
- (404): Görev bulunamazsa
- (400): Title alanı boş ise

### 5. Görevi Sil
**DELETE** `/api/v1/tasks/{id}`

Örnek: `DELETE /api/v1/tasks/1`

Başarılı Yanıt (204): İçerik yok

Hata Yanıtı (404): Görev bulunamazsa

## 💻 Veri Modelleri

### TaskItem
```csharp
public record TaskItem(
    int Id,                      // Görev ID'si (otomatik artan)
    string Title,                // Görev başlığı (zorunlu)
    bool IsCompleted,            // Tamamlanma durumu
    string? Description = null   // Görev açıklaması (isteğe bağlı)
);
```

### TaskCreateRequest
```csharp
public record TaskCreateRequest(
    string Title,                // Görev başlığı (zorunlu)
    bool IsCompleted,            // Tamamlanma durumu
    string? Description          // Görev açıklaması
);
```

### TaskUpdateRequest
```csharp
public record TaskUpdateRequest(
    string Title,                // Yeni başlık (zorunlu)
    bool IsCompleted,            // Yeni tamamlanma durumu
    string? Description          // Yeni açıklama
);
```

## 🧪 Test Etme (cURL Örnekleri)

### Tüm görevleri al
```bash
curl http://localhost:5218/api/v1/tasks/
```

### Yeni görev oluştur
```bash
curl -X POST http://localhost:5218/api/v1/tasks/ \
  -H "Content-Type: application/json" \
  -d '{"title":"Yapılacaklar","isCompleted":false,"description":"Bugün yapılacak işler"}'
```

### Görevi güncelle
```bash
curl -X PUT http://localhost:5218/api/v1/tasks/1 \
  -H "Content-Type: application/json" \
  -d '{"title":"Güncellenmiş","isCompleted":true,"description":"Tamamlandı"}'
```

### Görevi sil
```bash
curl -X DELETE http://localhost:5218/api/v1/tasks/1
```

## 📁 Proje Yapısı

```
Task_Management_API/
├── Program.cs                          # Ana uygulama dosyası
├── Task_Management_API.csproj          # Proje yapı dosyası
├── Task_Management_API.http            # REST istemci dosyası
├── Properties/
│   └── launchSettings.json             # Başlatma ayarları
├── appsettings.json                    # Yapılandırma dosyası
├── appsettings.Development.json        # Geliştirme yapılandırması
└── README.md                           # Bu dosya
```

## 🔧 Teknolojiler

- **Framework**: ASP.NET Core 9.0
- **Dil**: C# (Minimal APIs)
- **API Dokümantasyonu**: Swagger/OpenAPI (Swashbuckle)
- **Veri Saklama**: In-Memory (ConcurrentDictionary)

## 📝 Notlar

- Tüm veriler **bellekte** saklanır. Uygulama kapatılırsa veriler kaybolur.
- Üretim ortamı için veritabanı entegrasyonu yapılması önerilir.
- Şu anda kimlik doğrulama (authentication) ve yetkilendirme (authorization) uygulanmamıştır.

## 🛑 Uygulamayı Durdurma

Terminal'de `Ctrl+C` tuşlarına basın.

## 📧 Destek

Sorularınız veya önerileriniz için lütfen iletişime geçin.

---

**Son Güncelleme**: 16 Şubat 2026
