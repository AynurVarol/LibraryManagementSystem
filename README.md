# LibraryManagementSystem
Bu uygulama, C# kullanarak yapılmış basit bir kütüphane yönetim sistemi konsol uygulamasıdır.
## Uygulama Özellikleri
-Tüm kitapalrı görüntüleme
- Kütüphaneye yeni bir kitap ekleme
- Kütüphanede kitap ismi ile arama yapabilme
- Kütüphanede ödünç verilebilir durumdaki kitapları görüntüleyebilme
- Kitap ödünç verme 
- Ödünç verilmiş olan kitapları görüntüleyebilme
- Ödünç alınmış kitabın geri iade işlemini yapabilme 
- Teslim tarihi geçmiş kitapları görüntüleme
- Çıkış 
## Nasıl Kullanılır?
BU TXT C KONUMUNDAN ALIN
1. Proje dosyasını indirin.
2.Proje dizinindeki terminal istemcisini açın.
3. `donet run` komutunu kullanarak uygulamayı başlatın.
4. Menüden gerekli numaralara basarak yapmak istediğiniz işlemi seçin.
## Önemli Notlar
-Kitap bilgileri, bir metin dosyasında saklanmaktadır.(`C:\LibraryManagementSystem.txt\Libarary1.txt `).
- Uygulamadan çıkış yapıldığında eklenen bilgiler otomatik olarak kaydedilir.
## Geliştirme Aşaması
- Uygulama içinde `Library`, `Book` ve `Program` adında üç sınıf bulunuyor.
-`Book` sınıfı, kitabın başlığı, yazarı, ISBN, kopya sayısı, ödünç alındığı tarih ve ödün alınan kopya sayısı gibi bilgileri içerir. Bu sınıfın nesneleri, `Library` sınıfındaki listelerde kullanılarak kütüphane yönetimi işlemleri gerçekleştirilir.
-`Program` sınıfı kütüphane yönetim sisteminin çalışma akışını kontrol eder ve kullanıcının etkileşimde bulunduğu ana noktadır.
- `Library` sınıfı, kitapları yönetmek ve kitap ekleme, tüm kitapları görüntüleme, yazar ve ya kitap başlığına göre kitap arama, kitrap ödünç verme, ödünç alınan kitabı ,iade etme, son teslim tarihi geçmiş kitapları görüntüleme, kütüphanede bulanan kitapların kopya sayılarını gösterme ve tüm işlemleri kaydetme gibi dosya işlemlerini gerçekleştirmek için kullanılır.
- Kodun daha düzenli ve anlaşılır olması için switch case kullanılmaktadır.
- Uygulamam, konsol üzerinden kullanıcı etkileşimli bir şekilde çalışmaktadır.