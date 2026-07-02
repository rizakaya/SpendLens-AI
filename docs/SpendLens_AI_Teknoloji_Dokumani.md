# SpendLens AI - Proje Teknoloji Dokümanı

## 1. Proje Özeti

**SpendLens AI**, kullanıcının CSV/Excel formatında yüklediği banka veya kredi kartı hareketlerini analiz eden, harcamaları kategorize eden, tekrar eden ödemeleri ve abonelik adaylarını tespit eden, aylık harcama içgörüleri üreten yapay zeka destekli kişisel finans analiz uygulamasıdır.

Temel hedef:

```text
Dağınık banka hareketleri
→ Merchant normalize
→ Kategori tahmini
→ Abonelik / tekrar eden ödeme tespiti
→ Aylık finans raporu
→ AI destekli finans asistanı
```

---

## 2. Ana Teknoloji Stack'i

| Katman | Teknoloji | Kullanım Amacı |
|---|---|---|
| Frontend | Angular | Kullanıcı arayüzü, dashboard, upload, rapor ve chat ekranları |
| API Gateway / BFF | Python FastAPI | Frontend isteklerini yönlendirme, response sadeleştirme, auth kontrolü |
| Core Backend | .NET 8 Web API | Finansal veri, transaction yönetimi, dashboard, rapor ve business logic |
| AI Service | Python FastAPI | LLM çağrıları, merchant normalize, kategori tahmini, rapor yorumu |
| Auth | Keycloak | Kullanıcı girişi, JWT, rol ve yetki yönetimi |
| Database | PostgreSQL | Transaction, kategori, merchant, abonelik, rapor verileri |
| Cache / Job State | Redis | Cache, job state, rate limit ve geçici veri |
| Object Storage | MinIO | Yüklenen CSV/Excel dosyalarının saklanması |
| Vector Database | Qdrant | Merchant benzerliği, açıklama eşleştirme, ileride semantic search |
| Logging | Serilog + Graylog | Merkezi loglama |
| Observability | OpenTelemetry | Trace, metric ve servisler arası izleme |
| Container | Docker / Docker Compose | Lokal geliştirme ve servis orkestrasyonu |

---

## 3. Genel Mimari

```text
[Angular Web UI]
        |
        v
[Python FastAPI Gateway / BFF]
        |
        +--------------------------+
        |                          |
        v                          v
[SpendLens Core API - .NET 8]   [SpendLens AI Service - Python]
        |                          |
        |                          +--> Ollama / OpenAI
        |                          +--> Qdrant
        |
        +--> PostgreSQL
        +--> Redis
        +--> MinIO

Logs:
Angular / Gateway / Core API / AI Service / Worker
        ↓
Graylog

Telemetry:
Services
        ↓
OpenTelemetry Collector
```

---

## 4. Servisler

## 4.1 SpendLens Web

**Teknoloji:** Angular

Sorumluluklar:

- Kullanıcı girişi
- Dashboard ekranı
- CSV/Excel dosya yükleme
- Kolon eşleştirme
- Transaction listeleme
- Kategori düzeltme
- Abonelik adaylarını görüntüleme
- Aylık rapor görüntüleme
- AI finans asistanı ekranı

Önerilen Angular yapı:

```text
src/app
  core/
    auth/
    interceptors/
    guards/
    services/
  shared/
    components/
    pipes/
    models/
  features/
    dashboard/
    imports/
    transactions/
    subscriptions/
    reports/
    chat/
```

---

## 4.2 SpendLens Gateway

**Teknoloji:** Python FastAPI

Amaç:

Frontend'in doğrudan tüm mikroservislere gitmesini engellemek ve daha sade bir BFF katmanı sağlamak.

Sorumluluklar:

- Angular isteklerini karşılamak
- JWT token kontrolü
- Core API isteklerini yönlendirmek
- AI Service isteklerini yönlendirmek
- Response formatlarını frontend için sadeleştirmek
- Rate limit uygulamak
- CorrelationId üretmek veya taşımak

