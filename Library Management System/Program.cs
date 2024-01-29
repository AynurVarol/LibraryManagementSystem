using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Library_Management_System
{
    public enum ChooseType
    {
        ShowAllBooks = 0,
        AddBook = 1,
        FindBook = 2,
        PrintAccesableBooks = 3, //ödünç verilebilir kitapları ekrana yazdır
        ToLendBooks = 4, //Ödünç Kitap
        PrintBorrowedBooks = 5,//ödünç kitapları ekrana yazdır
        BookReturn = 6,//iade
        OverdueBooks = 7,//teslimi geçmiş
        Save = 8,
        Exit = 9
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }

        public DateTime dueDate { get; set; } //ne zaman ödünç alındı
        public int CopyCount { get; set; } //kopya sayısı
        public int borrowedCount { get; set; }

      


    }
    class Program
    {

        static void Main(string[] args)
        {

            Library library = new Library();
            ChooseType type = ChooseType.ShowAllBooks;

            bool isOpen = true;
            while (isOpen)
            {
                Console.WriteLine("Hoşgeldiniz");
                Console.WriteLine("Yapmak istediğiniz işlemi seçiniz: ");

                Console.WriteLine("1: Tüm kitapları görüntüle");
                Console.WriteLine("2: Kitap Ekle");
                Console.WriteLine("3: Kitap ismi ara");
                Console.WriteLine("4: ödünç verilebilir kitaplar:");
                Console.WriteLine("5: Kitap Ödünç verme");
                Console.WriteLine("6: Ödünç alınan kitapları ekrana yazdır");
                Console.WriteLine("7: Kitap iadesi yap");
                Console.WriteLine("8: Son teslim tarihi geçmiş kitaplar");
                Console.WriteLine("9: Kaydet");
                Console.WriteLine("10: Çıkış yap");

                String choose = Console.ReadLine();

                int typeInt = Convert.ToInt32(choose);
                type = (ChooseType)(typeInt - 1);

                // string lendingBookTitle;

                switch (type)
                {
                    case ChooseType.ShowAllBooks:
                        library.PrintAllBooks();
                        break;
                    case ChooseType.AddBook:
                        AddBook(library);
                        break;
                    case ChooseType.FindBook:
                        Console.WriteLine("Arama yapmak istediğiniz kitap adını veya yazar adını girin:");
                        FindBook(library);
                        break;

                    case ChooseType.ToLendBooks:
                        Console.WriteLine("Ödünç verilmek istenen kitabın adını giriniz:");
                        ToLendBook(library);
                        break;

                    case ChooseType.PrintBorrowedBooks:
                        library.PrintBorrowedBooks();
                        break;

                    case ChooseType.BookReturn:
                        Console.WriteLine("iade istenen kitabın adını giriniz:");
                        BookReturn(library);
                        break;

                    case ChooseType.OverdueBooks:
                        library.PrintOverdueBooks();
                        break;

                    case ChooseType.PrintAccesableBooks:
                        Console.WriteLine("Ödünç alabileceğiniz kitaplar:");
                        library.PrintAccesableBooks();
                        break;

                    case ChooseType.Save:
                        library.SaveToTextFile();
                        break;


                    case ChooseType.Exit:
                        library.SaveToTextFile();
                        isOpen = false;
                        break;
                }



            }



        }


        private static void AddBook(Library library)
        {
            Console.WriteLine("Kitap Adı:");
            string title = Console.ReadLine();

            Console.WriteLine("Yazar:");
            string author = Console.ReadLine();

            Console.WriteLine("ISBN:");
            string isbn = Console.ReadLine();

            Console.WriteLine("Kopya Sayısı:");
            if (int.TryParse(Console.ReadLine(), out int copyCount))
            {

                Book newBook = new Book // Yeni kitap oluitur
                {
                    Author = author,
                    Title = title,
                    ISBN = isbn,
                    CopyCount = copyCount,
                    borrowedCount = 0,
                    dueDate = DateTime.MinValue


                };
                library.AddBook(newBook);



            }
            else
            {
                Console.WriteLine("Hatalı giriş! Lütfen tam sayı giriniz:");
            }

        }

        /*private static void FindBook(Library library) //kitap ismi arama ve bulma 
        {
            string titleNameToFind = Console.ReadLine();
            Book foundBook = library.FindBookWithTitle(titleNameToFind);
            if (foundBook != null)
            {
                Console.WriteLine("Kitap mevcut:" + foundBook.Title + ", Kopya sayısı: " + foundBook.CopyCount);

            }
            else
            {
                Console.WriteLine("Aradığınız kitap bulunamadı.");
            }
        }*/

        /// <summary>
        /// Kitap ismi arama, bulma ve ekrana yazdırma
        /// </summary>
        /// <param name="library"></param>
        private static void FindBook(Library library) 
        {
         
            string searchTerm = Console.ReadLine();

            List<Book> foundBooks = library.FindBooksWithTitleOrAuthor(searchTerm);

            if (foundBooks.Count > 0)
            {
                Console.WriteLine("Arama sonuçları:");

                foreach (var book in foundBooks)
                {
                    Console.WriteLine($"Kitap adı: {book.Title}, Yazar: {book.Author}, Kopya sayısı: {book.CopyCount}");
                }
            }
            else
            {
                Console.WriteLine("Aradığınız kitap veya yazar bulunamadı.");
            }
        }






        private static void ToLendBook(Library library) //ödünç kitap verme kodunu çağırmak için 
        {
            string lendingBookTitle;

            lendingBookTitle = Console.ReadLine();

            Book lendingBook = library.accessableBooks.FirstOrDefault(book => book.Title.Trim() == lendingBookTitle.Trim());
            if (lendingBook != null && lendingBook.CopyCount > 1)
            {
                library.BorrowBook(lendingBook);
                Console.WriteLine("Kitap başarıyla ödünç verildi.");
            }
            else if (lendingBook == null)
            {
                Console.WriteLine("Kitap bulunamadı.");
            }
            else if (lendingBook.CopyCount <= 1) // SORU 1den küçük yada 1 e eşitse kopya yeterli edğil ödünç vermiyor ya bunu 0 a eşit yapsak?
            {
                Console.WriteLine("Bu kitap ödünç verilemez.Kopya sayısı yeterli değil.");
            }


        }

        private static void BookReturn(Library library) //İADE
        {
            string lendingBookTittle = Console.ReadLine();
            Book wantedBook = library.FindBookWithTitle(lendingBookTittle);

            if (wantedBook != null)
            {
                library.ReturnBook(wantedBook);
            }

        }

        
    }

    public class Library
    {
        public string filePath = @" C:\LibraryManagementData\Library1.txt";
        public List<Book> allBooks;
        public List<Book> borrowedBooks;
        public List<Book> accessableBooks;
       


        public Library()
        {
            AllBooks();
        }

        public Book CopyBook(Book willCopy)
        {
            // Yeni bir Book nesnesi oluştur ve değerleri kopyala
            Book copiedBook = new Book
            {
                Author = willCopy.Author,
                Title = willCopy.Title,

                CopyCount = willCopy.CopyCount,
                borrowedCount = willCopy.borrowedCount,
                dueDate = willCopy.dueDate

            };
                 
           
              return copiedBook;

        }
        public Book CopyAndAddToAccessableBooks(Book newBook)
        {
            Book accessibleBook = CopyBook(newBook);
            accessableBooks.Add(accessibleBook);

            return accessibleBook;
        }

        // Yeni fonksiyon: CopyAndAddToBorrowedBooks
        public Book CopyAndAddToBorrowedBooks(Book newBook)
        {
            Book borrowableBook = CopyBook(newBook);
            borrowedBooks.Add(borrowableBook);

            return borrowableBook;
        }


        public void AllBooks() //Tüm Kitaplar
        {

            allBooks = new List<Book>();
            borrowedBooks = new List<Book>();
            accessableBooks = new List<Book>();
            

            // Dosya var mı kontrol et
            if (File.Exists(filePath))
            {
                // Dosyayı satır satır okumak için 
                string[] satirlar = File.ReadAllLines(filePath);

                foreach (var satir in satirlar)
                {
                    // Satırdaki bilgileri virgül ile ayır
                    string[] kitapBilgileri = satir.Split(',');

                    // Yeni bir Kitap nesnesi oluştur ve bilgileri ata
                    Book newBook = new Book
                    {

                        Author = kitapBilgileri[0],
                        Title = kitapBilgileri[1],
                        ISBN = kitapBilgileri[2],
                        CopyCount = int.Parse(kitapBilgileri[3]),// CopyCount eklendi ve dönüştürüldü
                        borrowedCount = int.Parse(kitapBilgileri[4]),
                        dueDate = DateTime.Parse(kitapBilgileri[5])


                    };
                    // Kitabın durumuna göre listelere ekleme
                    if (newBook.dueDate == DateTime.MinValue)
                    {
                        // accessableBooks listesine kopya ekleyerek yeni kitap eklemek 
                        Book accessableBooks = CopyAndAddToAccessableBooks(newBook);
                    }
                    else
                    {

                        if (newBook.CopyCount - newBook.borrowedCount > 0)
                        {
                           // accessableBooks listesine kopya ekleyerek yeni kitap ekleme
                           Book accessibleBook = CopyAndAddToAccessableBooks(newBook);

                        }

                        // borrowedBooks listesine kopya ekleyerek yeni kitap ekleme
                        Book borrowableBook = CopyAndAddToBorrowedBooks(newBook);


                    }

                    // Kitabı genel listeye ekle
                    allBooks.Add(newBook);


                }



            }
              
        
        }

        

        /// <summary>
        /// bu fonksiyon kitaplarla ilgili tüm bilgileri konsola yazdırır
        /// </summary>
        public void PrintAllBooks()
        {
            // Kitapları ekrana yazdırmak için:
            foreach (var book in allBooks)
            {
                if(book.dueDate == DateTime.MinValue)
                {
                    Console.WriteLine($"Yazar:{book.Author}, " +
                    $"Kitap Adı:{book.Title}, " +
                    $"ISBN:{book.ISBN}, " +
                    $"Kopya Sayısı:{book.CopyCount}, "+
                    $"Ödünç Alınan:{book.borrowedCount}, ");
                }
                
            }
        }
        /// <summary>
        /// ödünç alınmış olan kitaplarla ilgili bilgileri konsola yazdırır
        /// </summary>
        public void PrintBorrowedBooks()
        {
            foreach (var book in borrowedBooks)
            {
                Console.WriteLine($"Yazar:{book.Author}, " +
                    $"Kitap Adı:{book.Title}, " +
                    $"ISBN:{book.ISBN}, " +
                   $"Kopya Sayısı:{book.CopyCount}");
            }

            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("Ödünç alınmış bir kitap yok.");
            }

        }
        public void PrintAccesableBooks()
        {
            foreach (var book in accessableBooks)
            {
                Console.WriteLine($"Yazar:{book.Author}, " +
                    $"Kitap Adı:{book.Title}, " +
                    $"ISBN:{book.ISBN}, " +
                    $"Kopya Sayısı:{book.CopyCount}");
            }
        }
        /// <summary>
        /// ödünç kitap vermek için gerekli kodlar burada
        /// </summary>
        /// <param name="book"></param>
        public void BorrowBook(Book book)
        {
            if (book.CopyCount > 0)
            {


                int index = accessableBooks.IndexOf(book);
                accessableBooks[index].CopyCount--; //1 kopya ödünç verildiği için kopya sayısını azalt

                Book borrowedBook = CopyBook(book);
                borrowedBook.dueDate = DateTime.Now.AddDays(7);
                borrowedBook.CopyCount = 1;
                borrowedBook.borrowedCount = 1;
                borrowedBooks.Add(borrowedBook);


                Book baseBook = allBooks.FirstOrDefault(book2 => book2.Title.Trim() == book.Title.Trim());
               
                baseBook.borrowedCount++;

                allBooks.Add(borrowedBook);


            }

            else
            {
                Console.WriteLine("Almak istediğiniz kitap kütüphanemizde kalmamıştır.");
            }

        }


        /// <summary>
        /// ödünç alınan kitabı iade etmek için gerekli kısım
        /// </summary>
        /// <param name="book"></param>
        public void ReturnBook(Book book) //iade yapma
        {


            int index = accessableBooks.IndexOf(book);
            accessableBooks[index].CopyCount++; //iade edilen kitap için kopya sayısını 1 arttır
            borrowedBooks[index].CopyCount--;

            Console.WriteLine("Kitap iade işlemi gerçekleşti");
        }
        public Book FindBookWithTitle(string name)//kitap iade için arama
        {
            foreach (var book in allBooks)
            {
                if (String.Equals(book.Title.Trim(), name.Trim()))
                {
                    return book;
                }
            }
            return null;
        }


        /// <summary>
        /// kitap ve yazar ismine göre arama yapma
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 

        /*public Book FindBookWithAuthor(string name)
        {
            foreach (var book in allBooks)
            {
                if (String.Equals(book.Author.Trim(), name.Trim()))
                {
                    return book;
                }
            }
            return null;
        }*/
        public List<Book> FindBooksWithTitleOrAuthor(string searchTerm) 
        {
            List<Book> foundBooks = new List<Book>();

            foreach (var book in allBooks)
            {
                if (book.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 || book.Author.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundBooks.Add(book);
                }
            }

            return foundBooks;
        }

        /// <summary>
        /// kitap eklme
        /// </summary>
        /// <param name="newBook"></param>

        public void AddBookToFile(Book newBook)
        {
            Book existingBook = allBooks.Find(b => b.Title.Trim().Equals(newBook.Title.Trim()));
            if (existingBook != null)
            {
                existingBook.CopyCount += newBook.CopyCount;
                Console.WriteLine("Var olan bir kitap eklendi. Kopya sayısı arttırıldı.");


            }
            else
            {
                allBooks.Add(newBook);
                Console.WriteLine("Yeni kitap ekleme işlemi başarıyla gerçekleşti.");
            }



        }

        /// <summary>
        ///  ödünç alınmış kitapları döngü ile kontrol eder ve geçmiş teslim tarihine sahip olanları ekrana yazdırır.
        /// </summary>
        public void PrintOverdueBooks()
        {
            DateTime currentDate = DateTime.Now;

            Console.WriteLine("Geçmiş Teslim Tarihine Sahip Kitaplar:");
            foreach (var book in borrowedBooks)
            {
                if (book.dueDate < currentDate)
                {
                    Console.WriteLine($"Yazar:{book.Author}, " +
                                      $"Kitap Adı:{book.Title}, " +
                                      $"ISBN:{book.ISBN}, " +
                                      $"Teslim Tarihi:{book.dueDate}");
                }
            }
        }
        public void AddBook(Book newBook)
        {
            Book existingBook = allBooks.Find(b => b.Title.Trim().Equals(newBook.Title.Trim()));
            if (existingBook != null)
            {
                existingBook.CopyCount += newBook.CopyCount;
                Console.WriteLine("Var olan bir kitap eklendi. Kopya sayısı arttırıldı.");

                // Var olan kitabın kopya sayısı arttığı için accessableBooks listesine ekleniyor.

                if (!accessableBooks.Contains(existingBook))
                {
                    accessableBooks.Add(existingBook);
                }

                else
                {  // Var olan kitap accessableBooks listesinde zaten varsa indexini bul
                    int index = accessableBooks.FindIndex(b => b.Title.Trim().Equals(newBook.Title.Trim()));
                    if (index != -1) //index -1 değilse 
                    {
                        // Var olan kitabın kopya sayısını arttır
                        //accessableBooks[index].CopyCount += newBook.CopyCount;
                    }

                    else
                    {
                        // accessableBooks listesinde olmayan bir kitap, listeye ekle
                        accessableBooks.Add(newBook);
                        Console.WriteLine("Yeni kitap eklendi.");
                    }
                }




            }

            else
            {
                allBooks.Add(newBook);
                // Yeni bir kitap ekleniyor
                accessableBooks.Add(newBook);


                Console.WriteLine("Yeni kitap eklendi.");
            }
        }

       

        /// <summary>
        /// verileri kaydetmek için 
        /// </summary>
        public void SaveToTextFile()
        {
            File.WriteAllText(filePath, string.Empty);

            foreach (var book in allBooks)
            {

                using (StreamWriter sw = new StreamWriter(filePath, true)) //File.AppendText(file_path))
                {
                    sw.WriteLine($"{book.Author}, " +
                                $"{book.Title}, " +
                                $"{book.ISBN}, " +
                                $"{book.CopyCount}, " +
                                $"{book.borrowedCount}, " +
                                $"{book.dueDate}"

                                );
                }
            }
        }


    }

   

}




