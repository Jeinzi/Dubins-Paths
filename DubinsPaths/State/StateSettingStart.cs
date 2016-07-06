using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DubinsPaths.State
{
	class StateSettingStart : State
	{
		public StateSettingStart(Form1 form)
			: base(form)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(@"{\rtf\ansi\b Any mouse button: \b0 Set start point.");
			sb.Append(@"\line Drag to change angle.}");
			helpString = sb.ToString();
		}

		public override void Init()
		{
			base.Init();
			paths.AsParallel().ForAll(x => x.Start = null);
			paths.AsParallel().ForAll(x => x.Target = null);
		}

		public override void MouseDown(object sender, MouseEventArgs e)
		{
			base.MouseDown(sender, e);

			// Save the location the user clicked on as start point.
			startPoint = new DirectionalPoint(e.Location, 0);
			paths.AsParallel().ForAll(x => x.Start = startPoint);
		}

		public override void MouseMove(object sender, MouseEventArgs e)
		{
			base.MouseMove(sender, e);
			if (!mousePressed) return;

			startPoint.Angle = AngleBetweenPoints(startPoint.Position, e.Location);
		}

		public override void MouseUp(object sender, MouseEventArgs e)
		{
			base.MouseUp(sender, e);

			// Transistion into the next state.
			stateManager.ActivateStateByName("StateSettingTarget");
		}
	}
}
