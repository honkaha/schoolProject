
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Json;

namespace MobilePracticalWork
{
	[Activity (Label = "BrandLocationSelectionActivity")]			
	public class BrandLocationSelectionActivity : ListActivity
	{
		private string _brandId;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.Title = (this as Context).Resources.GetString (Resource.String.SelectBrandLocationButton);

			_brandId = Intent.GetStringExtra("brandId");
			var result = RestQuery.GetBrandLocations (_brandId);
			this.ListAdapter = new BrandLocationAdapter (this, result);
			this.ListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
			{
				var mainView = new Intent (this, typeof(MainViewActivity));
				mainView.PutExtra("brandLocationId", (string)(this.ListAdapter as BrandLocationAdapter)[e.Position]["idBrandLocation"]);
				mainView.PutExtra("brandId", _brandId);
				StartActivity (mainView);
			};

		}
	}
}

