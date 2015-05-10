using System;
using Android.Widget;
using System.Json;
using Android.App;
using System.Collections.Generic;
using Android.Views;

namespace MobilePracticalWork
{
	public class BrandLocationAdapter : BaseAdapter<JsonObject>
	{
		List<JsonObject> _items;
		Activity context;

		public BrandLocationAdapter(Activity context, List<JsonObject> brandlocationValue) : base() {
			this.context = context;
			this._items = new List<JsonObject> ();
			foreach (var json in brandlocationValue)
			{
				this._items.Add (json);
			}

		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override JsonObject this[int position] {  
			get { return _items[position]; }
		}

		public override int Count {
			get { return _items.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = _items[position];
			View view = convertView; // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.BrandLocationListItem, null);

			var brand = RestQuery.GetBrand(item["fkBrand"]);
			JsonObject address = null;
			JsonObject location = null;

			try 
			{
				address = RestQuery.GetAddress(item["fkAddress"]);
			} 
			catch (Exception exp) {
				Console.Out.WriteLine ("Address getting failed" + exp.Message);
			}

			try 
			{
				location = RestQuery.GetLocation(item["fkLocation"]);
			} 
			catch (Exception exp) {
				Console.Out.WriteLine ("Location getting failed" + exp.Message);
			}

			view.FindViewById<TextView>(Resource.Id.brandTextView).Text = brand["name"];
			view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item["name"];

			if (address != null) {
				view.FindViewById<TextView> (Resource.Id.addressTextView).Text = 
					address ["address"] + ", " + address ["town"] + ", " + address ["province"] + ", " + address ["country"];
			} else if (location != null) {
				view.FindViewById<TextView> (Resource.Id.addressTextView).Text = 
					location ["latitude"] + ", " + location ["longitude"];
					
			} else {
				view.FindViewById<TextView> (Resource.Id.addressTextView).Text = "No data";
			}


			return view;
		}
	}
}