Başlangıçta zorunlu değildir. MVP'de Angular doğrudan .NET API'ye bağlanabilir. Ancak API Gateway deneyimi kazanmak için projeye dahil edilmesi önerilir.

---

## 4.3 SpendLens Core API

**Teknoloji:** .NET 8 Web API

Ana business logic burada bulunur.

Sorumluluklar:

- Import dosyası yönetimi
- Transaction kayıtları
- Merchant kayıtları
- Kategori yönetimi
- Kullanıcı kategori düzeltmeleri
- Abonelik adayları
- Tekrar eden ödeme tespiti
- Dashboard verileri
- Aylık rapor verileri
- Chat tool endpointleri
- Yetkilendirme kontrolleri

Önerilen proje yapısı:

```text
spendlens-backend/
  src/
    SpendLens.Api/
    SpendLens.Application/
    SpendLens.Domain/
    SpendLens.Infrastructure/
    SpendLens.Worker/
  tests/
    SpendLens.UnitTests/
    SpendLens.IntegrationTests/
```

Önerilen .NET paketleri:

```text
Microsoft.EntityFrameworkCore
Npgsql.EntityFrameworkCore.PostgreSQL
FluentValidation
MediatR
Serilog.AspNetCore
Serilog.Sinks.Graylog
OpenTelemetry.Extensions.Hosting
OpenTelemetry.Instrumentation.AspNetCore
OpenTelemetry.Instrumentation.Http
Hangfire.AspNetCore
Hangfire.PostgreSql
Minio
CsvHelper
ClosedXML
```

---

## 4.4 SpendLens AI Service

**Teknoloji:** Python FastAPI

LLM ile ilgili işlemler burada çalışır.

Sorumluluklar:

- Raw transaction açıklamasını temizlemek
- Merchant ismini tahmin etmek
- Kategori tahmini yapmak
- Abonelik adayı olup olmadığını yorumlamak
- Aylık rapor için doğal dil yorumu üretmek
- Chat mesajını intent/tool çağrısına çevirmek
- Structured output üretmek

Önerilen klasör yapısı:

```text
spendlens-ai-service/
  app/
    main.py
    api/
      enrichment_routes.py
      report_routes.py
      chat_routes.py
    schemas/
      transaction_schema.py
      enrichment_schema.py
      report_schema.py
    services/
      merchant_normalizer.py
      category_classifier.py
      report_generator.py
      chat_orchestrator.py
    prompts/
      transaction_enrichment_prompt.txt
      monthly_report_prompt.txt
    clients/
      ollama_client.py
      openai_client.py
      qdrant_client.py
  tests/
  Dockerfile
```

Önerilen Python paketleri:

```text
fastapi
uvicorn
pydantic
pandas
openpyxl
httpx
ollama
openai
qdrant-client
python-dotenv
structlog
```

---

## 4.5 SpendLens Worker

**Teknoloji seçenekleri:**

- .NET Worker Service + Hangfire
- Python Celery + Redis

İlk tercih:

```text
.NET Worker Service + Hangfire
```

Neden?

Core API zaten .NET tarafında olduğu için transaction import, enrichment job ve subscription detection gibi işler .NET tarafında daha kontrollü yönetilebilir.

Worker görevleri:

- Dosya parse etme
- Transaction import
- Toplu AI enrichment başlatma
- Düşük confidence işlem işaretleme
- Abonelik ve tekrar eden ödeme tespiti
- Aylık rapor üretimi
- Eski dosya temizleme

---

## 5. Veri Katmanı

## 5.1 PostgreSQL

Ana ilişkisel veritabanı olarak kullanılacak.

Tutulacak ana veriler:

- Kullanıcı bilgileri
- Import dosyaları
- Transaction kayıtları
- Transaction enrichment sonuçları
- Merchant bilgileri
- Kategori bilgileri
- Kullanıcı kategori düzeltmeleri
- Subscription adayları
- Recurring payment kayıtları
- Monthly report kayıtları
- Chat geçmişi

Temel tablolar:

