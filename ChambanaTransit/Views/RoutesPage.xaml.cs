using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChambanaTransit.Models;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;

namespace ChambanaTransit.Views
{
    public sealed partial class RoutesPage : Page
    {
        public ObservableCollection<Route> Routes { get; set; } = new ObservableCollection<Route>();

        public RoutesPage()
        {
            ParseRoutes();
            InitializeComponent();
        }

        public async void ParseRoutes()
        {

            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://developer.cumtd.com/api/v2.2/json/GetRoutes?" + App.ApiKey);

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
            }
            JsonObject SortedJsonRoutesObject;
            if (!JsonObject.TryParse(httpResponseBody, out SortedJsonRoutesObject))
            {
                httpResponseBody = "Error: Could not Parse Information Correctly";
            }
            else
            {
                //BusList;
                JsonArray routeArray = SortedJsonRoutesObject.GetNamedArray("routes");
                foreach (var route in routeArray)
                {
                    Routes.Add(new Route(route.GetObject()));
                }
            }
        }
    }
}
