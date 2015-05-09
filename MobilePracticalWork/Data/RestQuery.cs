using System;
using System.Json;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MobilePracticalWork
{
	public class RestQuery
	{
		private const string QUERY_STRING = @"http://home.tamk.fi/~e4hhonka/CMD2/index.php/{0}/exampleKey";

		private static JsonValue Query(string tableId)
		{
			var request = 
				HttpWebRequest.Create(string.Format(QUERY_STRING, tableId));
			request.ContentType = "application/json";
			request.Method = "GET";
			request.Timeout = 10000;
			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				if (response.StatusCode != HttpStatusCode.OK)
					Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					var content = reader.ReadToEnd();
					if(string.IsNullOrWhiteSpace(content)) {
						Console.Out.WriteLine("Response contained empty body...");
						return null;
					}
					else {
						Console.Out.WriteLine("Response Body: \r\n {0}", content);
						return JsonValue.Parse(content);
					}
				}
			}
		}

		public static List<JsonObject> GetBrands()
		{
			var replyJson = Query("Brand");

			return (replyJson["Brand"] as JsonArray)
				.Select(j => j as JsonObject)
				.ToList();
		}

		public static List<JsonObject> GetBrand(string id)
		{
			var replyJson = Query ("Brand");
			return ((replyJson ["Brand"] as JsonArray)
				.Where (v => v ["idBrand"] == id))
				.Select(j => j as JsonObject)
				.ToList();
		}

		public static List<JsonObject> GetBrandLocations(string brandId)
		{
			var replyJson = Query("BrandLocation");

			var x = (replyJson ["BrandLocation"] as JsonArray);
			var aff	= x.Where (v => v ["fkBrand"] == brandId).ToList();
			var z =	aff.Select(j => j as JsonObject).ToList();
			return	z;
		}

		public static JsonObject GetBrandLocation(string brandLocationId)
		{
			var replyJson = Query("BrandLocation");

			return (replyJson ["BrandLocation"] as JsonArray)
				.First (v => v ["idBrandLocation"] == brandLocationId) as JsonObject;
		}

		public static JsonObject GetAddress(string id)
		{
			var replyJson = Query ("Address");
			return (replyJson ["Address"] as JsonArray)
				.First (v => v ["idAddress"] == id) as JsonObject;
		}
	}
}

