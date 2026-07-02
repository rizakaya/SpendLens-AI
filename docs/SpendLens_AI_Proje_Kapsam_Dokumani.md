# SpendLens AI - Proje Kapsam Dokümanı

## 1. Doküman Amacı

Bu doküman, **SpendLens AI** projesinin kapsamını, hedeflerini, kullanıcı senaryolarını, MVP sınırlarını, modüllerini, kapsam dışı bırakılan konuları ve kabul kriterlerini tanımlar.

Bu dokümanın amacı şudur:

```text
Ne yapılacak?
Ne yapılmayacak?
İlk sürümde hangi özellikler olacak?
Hangi özellikler sonraki fazlara bırakılacak?
Başarılı bir MVP için kabul kriterleri ne olacak?
```

---

## 2. Proje Tanımı

**SpendLens AI**, kullanıcıların banka veya kredi kartı hareketlerini CSV/Excel dosyası olarak yükleyip harcamalarını analiz edebileceği, yapay zeka destekli kişisel finans analiz uygulamasıdır.

Uygulama, banka hareketlerindeki karmaşık açıklamaları anlamlandırır, harcamaları kategorilere ayırır, tekrar eden ödeme ve abonelik adaylarını tespit eder, aylık finansal özet ve içgörüler üretir.

Temel yaklaşım:

```text
Kullanıcı dosya yükler
↓
Sistem banka hareketlerini okur
↓
İşlemler normalize edilir
↓
AI merchant ve kategori tahmini yapar
↓
Tekrar eden ödemeler bulunur
↓
Abonelik adayları çıkarılır
↓
Dashboard ve aylık rapor oluşturulur
```

---

## 3. Proje Vizyonu

SpendLens AI'nın vizyonu, kullanıcının dağınık ve okunması zor banka hareketlerini anlaşılır, takip edilebilir ve aksiyon alınabilir finansal içgörülere dönüştürmektir.

Proje, kullanıcıya şunları sağlamayı hedefler:

```text
Param nereye gidiyor?
Hangi harcamalar tekrar ediyor?
Unuttuğum abonelikler var mı?
Hangi kategoride artış olmuş?
Hangi harcamaları kontrol altına alabilirim?
```

---

## 4. Problem Tanımı

Kullanıcılar banka ekstrelerinde çok sayıda işlem görür. Ancak bu işlemler genellikle karışık açıklamalarla gelir.

Örnekler:

```text
IYZICO*SPOTIFY P2D
GOOGLE *YOUTUBE
MOKA/ABC TEKNOLOJI
PARAM POS 3482
MIGROS SANAL MARKET
APPLE.COM/BILL
```

Bu açıklamaları manuel olarak sınıflandırmak zordur.

Mevcut problem alanları:

```text
Banka açıklamaları okunması zor ve tutarsızdır.
Aynı firma farklı açıklamalarla görünebilir.
Abonelikler zamanla unutulabilir.
Küçük ama sık harcamalar fark edilmeyebilir.
Aylık harcama artışının nedeni net görülemeyebilir.
Kullanıcı finansal tablo yerine sade açıklama ister.
```

---

## 5. Hedef Kitle

İlk hedef kitle bireysel kullanıcılardır.

Hedef kullanıcı profilleri:

```text
Kişisel harcamalarını takip etmek isteyen kullanıcılar
Aboneliklerini kontrol etmek isteyen kullanıcılar
Banka hareketlerini manuel kategorize etmek istemeyen kullanıcılar
Aylık harcama raporu görmek isteyen kullanıcılar
Dosya yükleyerek privacy-first analiz yapmak isteyen kullanıcılar
```

Kurumsal kullanım MVP kapsamı dışındadır.

---

## 6. Proje Hedefleri

## 6.1 Ana Hedefler

```text
CSV/Excel banka hareketlerini içeri aktarmak
İşlemleri standart transaction modeline çevirmek
Merchant bilgisini normalize etmek
Harcamaları kategorize etmek
Tekrar eden ödemeleri tespit etmek
Abonelik adaylarını belirlemek
Aylık dashboard oluşturmak
AI destekli finans raporu üretmek
Kullanıcının yanlış kategorileri düzeltebilmesini sağlamak
```

---

## 6.2 AI Kullanım Hedefleri

