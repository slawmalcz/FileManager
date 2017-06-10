using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TComander.Resourses.DiscObjects {
    class MyDirectory : DiscElement {
        /// <summary>
        /// CreationTime - data utworzenia folderu
        ///     odczyt  PUBLIC
        /// </summary>
        public override DateTime CreationTime {
            get {
                return Directory.GetCreationTime(Path);
            }
        }
        /// <summary>
        /// Name - nazwa folderu
        ///     odczyt  PUBLIC
        /// </summary>
        public override string Name {
            get {
                String temp = "";
                try
                {
                    temp = Path.Replace(Directory.GetParent(Path).ToString(), "");
                    if (temp[0] == '\\')
                    {
                        temp = temp.Replace("\\", "");
                    }
                }
                catch
                {

                }
                return temp;
            }
        }
        /// <summary>
        /// NumberOfSubDirectories - ilość podfolderów, które zawiera dany folder
        ///     odczyt  PUBLIC
        /// </summary>
        public int NumberOfSubDirectories {
            get {
                return Directory.GetDirectories(Path).Length;
            }
        }
        /// <summary>
        /// NumberOfRecursiveFiles - ilość plików, które zawiera folder
        ///     odczyt  PUBLIC        /// </summary>
        public int NumberOfRecursiveFiles {
            get {
                return GetAllFilesRecursively().Count();
            }
        }

        /// <summary>
        /// zmienna tzrymająca ikone folderu
        /// </summary>
        public static Image icon = new Image();

        /// <summary>
        /// Konstruktor klasy folderów. Jego głównym zadaniem jest przekazanie zmiennej
        /// do klasy nadrzędnej i przypisanie do obiektu ikony
        /// </summary>
        /// <param name="directoryPath">Scierzka dostępu do folderu</param>
        public MyDirectory(String directoryPath) : base(directoryPath) {
            icon.Source = new BitmapImage(new Uri("C:\\Users\\Mindi\\Desktop\\POB\\MySolution\\TComander\\TComander\\Resourses\\DiscObjects\\Images\\Directory.bmp"));
        }

        /// <summary>
        /// Metoda zwracająca wszystkie elementy folderu, czyli foldery i pliki
        /// </summary>
        /// <returns>Lista wszystkich elementów danego folderu</returns>
        public List<DiscElement> GetSubDiscElements() {
            List<DiscElement> resoult = new List<DiscElement>();
            resoult.AddRange(GetAllFiles());
            resoult.AddRange(GetSubDirectories());
            return resoult;
        }
        /// <summary>
        /// Metoda zwracająca wszystkie pliki zawarte w folderze
        /// </summary>
        /// <returns>Lista wszystkich pliki zawartych w folderze</returns>
        public List<MyFile> GetAllFiles() {
            String[] subFiles = Directory.GetFiles(Path);
            List<MyFile> resoult = new List<MyFile>();
            foreach (String file in subFiles) {
                resoult.Add(new MyFile(file));
            }
            return resoult;
        }
        /// <summary>
        /// Metoda zwracająca wszystkie pod-foldery zawarte w folderze
        /// </summary>
        /// <returns>Lista wszystkich pod-folderów zawartych w folderze</returns>
        public MyDirectory[] GetSubDirectories() {
            String[] subDirectories = Directory.GetDirectories(Path);
            List<MyDirectory> resoult = new List<MyDirectory>();
            foreach (String directory in subDirectories) {
                resoult.Add(new MyDirectory(directory));
            }
            return resoult.ToArray();
        }

        /// <summary>
        /// Metoda zwracająca wszystkie pod-foldery zawarte w folderze rekurencyjnie
        /// </summary>
        /// <returns>Lista wszystkich pod-folderów zawartych w folderach</returns>
        public List<MyDirectory> GetSubDirectoriesRecursively() {
            String[] subDirectories = Directory.GetDirectories(Path);
            List<MyDirectory> resoult = new List<MyDirectory>();
            foreach (String directory in subDirectories) {
                try {
                    MyDirectory subDirectory = new MyDirectory(directory);
                    resoult.Add(subDirectory);
                    resoult.AddRange(subDirectory.GetSubDirectoriesRecursively());
                }
                catch {

                }

            }
            return resoult;
        }
        /// <summary>
        /// Metoda zwracająca wszystkie pliki zawarte w folderze rekurencyjnie
        /// </summary>
        /// <returns>Lista wszystkich plików zawartych w folderach</returns>
        public List<MyFile> GetAllFilesRecursively() {
            List<MyDirectory> subDirectories = GetSubDirectoriesRecursively();
            List<MyFile> resoult = new List<MyFile>();
            foreach (MyDirectory directory in subDirectories) {
                resoult.AddRange(directory.GetAllFiles());
            }
            return resoult;
        }

        /// <summary>
        /// Metoda generująca opis danego folderu: Nazwa , Czas Utworzenia i ilość pod folderów
        /// </summary>
        /// <returns>Opis folderu</returns>
        public override string GetDescription() {
            return Name + " Creation time: " + CreationTime + " Number of sub-directories: " + NumberOfSubDirectories;
        }
    }
}
