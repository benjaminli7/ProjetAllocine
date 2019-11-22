using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Entity;
using gestionBDD;
using Windows.Devices.Geolocation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace projetAllociné
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }


        private Cinema cinema;
        private gstBDD bdd;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            cinema = e.Parameter as Cinema;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bdd = new gstBDD();
            txtBlockNomCine.Text = "Pour le cinéma " + cinema.nomCine;
            gvFilms.ItemsSource = bdd.getListFilmsByIdCinema(cinema.codeCine);
            txtBlockAdresse.Text = "Adresse : " + cinema.adresseCine;
            txtBlockLatitude.Text = "Latitude : " + cinema.latitudeCine.ToString();
            txtBlockLongitude.Text = "Longitude : " + cinema.longitudeCine.ToString();
            BasicGeoposition snPosition = new BasicGeoposition { Latitude = cinema.latitudeCine, Longitude = cinema.longitudeCine };
            Geopoint snPoint = new Geopoint(snPosition);

            var MyLandmarks = new List<MapElement>();
            var spaceNeedleIcon = new MapIcon
            {
                Location = snPoint,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                ZIndex = 0,
                Title = cinema.nomCine
            };
            MyLandmarks.Add(spaceNeedleIcon);
            var LandmarksLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = MyLandmarks
            };
            map.Layers.Add(LandmarksLayer);
            map.Center = snPoint;
            map.ZoomLevel = 18;
        }

        private void GvFilms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(gvFilms.SelectedItem != null)
            {
                gvActeurs.ItemsSource = null;
                gvActeurs.ItemsSource = bdd.getListActeurByFilm(((gvFilms.SelectedItem) as Film).codeFilm);
            }
            txtBlockNbrVote.Text = bdd.getNbrVote((gvFilms.SelectedItem as Film).codeFilm).ToString();
            txtBlockVoteTotal.Text = bdd.getTotalVote((gvFilms.SelectedItem as Film).codeFilm).ToString();
        }

        private void BtnVoter_Click(object sender, RoutedEventArgs e)
        {
            float value = Convert.ToSingle(sldVote.Value);
            bdd.updateVoteByFilm((gvFilms.SelectedItem as Film).codeFilm, value);
            txtBlockNbrVote.Text = bdd.getNbrVote((gvFilms.SelectedItem as Film).codeFilm).ToString();
            txtBlockVoteTotal.Text = bdd.getTotalVote((gvFilms.SelectedItem as Film).codeFilm).ToString();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