Yapay zeka şu alanlarda kullanılacaktır:

```text
Banka açıklamalarını anlamlandırma
Merchant adını tahmin etme
Kategori önerme
Abonelik adayı yorumlama
Aylık finans raporunu doğal dile çevirme
Kullanıcı sorusunu uygun veri sorgusuna dönüştürme
```

Önemli karar:

```text
Finansal hesaplamalar LLM tarafından yapılmayacaktır.
Toplam, oran, karşılaştırma ve sayısal hesaplamalar backend tarafından yapılacaktır.
LLM sadece yorumlama, sınıflandırma ve doğal dil üretimi için kullanılacaktır.
```

---

## 7. MVP Kapsamı

MVP, projenin ilk çalışan ve gösterilebilir sürümüdür.

## 7.1 MVP İçinde Olacak Özellikler

```text
Kullanıcı girişi
CSV/Excel dosya yükleme
Dosya önizleme
Kolon eşleştirme
Transaction import
Transaction listeleme
AI ile merchant normalize
AI ile kategori tahmini
Confidence skoru gösterimi
Manuel kategori düzeltme
Tekrar eden ödeme tespiti
Abonelik adayı tespiti
Aylık dashboard
Kategori bazlı harcama dağılımı
En çok harcama yapılan merchant listesi
Aylık AI raporu
Graylog merkezi loglama
```

---

## 7.2 MVP Dışında Kalacak Özellikler

Aşağıdaki özellikler ilk sürümde yapılmayacaktır:

```text
Banka hesabı doğrudan bağlama
Open Banking entegrasyonu
Mobil uygulama
PDF ekstre okuma
OCR ile fiş/fatura okuma
Gerçek zamanlı bildirim
Kredi skoru analizi
Yatırım tavsiyesi
Bütçe planlama motoru
Otomatik abonelik iptali
Kart veya IBAN saklama
Çoklu kullanıcı aile hesabı
Kurumsal muhasebe entegrasyonu
```

---

## 8. Kapsam Dışı Konular

Bu proje aşağıdaki konuları kapsamaz:

```text
Finansal yatırım danışmanlığı
Kredi önerisi
Hisse/fon/kripto tavsiyesi
Vergi danışmanlığı
Banka API entegrasyonu
Kullanıcı adına ödeme yapma
Kullanıcı adına abonelik iptal etme
Kullanıcı adına finansal karar alma
```

Uygulama sadece analiz ve içgörü sunar.

---

## 9. Ana Kullanıcı Senaryoları

## 9.1 Dosya Yükleme Senaryosu

Kullanıcı banka hareketlerini CSV veya Excel dosyası olarak yükler.

Akış:

```text
Kullanıcı sisteme giriş yapar.
Dosya yükleme ekranına gelir.
CSV veya Excel dosyası seçer.
Sistem dosyayı önizler.
Kullanıcı kolonları eşleştirir.
Sistem işlemleri içeri aktarır.
Kullanıcı hareket listesini görür.
```

Kabul kriterleri:

```text
CSV dosyası yüklenebilmelidir.
Excel dosyası yüklenebilmelidir.
Dosya kolonları kullanıcı tarafından eşleştirilebilmelidir.
Hatalı satırlar kullanıcıya raporlanmalıdır.
Başarılı satırlar transaction olarak kaydedilmelidir.
```

---

## 9.2 AI Kategorileme Senaryosu

Kullanıcı içeri aktarılan işlemleri AI ile kategorize eder.

Akış:

```text
Sistem transaction listesini alır.
AI Service'e açıklama, tarih, tutar ve para birimi bilgisi gönderilir.
AI merchant adı ve kategori önerir.
Sonuç confidence skoru ile kaydedilir.
Kullanıcı düşük güvenli işlemleri kontrol eder.
Yanlış kategorileri manuel düzeltir.
```

Kabul kriterleri:

```text
Her transaction için merchant tahmini üretilebilmelidir.
Her transaction için kategori önerilebilmelidir.
Confidence skoru gösterilmelidir.
Kategori allowed category list içinde olmalıdır.
Kullanıcı kategori düzeltebilmelidir.
Kullanıcı düzeltmesi kaydedilmelidir.
```

---

## 9.3 Abonelik Tespiti Senaryosu