```text
Users
ImportFiles
Transactions
TransactionEnrichments
Merchants
Categories
UserCategoryCorrections
Subscriptions
RecurringPayments
SpendingInsights
MonthlyReports
ChatSessions
ChatMessages
```

---

## 5.2 Redis

İlk MVP için zorunlu değildir. Ancak ilerleyen aşamada kullanılmalıdır.

Kullanım alanları:

- Cache
- Rate limiting
- Job state
- Geçici işlem durumları
- Chat session kısa süreli state
- Distributed lock

---

## 5.3 MinIO

Yüklenen dosyalar doğrudan PostgreSQL içinde saklanmamalıdır.

MinIO kullanım alanları:

- CSV dosyaları
- Excel dosyaları
- Import edilmiş orijinal ekstreler
- İleride PDF ekstreler
- Export edilmiş rapor dosyaları

PostgreSQL'de sadece metadata tutulmalıdır:

```text
FileId
UserId
OriginalFileName
StorageBucket
StorageObjectName
ContentType
Size
UploadedAt
ImportStatus
```

---

## 5.4 Qdrant

İlk MVP için şart değildir. Ancak projenin AI tarafını güçlendirmek için iyi bir adımdır.

Kullanım alanları:

- Benzer merchant açıklamalarını eşleştirme
- Kullanıcı kategori düzeltmelerinden öğrenme
- Aynı merchant'ın farklı banka açıklamalarını gruplayabilme
- Semantic search
- İleride finansal açıklama örneklerinden RAG benzeri yapı

Örnek:

```text
GOOGLE *YOUTUBE
YT PREMIUM
YOUTUBE PREMIUM
GOOGLE SERVICES YT
```

Bu açıklamalar embedding ile yakın sonuçlar döndürerek tek bir merchant altında toplanabilir:

```text
YouTube Premium
```

---

## 6. Authentication ve Authorization

## 6.1 Keycloak

Keycloak kullanıcı ve rol yönetimi için kullanılacak.

Başlangıç rolleri:

```text
User
Admin
```

İleride eklenebilecek roller:

```text
PremiumUser
SupportUser
ReadOnlyUser
```

JWT akışı:

```text
Angular
  ↓ login redirect
Keycloak
  ↓ access token
Angular
  ↓ Authorization: Bearer token
Gateway / Core API
  ↓ token validation
Protected endpoints
```

---

## 6.2 Yetkilendirme Kuralları

Temel güvenlik kuralları:

- Kullanıcı sadece kendi transaction verilerini görebilir.
- Import dosyası UserId ile izole edilmelidir.
- Admin rolü sistem ayarlarını yönetebilir.
- AI Service'e doğrudan frontend erişimi olmamalıdır.
- LLM'e gönderilen veri minimize edilmelidir.
- Sensitive alanlar loglanmamalıdır.

---

## 7. AI Kullanım Alanları

## 7.1 Transaction Enrichment

Amaç:

Raw banka açıklamasını anlamlı hale getirmek.

Input:

```json
{
  "transactionDate": "2026-07-01",
  "rawDescription": "IYZICO*SPOTIFY P2D ISTANBUL",
  "amount": -59.99,
  "currency": "TRY"
}
```

Output:

```json
{
  "merchantName": "Spotify",
  "normalizedDescription": "Spotify dijital müzik aboneliği",
  "category": "Abonelik",
  "subcategory": "Müzik",
  "isSubscriptionCandidate": true,
  "isEssential": false,
  "confidence": 0.94,
  "tags": [
    "digital_service",
    "recurring",
    "entertainment"
  ],
  "reasoningSummary": "Açıklamada Spotify geçtiği için dijital müzik aboneliği olarak sınıflandırıldı."
}
```

---

## 7.2 Kategori Tahmini

Başlangıç kategori listesi:

```text
Market
Yeme İçme
Ulaşım
Yakıt
Fatura
Abonelik
Alışveriş
Sağlık
Eğitim
Eğlence
Kira / Aidat
Finans / Banka
Para Transferi
Gelir
Diğer
```

Kural:

