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
    /// Interaction logic for AdaugaReteta.xaml
    /// </summary>
    public partial class AdaugaReteta : Page
    {
        Dictionary<string,KeyValuePair<int,string>> ingrediente = new Dictionary<string, KeyValuePair<int, string>>();
        Retete ret;
        List<string> pasi = new List<string>();
        public AdaugaReteta()
        {
            InitializeComponent();
            Titlu.Foreground = Brushes.DarkSalmon;
            Prompt.Foreground = Brushes.Gray;
            Prompt1.Foreground = Brushes.Gray;
        }

        private void AdaugaIngredient(object sender, RoutedEventArgs e)
        {
            var denumire = Ingredient.Text.ToString();
            var cantitate=0;
            if(Cantitate.Text!="")
                cantitate = Int32.Parse(Cantitate.Text);
            if (cantitate == 0)
            {
                    Error.Content = "EROARE-Nu exista cantitate";
                    Error.Foreground = Brushes.Red;
                    return;
            }
            else
            {
                foreach(var ing in ingrediente)
                {
                    if (ing.Key == denumire)
                    {
                        Error.Content = "EROARE-Ingredientul a fost deja adaugat";
                        Error.Foreground = Brushes.Red;
                        return;
                    }
                }
                KeyValuePair<int, string> pereche = new KeyValuePair<int, string>(cantitate, Unitate.Text.ToString());
                ingrediente.Add(denumire, pereche);
                Ingredient.Text = "";
                Cantitate.Text = "";
                Unitate.Text = "";
            }
            
        }

        private void AdaugaLista(object sender, RoutedEventArgs e)
        {
            var context = new Organizator_ReteteDataContext();
            if (ingrediente.Count() == 0)
            {
                Error.Visibility = Visibility.Visible;
                Error.Content = "EROARE-0 ingrediente";
                Error.Foreground = Brushes.Red;
                return;
            }
            foreach (var reteta in context.Retetes)
                if(reteta.Denumire==Reteta.Text.ToString())
                {
                    Error.Visibility = Visibility.Visible;
                    Error.Content = "EROARE-Reteta exista deja";
                    Error.Foreground = Brushes.Red;
                    return;
                }
            
            ret = new Retete
            {
                Denumire = Reteta.Text
            };
            context.Retetes.InsertOnSubmit(ret);
            context.SubmitChanges();
            
            foreach(var ing in ingrediente)
            {
                bool ok = false;
                foreach (var i in context.Ingredientes)
                {
                    if (ing.Key == i.Denumire)
                    {
                        ReteteIngrediente ri = new ReteteIngrediente
                        {
                            RetetaID = ret.RetetaID,
                            IngredientID = i.IngredientID,
                            Cantitate = ing.Value.Key,
                            Unitate=ing.Value.Value
                        };
                        context.ReteteIngredientes.InsertOnSubmit(ri);
                        context.SubmitChanges();
                        ok = true;
                        break;
                    }
                }
                    if(ok==false)
                    {
                        Ingrediente ingredient = new Ingrediente
                        {
                            Denumire = ing.Key,
                        };
                        context.Ingredientes.InsertOnSubmit(ingredient);
                        context.SubmitChanges();
                        ReteteIngrediente ri = new ReteteIngrediente
                        {
                            RetetaID = ret.RetetaID,
                            IngredientID = ingredient.IngredientID,
                            Cantitate = ing.Value.Key,
                            Unitate = ing.Value.Value
                        };
                        context.ReteteIngredientes.InsertOnSubmit(ri);
                        context.SubmitChanges();
                    }
            }
            Reteta.Text = "";
            Error.Visibility = Visibility.Visible;
            Error.Content = "Rețetă adăugată";
            Pasi.Visibility = Visibility.Visible;
        }

        private void AdaugaPasi(object sender, RoutedEventArgs e)
        {
            Stack1.Visibility = Visibility.Hidden;
            Stack2.Visibility = Visibility.Visible;

        }

        private void AdaugaPas(object sender, RoutedEventArgs e)
        {
            if (Pas.Text.ToString() != "")
            {
                pasi.Add(Pas.Text.ToString());
                Pas.Text = "";
                NrPas.Content = Int32.Parse(NrPas.Content.ToString()) + 1;
                Error1.Content = "Pas adăugat cu succes";
            }
            else
            {
                Error1.Content = "ERROR: Descriere invalida";
                return;
            }
        }

        private void finalizeaza(object sender, RoutedEventArgs e)
        {
            var context = new Organizator_ReteteDataContext();
            foreach(var pas in pasi)
            {
                Pasi p = new Pasi
                {
                    RetetaID = ret.RetetaID,
                    Actiune = pas
                };
                context.Pasis.InsertOnSubmit(p);
            }
            context.SubmitChanges();
            Stack2.Visibility = Visibility.Hidden;
            Stack3.Visibility = Visibility.Visible;
        }
    }
}
