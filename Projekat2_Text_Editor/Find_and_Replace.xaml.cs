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

namespace Projekat2_Text_Editor
{
    /// <summary>
    /// Interaction logic for Find_and_Replace.xaml
    /// </summary>
    public partial class Find_and_Replace : Window
    {

        private bool rez = false;

        public Find_and_Replace()
        {
            InitializeComponent();
        }

        private void BtnZavrsi_Click(object sender, RoutedEventArgs e)
        {
            if (provera())
            {
                MainWindow.Pronadji = textboxPronadji.Text;
                MainWindow.Zameni = textboxZameni.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Molimo vas popunite polja!", "Greksa", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool provera()
        {
            rez = true;

            if (textboxPronadji.Text.Equals(string.Empty))
            {
                rez = false;
                textboxPronadji.BorderBrush = Brushes.Red;
                textboxPronadji.BorderThickness = new Thickness(1);
            }
            else
            {
                textboxPronadji.BorderBrush = Brushes.Green;
                textboxPronadji.BorderThickness = new Thickness(1);
            }

            if (textboxZameni.Text.Equals(string.Empty))
            {
                rez = false;
                textboxZameni.BorderBrush = Brushes.Red;
                textboxZameni.BorderThickness = new Thickness(1);
            }
            else
            {
                textboxZameni.BorderBrush = Brushes.Green;
                textboxZameni.BorderThickness = new Thickness(1);
            }
            return rez;

        }


    }
}
