using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DubinsPaths
{
	/// <summary>
	/// Represents a point with a corresponding angle.
	/// </summary>
	class DirectionalPoint : GeometricalObject
	{
		/**** Variables ****/

		/// <summary>
		/// The length of the lines forming the cross.
		/// </summary>
		private int lineLength = 10;
		/// <summary>
		/// The angle in radians from the x axis to the direction of the point.
		/// </summary>
		private float angle;
		private Point point;


		/**** Functions ****/

		public DirectionalPoint(Point point, float angle)
		{
			this.point = point;
			this.angle = angle;
		}

		/// <summary>
		/// Renders the point marked by a cross and an arrow showing the dirction.
		/// </summary>
		/// <param name="g">The graphics object to render to.</param>
		public override void Render(Graphics g)
		{
			// Cross: Rotate first by 45°, then again by 90°
			// and draw a line each time.
			g.TranslateTransform(point.X, point.Y);
			for (int angle = 45; angle <= 90; angle += 45)
			{
				g.RotateTransform(angle);
				Point left = new Point(-lineLength / 2, 0);
				Point right = new Point(lineLength / 2, 0);
				g.DrawLine(pen, left, right);
			}
			g.ResetTransform();

			// Draw the arrow indication the direction of the point.
			g.TranslateTransform(point.X, point.Y);
			float angleInDegrees = angle * (180 / (float)Math.PI);
			g.RotateTransform(-angleInDegrees);
			g.DrawLine(new Pen(Color.Red, 2), 0, 0, 30, 0);
			g.ResetTransform();
		}

		/// <summary>
		/// The angle in radians from the x axis to the direction of the point.
		/// </summary>
		public float Angle
		{
			get { return (angle); }
			set
			{
				if (!double.IsNaN(value))
					angle = value;
			}
		}

		/// <summary>
		/// The x position of the point.
		/// </summary>
		public int X
		{
			get { return (point.X); }
			set { point.X = value; }
		}

		/// <summary>
		/// The y position of the point.
		/// </summary>
		public int Y
		{
			get { return (point.Y); }
		}

		public Point Position
		{
			get { return (point); }
			set { point = value; }
		}

		public override float Length
		{
			get { return (0); }
		}
	}
}
