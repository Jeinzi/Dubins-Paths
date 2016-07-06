using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DubinsPaths.State
{
	class StateSettingTarget : State
	{
		public StateSettingTarget(Form1 form)
			: base(form)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(@"{\rtf\ansi\b Any mouse button: \b0 Set target point.");
			sb.Append(@"\line Drag to change angle.}");
			helpString = sb.ToString();
		}

		public override void MouseDown(object sender, MouseEventArgs e)
		{
			base.MouseDown(sender, e);

			// Save the location the user clicked on as target point.
			targetPoint = new DirectionalPoint(e.Location, 0);
			paths.AsParallel().ForAll(x => x.Target = targetPoint);
		}

		public override void MouseMove(object sender, MouseEventArgs e)
		{
			base.MouseMove(sender, e);
			if (!mousePressed) return;

			targetPoint.Angle = AngleBetweenPoints(targetPoint.Position, e.Location);
		}

		public override void MouseUp(object sender, MouseEventArgs e)
		{
			base.MouseUp(sender, e);

			// Transistion into the next state.
			stateManager.ActivateStateByName("StateAllSet");
		}
	}
}
