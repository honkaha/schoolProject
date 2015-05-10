using System;
using Android.Widget;
using System.Json;
using Android.App;
using System.Collections.Generic;
using Android.Views;

namespace MobilePracticalWork
{
	public class BrandAdapter : BaseAdapter<JsonObject>
	{
		List<JsonObject> _items;
		Activity context;

		public BrandAdapter(Activity context, List<JsonObject> brandValues) : base() {
			this.context = context;
			this._items = new List<JsonObject> ();
			foreach (var json in brandValues)
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
				view = context.LayoutInflater.Inflate(Resource.Layout.BrandListItem, null);
			
			view.FindViewById<TextView>(Resource.Id.TypeText).Text = item["type"];
			view.FindViewById<TextView>(Resource.Id.NameText).Text = item["name"];

			return view;
		}
	}
}