```text
LLM kategori önerir.
Kullanıcı kategoriyi düzeltebilir.
Kullanıcı düzeltmesi sonraki analizlerde öncelikli olmalıdır.
```

---

## 7.3 Abonelik Tespiti

Abonelik tespiti sadece LLM'e bırakılmamalıdır.

Algoritmik sinyaller:

```text
Aynı merchant
Benzer tutar
25-35 gün arası tekrar
En az 2 veya 3 tekrar
Son ödeme tarihi yakın mı?
```

LLM sinyalleri:

```text
Açıklama abonelik gibi mi?
Merchant dijital servis mi?
Ödeme düzenli hizmete mi benziyor?
```

---

## 7.4 Aylık Rapor Üretimi

Finansal hesaplamaları backend yapar.

LLM'e şu tarz özet veri gönderilir:

```json
{
  "month": "2026-07",
  "totalExpense": 34250,
  "totalIncome": 75000,
  "topCategories": [
    {
      "name": "Market",
      "amount": 8250
    },
    {
      "name": "Yeme İçme",
      "amount": 6100
    }
  ],
  "subscriptionsTotal": 1240,
  "monthOverMonthChange": 18.5
}
```

LLM sadece doğal dil yorumu üretir.

---

## 7.5 AI Finans Asistanı

Kullanıcı doğal dille soru sorar.

Örnek sorular:

```text
Bu ay aboneliklere ne kadar vermişim?
En çok hangi markete harcama yapmışım?
Geçen aya göre hangi kategoriler artmış?
Kategorisi belirsiz işlemleri göster.
```

LLM doğrudan hesaplama yapmaz. Tool çağırır.

Tool örnekleri:

```text
GetMonthlySpendingSummary
GetCategoryBreakdown
GetTopMerchants
GetSubscriptions
GetRecurringPayments
CompareMonthlySpending
GetUncategorizedTransactions
```

---

## 8. Observability

## 8.1 Graylog

Graylog merkezi loglama için kullanılacak.

Loglanacak bilgiler:

- Request başlangıç/bitiş
- Import işlemleri
- Dosya parse hataları
- AI Service çağrıları
- LLM response süresi
- Enrichment hataları
- Subscription detection job sonuçları
- Kullanıcı düzeltmeleri
- Kritik exceptionlar

Loglanmaması gerekenler:

- Kart numarası
- IBAN
- TCKN
- Ham finansal dosya içeriği
- Kullanıcıya ait hassas açıklama detayları
- Token bilgileri

---

## 8.2 OpenTelemetry

Servisler arası trace için kullanılacak.

Trace edilecek akışlar:

```text
File Upload
Transaction Import
AI Enrichment
Subscription Detection
Monthly Report Generation
Chat Request
```

Örnek trace:

```text
POST /api/import-files
  → save file metadata
  → upload file to MinIO
  → create import job
  → parse CSV
  → insert transactions
  → call AI service
  → save enrichments
```

---

## 8.3 Health Checks

Her servis için health endpoint olmalı:

```text
Gateway: /health
Core API: /health
AI Service: /health
PostgreSQL: container healthcheck
Redis: container healthcheck
Qdrant: container healthcheck
MinIO: container healthcheck
Graylog: container healthcheck
```

---

## 9. Güvenlik ve Gizlilik

Bu proje finansal veri ile çalıştığı için privacy-first yaklaşım önemlidir.

Temel kararlar:

- Banka hesabı bağlantısı MVP'de olmayacak.
- Kullanıcı sadece manuel CSV/Excel yükleyecek.
- LLM'e tam ekstre gönderilmeyecek.
- LLM'e sadece gerekli alanlar gönderilecek:
  - Açıklama
  - Tutar
  - Tarih
  - Para birimi
- Kart numarası, IBAN, TCKN gibi alanlar maskelenecek.
- Sensitive data loglanmayacak.
- Dosya silme özelliği olacak.
- Kullanıcı verisi UserId ile izole edilecek.

Maskeleme örnekleri:

```text
TR12 0006 1005 1978 6457 8413 26 → TR12 **** **** **** **** **13 26
1234 5678 9012 3456 → **** **** **** 3456
```

