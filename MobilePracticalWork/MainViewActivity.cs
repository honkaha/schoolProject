
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

namespace MobilePracticalWork
{
	[Activity (Label = "MainViewActivity")]			
	public class MainViewActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MainView);

			var brandId = Intent.GetStringExtra("brandId");
			var brandLocationId = Intent.GetStringExtra("brandLocationId");

			Button brandLocationSelectButton = FindViewById<Button> (Resource.Id.buttonBrandLocationSelect);
			brandLocationSelectButton.Click += (sender, e) =>
			{
				var mainView = new Intent (this, typeof(BrandLocationSelectionActivity));
				mainView.PutExtra("brandId", brandId);
				StartActivity (mainView);
			};

			Button brandSelectionButton = FindViewById<Button> (Resource.Id.buttonBrandSelection);
			brandSelectionButton.Click += (sender, e) =>
			{
				StartActivity (new Intent (this, typeof(BrandSelectionActivity)));
			};
		}
	}
}

