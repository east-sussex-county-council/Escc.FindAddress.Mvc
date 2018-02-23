using Escc.AddressAndPersonalDetails;
using Escc.Net;
using Exceptionless;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Escc.FindAddress.Mvc
{
    public class FindAddressController : Controller
    {
        [HttpGet]
        public JsonResult FindAddressClick(string postCode)
        {
            if (string.IsNullOrWhiteSpace(postCode))
                return Json(new { success = false, responseText = "Please enter a postcode." }, JsonRequestBehavior.AllowGet);

            IList<KeyValuePair<string, string>> addresses = null;

            // We need to get a list of addresses to return
            try
            {
                var locateApiUrl = ConfigurationManager.AppSettings["LocateApiAddressesUrl"];
                var locateApiToken = ConfigurationManager.AppSettings["LocateApiToken"];
                if (String.IsNullOrEmpty(locateApiUrl))
                {
                    new ConfigurationErrorsException("LocateApiAddressesUrl was not set in web.config appSettings").ToExceptionless().Submit();
                }
                else if (String.IsNullOrEmpty(locateApiToken))
                {
                    new ConfigurationErrorsException("LocateApiToken was not set in web.config appSettings").ToExceptionless().Submit();
                }
                else
                {
                    var addressfinder = new LocateApiAddressLookup(new Uri(locateApiUrl), locateApiToken, new ConfigurationProxyProvider());
                    IList<AddressInfo> addressData = addressfinder.AddressesFromPostcode(postCode);
                    addresses = addressData.Select(x => new KeyValuePair<string, string>(x.BS7666Address.Uprn, x.BS7666Address.GetSimpleAddress().ToString(", "))).ToList();
                }
            }
            catch (WebException ex)
            {
                // Service unavailable
                ex.ToExceptionless().Submit();
            }

            if (addresses == null || addresses.Count == 0)
            {
                //  Send "Not a success"
                return Json(new { success = false, responseText = "No addresses were found for the postcode supplied." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //  Send "Success"
                return Json(new { success = true, data = addresses }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ConfirmAddressClick(string postCode, string uprn)
        {
            if (string.IsNullOrWhiteSpace(postCode))
                return Json(new { success = false, responseText = "Please enter a postcode." }, JsonRequestBehavior.AllowGet);

            AddressInfo address = null;

            // We need to get a list of addresses to return
            try
            {
                var locateApiUrl = ConfigurationManager.AppSettings["LocateApiAddressesUrl"];
                var locateApiToken = ConfigurationManager.AppSettings["LocateApiToken"];
                if (String.IsNullOrEmpty(locateApiUrl))
                {
                    new ConfigurationErrorsException("LocateApiAddressesUrl was not set in web.config appSettings").ToExceptionless().Submit();
                }
                else if (String.IsNullOrEmpty(locateApiToken))
                {
                    new ConfigurationErrorsException("LocateApiToken was not set in web.config appSettings").ToExceptionless().Submit();
                }
                else
                {
                    var addressfinder = new LocateApiAddressLookup(new Uri(locateApiUrl), locateApiToken, new ConfigurationProxyProvider());
                    IList<AddressInfo> addresses = addressfinder.AddressesFromPostcode(postCode);
                    address = addresses.FirstOrDefault(x => x.BS7666Address.Uprn == uprn);
                }
            }
            catch (WebException ex)
            {
                // Service unavailable
                ex.ToExceptionless().Submit();
            }

            if (address == null)
            {
                //  Send "Not a success"
                return Json(new { success = false, responseText = "No addresses were found for the postcode supplied." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //  Send "Success"
                return Json(new { success = true, data = address }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}