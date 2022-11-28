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
    /// Interaction logic for Pagina_Principala.xaml
    /// </summary>
    public partial class Pagina_Principala : Page
    {
        string User;
        public Pagina_Principala(string user)
        {
            InitializeComponent();
            Persoana.Foreground = Brushes.White;
            logout.Foreground = Brushes.White;
            copyright.Foreground = Brushes.Gray;
            Persoana.Content = user;
            User = user;
        }
        

        private void ArataRetete(object sender,RoutedEventArgs e)
        {
            Main.Navigate(new ArataRetete("IonutCorbu"));
        }

        private void Profilulmeu(object sender, RoutedEventArgs e)
        {
           Main.Navigate(new ContulMeu("IonutCorbu"));
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            //Main.Navigate(new Login(Persoana.Content.toString()));
        }
    }
}
