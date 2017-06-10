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
    /// Interaction logic for CreateNewDirectory.xaml
    /// </summary>
    public partial class CreateNewDirectory : Window {
        /// <summary>
        /// Konstruktor okna generowania nowego folderu w jednej z dwuch zadanych lokalizacji
        /// </summary>
        /// <param name="directoryLeft">Lokalizacja nr.1</param>
        /// <param name="directoryRight">Lokalizacja nr.2</param>
        public CreateNewDirectory(String directoryLeft, String directoryRight) {
            InitializeComponent();
            textBox_nazwa.Text = "Nowy Folder";
            radioButton_left.Content = directoryLeft;
            if (directoryLeft != directoryRight) {
                radioButton_right.Content = directoryRight;
            }else {
                radioButton_right.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// Delegat obsługujący eveny tworzenia nowego folderu
        /// </summary>
        /// <param name="creationPath">ścieżka nowo tworzonego folderu</param>
        public delegate void DirectoryCreateEvent(String creationPath);
        /// <summary>
        /// Event przypisany do tworzenia nowego folderu
        /// </summary>
        public event DirectoryCreateEvent createDirectory;

        /// <summary>
        /// Pzrycisk wysyłający żądanie utworzenia nowego folderu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_utworz_Click(object sender, RoutedEventArgs e) {
            String test = "";
            if (radioButton_left.IsChecked == true) {
                test += radioButton_left.Content;
                test += "\\" + textBox_nazwa.Text;
            }else {
                test += radioButton_right.Content;
                test += "\\" + textBox_nazwa.Text;
            }
            createDirectory.Invoke(test);
            this.Close();
        }

        /// <summary>
        /// Przycisk anulujący tworzenie folderu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_anuluj_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
