using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DubinsPaths
{
	class Arc : GeometricalObject
	{
		/**** Variables ****/

		private float radius;
		private float startAngle;
		private float sweepAngle;
		private PointF position;
		private RectangleF outline;

		/**** Functions ****/

		public Arc(PointF position, float radius, double startAngle, double sweepAngle)
		{
			this.radius = radius;
			this.startAngle = (float)startAngle;
			this.position = position;
			this.sweepAngle = (float)sweepAngle;

			float x = position.X - radius;
			float y = position.Y - radius;
			outline = new RectangleF(x, y, radius * 2, radius * 2);
		}

		public override void Render(Graphics g)
		{
			if (Math.Abs(sweepAngle) < 0.005)
			{
				return;
			}

			float startAngleDeg = startAngle * (180 / (float)Math.PI);
			float sweepAngleDeg = sweepAngle * (180 / (float)Math.PI);
			g.DrawArc(pen, outline, -startAngleDeg, -sweepAngleDeg);
		}

		public override float Length
		{
			get
			{
				float length = Math.Abs(radius * sweepAngle);
				return (length);
			}
		}
	}
}