Sistem tekrar eden ödemeleri ve abonelik adaylarını bulur.

Akış:

```text
Sistem aynı merchant'a ait işlemleri gruplar.
Benzer tutarlı ve düzenli aralıklarla gelen ödemeleri analiz eder.
25-35 gün aralığında tekrar eden ödemeler aday olarak işaretlenir.
AI açıklamaya göre abonelik olasılığını yorumlar.
Kullanıcı abonelik adaylarını listede görür.
Kullanıcı adayı onaylayabilir veya yok sayabilir.
```

Kabul kriterleri:

```text
Aynı merchant için tekrar eden işlemler bulunabilmelidir.
Benzer tutarlı aylık ödemeler abonelik adayı olarak işaretlenmelidir.
Kullanıcı adayın durumunu değiştirebilmelidir.
Aday, Onaylandı, Yok Sayıldı durumları desteklenmelidir.
```

---

## 9.4 Dashboard Senaryosu

Kullanıcı aylık harcama özetini dashboard üzerinden görür.

Dashboard bileşenleri:

```text
Toplam harcama
Toplam gelir
Net durum
En yüksek kategori
Abonelik toplamı
Tekrar eden ödeme sayısı
En çok harcama yapılan merchantlar
Kategori dağılım grafiği
Aylık harcama trendi
```

Kabul kriterleri:

```text
Kullanıcı ay seçebilmelidir.
Seçilen ayın toplam harcaması gösterilmelidir.
Kategori dağılımı gösterilmelidir.
En çok harcama yapılan merchantlar gösterilmelidir.
Abonelik toplamı gösterilmelidir.
```

---

## 9.5 Aylık Rapor Senaryosu

Kullanıcı seçtiği ay için AI destekli finans raporu alır.

Akış:

```text
Backend aylık sayısal verileri hesaplar.
Kategori toplamları çıkarılır.
Merchant toplamları çıkarılır.
Abonelik toplamı hesaplanır.
Geçen ay verisi varsa karşılaştırma yapılır.
AI Service'e sadece özet veri gönderilir.
AI doğal dille aylık rapor üretir.
```

Kabul kriterleri:

```text
Rapor sayısal hesapları backend verilerine dayanmalıdır.
AI hesaplama yapmamalıdır.
AI rapor sade ve anlaşılır olmalıdır.
Rapor içinde en az 3 finansal içgörü bulunmalıdır.
```

---

## 9.6 AI Finans Asistanı Senaryosu

Bu özellik MVP sonrasında geliştirilebilir.

Kullanıcı doğal dille soru sorar.

Örnek sorular:

```text
Bu ay aboneliklere ne kadar vermişim?
En çok hangi markete harcama yapmışım?
Geçen aya göre hangi kategoriler artmış?
Kategorisi belirsiz işlemleri göster.
Sabit giderlerim neler?
```

Kabul kriterleri:

```text
LLM kullanıcı sorusunu analiz etmelidir.
Gerekli backend tool seçilmelidir.
Sayısal hesaplama backend tarafından yapılmalıdır.
LLM sonucu anlaşılır dile çevirmelidir.
```

---

## 10. Modül Kapsamları

## 10.1 Auth Modülü

Kapsam:

```text
Kullanıcı login
JWT token doğrulama
Kullanıcı rolü kontrolü
Endpoint bazlı authorization
```

Kapsam dışı:

```text
Kullanıcı kayıt ekranı
Şifre sıfırlama ekranı
Sosyal login
Multi-tenant yapı
```

---

## 10.2 Import Modülü

Kapsam:

```text
CSV upload
Excel upload
Dosya önizleme
Kolon eşleştirme
Import status
Hatalı satır raporu
Transaction oluşturma
```

Kapsam dışı:

```text
PDF okuma
OCR
Banka API entegrasyonu
E-posta ile ekstre alma
```

---

## 10.3 Transaction Modülü

Kapsam:

```text
Transaction listeleme
Filtreleme
Kategori görüntüleme
Merchant görüntüleme
Confidence görüntüleme
Manuel kategori düzeltme
```

Kapsam dışı:

```text
Transaction silme
Transaction birleştirme
Manuel transaction ekleme
Çoklu para birimi dönüşümü
```

---

## 10.4 AI Enrichment Modülü

Kapsam:

```text
Raw açıklama temizleme
Merchant tahmini
Kategori tahmini
Abonelik adayı işaretleme
Confidence skoru
Structured output
```

Kapsam dışı:

```text
Model fine-tuning
Kullanıcıya özel model eğitimi
Tam otomatik finansal karar
```

---

## 10.5 Abonelik Modülü

Kapsam:

```text
Recurring payment detection
Subscription candidate detection
Adayları listeleme
Kullanıcı onayı
Kullanıcı yok sayma
Tahmini aylık tutar
Son ödeme tarihi
```

Kapsam dışı:

```text
Abonelik iptal etme
Servis sağlayıcıya bağlanma
Otomatik ödeme durdurma
```

---

## 10.6 Dashboard Modülü

Kapsam:

```text
Aylık özet
Kategori dağılımı
Top merchant listesi
Abonelik toplamı
Aylık karşılaştırma
```

Kapsam dışı:

```text
Gelişmiş bütçe planlama
Tahminleme
Gelir-gider simülasyonu
```

---

## 10.7 Rapor Modülü

Kapsam:

```text
Aylık rapor oluşturma
AI finans yorumu
Öne çıkan artışlar
Tasarruf adayları
Abonelik özeti
```

Kapsam dışı:

```text
PDF rapor export
E-posta ile rapor gönderimi
Otomatik zamanlanmış rapor
```

---

## 11. Ekran Kapsamı

## 11.1 Login Ekranı

```text
Keycloak login yönlendirmesi
Login sonrası dashboard'a yönlendirme
```

---

## 11.2 Dashboard Ekranı

```text
Ay seçimi
Toplam harcama kartı
Toplam gelir kartı
Abonelik toplamı kartı
Kategori dağılım grafiği
En çok harcama yapılan merchant listesi
```

---

## 11.3 Dosya Yükleme Ekranı

```text
CSV/Excel yükleme
Dosya önizleme
Kolon eşleştirme
Import başlatma
Import sonuç özeti
```

---

## 11.4 Transaction Listesi

```text
Tarih
Ham açıklama
Merchant
Kategori
Tutar
Confidence
Abonelik adayı bilgisi
Kategori düzeltme işlemi
```

Filtreler:

```text
Ay
Kategori
Merchant
Gelir/Gider
Düşük confidence
Abonelik adayları
```

---

## 11.5 Abonelikler Ekranı

```text
Merchant
Tahmini aylık tutar
İlk görülme tarihi
Son görülme tarihi
Tespit edilen ödeme sayısı
Confidence
Durum
```

---

## 11.6 Aylık Rapor Ekranı

```text
Aylık özet
Kategori bazlı yorum
Abonelik yorumu
Öne çıkan artışlar
Tasarruf adayları
AI rapor metni
```

---

## 11.7 AI Finans Asistanı Ekranı

MVP sonrası.

```text
Chat mesaj listesi
Hazır örnek sorular
Kullanıcı sorusu
AI cevabı
Kullanılan veri aralığı bilgisi
```

---

## 12. Veri Kapsamı

## 12.1 Desteklenecek Minimum Transaction Alanları

```text
TransactionDate
RawDescription
Amount
Direction
Currency
```

---

## 12.2 Opsiyonel Alanlar

```text
BalanceAfterTransaction
AccountName
CardName
ReferenceNumber
BankName
InstallmentInfo
```

---

## 12.3 AI Enrichment Alanları

```text
MerchantName
NormalizedDescription
Category
Subcategory
IsSubscriptionCandidate
IsRecurringCandidate
IsEssential
Confidence
Tags
ReasoningSummary
```

---

## 13. Kategori Kapsamı

İlk kategori listesi:

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

Kategori yönetimi MVP'de sistem sabiti olarak başlayabilir.

Sonraki fazda kullanıcı kendi kategorilerini oluşturabilir.

---

## 14. İş Kuralları

## 14.1 Import Kuralları

```text
Tarih alanı zorunludur.
Açıklama alanı zorunludur.
Tutar alanı zorunludur.
Para birimi boş ise TRY kabul edilir.
Direction yoksa tutar işaretine göre belirlenebilir.
Aynı dosya tekrar yüklenirse kullanıcı uyarılmalıdır.
Hatalı satırlar import işlemini tamamen durdurmamalıdır.
```

