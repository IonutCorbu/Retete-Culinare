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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proiect
{
    /// <summary>
    /// Interaction logic for ContulMeu.xaml
    /// </summary>
    public partial class ContulMeu : Page
    {
        string User;
        public ContulMeu(string user)
        {
            InitializeComponent();
            User = user;
            Persoana.Content = user;
            copyright.Foreground = Brushes.Gray;
            copyright.Opacity = 0.7;
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            //Main.Navigate(new Login(Persoana.Content.toString()));
        }
        private void Arata_retete(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new ArataRetete("IonutCorbu"));
        }
        private void Adaugaretete(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new AdaugaReteta("IonutCorbu"));
        }
        private void back(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new Pagina_Principala("IonutCorbu"));
        }
    }
}
