using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using TComander.Resourses.DiscObjects;
using TComander.Resourses.View;

namespace TComander {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        //POLA

        /// <summary>
        /// Pole RightListSelected (private) jest używane acy określić które 
        /// pliki zostały wybrane z lewego drzewa za pomocą checkBox
        /// </summary>
        private List<DiscElement> RightListSelected = new List<DiscElement>();
        /// <summary>
        /// Pole LeftListSelected (private) jest używane acy określić które 
        /// pliki zostały wybrane z lewego drzewa za pomocą checkBox
        /// </summary>
        private List<DiscElement> LeftListSelected = new List<DiscElement>();
        /// <summary>
        /// Pole opisujące delegata i określające jaki typ sortowania stosować
        /// </summary>
        private TypeOfSorting sort = TypeSort;

        // DELEGATY

        /// <summary>
        /// Delegat określający typ sortowania
        /// </summary>
        /// <param name="unorderedList">Nieposortowana lista elementó</param>
        /// <returns>Posortowana lista względem odpowiednij funkcji</returns>
        public delegate List<DiscElement> TypeOfSorting(List<DiscElement> unorderedList);

        //KONSTRUKTORY

        /// <summary>
        /// Po inicjalizacji komponentów, wczytuje drzewo i następnie dodaje 
        /// FileSystemWatcher do obu drzew, aby wykrywały przenoszenie plików
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            ResetTree();
            // Inicjalizacja FileSystemWatcher dla lwewgo drzewa
            FileSystemWatcher fileSystemWatcher_left = new FileSystemWatcher(textBox_directoryLeft.Text);
            fileSystemWatcher_left.Created += FileSystemWatcher_Created;
            fileSystemWatcher_left.EnableRaisingEvents = true;
            // Inicjalizacja FileSystemWatcher dla prawego drzewa
            FileSystemWatcher fileSystemWatcher_right = new FileSystemWatcher(textBox_directoryLeft.Text);
            fileSystemWatcher_right.Created += FileSystemWatcher_Created;
            fileSystemWatcher_right.EnableRaisingEvents = true;

        }

        //METODY

        /// <summary>
        /// Matoda wywołująca okno tworzenia nowego folderu
        /// </summary>
        /// <param name="newPathLeft">Lokalizacja lewego drzewa</param>
        /// <param name="newPathRight">Lokalizacja prawego drzewa</param>
        private void CreateDirectory(String newPathLeft, String newPathRight) {
            CreateNewDirectory instance = new CreateNewDirectory(newPathLeft, newPathRight);
            instance.createDirectory += createdirectory;
            instance.Show();
        }
        /// <summary>
        /// Metoda resetująca zawartośc w obdwy drzewach w zależności od ścieżek podanych w text boxach
        /// Dodatkowo resetuje obydwie listy zaznaczonych elementów
        /// </summary>
        private void ResetTree() {
            FillTree(treeView_directoryLeft, textBox_directoryLeft);
            FillTree(treeView_directoryRight, textBox_directoryRight);
            LeftListSelected = new List<DiscElement>();
            RightListSelected = new List<DiscElement>();
        }
        /// <summary>
        /// Metoda słurząca do uzupełniania podanego drzewa , korzystając z textbox  podanego jako parametr
        /// </summary>
        /// <param name="display">Drzewo, które ma być zapełnione</param>
        /// <param name="dirBox">Textbox , z którego będzie pobrana lokalizacja do uzupełniania drzewa</param>
        private void FillTree(TreeView display, TextBox dirBox) {
            TreeViewItem root = createRootNode(dirBox.Text, display);
            display.Items.Clear();
            List<DiscElement> allSubElements = new MyDirectory(dirBox.Text).GetSubDiscElements();
            allSubElements = sort(allSubElements);
            foreach (DiscElement element in allSubElements) {
                root.Items.Add(createNode(element, display));
            }
            display.Items.Add(root);
        }
        /// <summary>
        /// Tworzy korzeń drzewa do którego, będo podłączane pozostałe elementy lokalizacji 
        /// </summary>
        /// <param name="inputPath">Scieżka do elementu</param>
        /// <param name="display">Określenie dla którego drzewa powinien zostać dodany korzeń</param>
        /// <returns>Element TreeVievItem, który jest korzeniem</returns>
        private TreeViewItem createRootNode(String inputPath, TreeView display) {
            MyDirectory rootDirectory = new MyDirectory(inputPath);
            UCDiscElement rootUC = new UCDiscElement(rootDirectory, display == treeView_directoryRight ? true : false);
            rootUC.button_Delete.Visibility = Visibility.Hidden;
            rootUC.checkBox.Visibility = Visibility.Hidden;
            rootUC.fileOpened += DiscElementVievDirectoryOpened;
            TreeViewItem root = new TreeViewItem();
            root.Header = rootUC;
            root.IsExpanded = true;
            return root;
        }
        /// <summary>
        /// Tworzy element drzewa, określający element dysku
        /// </summary>
        /// <param name="element">Scieżka do elementu</param>
        /// <param name="display">Określenie dla którego drzewa powinien zostać dodany element</param>
        /// <returns>Element TreeVievItem</returns>
        private TreeViewItem createNode(DiscElement element, TreeView display) {
            UCDiscElement discElementViev = new UCDiscElement(element, display == treeView_directoryRight ? true : false);
            discElementViev.fileDeleted += DiscElementVievFileDeleted;
            discElementViev.fileChecked += DiscElementVievFileOperand;
            if (element is MyFile) {
                discElementViev.fileOpened += DiscElementVievFileOpened;
            }
            else if (element is MyDirectory) {
                discElementViev.fileOpened += DiscElementVievDirectoryOpened;
            }
            return new TreeViewItem() { Header = discElementViev };
        }
        /// <summary>
        /// Rozwiązuje problem odznaczania i zaznaczania elementów znajdujących się na prawym drzewie
        /// </summary>
        /// <param name="element">Element względem którego rozpatrywana jest zmiana zaznaczenia</param>
        /// <param name="statusChecked">Aktualny stan zaznaczenia obiektu</param>
        private void RightListSelectedOperator(DiscElement element, bool statusChecked) {
            if (statusChecked) {
                RightListSelected.Remove(element);
            }
            else {
                RightListSelected.Add(element);
            }
        }
        /// <summary>
        /// Rozwiązuje problem odznaczania i zaznaczania elementów znajdujących się na lewym drzewie
        /// </summary>
        /// <param name="element">Element względem którego rozpatrywana jest zmiana zaznaczenia</param>
        /// <param name="statusChecked">Aktualny stan zaznaczenia obiektu</param>
        private void LeftListSelectedOperator(DiscElement element, bool statusChecked) {
            if (statusChecked) {
                LeftListSelected.Remove(element);
            }
            else {
                LeftListSelected.Add(element);
            }
        }
        /// <summary>
        /// Metoda usuwająca aktualnie zaznaczone elementy
        /// Obsługa błędów:
        ///     -Plik nie możliwy do usunięcia (Rozwiązaon)
        /// </summary>
        private void Bulck_Delete() {
            List<DiscElement> jointDeck = new List<DiscElement>();
            jointDeck.AddRange(LeftListSelected);
            foreach (DiscElement element in RightListSelected) {
                if (!jointDeck.Exists(e => e.Path == element.Path)) {
                    jointDeck.Add(element);
                }
            }
            String temp = "";
            foreach (DiscElement element in jointDeck) {
                temp += element.Path + "\n";
            }
            if (MessageBox.Show("Delete following files?\n" + temp, "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                String ocurredExeptions = "";
                foreach (DiscElement element in jointDeck) {
                    try {
                        if (element is MyDirectory) {
                            Directory.Delete(element.Path);
                        }
                        else if (element is MyFile) {
                            System.IO.File.Delete(element.Path);
                        }
                        else {
                            throw new NotImplementedException();
                        }
                    }
                    catch (Exception notDeleted) {
                        ocurredExeptions += notDeleted.Message + "\n";
                    }
                }
                if (ocurredExeptions.Length > 0) {
                    MessageBox.Show("Nie udało sie usunąć plików: \n" + ocurredExeptions, "Błąd usuwania");
                }
            }
        }
        /// <summary>
        /// Metoda generująca listę, które elementy są zaznaczone
        /// </summary>
        private void GetSelectedDescription() {
            String temp = "";
            temp += "Prawa : \n";
            foreach (DiscElement element in RightListSelected) {
                temp += element.Path + "\n";
            }
            temp += "Lewa : \n";
            foreach (DiscElement element in LeftListSelected) {
                temp += element.Path + "\n";
            }
            MessageBox.Show(temp);
        }
        /// <summary>
        /// Metoda sł€żąca do przenoszenia elementów z jednej lokacji do drógiej
        /// Obsłużone błędy:
        ///     -Obiekt nie może być przeniesiony(Obsłużono)
        ///     -Lokacja początkowa i końcowa są takie same(Obsłużono)
        /// </summary>
        private void MoveElements() {
            if (textBox_directoryLeft.Text != textBox_directoryRight.Text) {
                String ocurredExeption = "";
                foreach (DiscElement element in LeftListSelected) {
                    try {
                        File.Move(element.Path, textBox_directoryRight.Text + "\\" + element.Name);
                    }
                    catch (Exception imposibleToMove) {
                        ocurredExeption += imposibleToMove.Message + "\n";
                    }
                }
                foreach (DiscElement element in RightListSelected) {
                    try {
                        File.Move(element.Path, textBox_directoryLeft.Text + "\\" + element.Name);
                    }
                    catch (Exception imposibleToMove) {
                        ocurredExeption += imposibleToMove.Message + "\n";
                    }
                }
                if (ocurredExeption.Length > 0) {
                    MessageBox.Show("Nie udało sie przenieśc plików: \n" + ocurredExeption, "Błąd przenoszenia");
                }
                ResetTree();
            }
            else {
                MessageBox.Show("Nie można przenieść do tej samej lokacji");
            }
        }
        /// <summary>
        /// Metoda służaca do kopiowania elementów
        /// </summary>
        private void CopyElements() {
            String ocurredExeption = "";
            if (textBox_directoryLeft.Text != textBox_directoryRight.Text) {
                foreach (DiscElement element in LeftListSelected) {
                    try {
                        File.Copy(element.Path, textBox_directoryRight.Text + "\\" + element.Name);
                    }
                    catch (Exception impossibleToCopy) {
                        ocurredExeption += impossibleToCopy.Message + "\n";
                    }
                }
                foreach (DiscElement element in RightListSelected) {
                    try {
                        File.Copy(element.Path, textBox_directoryLeft.Text + "\\" + element.Name);
                    }
                    catch (Exception impossibleToCopy) {
                        ocurredExeption += impossibleToCopy.Message + "\n";
                    }
                }
                ResetTree();
            }
            else {
                foreach (DiscElement element in LeftListSelected) {
                    try {
                        File.Copy(element.Path, textBox_directoryRight.Text + "\\" + element.Name.Split('.')[0]+"_copy."+ element.Name.Split('.')[1]);
                    }
                    catch (Exception impossibleToCopy) {
                        ocurredExeption += impossibleToCopy.Message + "\n";
                    }
                }
                ResetTree();
            }
            if (ocurredExeption.Length > 0) {
                MessageBox.Show("Nie udało sie skopiować plików: \n" + ocurredExeption, "Błąd kopiowania");
            }
        }
        /// <summary>
        /// Funkcja wyszukująca nazw elementów zaczynająccych się na podany ciąg znaków i wyświetla go na lewym drzewie
        /// </summary>
        /// <param name="startWith">Ciąg znaków po którym będą wyszukiwane obiekty</param>
        private void Search(String startWith) {
            TreeViewItem root = createRootNode(textBox_directoryLeft.Text, treeView_directoryLeft);
            treeView_directoryLeft.Items.Clear();
            List<DiscElement> allSubElements = new MyDirectory(textBox_directoryLeft.Text).GetSubDiscElements();
            allSubElements = allSubElements.Where(element => element.Name.StartsWith(startWith)).ToList();
            foreach (DiscElement element in allSubElements) {
                root.Items.Add(createNode(element, treeView_directoryLeft));
            }
            treeView_directoryLeft.Items.Add(root);
        }

        //METODY PRZYPISANE DO EVENTÓW

        /// <summary>
        /// Metoda obsługująca FileSystemWatcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e) {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { button_Click(this, null); }));
        }
        /// <summary>
        /// Metoda eventu obsługująca zaznaczanie i odznaczanie elementów
        /// </summary>
        /// <param name="discElement">Element względem którego rozpatrywana jest zmiana zaznaczenia</param>
        /// <param name="statusChecked">Aktualny stan zaznaczenia obiektu</param>
        /// <param name="isRightDisplay">Czy drzewo dla którego rozpatrujemy zmiane jest prawe</param>
        private void DiscElementVievFileOperand(DiscElement discElement, bool statusChecked, bool isRightDisplay) {
            if (isRightDisplay) {
                RightListSelectedOperator(discElement, statusChecked);
            }
            else {
                LeftListSelectedOperator(discElement, statusChecked);
            }
        }
        /// <summary>
        /// Metoda eventu obsługująca otwieranie plików
        /// </summary>
        /// <param name="discElement">Element, który zostanie otwary</param>
        /// <param name="nullificate">Parametr zbędny</param>
        private void DiscElementVievFileOpened(DiscElement discElement, bool nullificate) {
            Process process = new Process();
            process.StartInfo.FileName = discElement.Path;
            try {
                process.Start();
            }
            catch (Exception unableToRun) {
                MessageBox.Show(unableToRun.Message);
            }
        }
        /// <summary>
        /// Metoda eventu obsługująca otwieranie folderów
        /// </summary>
        /// <param name="discElement">Element, który zostanie otwary</param>
        /// <param name="nullificate">Czy drzewo dla którego rozpatrujemy zmiane lokalizacji jest prawe</param>
        private void DiscElementVievDirectoryOpened(DiscElement discElement, bool isRightDisplay) {
            if (!isRightDisplay) {
                if (discElement.Path == textBox_directoryLeft.Text) {
                    textBox_directoryLeft.Text = Directory.GetParent(discElement.Path).ToString();
                }
                else {
                    textBox_directoryLeft.Text = discElement.Path;
                }
            }
            else {
                if (discElement.Path == textBox_directoryRight.Text) {
                    textBox_directoryRight.Text = Directory.GetParent(discElement.Path).ToString();
                }
                else {
                    textBox_directoryRight.Text = discElement.Path;
                }
            }
            ResetTree();
        }
        /// <summary>
        /// Metoda przypisana do eveny usuwania i resetuje drzewo po usunięciach
        /// </summary>
        /// <param name="discElement">Usunięty element</param>
        private void DiscElementVievFileDeleted(DiscElement discElement) {
            ResetTree();
        }
        /// <summary>
        /// Metoda przypisana do eventu dodawania folderu
        /// </summary>
        /// <param name="creationPath">ścieżka tworzonego folderu</param>
        private void createdirectory(String creationPath) {
            Directory.CreateDirectory(creationPath);
        }

        // METODY PRZYPISANE DO DELEGATÓW

        /// <summary>
        /// Zwraca liste posortowaną według typów
        /// </summary>
        /// <param name="unorderedList">Nieposortowana lista obiektów</param>
        /// <returns>Posortowana lista obiektów względem typów</returns>
        private static List<DiscElement> TypeSort(List<DiscElement> unorderedList) {
            return unorderedList;
        }
        /// <summary>
        /// Zwraca liste posortowaną według daty powstania rosnąco
        /// </summary>
        /// <param name="unorderedList">Nieposortowana lista obiektów</param>
        /// <returns>Posortowana lista obiektów względem daty powstania rosnąco</returns>
        private static List<DiscElement> DataSortAscendet(List<DiscElement> unorderedList) {
            List<DiscElement>OrderedList = unorderedList.OrderBy(e => e.CreationTime).ToList();
            return OrderedList;
        }
        /// <summary>
        /// Zwraca liste posortowaną według daty powstania malejąco
        /// </summary>
        /// <param name="unorderedList">Nieposortowana lista obiektów</param>
        /// <returns>Posortowana lista obiektów względem daty powstania malejąco</returns>
        private static List<DiscElement> DataSortDescendet(List<DiscElement> unorderedList) {
            List<DiscElement> OrderedList = unorderedList.OrderByDescending(e => e.CreationTime).ToList();
            return OrderedList;
        }
        /// <summary>
        /// Zwraca liste posortowaną według nazwy rosnąco
        /// </summary>
        /// <param name="unorderedList">Nieposortowana lista obiektów</param>
        /// <returns>Posortowana lista obiektów względem daty powstania malejąco</returns>
        private static List<DiscElement> NameSortAscendet(List<DiscElement> unorderedList) {
            List<DiscElement> OrderedList = unorderedList.OrderBy(e => e.Name).ToList();
            return OrderedList;
        }
        /// <summary>
        /// Zwraca liste posortowaną według nazwy malejąco
        /// </summary>
        /// <param name="unorderedList">Nieposortowana lista obiektów</param>
        /// <returns>Posortowana lista obiektów względem daty powstania malejąco</returns>
        private static List<DiscElement> NameSortDescendet(List<DiscElement> unorderedList) {
            List<DiscElement> OrderedList = unorderedList.OrderByDescending(e => e.Name).ToList();
            return OrderedList;
        }

        //OBSŁUGA PRZYCISKÓW

        /// <summary>
        /// Przycisk wywołujący spis zaznaczonych obiektów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e) {
            GetSelectedDescription();
        }
        /// <summary>
        /// Przycisk wywołujący usunięcie zaznaczonych elementów i odświerzenie drzewa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_delet_Click(object sender, RoutedEventArgs e) {
            Bulck_Delete();
            ResetTree();
        }
        /// <summary>
        /// Przycisk wywołujący przeniesienie zaznaczonych elementów do innej lokacji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_move_Click(object sender, RoutedEventArgs e) {
            MoveElements();
            ResetTree();
        }
        /// <summary>
        /// Przycisk wywołujący skopiowanie zaznaczonych elementów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_copy_Click(object sender, RoutedEventArgs e) {
            CopyElements();
            ResetTree();
        }
        /// <summary>
        /// Przycisk wywołujący odznaczenie wszystkich elementów (resetowanie drzewa)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_deselect_Click(object sender, RoutedEventArgs e) {
            ResetTree();
        }
        /// <summary>
        /// Przycisk wywołujący metode szukania w lokalizacji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_search_Click(object sender, RoutedEventArgs e) {
            Search(textBox_search.Text);
        }
        /// <summary>
        /// Przycisk wywołujący tworzenie nowego folderu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click_1(object sender, RoutedEventArgs e) {
            CreateDirectory(textBox_directoryLeft.Text, textBox_directoryRight.Text);
        }

        //OBSŁUGA RADIO BUTONÓW

        /// <summary>
        /// Przypisanie do delegata sort sortowania względem typu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_typeSort_Checked(object sender, RoutedEventArgs e) {
            sort = TypeSort;
        }
        /// <summary>
        /// Przypisanie do delegata sort sortowania względem daty powstania rosnąco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_dataAscent_Checked(object sender, RoutedEventArgs e) {
            sort = DataSortAscendet;
        }
        /// <summary>
        /// Przypisanie do delegata sort sortowania względem daty powstania malejąco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_dataDescend_Checked(object sender, RoutedEventArgs e) {
            sort = DataSortDescendet;
        }
        /// <summary>
        /// Przypisanie do delegata sort sortowania względem nazwy rosnąco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_nameAscend_Checked(object sender, RoutedEventArgs e) {
            sort = NameSortAscendet;
        }
        /// <summary>
        /// Przypisanie do delegata sort sortowania względem nazwy malejąco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_nameDescend_Checked(object sender, RoutedEventArgs e) {
            sort = NameSortDescendet;
        }
    }
}