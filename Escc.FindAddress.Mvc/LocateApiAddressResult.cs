using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Escc.FindAddress.Mvc
{
    /// <summary>
    /// Deserialised version of an object returned in a Locate API response
    /// </summary>
    public class LocateApiAddressResult
    {
        public string gssCode { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
        public string uprn { get; set; }
        public string createdAt { get; set; }
        public Presentation presentation { get; set; }
        public Details details { get; set; }
        public Location location { get; set; }
        public Ordering ordering { get; set; }
    }

    /// <summary>
    /// Deserialised version of an object returned in a Locate API response
    /// </summary>
    public class Presentation
    {
        public string property { get; set; }
        public string street { get; set; }
        public string town { get; set; }
        public string area { get; set; }
        public string postcode { get; set; }
    }

    /// <summary>
    /// Deserialised version of an object returned in a Locate API response
    /// </summary>
    public class Details
    {
        public string usrn { get; set; }
        public bool isResidential { get; set; }
        public bool isCommercial { get; set; }
        public bool isElectoral { get; set; }
        public bool isPostalAddress { get; set; }
        public string classification { get; set; }
        public string state { get; set; }
        public string organisation { get; set; }
        public string primaryClassification { get; set; }
        public string secondaryClassification { get; set; }
        public string file { get; set; }
        public string blpuUpdatedAt { get; set; }
        public string blpuCreatedAt { get; set; }
    }

    /// <summary>
    /// Deserialised version of an object returned in a Locate API response
    /// </summary>
    public class Location
    {
        [JsonProperty(PropertyName = "lat")]
        public float latitude { get; set; }
        [JsonProperty(PropertyName = "long")]
        public float longitude { get; set; }
    }

    /// <summary>
    /// Deserialised version of an object returned in a Locate API response
    /// </summary>
    public class Ordering
    {
        public string paoText { get; set; }
        public string street { get; set; }
    }

}
