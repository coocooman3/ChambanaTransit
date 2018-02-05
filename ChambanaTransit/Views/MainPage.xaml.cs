using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Data.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ChambanaTransit.Models;

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
        string ApiKey = "key=ee652d6a7a2346049b9f7750dd0cda90";
        private async void Grainger_Button_Click(object sender, RoutedEventArgs e)
        {
            //Create an HTTP client object
            ParsingGraingerProgressIndicator.Visibility = Visibility.Visible;
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient(); 
            Uri requestUri = new Uri("https://developer.cumtd.com/api/v2.2/json/getdeparturesbystop?stop_id=WRTSPFLD&" + ApiKey);

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
            Uri requestUri = new Uri("https://developer.cumtd.com/api/v2.2/json/getdeparturesbystop?stop_id=PKLN&" + ApiKey);


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
            Uri requestUri = new Uri("https://developer.cumtd.com/api/v2.2/json/getdeparturesbystop?stop_id=GRN6TH&" + ApiKey);


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
    }
}
