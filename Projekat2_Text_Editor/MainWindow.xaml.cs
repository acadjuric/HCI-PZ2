using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Projekat2_Text_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Pronadji { get; set; }
        public static string Zameni { get; set; }
        private bool sacuvao = true;
        public List<double> velicina_teksta = new List<double>();
        public Dictionary<string, SolidColorBrush> boje = new Dictionary<string, SolidColorBrush>();


        public MainWindow()
        {
            InitializeComponent();

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontFamily.Text = "Times New Roman";

            dodaj_brojeve_u_list();
            cmbSize.ItemsSource = velicina_teksta;
            cmbSize.Text = 16.ToString();

            dodaj_boje();
            cmbColor.ItemsSource = boje.Keys;
            cmbColor.Text = "Black";
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
            {
                rtbEditor.Focus();
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
                
            }

        }
        private void CmbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSize.SelectedItem != null)
            {
                rtbEditor.Focus();
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbSize.SelectedItem);
                
            }

        }
        private void CmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColor.SelectedItem != null)
            {
                Object temp = cmbColor.SelectedItem;
                string boja_kljuc = temp.ToString();
                SolidColorBrush temp_boja = new SolidColorBrush();
                temp_boja = boje[boja_kljuc];
              
                rtbEditor.Focus();
                rtbEditor.Selection.ApplyPropertyValue(Inline.ForegroundProperty, temp_boja);
                

            }
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            sacuvao = false;
            rtbEditor.Focus();

            //dugme BOLD
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) &&
                (temp.Equals(FontWeights.Bold));

            //CMB FONTOVI
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;


            //dugme ITALIC
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            //CMB VELICINA SLOVA
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbSize.SelectedItem = temp;


            //dugme underline
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderLine.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            //cmb COLOR
            temp = rtbEditor.Selection.GetPropertyValue(Inline.ForegroundProperty);

            foreach (KeyValuePair<string, SolidColorBrush> kp in boje)
            {
                if (kp.Value == temp)
                {
                    cmbColor.SelectedItem = kp.Key;
                    break;
                }
                if (temp.ToString() == "#FF000000")
                {
                    cmbColor.SelectedItem = "Black";
                    break;
                }

            }



            string ceo_tekst = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            int broj_reci = Izbroj_reci(ceo_tekst);

            tbStatusBar.Text = "Broj reči: " + broj_reci;
            rtbEditor.Focus();

        } 

        private int Izbroj_reci(string text)
        {
            int brojac = 0;
            string izraz = "[A-Za-z]{2,60}";
            Regex re = new Regex(izraz);
            MatchCollection mc = re.Matches(text);
            brojac = mc.Count;
            return brojac;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rezultat = MessageBox.Show("Da li zelite da sacuvate?", "Pitanje", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (rezultat == MessageBoxResult.Yes)
            {
                BtnSave_Click(sender, e);
                this.Close();
            }
            else if (rezultat == MessageBoxResult.No)
            { this.Close(); }



        }


        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li zelite da sacuvate?", "Pitanje", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BtnSave_Click(sender, e);
            }
            rtbEditor.Document.Blocks.Clear();

        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {




            OpenFileDialog fp = new OpenFileDialog();
            fp.Filter = "Text files (*.txt)|*.txt|RTF files (*.rtf)|*.rtf" ;
            fp.Title = "Izaberite fajl";
            if (fp.ShowDialog() != null)
            {
                if (fp.FileName != "")
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fp.FileName);
                    if (sacuvao == false)
                    {
                        if (MessageBox.Show("Da li zelite da sacuvate?", "Pitanje", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            BtnSave_Click(sender, e);
                            sacuvao = true;

                        }
                    }
                    rtbEditor.Document.Blocks.Clear();
                    rtbEditor.AppendText(sr.ReadToEnd());
                    sr.Close();


                    TextPointer txt = rtbEditor.CaretPosition;
                    txt = txt.DocumentEnd;
                    rtbEditor.CaretPosition = txt;
                }
            }



        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Text files (*.txt)|*.txt|RTF files (*.rtf)|*.rtf";
            sf.Title = "Izaberite fajl";
            if (sf.ShowDialog() != null)
            {
                if (sf.FileName != "")
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(sf.FileName);
                    string temp = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
                    sw.Write(temp,DataFormats.Rtf);
                    sw.Close();

                }
            }

        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            Find_and_Replace fr = new Find_and_Replace();
            fr.ShowDialog();
            string pronadji, zameni;
            pronadji = Pronadji; Pronadji = string.Empty;
            zameni = Zameni; Zameni = string.Empty;

            ZameniOneKojePronadjes(pronadji, zameni);
        }



        private void ZameniOneKojePronadjes(string onajKojiSeTrazi, string onajKojimSeMenja)
        {

            TextRange text = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            TextPointer current = text.Start.GetInsertionPosition(LogicalDirection.Forward);
           
            while (current != null)
            {
                string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                if (!string.IsNullOrWhiteSpace(textInRun))
                {
                    int index = textInRun.IndexOf(onajKojiSeTrazi);
                    if (index != -1)
                    {
                        
                        TextPointer selectionStart = current.GetPositionAtOffset(index, LogicalDirection.Forward);
                        TextPointer selectionEnd = selectionStart.GetPositionAtOffset(onajKojiSeTrazi.Length, LogicalDirection.Forward);
                        TextRange selection = new TextRange(selectionStart, selectionEnd);
                        var velicina = selection.GetPropertyValue(FontSizeProperty);
                        var font = selection.GetPropertyValue(FontFamilyProperty);
                        var bold = selection.GetPropertyValue(FontWeightProperty);
                        var italic = selection.GetPropertyValue(FontStyleProperty);
                        var boja = selection.GetPropertyValue(ForegroundProperty);
                        var underline = selection.GetPropertyValue(Inline.TextDecorationsProperty);

                        selection.Text = onajKojimSeMenja;
                        selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

                        selection.ApplyPropertyValue(TextElement.FontSizeProperty, velicina);
                        selection.ApplyPropertyValue(TextElement.FontFamilyProperty, font);
                        selection.ApplyPropertyValue(TextElement.FontWeightProperty, bold);
                        selection.ApplyPropertyValue(TextElement.FontStyleProperty, italic);
                        selection.ApplyPropertyValue(TextElement.ForegroundProperty, boja);
                        selection.ApplyPropertyValue(Inline.TextDecorationsProperty, underline);


                        rtbEditor.Selection.Select(selection.Start, selection.End);
                        rtbEditor.Focus();
                    }
                }
                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }

        }

        private void BtnDate_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.CaretPosition.InsertTextInRun(DateTime.Now.ToString());
            TextPointer txt = rtbEditor.CaretPosition;
            txt = txt.DocumentEnd;
            rtbEditor.CaretPosition = txt;

        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                string vreme = DateTime.Now.ToString();
                rtbEditor.CaretPosition.InsertTextInRun(vreme);
                TextPointer txt = rtbEditor.CaretPosition;
                txt = txt.DocumentEnd;
                rtbEditor.CaretPosition = txt;
            }


        }

        

        private void dodaj_brojeve_u_list()
        {
            for (double i = 8; i <= 100; i += 2) { velicina_teksta.Add(i); }
        }

        private void dodaj_boje()
        {

            boje.Add("Red", Brushes.Red); boje.Add("Green", Brushes.Green); boje.Add("Blue", Brushes.Blue); boje.Add("Yellow", Brushes.Yellow);
            boje.Add("Pink", Brushes.Pink); boje.Add("Gray", Brushes.Gray);
            boje.Add("Black", Brushes.Black); boje.Add("Khaki", Brushes.Khaki); 
            boje.Add("Fuchisa", Brushes.Fuchsia); boje.Add("Purple", Brushes.Purple); boje.Add("Orange", Brushes.Orange);
            boje.Add("Aqua", Brushes.Aqua); boje.Add("Brown", Brushes.Brown); boje.Add("Gold", Brushes.Gold); boje.Add("Silver", Brushes.Silver);
            boje.Add("Navy", Brushes.Navy); boje.Add("Magneta", Brushes.Magenta); boje.Add("Maroon", Brushes.Maroon);

        }

        
    }
}
