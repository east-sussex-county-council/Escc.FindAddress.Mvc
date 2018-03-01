using System.Collections.Generic;
using Escc.AddressAndPersonalDetails;

namespace Escc.FindAddress.Mvc
{
    /// <summary>
    /// A strategy to look up the addresses that share a postcode
    /// </summary>
    public interface IAddressLookup
    {
        /// <summary>
        /// Gets the addresses that share a postcode
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns></returns>
        IList<BS7666Address> AddressesFromPostcode(string postcode);
    }
}