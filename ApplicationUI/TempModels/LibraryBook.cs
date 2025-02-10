using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUI.TempModels
{
    public class LibraryBook
    {
        public int Id { get; set; } // щоби після обрання в ListBox певної книги для закачування користувачем, можна було легше дізнатися яку саме закачувати
        public string Name { get; set; } // Назва книги
        public string Author { get; set; } // Автор (вся string, яка відноситься до Author)
        public string FilePath { get; set; } // Шлях до завантаженого файлу на пристрої
        public string CoverURL { get; set; } // url посилання на фото буде вже на самій сторінці, коли переходиться на певну книгу)
        public string BookPageLink { get; set; }
    }
}
