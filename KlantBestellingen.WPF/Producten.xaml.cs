using AdresBeheerOpdracht.Model;
using KlantBestellingen.WPF.Languages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace KlantBestellingen.WPF
{

    /// <summary>
    /// Interaction logic for Producten.xaml
    /// </summary>
    public partial class Producten : Window
    {
        // Interface INotifyPropertyChanged
        private ObservableCollection<Product> _producten = null;
        private ObservableCollection<Bestelling> _bestellingen = null;
        private List<Product>_productenInBestellingen = new List<Product>();

        public Producten()
        {
            InitializeComponent();
            _producten = new ObservableCollection<Product>(Context.ProductManager.GeefProducten());
            dgProducten.ItemsSource = _producten;
            _producten.CollectionChanged += _producten_CollectionChanged;
            _bestellingen = new ObservableCollection<Bestelling>(Context.BestellingManager.GeefBestellingen());
            foreach(var b in _bestellingen)
            {
                foreach(var p in b.GeefProducten().Keys.ToList<Product>()){
                    _productenInBestellingen.Add(p);
                }
            }
        }

        /// <summary>
        /// Doorgeven aan business laag dat klant werd toegevoegd of verwijderd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _producten_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Product product in e.OldItems)
                {
                    if (!_productenInBestellingen.Contains(product)) {
                        Context.ProductManager.VerwijderProduct(product);
                    }
                    else
                    {
                        MessageBox.Show(Translations.RemoveProductFromOrder, Translations.RemoveOrderError, MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Product product in e.NewItems)
                {
                    Context.ProductManager.VoegProductToe(product);
                }
            }
        }

        /// <summary>
        /// Kruip tussen wanneer de gebruiker met de delete toets een rij verwijdert uit een DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgProducten_PreviewDeleteCommandHandler(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (!(MessageBox.Show("Zeker dat je het product wenst te verwijderen?", "Bevestig.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                // Cancel Delete.
                e.Handled = true;
            }
        }

        private void BtnNieuwProduct_Click(object sender, RoutedEventArgs e)
        {
            // Preconditie
            if (string.IsNullOrEmpty(TbProductNaam?.Text) || string.IsNullOrEmpty(TbProductPrijs?.Text))
            {
                MessageBox.Show(Translations.AllProductDetails);
                return;
            }

            var product = new Product(TbProductNaam.Text, Double.Parse(TbProductPrijs.Text));
            // Omdat we een ObservableCollection<Klant> gebruiken, wordt onze wijziging meteen doorgegeven naar de gui (.Items wijzigen zou threading problemen geven):
            // Omdat we ObservableCollection<Klant> gebruiken en er een event gekoppeld is aan delete/add hiervan, wordt ook de business layer aangepast!
            _producten.Add(product);
        }

        private void Tb_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbProductNaam.Text) && !string.IsNullOrEmpty(TbProductPrijs.Text))
            {
                BtnNieuwProduct.IsEnabled = true;
            }
            else
            {
                BtnNieuwProduct.IsEnabled = false;
            }
        }
    }
}
