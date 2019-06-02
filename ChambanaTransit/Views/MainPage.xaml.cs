using ChambanaTransit.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Data.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChambanaTransit.Views
{
    public sealed partial class MainPage : Page
    {
        private List<Departure> Departures;


        public MainPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private async void Grainger_Button_Click(object sender, RoutedEventArgs e)
        {
            //Create an HTTP client object
            ParsingGraingerProgressIndicator.Visibility = Visibility.Visible;
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient(); 
            Uri requestUri = new Uri("https://developer.cumtd.com/api/v2.2/json/getdeparturesbystop?stop_id=WRTSPFLD&" + App.ApiKey);

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                ParsingGraingerProgressIndicator.Visibility = Visibility.Collapsed;
            }
            JsonObject SortedJsonRoutesObject;
            if (!JsonObject.TryParse(httpResponseBody, out SortedJsonRoutesObject))
            {
                httpResponseBody = "Error: Could not Parse Information Correctly";
            }
            else
            {
                //BusList;
                BusStop SelectedBusStop = new BusStop(SortedJsonRoutesObject);
                GraingerBusList.Items.Clear();
                foreach(Departure departure in SelectedBusStop.departures)
                {
                    GraingerBusList.Items.Add(departure);
                }
                //ContentArea.Children.Add(BusList);
            }
            ParsingGraingerProgressIndicator.Visibility = Visibility.Collapsed;
        }

        private async void Parkland_Button_Click(object sender, RoutedEventArgs e)
        {
            //Create an HTTP client object
            ParsingParklandProgressIndicator.Visibility = Visibility.Visible;
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://developer.cumtd.com/api/v2.2/json/getdeparturesbystop?stop_id=PKLN&" + App.ApiKey);


            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                ParsingParklandProgressIndicator.Visibility = Visibility.Collapsed;
            }
            JsonObject SortedJsonRoutesObject;
            if (!JsonObject.TryParse(httpResponseBody, out SortedJsonRoutesObject))
            {
                httpResponseBody = "Error: Could not Parse Information Correctly";
            }
            else
            {
                BusStop SelectedBusStop = new BusStop(SortedJsonRoutesObject);
                ParklandBusList.Items.Clear();
                Departures = SelectedBusStop.departures;
                //foreach (Departure departure in SelectedBusStop.departures)
                //{
                //    var TempObj = new Tuple<string, double>(departure.headsign, departure.expected_mins);
                //    ParklandBusList.Items.Add(TempObj);
                //}
                //Stop2.Text = Departures[0].headsign;
                foreach (Departure departure in SelectedBusStop.departures)
                {
                    ParklandBusList.Items.Add(departure);
                }
            }
            ParsingParklandProgressIndicator.Visibility = Visibility.Collapsed;
        }

        private async void Green_Button_Click(object sender, RoutedEventArgs e)
        {
            //Create an HTTP client object
            ParsingGreenProgressIndicator.Visibility = Visibility.Visible;
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://developer.cumtd.com/api/v2.2/json/getdeparturesbystop?stop_id=GRN6TH&" + App.ApiKey);


            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                ParsingParklandProgressIndicator.Visibility = Visibility.Collapsed;
            }
            JsonObject SortedJsonRoutesObject;
            if (!JsonObject.TryParse(httpResponseBody, out SortedJsonRoutesObject))
            {
                httpResponseBody = "Error: Could not Parse Information Correctly";
            }
            else
            {
                BusStop SelectedBusStop = new BusStop(SortedJsonRoutesObject);
                GreenBusList.Items.Clear();
                Departures = SelectedBusStop.departures;
                foreach (Departure departure in SelectedBusStop.departures)
                {
                    GreenBusList.Items.Add(departure);
                }
            }
            ParsingGreenProgressIndicator.Visibility = Visibility.Collapsed;
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // you can also add items in code behind
            NavView.MenuItems.Add(new NavigationViewItemSeparator());
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "My content", Icon = new SymbolIcon(Symbol.Folder), Tag = "content" });

            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "home")
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }

            ContentFrame.Navigated += On_Navigated;

            // add keyboard accelerators for backwards navigation
            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;

        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;

                case "apps":
                    ContentFrame.Navigate(typeof(AppsPage));
                    break;

                case "games":
                    ContentFrame.Navigate(typeof(GamesPage));
                    break;

                case "music":
                    ContentFrame.Navigate(typeof(MusicPage));
                    break;

                case "content":
                    ContentFrame.Navigate(typeof(MyContentPage));
                    break;
            }
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private bool On_BackRequested()
        {
            bool navigated = false;

            // don't go back if the nav pane is overlayed
            if (NavView.IsPaneOpen && (NavView.DisplayMode == NavigationViewDisplayMode.Compact || NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
            {
                return false;
            }
            else
            {
                if (ContentFrame.CanGoBack)
                {
                    ContentFrame.GoBack();
                    navigated = true;
                }
            }
            return navigated;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                NavView.SelectedItem = NavView.SettingsItem as NavigationViewItem;
            }
            else
            {
                Dictionary<Type, string> lookup = new Dictionary<Type, string>()
        {
            {typeof(HomePage), "home"},
            {typeof(AppsPage), "apps"},
            {typeof(GamesPage), "games"},
            {typeof(MusicPage), "music"},
            {typeof(MyContentPage), "content"}
        };

                String stringTag = lookup[ContentFrame.SourcePageType];

                // set the new SelectedItem  
                foreach (NavigationViewItemBase item in NavView.MenuItems)
                {
                    if (item is NavigationViewItem && item.Tag.Equals(stringTag))
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
            }
        }
    }
}