---

## 14.2 Kategori Kuralları

```text
AI sadece tanımlı kategori listesinden kategori seçmelidir.
Confidence düşükse işlem manuel kontrol gerektirir.
Kullanıcı düzeltmesi AI önerisinin üzerine yazılmalıdır.
Kullanıcı düzeltmesi ileride aynı merchant için öncelikli kullanılmalıdır.
```

---

## 14.3 Abonelik Kuralları

```text
Aynı merchant için en az 2 tekrar aranır.
Aylık abonelik için 25-35 gün aralığı dikkate alınır.
Tutarlar birebir aynı olmak zorunda değildir.
Benzer tutar toleransı uygulanır.
Tek ödeme abonelik sayılmaz.
Kullanıcı yok saydığı adayı tekrar görmemelidir.
```

---

## 14.4 Rapor Kuralları

```text
Rapor verileri backend hesaplarına dayanmalıdır.
LLM hesaplama yapmamalıdır.
LLM sadece yorum üretmelidir.
Rapor içinde veri olmayan alanlar uydurulmamalıdır.
Geçen ay verisi yoksa karşılaştırma yapılmamalıdır.
```

---

## 15. Güvenlik ve Gizlilik Kapsamı

MVP'de uygulanacaklar:

```text
Keycloak ile giriş
JWT doğrulama
UserId bazlı veri izolasyonu
Sensitive alanları loglamama
LLM'e minimal veri gönderme
Dosya metadata takibi
```

MVP sonrası uygulanacaklar:

```text
Dosya silme
Veri anonimleştirme
Kart/IBAN maskeleme
Audit log
Rate limiting
Encryption at rest
```

---

## 16. Observability Kapsamı

MVP'de uygulanacaklar:

```text
Serilog ile structured logging
Graylog merkezi loglama
CorrelationId
Import job logları
AI Service çağrı logları
Exception logları
```

MVP sonrası uygulanacaklar:

```text
OpenTelemetry tracing
Prometheus metrics
Grafana dashboard
AI response time metric
Import duration metric
Low confidence ratio metric
```

---

## 17. Performans Kapsamı

MVP için hedefler:

```text
1.000 transaction içeren dosya kabul edilebilir sürede import edilmelidir.
AI enrichment batch olarak çalışmalıdır.
UI import sırasında kullanıcıya status göstermelidir.
Dashboard verileri backend tarafından aggregate edilmelidir.
```

İleri faz hedefleri:

```text
10.000+ transaction dosyası desteği
Background job queue
Batch AI enrichment
Cache
Pagination
Async processing
```

---

## 18. Hata Yönetimi Kapsamı

Ele alınacak hata tipleri:

```text
Dosya formatı desteklenmiyor
Zorunlu kolon eşleşmedi
Tarih parse edilemedi
Tutar parse edilemedi
AI Service cevap vermedi
LLM invalid JSON döndü
Database kayıt hatası
Yetkisiz erişim
```

Hata yaklaşımı:

```text
Kullanıcıya sade hata mesajı gösterilir.
Teknik detay loglara yazılır.
Import sırasında hatalı satırlar raporlanır.
Başarılı satırlar mümkünse işlenmeye devam eder.
```

---

## 19. Faz Planı

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

Çıktı:

```text
Sisteme login olunabilir.
Boş dashboard açılır.
API ve AI Service health endpointleri çalışır.
Loglar Graylog'a düşer.
```

---

## Faz 2 - Dosya Import

```text
CSV/Excel upload
Kolon eşleştirme
Transaction import
Transaction listeleme
```

Çıktı:

```text
Kullanıcı dosya yükleyip transaction listesini görebilir.
```

---

## Faz 3 - AI Enrichment

```text
Merchant normalize
Kategori tahmini
Confidence skoru
Manuel kategori düzeltme
```

Çıktı:

```text
İşlemler AI ile anlamlandırılır.
Kullanıcı hatalı kategoriyi düzeltebilir.
```

---

## Faz 4 - Abonelik Tespiti

```text
Recurring payment detection
Subscription candidate list
Candidate status management
```

Çıktı:

```text
Sistem tekrar eden ödeme ve abonelik adaylarını gösterir.
```

---

## Faz 5 - Dashboard ve Rapor

