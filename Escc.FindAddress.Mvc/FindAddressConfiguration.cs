using Escc.AddressAndPersonalDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.FindAddress.Mvc
{
    public class FindAddressConfiguration : TemplateInfo
    {
        public bool Required { get; set; }

        public string Label { get; set; }
    }
}