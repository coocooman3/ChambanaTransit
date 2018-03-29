using System;
using System.Collections.Generic;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;

namespace ChambanaTransit.Models
{
    public class Status
    {
        public double code { get; set; }
        public string msg { get; set; }
    }

    public class Params
    {
        public string stop_id { get; set; }
    }
    /*
    public class Rqst
    {
        public string method { get; set; }
        public Params @params { get; set; }
    }
    */
    public class Route
    {
        public string route_color { get; set; }
        public string route_id { get; set; }
        public string route_long_name { get; set; }
        public string route_short_name { get; set; }
        public string route_text_color { get; set; }
    }

    public class Trip
    {
        public string trip_id { get; set; }
        public string trip_headsign { get; set; }
        public string route_id { get; set; }
        public string block_id { get; set; }
        public string direction { get; set; }
        public string service_id { get; set; }
        public string shape_id { get; set; }
    }

    public class Origin
    {
        public string stop_id { get; set; }
    }

    public class Destination
    {
        public string stop_id { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Departure
    {
        public string stop_id { get; set; }
        public string headsign { get; set; }
        public Route route { get; set; }
        public Trip trip { get; set; }
        public string vehicle_id { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public bool is_monitored { get; set; }
        public bool is_scheduled { get; set; }
        public bool is_istop { get; set; }
        public DateTime scheduled { get; set; }
        public DateTime expected { get; set; }
        public double expected_mins { get; set; }
        public Location location { get; set; }

        public Departure(JsonObject departureObject)
        {
            stop_id = departureObject.GetNamedString("stop_id");
            headsign = departureObject.GetNamedString("headsign");
            //route
            //trip
            vehicle_id = departureObject.GetNamedString("vehicle_id");
            //origin
            //Destination
            is_monitored = departureObject.GetNamedBoolean("is_monitored");
            is_scheduled = departureObject.GetNamedBoolean("is_scheduled");
            is_istop = departureObject.GetNamedBoolean("is_istop");
            DateTime tempVar;
            if (DateTime.TryParse(departureObject.GetNamedString("scheduled"), out tempVar))
            {
                scheduled = tempVar;
            }
            if (DateTime.TryParse(departureObject.GetNamedString("expected"), out tempVar))
            {
                expected = tempVar;
            }
            expected_mins = departureObject.GetNamedNumber("expected_mins");
            //location
        }
    }

    public class BusStop
    {
        public DateTime time { get; set; }
        public bool new_changeset { get; set; }
        public Status status { get; set; }
        //public Rqst rqst { get; set; }
        public List<Departure> departures { get; set; }


        public BusStop()
        {
            time = DateTime.Now;
            new_changeset = false;
            status = null;
            //rqst = null;
            departures = new List<Departure>();
        }

        public BusStop(JsonObject SortedJsonRoutesObject) : this()
        {
            DateTime tempVar;
            if (DateTime.TryParse(SortedJsonRoutesObject.GetNamedString("time"), out tempVar))
            {
                time = tempVar;
            }
            new_changeset = SortedJsonRoutesObject.GetNamedBoolean("new_changeset");
            //status.code = SortedJsonRoutesObject.GetNamedObject("status").GetNamedNumber("code");
            foreach (IJsonValue departureValue in SortedJsonRoutesObject.GetNamedArray("departures"))
            {
                //TempString += departureValue.GetObject();//.GetNamedString("stop_id");
                if (departureValue.ValueType == JsonValueType.Object && departureValue != null)
                {
                    departures.Add(new Departure(departureValue.GetObject()));
                }
            }

        }
    }
}
