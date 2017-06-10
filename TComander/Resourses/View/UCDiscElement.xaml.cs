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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TComander.Resourses.DiscObjects;

namespace TComander.Resourses.View {
    /// <summary>
    /// Interaction logic for UCDiscElement.xaml
    /// </summary>
    public partial class UCDiscElement : UserControl {
        /// <summary>
        /// Element dysku do którego jest tworzony User Controler
        /// </summary>
        public DiscElement UCElement;
        /// <summary>
        /// Flaga oznaczająca czy User Controler znajduje się na prawym drzewie
        /// </summary>
        public bool isRightDispaly;
        /// <summary>
        /// Flaga oczaczająca czy element jest zaznaczony do operacji
        /// </summary>
        public bool ifIsChecked;

        /// <summary>
        /// Konstruktor User Controlera
        /// Przypisuje odpowiednią ikone i odpowiednio formatuje guziki zależnie od typu elementu
        /// </summary>
        /// <param name="newUCElement">Element do którego jest tworzony User Controler</param>
        /// <param name="display">Zienna pytającza czy User Controler dotyczy prawego drzewa</param>
        public UCDiscElement(DiscElement newUCElement,bool display) {
            this.isRightDispaly = display;
            this.UCElement = newUCElement;
            this.ifIsChecked = false;
            InitializeComponent();
            textBlock_Name.Text = UCElement.Name;
            textBlock_CreationTime.Text = UCElement.CreationTime.ToString();
            if(UCElement is MyDirectory) {
                image_Icon.Source = MyDirectory.icon.Source;
            }else if(UCElement is MyFile) {
                image_Icon.Source = MyFile.icon.Source;
            }
            else {
                throw new NotImplementedException();
            }
            HidingPrewievButton();
            
        }

        /// <summary>
        /// Metoda modyfikująca przycisk podglądu zależnie od typu elementu
        /// </summary>
        private void HidingPrewievButton() {
            if(!UCElement.Name.EndsWith(".txt")&&!UCElement.Name.EndsWith(".jpg")&&!UCElement.Name.EndsWith(".bmp")&&!UCElement.Name.EndsWith(".gif")&&!UCElement.Name.EndsWith(".html")) {
                button_preview.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Delegat obsługujący event usuwania elementu
        /// </summary>
        /// <param name="discElement">Element do usunięcia</param>
        public delegate void FileDeletedEvent(DiscElement discElement);
        /// <summary>
        /// Event przypisany do usuwania obiektu
        /// </summary>
        public event FileDeletedEvent fileDeleted;

        /// <summary>
        /// Przycisk wywołujący akcje usunięcia elementu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Delete_Click(object sender, RoutedEventArgs e) {
            try {
                if (MessageBox.Show("Delete this element?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                    if(UCElement is MyDirectory) {
                        Directory.Delete(UCElement.Path);
                    }else if(UCElement is MyFile) {
                        System.IO.File.Delete(UCElement.Path);
                    }else {
                        throw new NotImplementedException();
                    }
                }
            }catch(Exception inpossibleToDelete) {
                MessageBox.Show("Nie można usunąć: \n" + inpossibleToDelete.Message);
            }
            if(fileDeleted != null) {
                fileDeleted.Invoke(UCElement);
            }
        }

        /// <summary>
        /// Delegat obsługujący event otwierania elementu
        /// </summary>
        /// <param name="discElement">Element do otwarcia</param>
        /// <param name="display">Zienna pytającza czy element dotyczy prawego drzewa</param>
        public delegate void FileOpenedEvent(DiscElement discElement,bool display);
        /// <summary>
        /// Event przypisany do otwierania obiektu
        /// </summary>
        public event FileOpenedEvent fileOpened;

        /// <summary>
        /// Przycisk wywołujący akcje otwarcia elementu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Open_Click(object sender, RoutedEventArgs e) {
            if(fileOpened != null) {
                fileOpened.Invoke(UCElement, isRightDispaly);
            }
        }

        /// <summary>
        /// Delegat obsługujący event zaznaczania i odznaczania elementu
        /// </summary>
        /// <param name="discElement">Element do otwarcia</param>
        /// <param name="ifIsChecked">Aktualna flaga elementu</param>
        /// <param name="display">Zienna pytającza czy element dotyczy prawego drzewa</param>
        public delegate void FileCheckedEvent(DiscElement discElement,bool ifIsChecked,bool display);
        /// <summary>
        /// Event przypisany do zaznaczania i odznaczania obiektu
        /// </summary>
        public event FileCheckedEvent fileChecked;

        /// <summary>
        /// Przycisk wywołujący akcje zaznaczania lub odznaczania elementu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Checked(object sender, RoutedEventArgs e) {
            if(fileChecked != null) {
                fileChecked.Invoke(UCElement , ifIsChecked , isRightDispaly);
                ifIsChecked = !ifIsChecked;
            }
        }

        /// <summary>
        /// Przycisk wywołującu akcje podglądu obiektu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_preview_Click(object sender, RoutedEventArgs e) {
            if (UCElement.Name.EndsWith(".txt")|| UCElement.Name.EndsWith(".html")) {
                PrevievTxt test = new PrevievTxt(UCElement.Path);
                test.Show();
            }else{
                PreviewIamge test = new PreviewIamge(UCElement.Path);
                test.Show();
            }

        }
    }
}
