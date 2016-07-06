using System.Drawing;

namespace DubinsPaths
{
	class CrossPoint : GeometricalObject
	{
		/**** Variables ****/

		float lineLength;
		public PointF point;

		/**** Functions ****/

		public CrossPoint(PointF point)
		{
			lineLength = 20;
			this.point = point;
		}

		public override void Render(Graphics g)
		{
			// Cross: Rotate first by 45°, then again by 90°
			// and draw a line each time.
			g.TranslateTransform(point.X, point.Y);
			for (int angle = 45; angle <= 90; angle += 45)
			{
				g.RotateTransform(angle);
				PointF left = new PointF(-lineLength / 2, 0);
				PointF right = new PointF(lineLength / 2, 0);
				g.DrawLine(pen, left, right);
			}
			g.ResetTransform();
		}

		public override float Length
		{
			get { return (0); }
		}
	}
}