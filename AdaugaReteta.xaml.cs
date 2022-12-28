using System;
using System.Collections.Generic;
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
using System.Linq;
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
        string User;
        public AdaugaReteta(string user)
        {
            InitializeComponent();
            Prompt.Foreground = Brushes.Gray;
            Prompt1.Foreground = Brushes.Gray;
            User = user;
            Persoana.Content = user;
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

        private void UpdateWords()
        {
            var context = new Organizator_ReteteEntities();
            var cuvinte = ret.Denumire.Split(' ');
            foreach (var token in cuvinte)
            {
                bool ok = false;
                int id = -1;
                var res = (from c in context.Cuvinte_cheie
                              where c.Cuvant == token
                              select c).First();
                    if(res!=null)
                    {
                        ok = true;
                        id = res.CuvantCheieID;
                    }
                Retete_Cuvinte ret_cuv;
                if (ok == false)
                {
                    Cuvinte_cheie cuvant = new Cuvinte_cheie
                    {
                        Cuvant = token
                    };
                    var context1 = new Organizator_ReteteEntities();
                    context1.Cuvinte_cheie.Add(cuvant);
                    context1.SaveChanges();
                    ret_cuv = new Retete_Cuvinte
                    {
                        RetetaID = ret.RetetaID,
                        CuvantcheieID = cuvant.CuvantCheieID
                    };
                }
                else
                {
                    ret_cuv = new Retete_Cuvinte
                    {
                        RetetaID = ret.RetetaID,
                        CuvantcheieID = id
                    };

                }
                var context2 = new Organizator_ReteteEntities();
                context2.Retete_Cuvinte.Add(ret_cuv);
                context2.SaveChanges();
            }

        }

        private void AdaugaLista(object sender, RoutedEventArgs e)
        {
            var context = new Organizator_ReteteEntities();
            if (ingrediente.Count == 0)
            {
                Error.Visibility = Visibility.Visible;
                Error.Content = "EROARE-0 ingrediente";
                Error.Foreground = Brushes.Red;
                return;
            }
            var res = from r in context.Retetes
                      where r.Denumire == Reteta.Text.ToString()
                      select r;

            if (res.Count() != 0)
            {

                    Error.Visibility = Visibility.Visible;
                    Error.Content = "EROARE-Reteta exista deja";
                    Error.Foreground = Brushes.Red;
                    return;
            }
            int id = -1;
            var user = (from u in context.Useris
                       where u.Username == User
                       select u).First();
            if(user!=null)
            {
                    id = user.UserId;
            }
            ret = new Retete
            {
                Denumire = Reteta.Text,
                UserId = id
            };
            var context1 = new Organizator_ReteteEntities();
            context1.Retetes.Add(ret);
            context1.SaveChanges();
            UpdateWords();
            
            foreach(var ing in ingrediente)
            {
                bool ok = false;
                var query1 = from c in context.Ingredientes
                                  where c.Denumire == ing.Key
                                  select c;
                if(query1.Count()!=0)
                {
                    foreach (var i in query1)
                    {
                        ReteteIngrediente ri = new ReteteIngrediente
                        {
                            RetetaID = ret.RetetaID,
                            IngredientID = i.IngredientID,
                            Cantitate = ing.Value.Key,
                            Unitate = ing.Value.Value
                        };
                        context.ReteteIngredientes.Add(ri);
                        ok = true;
                    }
                }
                    if(ok==false)
                    {
                        Ingrediente ingredient = new Ingrediente
                        {
                            Denumire = ing.Key,
                        };
                        context.Ingredientes.Add(ingredient);
                        context.SaveChanges();
                        ReteteIngrediente ri = new ReteteIngrediente
                        {
                            RetetaID = ret.RetetaID,
                            IngredientID = ingredient.IngredientID,
                            Cantitate = ing.Value.Key,
                            Unitate = ing.Value.Value
                        };
                        context.ReteteIngredientes.Add(ri);
                    }
            }
            context.SaveChanges();
            Reteta.Text = "";
            Error.Visibility = Visibility.Visible;
            Error.Content = "Rețetă adăugată";
            buton.Visibility = Visibility.Hidden;
            Pasi.Visibility = Visibility.Visible;
            backbutton.Visibility = Visibility.Hidden;
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
            var context = new Organizator_ReteteEntities();
            foreach(var pas in pasi)
            {
                Pasi p = new Pasi
                {
                    RetetaID = ret.RetetaID,
                    Actiune = pas
                };
                context.Pasis.Add(p);
            }
            context.SaveChanges();
            Stack2.Visibility = Visibility.Hidden;
            Stack3.Visibility = Visibility.Visible;
        }
        private void Adauga(object sender, RoutedEventArgs e)
        {
            var context = new Organizator_ReteteEntities();
            Stack3.Visibility = Visibility.Hidden;
            if (VideoLink.ToString() != "")
            {
                Videouri video = new Videouri
                {
                    Adresa = VideoLink.Text.ToString(),
                    RetetaID = ret.RetetaID
                };
                context.Videouris.Add(video);
            }
            if (PhotoLink.ToString() != "")
            {
                Imagini imagine = new Imagini
                {
                    Calea_Imaginii = PhotoLink.Text.ToString(),
                    RetetaID = ret.RetetaID
                };
                context.Imaginis.Add(imagine);
            }
            context.SaveChanges();
            Main.Navigate(new ContulMeu("IonutCorbu"));
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            //Main.Navigate(new Login(Persoana.Content.toString()));
        }

        private void back(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new ContulMeu("IonutCorbu"));
        }
    }
}
