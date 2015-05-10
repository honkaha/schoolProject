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

		public CompassView (Context context) : base (context)
		{
			_parentView = (MainViewActivity)context;
			if (this.LayoutParameters == null) {
				this.LayoutParameters = new ViewGroup.LayoutParams (200, 200);
			} else {
				this.LayoutParameters.Height = 200;
			}
			// Construct a wedge-shaped path
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

			if (_parentView.CompassValues != null)
				canvas.Rotate (-_parentView.CompassValues[0]);

			canvas.DrawPath (_path, _paint);
		}
			
		public void OnAccuracyChanged (Sensor sensor, SensorStatus accuracy)
		{
			// Do nothing
		}

		public void OnSensorChanged (SensorEvent e)
		{
			_parentView.CompassValues = new List<float>(e.Values);

			Invalidate ();
		}

	}
}

