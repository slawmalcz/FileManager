using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TComander.Resourses.View {
    /// <summary>
    /// Interaction logic for PrevievTxt.xaml
    /// </summary>
    public partial class PrevievTxt : Window {
        /// <summary>
        /// Konstruktor okan słurzącego do podglądu tekstu typu:
        ///     -txt
        ///     -html
        /// </summary>
        /// <param name="path">ścieżka do pliku</param>
        public PrevievTxt(String path) {
            InitializeComponent();
            this.Title = path;
            String filling = "";
            filling = File.ReadAllText(path);
            textBlock.Text = filling;
        }
    }
}
