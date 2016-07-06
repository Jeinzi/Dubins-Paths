using System;
using System.Text;
using System.Windows.Forms;

namespace DubinsPaths.State
{
	class StateAllSet : State
	{
		/**** Variables ****/

		DirectionalPoint chosenPoint;


		/**** Functions ****/

		public StateAllSet(Form1 form)
			: base(form)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(@"{\rtf\ansi\b Left mouse button: \b0 Click to move point.");
			sb.Append(@"\line\line\b Right mouse button: \b0 Change angle.}");
			helpString = sb.ToString();
		}

		public override void MouseDown(object sender, MouseEventArgs e)
		{
			base.MouseDown(sender, e);

			// Distance cursor - start point.
			int dx = paths[0].Start.X - e.X;
			int dy = paths[0].Start.Y - e.Y;
			uint distanceStart = (uint)Math.Sqrt(dx * dx + dy * dy);
			// Distance cursor - target point.
			dx = paths[0].Target.X - e.X;
			dy = paths[0].Target.Y - e.Y;
			uint distanceTarget = (uint)Math.Sqrt(dx * dx + dy * dy);

			// Start moving / rotating, if the cursor is near a point.
			if (distanceStart <= selectionDistance ||
				distanceTarget <= selectionDistance)
			{
				// Choose the point. Note that the start and target points
				// of all DubinsPath objects reference the same data.
				if (distanceStart < distanceTarget)
				{
					chosenPoint = paths[0].Start;
				}
				else
				{
					chosenPoint = paths[0].Target;
				}
				// Activate moving or rotating mode
				// dependant on the button pressed.
				if (e.Button == MouseButtons.Left)
				{
					moving = true;
				}
				else if (e.Button == MouseButtons.Right)
				{
					rotating = true;
				}
			}
		}

		public override void MouseMove(object sender, MouseEventArgs e)
		{
			base.MouseMove(sender, e);
			if (!mousePressed) return;

			if (rotating)
			{
				chosenPoint.Angle = AngleBetweenPoints(chosenPoint.Position, e.Location);
			}
			else if (moving)
			{
				chosenPoint.Position = e.Location;
			}
		}
	}
}
