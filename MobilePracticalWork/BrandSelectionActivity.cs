
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
	[Activity (Label = "BrandSelectionActivity")]			
	public class BrandSelectionActivity : ListActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.Title = (this as Context).Resources.GetString (Resource.String.BrandSelectionTitle);

			this.ListAdapter = new BrandAdapter (this, RestQuery.GetBrands ());
		}
	}
}

