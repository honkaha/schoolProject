using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Timers;

namespace MobilePracticalWork
{
	[Activity (Theme = "@style/Theme.Splash", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			System.Threading.Thread.Sleep (100);
			StartActivity (new Intent (this, typeof(BrandSelectionActivity)));
		}
	}
}