```text
Aylık özet
Kategori dağılımı
Top merchants
AI aylık rapor
```

Çıktı:

```text
Kullanıcı aylık finans görünümünü ve AI yorumunu görür.
```

---

## Faz 6 - AI Finans Asistanı

```text
Chat ekranı
Tool calling
Finansal soru-cevap
Chat geçmişi
```

Çıktı:

```text
Kullanıcı kendi harcamaları hakkında doğal dille soru sorabilir.
```

---

## Faz 7 - Gelişmiş Altyapı

```text
Redis
MinIO
Qdrant
OpenTelemetry
Prometheus
Grafana
RabbitMQ
```

Çıktı:

```text
Proje daha ölçeklenebilir ve izlenebilir hale gelir.
```

---

## 20. MVP Kabul Kriterleri

MVP başarılı sayılmak için aşağıdaki kriterleri karşılamalıdır:

```text
Kullanıcı Keycloak ile giriş yapabilmelidir.
CSV veya Excel dosyası yüklenebilmelidir.
Kolon eşleştirme yapılabilmelidir.
Transaction kayıtları PostgreSQL'e yazılmalıdır.
Transaction listesi Angular ekranında görüntülenmelidir.
AI Service merchant ve kategori tahmini yapmalıdır.
Confidence skoru gösterilmelidir.
Kullanıcı kategori düzeltebilmelidir.
Tekrar eden ödemeler tespit edilmelidir.
Abonelik adayları listelenmelidir.
Dashboard aylık özet göstermelidir.
AI aylık rapor üretebilmelidir.
Loglar Graylog'a düşmelidir.
Kullanıcı sadece kendi verisini görebilmelidir.
```

---

## 21. Başarı Metrikleri

Teknik başarı metrikleri:

```text
Import edilen transaction sayısı
Başarılı import oranı
AI kategorileme başarı oranı
Düşük confidence işlem oranı
Kullanıcı düzeltmesi sayısı
Tespit edilen abonelik adayı sayısı
AI Service ortalama cevap süresi
Import job süresi
```

Kullanıcı değeri metrikleri:

```text
Kullanıcının fark ettiği abonelik sayısı
Kategori bazlı harcama görünürlüğü
Aylık raporun anlaşılabilirliği
Manuel düzeltme ihtiyacının azalması
```

---

## 22. Demo Senaryosu

Demo için örnek akış:

```text
1. Kullanıcı sisteme giriş yapar.
2. Örnek banka ekstresi CSV dosyasını yükler.
3. Kolonları eşleştirir.
4. 100 transaction içeri aktarılır.
5. AI enrichment başlatılır.
6. İşlemler merchant ve kategori bilgisiyle güncellenir.
7. 6 abonelik adayı tespit edilir.
8. Dashboard'da aylık harcama dağılımı görünür.
9. Kullanıcı aylık AI raporunu açar.
10. Sistem en önemli 3 içgörüyü gösterir.
```

---

## 23. Örnek AI Rapor Çıktısı

```text
Temmuz ayında toplam harcaman 34.250 TL olarak görünüyor. En yüksek harcama kategorilerin Market, Yeme İçme ve Abonelik oldu.

Abonelik harcamaların aylık 1.240 TL seviyesinde. YouTube Premium, Spotify ve Apple ödemeleri düzenli ödeme adayı olarak tespit edildi.

Yeme İçme harcamalarında geçen aya göre belirgin artış var. Özellikle küçük tutarlı ama sık yapılan harcamalar toplamda ciddi bir etki oluşturmuş.

Bu ay kontrol edilebilecek ilk alanlar abonelikler ve dışarıdan yemek harcamaları gibi görünüyor.
```

---

## 24. Sonuç

SpendLens AI projesinin ilk kapsamı; banka hareketlerini içeri aktarma, AI ile anlamlandırma, abonelik tespiti, dashboard ve aylık rapor üretimi üzerine kuruludur.

Projenin en önemli kapsam kararı şudur:

```text
LLM hesaplama motoru değildir.
LLM anlamlandırma, sınıflandırma ve açıklama üretme katmanıdır.
```

Bu yaklaşım sayesinde proje hem gerçek hayatta işe yarar hem de teknik olarak doğru bir AI-native uygulama örneği olur.
