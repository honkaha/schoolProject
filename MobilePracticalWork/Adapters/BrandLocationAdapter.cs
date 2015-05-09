﻿using System;
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
				Console.Out.WriteLine ("vitu: " + json["fkBrand"]);
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

			view.FindViewById<TextView>(Resource.Id.nameTextView).Text = item["name"];
		    view.FindViewById<TextView>(Resource.Id.addressTextView).Text = item["fkAddress"];
			view.FindViewById<TextView>(Resource.Id.phoneTextView).Text = item["phone"];
			view.FindViewById<TextView>(Resource.Id.emailTextView).Text = item["email"];
			view.FindViewById<TextView>(Resource.Id.infoTextView).Text = item["additionalInfo"];

			return view;
		}
	}
}