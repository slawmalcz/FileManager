using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TComander.Resourses.DiscObjects {
    class MyFile : DiscElement{
        /// <summary>
        /// CreationTime - data utworzenia folderu
        ///     odczyt  PUBLIC
        /// </summary>
        public override DateTime CreationTime {
            get {
                return File.GetCreationTime(Path);
            }
        }
        /// <summary>
        /// Name - nazwa folderu
        ///     odczyt  PUBLIC
        /// </summary>
        public override string Name {
            get {
                return System.IO.Path.GetFileName(Path);
            }
        }
        /// <summary>
        /// zmienna tzrymająca ikone folderu
        /// </summary>
        public static Image icon = new Image();

        /// <summary>
        /// Konstruktor klasy plików. Jego głównym zadaniem jest przekazanie zmiennej
        /// do klasy nadrzędnej i przypisanie do obiektu ikony
        /// </summary>
        /// <param name="path">ścieżka do pliku</param>
        public MyFile(String path) : base(path) {
            icon.Source = new BitmapImage(new Uri("C:\\Users\\Mindi\\Desktop\\POB\\MySolution\\TComander\\TComander\\Resourses\\DiscObjects\\Images\\File.bmp"));
        }

        /// <summary>
        /// Generuje krótki opis dla elemetu uwzględniającu nazwe i datę powstania
        /// </summary>
        /// <returns>Opis elementu</returns>
        public override string GetDescription() {
            return Name + " Creation time: " + CreationTime;
        }
    }
}
