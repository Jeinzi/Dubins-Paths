using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DubinsPaths
{
	/// <summary>
	/// Represents a drawable geometrical object.
	/// </summary>
	abstract class GeometricalObject
	{
		protected Pen pen = new Pen(Brushes.Black, 2);

		public abstract void Render(Graphics g);

		public abstract float Length { get; }
	}
}
