using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Escc.AddressAndPersonalDetails;
using Escc.Net;
using Newtonsoft.Json;

namespace Escc.FindAddress.Mvc
{
    /// <summary>
    /// Looks up addresses within a postcode using an implementation of the GOV.UK locate API defined at https://github.com/alphagov/locate-api
    /// </summary>
    /// <seealso cref="IAddressLookup" />
    public class LocateApiAddressLookup : IAddressLookup
    {
        private readonly Uri _locateApiUrl;
        private readonly string _authenticationToken;
        private readonly IProxyProvider _proxyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocateApiAddressLookup" /> class.
        /// </summary>
        /// <param name="locateApiUrl">The locate API URL.</param>
        /// <param name="authenticationToken">The authentication token.</param>
        /// <param name="proxyProvider">The proxy provider.</param>
        /// <exception cref="System.ArgumentNullException">authenticationToken</exception>
        public LocateApiAddressLookup(Uri locateApiUrl, string authenticationToken, IProxyProvider proxyProvider)
        {
            if (locateApiUrl == null) throw new ArgumentNullException(nameof(locateApiUrl));
            if (String.IsNullOrEmpty(authenticationToken)) throw new ArgumentNullException(nameof(authenticationToken));

            _locateApiUrl = locateApiUrl;
            _authenticationToken = authenticationToken;
            _proxyProvider = proxyProvider;
        }

        /// <summary>
        /// Gets the addresses that share a postcode
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <exception cref="WebException"></exception>
        /// <returns></returns>
        public IList<AddressInfo> AddressesFromPostcode(string postcode)
        {
            var query = Regex.Replace(postcode, "[^A-Za-z0-9]", String.Empty);
            if (String.IsNullOrEmpty(query)) return new List<AddressInfo>();

            try
            {
                using (var client = new WebClient())
                {
                    if (_proxyProvider != null)
                    {
                        client.Proxy = _proxyProvider.CreateProxy();
                    }
                    client.Headers.Add("Authorization", "Bearer " + _authenticationToken);

                    var queryUrl = String.Format(_locateApiUrl.ToString(), query);

                    using (var stream = new StreamReader(client.OpenRead(queryUrl)))
                    {
                        var json = stream.ReadToEnd();
                        var results = JsonConvert.DeserializeObject<LocateApiAddressResult[]>(json);
                        var addresses = new List<AddressInfo>();
                        foreach (var result in results)
                        {
                            var address = new AddressInfo();
                            address.BS7666Address = new BS7666Address(result.presentation.property, String.Empty, result.presentation.street, String.Empty, result.presentation.town, result.presentation.area, result.presentation.postcode)
                            {
                                Uprn = result.uprn
                            };
                            address.GeoCoordinate = new GeoCoordinate
                            {
                                Latitude = result.location.latitude,
                                Longitude = result.location.longitude
                            };
                            addresses.Add(address);
                        }
                        return addresses;
                    }
                }
            }
            catch (WebException exception)
            {
                if (exception.Message.Contains("(422) Unprocessable Entity"))
                {
                    return new List<AddressInfo>();
                }
                throw;
            }
        }
    }
}