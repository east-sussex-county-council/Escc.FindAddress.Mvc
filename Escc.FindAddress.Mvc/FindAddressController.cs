using Escc.AddressAndPersonalDetails;
using Escc.Net;
using Escc.Net.Configuration;
using Exceptionless;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Escc.FindAddress.Mvc
{
    /// <summary>
    /// Web API which looks up addresses using a <see cref="LocateApiAddressLookup"/>
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class FindAddressController : ApiController
    {
        /// <summary>
        /// Returns an object with a <c>data</c> property that contains a list of addresses matching the given postcode
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns></returns>
        [HttpGet]
        public object AddressesMatchingPostcode(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return new { success = false, responseText = "Please enter a postcode." };
            }

            var addresses = LookupMatchingAddresses(postcode);

            if (addresses == null || addresses.Count() == 0)
            {
                //  Send "Not a success"
                return new { success = false, responseText = "No addresses were found for the postcode supplied." };
            }
            else
            {
                //  Send "Success"
                return new { success = true, data = addresses.Select(x => new KeyValuePair<string, string>(x.Uprn, x.GetSimpleAddress().ToString(", "))).ToList() };
            }
        }

        /// <summary>
        /// Returns an object with a <c>data</c> property that contains an address matching the given postcode and UPRN
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <param name="uprn">The UPRN.</param>
        /// <returns></returns>
        [HttpGet]
        public object AddressMatchingPostcodeAndUprn(string postcode, string uprn)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return new { success = false, responseText = "Please enter a postcode." };
            }

            // We need to get an address to return
            var addresses = LookupMatchingAddresses(postcode);
            var address = addresses.FirstOrDefault(x => x.Uprn == uprn);

            if (address == null)
            {
                //  Send "Not a success"
                return new { success = false, responseText = "No addresses were found for the postcode supplied." };
            }
            else
            {
                //  Send "Success"
                return new { success = true, data = address };
            }
        }

        private IList<BS7666Address> LookupMatchingAddresses(string postcode)
        {
            // We need to get a list of addresses to return
            IList<BS7666Address> addresses = null;
            var locateApiUrl = ConfigurationManager.AppSettings["LocateApiAddressesUrl"];
            var locateApiToken = ConfigurationManager.AppSettings["LocateApiToken"];
            try
            {
                if (!string.IsNullOrWhiteSpace(postcode))
                {
                    if (String.IsNullOrEmpty(locateApiUrl))
                    {
                        // Missing configuration - report the real error but show the user a 'No addresses found' error
                        new ConfigurationErrorsException("LocateApiAddressesUrl was not set in web.config appSettings").ToExceptionless().Submit();
                    }
                    else if (String.IsNullOrEmpty(locateApiToken))
                    {
                        // Missing configuration - report the real error but show the user a 'No addresses found' error
                        new ConfigurationErrorsException("LocateApiToken was not set in web.config appSettings").ToExceptionless().Submit();
                    }
                    else
                    {
                        var addressfinder = new LocateApiAddressLookup(new Uri(locateApiUrl), locateApiToken, new ConfigurationProxyProvider());
                        addresses = addressfinder.AddressesFromPostcode(postcode);
                    }
                }
            }
            catch (WebException ex)
            {
                // A problem connecting to the remote service - report the real error but show the user a 'No addresses found' error
                ex.Data.Add("URL requested", locateApiUrl);
                ex.Data.Add("Postcode", postcode);
                ex.ToExceptionless().Submit();
            }

            return addresses;
        }
    }
}