---

## 10. Docker Compose Servisleri

Başlangıç docker-compose servisleri:

```text
spendlens-web
spendlens-gateway
spendlens-api
spendlens-ai-service
postgres
keycloak
graylog
mongodb
opensearch
```

Graylog için gerekli servisler:

```text
graylog
mongodb
opensearch
```

İleri seviye docker-compose servisleri:

```text
redis
minio
qdrant
otel-collector
prometheus
grafana
rabbitmq
```

---

## 11. Environment Değişkenleri

Örnek `.env` alanları:

```env
POSTGRES_HOST=postgres
POSTGRES_PORT=5432
POSTGRES_DB=spendlens
POSTGRES_USER=spendlens
POSTGRES_PASSWORD=spendlens_password

KEYCLOAK_URL=http://keycloak:8080
KEYCLOAK_REALM=spendlens
KEYCLOAK_CLIENT_ID=spendlens-web

GRAYLOG_HOST=graylog
GRAYLOG_GELF_UDP_PORT=12201

AI_SERVICE_URL=http://spendlens-ai-service:8000
OLLAMA_BASE_URL=http://ollama:11434
LLM_PROVIDER=ollama
LLM_MODEL=qwen3.6:latest

MINIO_ENDPOINT=minio:9000
MINIO_ACCESS_KEY=spendlens
MINIO_SECRET_KEY=spendlens_secret
MINIO_BUCKET=spendlens-files

QDRANT_URL=http://qdrant:6333
REDIS_CONNECTION=redis:6379
```

---

## 12. MVP Teknoloji Kapsamı

İlk çalışan sürümde kullanılacaklar:

```text
Angular
.NET 8 Web API
Python FastAPI AI Service
PostgreSQL
Keycloak
Docker Compose
Serilog
Graylog
Ollama veya OpenAI
```

MVP'de ertelenebilir:

```text
Python Gateway
Redis
MinIO
Qdrant
OpenTelemetry
Prometheus
Grafana
RabbitMQ
```

---

## 13. Fazlara Göre Teknoloji Kullanımı

## Faz 1 - Altyapı

```text
Docker Compose
PostgreSQL
Keycloak
.NET 8 API
Angular
Python AI Service
Graylog
```

## Faz 2 - Dosya Import

```text
CsvHelper
ClosedXML
PostgreSQL
Angular upload ekranı
```

## Faz 3 - AI Enrichment

```text
Python FastAPI
Pydantic
Ollama / OpenAI
Structured output
```

## Faz 4 - Abonelik Tespiti

```text
.NET algoritmik analiz
PostgreSQL sorguları
Opsiyonel AI destekli yorumlama
```

## Faz 5 - Dashboard ve Rapor

```text
Angular chart library
.NET dashboard endpoints
Python AI report generator
```

## Faz 6 - Chat Asistanı

```text
Tool calling
Chat history
Intent detection
Core API query tools
```

## Faz 7 - Profesyonelleştirme

```text
Redis
MinIO
Qdrant
OpenTelemetry
Prometheus
Grafana
RabbitMQ
```

---

## 14. Teknoloji Kararları

## 14.1 Neden .NET Core API?

- Finansal business logic için güçlü tip güvenliği sağlar.
- EF Core ile PostgreSQL entegrasyonu kolaydır.
- Background job, validation, logging ve API tasarımı güçlüdür.
- Kurumsal backend mimarisi için uygundur.

---

## 14.2 Neden Python AI Service?

- LLM, embedding, veri işleme ve AI kütüphaneleri Python tarafında daha zengindir.
- Pandas ile CSV/Excel ön analizleri kolaydır.
- FastAPI ile hızlı servis geliştirilir.
- AI tarafı Core API'den ayrılmış olur.

---

## 14.3 Neden Keycloak?

- Kullanıcı ve rol yönetimi hazır gelir.
- JWT/OIDC desteği vardır.
- Mikroservis mimarisine uygundur.
- İleride tenant veya role-based access control kolaylaşır.

---

