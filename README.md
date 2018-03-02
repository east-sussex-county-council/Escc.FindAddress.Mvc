# Escc.FindAddress.Mvc

An address field with a postcode lookup for use in ASP.NET MVC projects.

It can also be used by Umbraco Forms when wrapped by a custom field class - see [Escc.EastSussexGovUK.Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco) for an example.

## Using the field in a Razor view

The `HtmlFieldPrefix` property should match the name of the address property on the model in order for MVC model binding to work. `ApiControllerUrl` is optional and defaults to the built-in Web API documented below. This can be useful when hosting the field in a virtual directory for example.

 	@Html.Partial("_FindAddress", Model.ExampleAddress, new ViewDataDictionary()
    {
    	TemplateInfo = new FindAddressConfiguration() { 
			HtmlFieldPrefix = "ExampleAddress", 
			Required = true, 
			Label = "Address lookup",
			Description = "This is optional and appears between the label and the postcode lookup field",
			ApiControllerUrl = new Uri(Url.Content("~/api/FindAddress/"), UriKind.Relative)
		}
    })

## Required web service

The postcode lookup needs to connect to a web service supporting two methods with JSON responses in the following format:

Example response for an `AddressesMatchingPostcode(string postcode)` method:

	{
		"success" : true,
		"data" : [
					{ 
						"Key" : "10033260751",
						"Value": "Transport And Environment County Hall, St Annes Crescent, Lewes, East Sussex BN7 1UE"
					}
				 ]
	}

The `data` property is an array which would usually contain multiple addresses, and the `Key` property is based on the UPRN for the address being returned.

Example response for an `AddressMatchingPostcodeAndUprn(string postcode, string urpn)` method:

	{
		"success" : true,
		"data" : {
					"GeoCoordinate" : 
					{
						"Latitude" : 50.871784210205078,
						"Longitude" : 0.00089050002861768007
					},
					"Saon" : "",
					"Paon" : "Transport And Environment County Hall",
					"StreetName" : "St Annes Crescent",
					"Locality" : "",
					"Town" : "Lewes",
					"AdministrativeArea" : "East Sussex",
					"Postcode" : "BN7 1UE",
					"Usrn" : "23301449",
					"Uprn" : "10033260751"
				}
	}

An error response looks like this:

	{
		"success" : false,
		"responseText" : "Please enter a postcode."
	}

### Using the built-in Web API
There is a Web API installed with the package which supports these methods. It requires an implementation of the [GOV.UK locate API](https://github.com/alphagov/locate-api) standard. To use the built-in Web API your project must set up the route:

**~/Global.asax.cs**

	protected void Application_Start(object sender, EventArgs e)
    {
        GlobalConfiguration.Configure(WebApiConfig.Register);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }

**~/App_Start/WebApiConfig.cs**

	public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

To use the built-in Web API your project must also configure the Locate API service in `web.config`:

	<configuration>
	  <appSettings>
	    <add key="LocateApiAddressesUrl" value="https://hostname/addresses?query=residentialAndCommercial&amp;format=all&amp;postcode={0}" />
	    <add key="LocateApiToken" value="" />
	  </appSettings>
	 </configuration>

