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
        public Pagina_Principala()
        {
            InitializeComponent();
            Titlu.Foreground = Brushes.DarkSalmon;
        }
        

        private void ArataRetete(object sender,RoutedEventArgs e)
        {
            Main.Navigate(new ArataRetete());
        }

        private void AdaugaLista(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new AdaugaReteta());
        }
    }
}
