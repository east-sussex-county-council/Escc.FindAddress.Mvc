// Build this file in debug only as it's for local testing, but we don't want it to be included in the NuGet package
#if DEBUG
using Escc.AddressAndPersonalDetails;

namespace Escc.FindAddress.Mvc.Test
{
    public class TestModel
    {
        public BS7666Address Address1 {get;set;}
        public BS7666Address Address2 {get;set;}
    }
}
#endif