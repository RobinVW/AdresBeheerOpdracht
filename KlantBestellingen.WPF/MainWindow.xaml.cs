﻿using AdresBeheerOpdracht.Model;
using KlantBestellingen.WPF.Languages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace KlantBestellingen.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        private Klanten _customerWindow = new Klanten();
        private Producten _productsWindow = new Producten();
        private BestellingDetail _bestellingDetailWindow = new BestellingDetail();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            _customerWindow.Closing += _Window_Closing;
            _productsWindow.Closing += _Window_Closing;
            _bestellingDetailWindow.Closing += _Window_Closing;
            _bestellingDetailWindow.Update += Refresh;

            //als het het venster laden, initieel de bestellingButton disable aangezien de textbox dan toch empty is
            BtnNieuweBestelling.IsEnabled = false;
        }

        /// <summary>
        /// We verbergen de vensters in plaats van ze te sluiten: alles blijft klaarstaan; dit is sneller en efficienter bij vensters die maar eenmaal op het scherm komen tegelijkertijd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Sluit het venster niet echt af en verberg het: we kruipen tussen en vertellen WPF dat het afsluiten al gebeurd is
            // We moeten de Hide() uitvoeren op de UI-thread (main thread)
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate (object o)
            {
                /* Nuttige code: */
                ((Window)sender).Hide();
                /* ... tot hier! */
                return null;
            }, null);
            // We zeggen nu dat de closing event afgehandeld is aan WPF:
            e.Cancel = true;
        }

        /// <summary>
        /// We sluiten de applicatie volledig af wanneer het hoofdvenster gesloten wordt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_Klanten_Click(object sender, RoutedEventArgs e)
        {
            if(_customerWindow != null)
                _customerWindow.Show();
        }

        private void MenuItem_Producten_Click(object sender, RoutedEventArgs e)
        {
            if (_productsWindow != null)
                _productsWindow.Show();
        }

        /// <summary>
        /// We sluiten de volledige applicatie af
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSluiten_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Als je een string toekrijgt, controleer dan steeds of deze wel een bruikbare waarde heeft bij aanvang (preconditie)
            if(string.IsNullOrEmpty(tbKlant.Text))
            {
                BtnNieuweBestelling.IsEnabled = false;
                cbKlanten.ItemsSource = null;
                return;
            }
            else
            {
                BtnNieuweBestelling.IsEnabled = true;
            }
            // Tip: maak dit case insensitive voor "meer punten" ;-) Nog beter: reguliere expressies gebruiken
            var klanten = Context.KlantManager.GeefKlanten(k => k.Naam.ToLower().Contains(tbKlant.Text.ToLower()));
            cbKlanten.ItemsSource = klanten;
            // Indien er effectief klanten zijn, maak dan dat de eerste klant in de lijst meteen voorgeselecteerd is in de combobox:
            if(klanten.Count > 0)
            {
                cbKlanten.SelectedIndex = 0; // het 0-de item is de eerste klant want C# is zero-based
            
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh(sender, e);
        }

        /// <summary>
        /// Tip: interessant voor eindopdracht!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Refresh(object sender, EventArgs e)
        {
            if (cbKlanten.SelectedItem != null)
            {
                // Indien er een klant geselecteerd is, dan tonen we de bestellingen van die klant
                
                var bestellingen = Context.BestellingManager.GeefBestellingen(b => b.Klant.Equals(cbKlanten.SelectedItem));
                dgOrderSelection.ItemsSource = bestellingen;
            }
            else
            {
                // Indien er geen klant geselecteerd is, tonen we geen bestellingen
                dgOrderSelection.ItemsSource = null;
            }
        }

        private void MaakBestelling_Click(object sender, RoutedEventArgs e)
        {
            // Indien het detailvenster voor de bestelling bestaat en dit bestaat eigenlijk altijd, en er is een klant geselecteerd, dan toon ik het venster voor die klant:
            if (_bestellingDetailWindow == null || cbKlanten.SelectedIndex < 0)
            {
                return;
            }
            

            _bestellingDetailWindow.Klant = cbKlanten.SelectedItem as AdresBeheerOpdracht.Model.Klant;
            _bestellingDetailWindow.Order = null;
            _bestellingDetailWindow.Show();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*DataGridRow row = sender as DataGridRow;
            row.Background = new SolidColorBrush(Colors.GreenYellow);*/
            _bestellingDetailWindow.Klant = cbKlanten.SelectedItem as AdresBeheerOpdracht.Model.Klant;
            _bestellingDetailWindow.Order = dgOrderSelection.SelectedItem as AdresBeheerOpdracht.Model.Bestelling; 
            _bestellingDetailWindow.Show();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var bestelling = dgOrderSelection.SelectedItem as AdresBeheerOpdracht.Model.Bestelling;
            int count = 0;
            if(bestelling != null)
            {
                count = bestelling.GeefProducten().Count;
                TbStatusInformation.Text = Translations.OrderProductCount + count + Translations.ProductProducts;
            }
            if(count == 0)
            {
                TbStatusInformation.Text = "";
            }
        }
        /*private void MenuItem_Dutch_Click(object sender, RoutedEventArgs e)
        {
            Translations.Culture = new System.Globalization.CultureInfo("nl-BE");
            
        }

        private void MenuItem_English_Click(object sender, RoutedEventArgs e)
        {
            Translations.Culture = new System.Globalization.CultureInfo("en-US");
        }*/

    }
}
