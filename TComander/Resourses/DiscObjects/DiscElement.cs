using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TComander.Resourses.DiscObjects {
    public abstract class DiscElement {
        /// <summary>
        /// Path - ścieżka do podanego elementu
        ///     odczyt  PUBLIC
        ///     zapis   PRIVATE
        /// </summary>
        public String Path {
            get;
            private set;
        }
        /// <summary>
        /// CreationTime - data utworzenia elementu
        ///     odczyt  PUBLIC
        /// </summary>
        public abstract DateTime CreationTime {
            get;
        }
        /// <summary>
        /// Name - nazwa obiektu
        ///     odczyt  PUBLIC
        /// </summary>
        public abstract String Name {
            get;
        }

        /// <summary>
        /// Podstawowy konstruktor klasy wykorzystujący ścieżke dostępu
        /// </summary>
        /// <param name="path">Scieżka dostępu</param>
        public DiscElement(String path) {
            this.Path = path;
        }

        /// <summary>
        /// Funkcja abstrakcyjna zwracająca zformatowane dane do wyświetlenia w listBox
        /// </summary>
        /// <returns>(String) opis elementu</returns>
        public abstract String GetDescription();
    }
}
