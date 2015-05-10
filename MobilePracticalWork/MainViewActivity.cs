
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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Json;
using Android.Hardware;
using Android.Content.PM;

namespace MobilePracticalWork
{
	[Activity (Label = "MainViewActivity", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class MainViewActivity : Activity, IOnMapReadyCallback, ILocationListener
	{
		private GoogleMap _map;
		private string _brandId;
		private string _brandLocationId;
		private Location _currentLocation;

		private SensorManager _sensorManager;
		private CompassView _compassView;
		private TextView _distanceTextView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MainView);
			this.Title = (this as Context).Resources.GetString (Resource.String.MainViewTitle);

			_brandId = Intent.GetStringExtra("brandId");
			_brandLocationId = Intent.GetStringExtra("brandLocationId");

			Button brandLocationSelectButton = FindViewById<Button> (Resource.Id.buttonBrandLocationSelect);
			brandLocationSelectButton.Click += (sender, e) =>
			{
				var mainView = new Intent (this, typeof(BrandLocationSelectionActivity));
				mainView.PutExtra("brandId", _brandId);
				StartActivity (mainView);
			};

			Button brandSelectionButton = FindViewById<Button> (Resource.Id.buttonBrandSelection);
			brandSelectionButton.Click += (sender, e) =>
			{
				StartActivity (new Intent (this, typeof(BrandSelectionActivity)));
			};
				

			var locationManager = GetSystemService (Context.LocationService) as LocationManager;
			String provider = locationManager.GetBestProvider (new Criteria(), true);
			_currentLocation = locationManager.GetLastKnownLocation (provider);

			_sensorManager = (SensorManager)GetSystemService (Context.SensorService);
			_compassView = new CompassView (this);
			_distanceTextView = new TextView (this);
			_distanceTextView.Text = "Loading...";
			_distanceTextView.SetHeight (200);
			_distanceTextView.SetMinimumWidth (300);

			var compassLayout = FindViewById<LinearLayout> (Resource.Id.compassLayout);
			compassLayout.AddView (_compassView);
			compassLayout.AddView (_distanceTextView);
			locationManager.RequestLocationUpdates(provider, 20000, 0, this);
			MapFragment mapFrag = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.mainViewMap);
			mapFrag.GetMapAsync (this);
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			var ori = _sensorManager.GetDefaultSensor (SensorType.Orientation);

			_sensorManager.RegisterListener (_compassView, ori, SensorDelay.Fastest);
		}

		protected override void OnStop ()
		{
			_sensorManager.UnregisterListener (_compassView);
			base.OnStop ();
		}
			
		public void OnLocationChanged (Location currentLocation)
		{
			_compassView.CurrentLocation = currentLocation;
			_currentLocation = currentLocation;
		}

		public void OnMapReady (GoogleMap googleMap)
		{
			_map = googleMap;
			if (_map.MyLocationEnabled == false) {
				googleMap.MyLocationEnabled = true;
				MapFragment mapFrag = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.mainViewMap);
				mapFrag.GetMapAsync (this);
				return;
			}
				
			var currentLatLng = new LatLng(_currentLocation.Latitude, _currentLocation.Longitude);

			if (_brandLocationId == null) {
				_brandLocationId = GetClosestStore (currentLatLng, _brandId);
			}

			var targetLocation = GetCurrentStoreLocation (_brandLocationId);

			if (targetLocation == null) {
				var cu = CameraUpdateFactory.NewLatLngZoom (currentLatLng, 15);
				_map.AnimateCamera (cu);
				return;
			}
			_compassView.TargetLocation = targetLocation;
			var targetLatLng = new LatLng(targetLocation.Latitude, targetLocation.Longitude);
			var targetOpt = new MarkerOptions();
			targetOpt.SetPosition(targetLatLng);
			var title = GetCurrentStoreTitle (_brandLocationId);
			targetOpt.SetTitle(title);
			FindViewById<TextView> (Resource.Id.textViewName).Text = title;
			_map.AddMarker (targetOpt);

			LatLngBounds.Builder builder = new LatLngBounds.Builder();
			builder.Include(currentLatLng);
			builder.Include(targetLatLng);
			LatLngBounds bounds = builder.Build();
			int padding = 100; 
			_map.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, padding));

		}

		public void OnProviderDisabled (string provider)
		{
			//stub
		}

		public void OnProviderEnabled (string provider)
		{
			//stub
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			//stub
		}

		private string GetCurrentStoreTitle(string brandLocationId)
		{
			var brandLocation = RestQuery.GetBrandLocation (brandLocationId);
			var brand = RestQuery.GetBrand(brandLocation["fkBrand"]);

			return brand ["name"] + ", " + brandLocation ["name"];
		}

		private Location GetCurrentStoreLocation(string brandLocationId)
		{
			var locLatLng = ParseLocationFromBrandLocationJson (RestQuery.GetBrandLocation (brandLocationId));

			var retLocation = new Location ("dummy");
			retLocation.Latitude = locLatLng.Latitude;
			retLocation.Longitude = locLatLng.Longitude;
			return retLocation;
		}
			
		private string GetClosestStore(LatLng currentLocation, string brandId) {
			var brandLocations = RestQuery.GetBrandLocations (brandId);
			string closest = "";
			double closestDistance = 0;

			foreach (var bl in brandLocations) {
				var blLatLng = ParseLocationFromBrandLocationJson (bl);
				var distance = GetDistance (blLatLng, currentLocation);
				if (closest == "" ||
				    distance < closestDistance) {
					closestDistance = distance;
					closest = bl ["idBrandLocation"];
				}

			}
			_distanceTextView.Text = closestDistance.ToString () + " m";
			return closest;
		}

		private double GetDistance(LatLng LatLng1, LatLng LatLng2) {
			double distance = 0;
			Location locationA = new Location("A");
			locationA.Latitude = LatLng1.Latitude;
			locationA.Longitude = LatLng1.Longitude;
			Location locationB = new Location("B");
			locationB.Latitude = LatLng2.Latitude;
			locationB.Longitude = LatLng2.Longitude;
			distance = locationA.DistanceTo(locationB);
			return distance;

		}

		private LatLng ParseLocationFromBrandLocationJson(JsonObject brandLocation)
		{
			JsonObject address = null;
			JsonObject location = null;

			try 
			{
				location = RestQuery.GetLocation(brandLocation["fkLocation"]);
			} 
			catch (Exception exp) {
				Console.Out.WriteLine ("Location getting failed" + exp.Message);
			}

			if (location != null) {
				return new LatLng (Double.Parse (location ["latitude"]), 
					Double.Parse (location ["longitude"]));
			}

			try 
			{
				address = RestQuery.GetAddress(brandLocation["fkAddress"]);
			} 
			catch (Exception exp) {
				Console.Out.WriteLine ("Address getting failed" + exp.Message);
				return null;
			}
				
			string addressString = 
				address ["address"] + ", " + address ["town"] + ", " + 
				address ["province"] + ", " + address ["country"];
			
			var geocoder = new Geocoder(this);
			var addresses = geocoder.GetFromLocationName(addressString, 1);
			if(addresses.Count > 0) {
				return new LatLng (addresses[0].Latitude, addresses[0].Longitude);
			}
			return new LatLng(0,0);
		}
	}
}

