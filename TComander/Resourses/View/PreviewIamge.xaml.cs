using System;
using System.Collections.Generic;
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
    /// Interaction logic for PreviewIamge.xaml
    /// </summary>
    public partial class PreviewIamge : Window {
        /// <summary>
        /// Konstruktor okan słurzącego do podglądu obrazu typu:
        ///     -jpg
        ///     -png
        ///     -gif
        ///</summary>
        /// <param name="path">ścieżka dostępu do pliku</param>
        public PreviewIamge(String path) {
            InitializeComponent();
            this.Title = path;
            Image previev = new Image();
            previev.Source = new BitmapImage(new Uri(path));
            this.Height = previev.Height;
            this.Width = previev.Width;
            image_previev.Source = previev.Source;
        }
    }
}
