using System;
using System.Json;
using System.Net;
using System.IO;
using System.Collections.Generic;

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

		public static JsonValue GetBrands()
		{
			var replyJson = Query("Brand");

			return replyJson["Brand"];

		}
	}
}

