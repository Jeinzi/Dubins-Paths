using System;
using System.Collections.Generic;
using System.Drawing;

namespace DubinsPaths
{
	abstract class DubinsPath
	{
		/**** Variables ****/

		protected bool valid;
		protected bool changed;
		protected float length;
		protected float rMin;
		protected DirectionalPoint start;
		protected DirectionalPoint target;
		protected List<GeometricalObject> objects;


		/**** Functions ****/

		protected DubinsPath()
		{
			valid = false;
			changed = false;
			rMin = 60;
			objects = new List<GeometricalObject>();
		}

		/// <summary>
		/// Calculates the distance between to points.
		/// </summary>
		/// <param name="pointA">The first point.</param>
		/// <param name="pointB">The second point.</param>
		/// <returns>The distance between the two points.</returns>
		protected float CalculateDistance(DirectionalPoint pointA, DirectionalPoint pointB)
		{
			int dx = pointB.X - pointA.X;
			int dy = pointB.Y - pointA.Y;
			float distance = (float)Math.Sqrt(dx * dx + dy * dy);
			return (distance);
		}

		public static System.Windows.Vector RotateVector(System.Windows.Vector vector, double angle)
		{
			System.Windows.Vector rotated = new System.Windows.Vector();
			double sin = Math.Sin(angle);
			double cos = Math.Cos(angle);
			rotated.X = vector.X * cos + vector.Y * sin;
			rotated.Y = -vector.X * sin + vector.Y * cos;
			return (rotated);
		}

		public static float NormalizeAngle(float angle)
		{
			float Pi2 = 2 * (float)Math.PI;
			while (angle >= Pi2) angle -= Pi2;
			while (angle <= -Pi2) angle += Pi2;
			return (angle);
		}

		public static System.Drawing.PointF ToDrawingPoint(System.Windows.Point wPoint)
		{
			System.Drawing.PointF dPoint = new System.Drawing.PointF();
			dPoint.X = (float)wPoint.X;
			dPoint.Y = (float)wPoint.Y;
			return (dPoint);
		}

		public static System.Windows.Point ToWindowsPoint(System.Drawing.PointF dPoint)
		{
			System.Windows.Point wPoint = new System.Windows.Point();
			wPoint.X = dPoint.X;
			wPoint.Y = dPoint.Y;
			return (wPoint);
		}

		/// <summary>
		/// Renders the geometrical objects the dubins paths consists of.
		/// </summary>
		/// <param name="g"></param>
		public void Render(Graphics g)
		{
			if (start != null) start.Render(g);
			if (target != null) target.Render(g);
			for (int i = 0; i < objects.Count; i++)
			{
				objects[i].Render(g);
			}
		}

		public abstract void Update();

		protected abstract void CalculateTrajectory();

		public DirectionalPoint Start
		{
			get { return (start); }
			set
			{
				start = value;
				changed = true;
			}
		}

		public DirectionalPoint Target
		{
			get { return (target); }
			set
			{
				target = value;
				changed = true;
			}
		}

		public float Length
		{
			get
			{
				float length = 0;
				for(int i = 0; i < objects.Count; i++)
				{
					length += objects[i].Length;
				}
				return (length);
			}
		}

		public float RMin
		{
			get { return (rMin); }
			set
			{
				if(value > 0 && value < 100000)
				{
					rMin = value;
					changed = true;
				}
			}
		}

		public bool Valid
		{
			get { return (valid); }
		}
	}

	public enum Rotation
	{
		Positive,
		Left = Positive,
		Counterclockwise = Positive,
		Negative,
		Right = Negative,
		Clockwise = Negative
	}
}
