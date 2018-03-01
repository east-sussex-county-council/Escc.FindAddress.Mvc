using Escc.AddressAndPersonalDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.FindAddress.Mvc
{
    /// <summary>
    /// Configuration settings which can be passed to _FindAddress.cshtml using Html.Partial
    /// </summary>
    /// <seealso cref="System.Web.Mvc.TemplateInfo" />
    public class FindAddressConfiguration : TemplateInfo
    {
        /// <summary>
        /// Gets or sets whether this an address is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the question being asked, eg Home address. Defaults to 'Address'.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the description which appears between the label and the address fields.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the API controller URL used to look up addresses
        /// </summary>
        public Uri ApiControllerUrl { get; set; } = new Uri("/api/FindAddress/", UriKind.Relative);
    }
}