## 14.4 Neden PostgreSQL?

- Finansal transaction verisi için güvenilir ilişkisel model sağlar.
- JSONB desteği ile AI enrichment metadata saklanabilir.
- İleride pgvector alternatifi değerlendirilebilir.
- Docker üzerinde kolay çalışır.

---

## 14.5 Neden Graylog?

- Merkezi loglama sağlar.
- .NET ve Python servislerinden log toplanabilir.
- Hata analizi ve servis izleme için uygundur.
- Daha sonra dashboard ve alert tanımlanabilir.

---

## 14.6 Neden Qdrant?

- Merchant açıklamalarının benzerliğini bulmak için uygundur.
- AI enrichment kalitesini artırır.
- Kullanıcı düzeltmeleri embedding ile tekrar kullanılabilir.
- Semantic search senaryolarına kapı açar.

---

## 15. Teknik Riskler

| Risk | Açıklama | Önlem |
|---|---|---|
| LLM yanlış kategori verebilir | Finansal açıklamalar bazen belirsizdir | Confidence skoru ve manuel düzeltme |
| LLM hesaplama hatası yapabilir | Toplamları yanlış hesaplayabilir | Hesaplamalar backend'de yapılmalı |
| Veri gizliliği riski | Finansal veri hassastır | Maskeleme, minimal prompt, sensitive logging engeli |
| Banka formatları farklıdır | Her CSV/Excel aynı kolonları taşımaz | Kolon eşleştirme ekranı |
| Servis sayısı artınca karmaşıklık artar | Mikroservis yapısı başlangıçta ağır gelebilir | MVP'de servisleri sınırlı tutmak |
| AI Service yavaş olabilir | Toplu enrichment uzun sürebilir | Background job, batch processing, cache |
| Merchant normalize tutarsız olabilir | Aynı firma farklı isimlerle gelebilir | Qdrant + kullanıcı düzeltmeleri |

---

## 16. Teknik Başarı Kriterleri

MVP sonunda sistem şunları yapabilmelidir:

```text
CSV/Excel dosyası yüklenebilmeli.
Kolon eşleştirme yapılabilmeli.
Transaction kayıtları PostgreSQL'e yazılmalı.
Raw açıklamalar AI ile merchant ve kategoriye dönüştürülmeli.
Confidence skoru gösterilmeli.
Kullanıcı kategori düzeltebilmeli.
Tekrar eden ödemeler bulunabilmeli.
Abonelik adayları listelenebilmeli.
Dashboard aylık özet gösterebilmeli.
AI aylık finans raporu üretebilmeli.
Loglar Graylog'a düşmeli.
Kullanıcı girişi Keycloak ile yapılmalı.
```

---

## 17. İlk Kurulum Hedefi

İlk teknik hedef:

```text
docker compose up
```

komutu ile aşağıdaki servisler ayağa kalkmalı:

```text
Angular Web
.NET Core API
Python AI Service
PostgreSQL
Keycloak
Graylog
MongoDB
OpenSearch
```

İlk demo akışı:

```text
1. Kullanıcı login olur.
2. CSV dosyası yükler.
3. Kolonları eşleştirir.
4. Transaction listesi oluşur.
5. AI enrichment çalışır.
6. Dashboard'da harcama özeti görünür.
7. Abonelik adayları listelenir.
8. Aylık AI raporu oluşur.
```

---

## 18. Sonuç

SpendLens AI teknoloji mimarisi; yapay zekayı doğrudan finansal hesaplama yapan bir kara kutu gibi değil, finansal veriyi anlamlandıran, açıklamaları normalize eden, kategori ve içgörü üreten yardımcı bir servis olarak konumlandırır.

Bu yaklaşım sayesinde proje:

- Günlük hayatta işe yarar.
- LLM kullanımını gerçekçi gösterir.
- Mikroservis, API Gateway, Keycloak, Graylog, Docker gibi teknolojileri anlamlı şekilde kullanır.
- AI + klasik backend ayrımını doğru kurar.
- GitHub ve portföy için güçlü bir proje haline gelir.
