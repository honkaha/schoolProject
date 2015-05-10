using System;
using Android.Views;
using Android.Graphics;
using Android.Hardware;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Json;

namespace MobilePracticalWork
{
	public class CompassView : View, ISensorEventListener
	{
		private Paint _paint = new Paint ();
		private Path _path = new Path ();
		private MainViewActivity _parentView;

		public Location CurrentLocation;
		public Location TargetLocation;
		public List<float> CompassValues;

		public CompassView (Context context) : base (context)
		{
			_parentView = (MainViewActivity)context;
			if (this.LayoutParameters == null) {
				this.LayoutParameters = new ViewGroup.LayoutParams (200, 200);
			} else {
				this.LayoutParameters.Height = 200;
			}
			_path.MoveTo (0, -50);
			_path.LineTo (-20, 60);
			_path.LineTo (0, 50);
			_path.LineTo (20, 60);
			_path.Close ();
		}

		protected override void OnDraw (Canvas canvas)
		{
			Paint paint = _paint;

			canvas.DrawColor (Color.White);

			paint.AntiAlias = true;
			paint.Color = Color.Black;
			paint.SetStyle (Paint.Style.Fill);

			int w = canvas.Width;
			int h = canvas.Height;
			int cx = w / 2;
			int cy = h / 2;

			canvas.Translate (cx, cy);

			if (CompassValues == null || 
				CurrentLocation == null ||
				TargetLocation == null) {
				
				canvas.DrawPath (_path, _paint);
				return;
			}

			float azimuth = CompassValues[0];
			float baseAzimuth = azimuth;


			var geoField = new GeomagneticField ((float)CurrentLocation.Latitude,
				(float)CurrentLocation.Longitude, 
				100f,
				Convert.ToInt64(DateTime.Now.ToUniversalTime ().Subtract (
	               new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
				).TotalMilliseconds));
					
			var aGeo = geoField.Declination; 
			azimuth += aGeo;
			float abearingTo = CurrentLocation.BearingTo( TargetLocation );
			if (abearingTo < 0) {
				abearingTo += 360;
			}
			azimuth -= abearingTo; 
			if (azimuth < 0) {
				azimuth += 360;
			}
			canvas.Rotate (-azimuth);
			canvas.DrawPath (_path, _paint);
		}
			
		public void OnAccuracyChanged (Sensor sensor, SensorStatus accuracy)
		{
		}

		public void OnSensorChanged (SensorEvent e)
		{
			CompassValues = new List<float>(e.Values);
			CompassValues [0] = e.Values [0];

			Invalidate ();
		}

	}
}

