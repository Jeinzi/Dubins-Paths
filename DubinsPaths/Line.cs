using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubinsPaths
{
	class Line : GeometricalObject
	{
		/**** Variables ****/

		private PointF start;
		private PointF target;

		/**** Functions ****/

		public Line(PointF start, PointF end)
		{
			this.start = start;
			this.target = end;
		}

		/// <summary>
		/// Renders the line.
		/// </summary>
		/// <param name="g">The graphics object to render to.</param>
		public override void Render(Graphics g)
		{
			g.DrawLine(pen, start, target);
		}

		public override float Length
		{
			get
			{
				float dx = target.X - start.X;
				float dy = target.Y - start.Y;
				float length = (float)Math.Sqrt(dx * dx + dy * dy);
				return (length);
			}
		}
	}
}
