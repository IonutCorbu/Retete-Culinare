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
    /// Interaction logic for ArataRetete.xaml
    /// </summary>
    public partial class ArataRetete : Page
    {
        string User;
        int id_user;
        Retete ret;
        public ArataRetete(string user)
        {

            InitializeComponent();
            User = user;
            Persoana.Content = user;
            var context = new Organizator_ReteteEntities();
            foreach(var us in context.Useris)
            {
                if (us.Username == User)
                {
                    id_user = us.UserId;
                    break;
                }
            }
            foreach (var ret in context.Retetes)
            {
                if (ret.UserId == id_user)
                {
                    Button but = new Button();
                    but.Content = ret.Denumire;
                    but.Click += treatclick;
                    but.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8B, 0x9E, 0xB7));
                    but.Width = reteta.Width;
                    but.Height = Math.Min(reteta.Height / context.Retetes.Count(), 40);
                    but.FontSize = 20;
                    but.HorizontalContentAlignment = HorizontalAlignment.Left;
                    but.FontWeight = FontWeights.Bold;
                    but.BorderBrush = Brushes.Black;
                    but.BorderThickness = new Thickness(3);
                    reteta.Children.Add(but);
                }
            }
           
        }
       
        private void treatclick(object sender, RoutedEventArgs e)
        {
            var context = new Organizator_ReteteEntities();
            Button but = (Button)sender;
            main.Visibility = Visibility.Hidden;
            stack_reteta.Visibility = Visibility.Visible;
            nume_reteta.Content = but.Content;
            ret=null;
            foreach (var r in context.Retetes)
            {
                if(r.Denumire==nume_reteta.Content.ToString())
                {
                    ret = r;
                    break;
                }
            }
            foreach(var image in context.Imaginis)
            {
                if (ret.RetetaID == image.RetetaID)
                    imagine.Source = new BitmapImage(new Uri(image.Calea_Imaginii));
            }
            int column = 0;
            int rows = 0;
            RowDefinition row;
            foreach (var ingredient in context.ReteteIngredientes)
            {
                if(ingredient.RetetaID==ret.RetetaID)
                {
                    Label l = new Label();
                    l.Content = "- "+ingredient.Cantitate + ingredient.Unitate + " ";
                    foreach(var ing in context.Ingredientes)
                    {
                        if (ingredient.IngredientID == ing.IngredientID)
                        {
                            l.Content += ing.Denumire;
                            break;
                        }
                    }
                    l.Margin = new Thickness(20,0, 0, 0);
                    l.FontSize = 13;
                    l.FontStyle = FontStyles.Italic;
                    Grid.SetColumn(l, column);
                    Grid.SetRow(l, rows);
                    ingrediente.Children.Add(l);
                    if (column == 0)
                    {
                        column = 1;
                        row = new RowDefinition();
                        row.Height = new GridLength(1.0, GridUnitType.Star);
                        ingrediente.RowDefinitions.Add(row);
                    }
                    else
                    {
                        column = 0;
                        rows++;
                    }
                     
                    }
            }
            int i = 1;
            foreach(var pas in context.Pasis)
            {
                if(pas.RetetaID==ret.RetetaID)
                {
                    Label pa = new Label();
                    pa.Content = "Pas " + i + ": " + pas.Actiune;
                    pa.FontWeight = FontWeights.Bold;
  
                    descriere.Children.Add(pa);
                    i++;
                }
            }
            foreach (var vid in context.Videouris)
            {
                if (vid.RetetaID == ret.RetetaID)
                {
                    hyperlink.Visibility = Visibility.Visible;
                    break;
                }
            }



        }
        private void onclick(object sender,RoutedEventArgs e)
        {
            var context = new Organizator_ReteteEntities();
            foreach (var vid in context.Videouris)
            {
                if (vid.RetetaID == ret.RetetaID)
                {
                    System.Diagnostics.Process.Start(vid.Adresa);
                    break;
                }
            }
            
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            //Main.Navigate(new Login(Persoana.Content.toString()));
        }

        private void back(object sender, RoutedEventArgs e)
        {
            descriere.Children.Clear();
            stack_reteta.Visibility = Visibility.Hidden;
            main.Visibility = Visibility.Visible;
            hyperlink.Visibility = Visibility.Hidden;
            ingrediente.Children.Clear();
            ingrediente.RowDefinitions.Clear();
        }
        private void back1(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new ContulMeu("IonutCorbu"));
        }
        
        private void search_handler(object sender,RoutedEventArgs e)
        {
            reteta.Children.Clear();
            var context = new Organizator_ReteteEntities();
            if (Search.Text=="")
            {
                foreach (var ret in context.Retetes)
                {

                    Button but = new Button();
                    but.Content = ret.Denumire;
                    but.Click += treatclick;
                    but.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8B, 0x9E, 0xB7));
                    but.Width = reteta.Width;
                    but.Height = Math.Min(reteta.Height / context.Retetes.Count(), 40);
                    but.FontSize = 20;
                    but.HorizontalContentAlignment = HorizontalAlignment.Left;
                    but.FontWeight = FontWeights.Bold;
                    but.BorderBrush = Brushes.Black;
                    but.BorderThickness = new Thickness(3);
                    reteta.Children.Add(but);
                }
                return;
            }
            var search_content = Search.Text.Split(' ');
            foreach (var ret in context.Retetes)
            {
                foreach (string cuv in search_content)
                {
                    if (ret.Denumire.ToUpper().Contains(cuv.ToUpper())&& ret.UserId == id_user)
                    {
                        Button but = new Button();
                        but.Content = ret.Denumire;
                        but.Click += treatclick;
                        but.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8B, 0x9E, 0xB7));
                        but.Width = reteta.Width;
                        but.Height = Math.Min(reteta.Height / context.Retetes.Count(), 40);
                        but.FontSize = 20;
                        but.HorizontalContentAlignment = HorizontalAlignment.Left;
                        but.FontWeight = FontWeights.Bold;
                        but.BorderBrush = Brushes.Black;
                        but.BorderThickness = new Thickness(3);
                        reteta.Children.Add(but);
                        break;
                    }
                }
            }
        }


    }
}
