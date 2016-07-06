using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace DubinsPaths
{
	public partial class Form1 : Form
	{
		/**** Variables ****/

		private bool running;
		private bool mousePressed;
		private bool rotating;
		private bool moving;
		private uint targetFps;
		private uint selectionDistance;
		private State state;
		private DirectionalPoint chosenPoint;
		private Graphics g;
		private Thread loopThread;
		private List<DubinsPaths.DubinsPath> paths;
		private List<string> stateHelp;


		/**** Functions ****/

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// Define variables and constants.
			running = true;
			mousePressed = false;
			rotating = false;
			moving = false;
			targetFps = 30;
			selectionDistance = 30;
			state = State.settingStart;

			// Create objects.
			g = pictureBox.CreateGraphics();
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			stateHelp = new List<string>();

			// Build help strings.
			StringBuilder sb = new StringBuilder();
			sb.Append(@"{\rtf\ansi\b Any mouse button: \b0 Set start point.");
			sb.Append(@"\line Drag to change angle.}");
			stateHelp.Add(sb.ToString());
			sb.Clear();

			sb.Append(@"{\rtf\ansi\b Any mouse button: \b0 Set target point.");
			sb.Append(@"\line Drag to change angle.}");
			stateHelp.Add(sb.ToString());
			sb.Clear();

			sb.Append(@"{\rtf\ansi\b Left mouse button: \b0 Click to move point.");
			sb.Append(@"\line\line\b Right mouse button: \b0 Change angle.}");
			stateHelp.Add(sb.ToString());
			sb.Clear();

			// Create dubins paths.
			paths = new List<DubinsPaths.DubinsPath>();
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Right, Rotation.Right));
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Right, Rotation.Left));
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Left, Rotation.Right));
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Left, Rotation.Left));
			paths.Add(new DubinsPaths.DubinsPathCCC(Rotation.Right));
			paths.Add(new DubinsPaths.DubinsPathCCC(Rotation.Left));

			// Gui.
			helpBox.Rtf = stateHelp[(int)state];
			textBoxRadius.Text = paths[0].RMin.ToString();
			trajectoryList.SelectedIndex = 0;

			// Start looping thread.
			loopThread = new Thread(Loop);
			loopThread.Start();
		}

		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			mousePressed = true;

			// If the start and target points are not set yet, save the
			// location the user clicked on as one of them.
			if (state != State.allSet)
			{
				DirectionalPoint point = new DirectionalPoint(new Point(e.X, e.Y), 0);
				if (state == State.settingStart)
					paths.AsParallel().ForAll(x => x.Start = point);
				else
					paths.AsParallel().ForAll(x => x.Target = point);
			}
			else
			{
				// Distance cursor - start point.
				int dx = paths[0].Start.X - e.X;
				int dy = paths[0].Start.Y - e.Y;
				uint distanceStart = (uint)Math.Sqrt(dx * dx + dy * dy);
				// Distance cursor - target point.
				dx = paths[0].Target.X - e.X;
				dy = paths[0].Target.Y - e.Y;
				uint distanceTarget = (uint)Math.Sqrt(dx * dx + dy * dy);
				
				// Start moving / rotating, if the cursor is near a point.
				if(distanceStart <= selectionDistance ||
					distanceTarget <= selectionDistance)
				{
					// Choose the point. The start and target points of all
					// DubinsPath objects reference the same data.
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
		}

		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (!mousePressed) return;

			if (state != State.allSet)
			{
				// Choose point to manipulate.
				DirectionalPoint point;
				if (state == State.settingStart)
					point = paths[0].Start;
				else
					point = paths[0].Target;

				// Calculate angle from point to cursor.
				point.Angle = AngleBetweenPoints(point.Position, e.Location);
			}
			else if(rotating)
			{
				chosenPoint.Angle = AngleBetweenPoints(chosenPoint.Position, e.Location);
			}
			else if(moving)
			{
				chosenPoint.Position = e.Location;
			}
		}

		private float AngleBetweenPoints(Point pointA, Point pointB)
		{
			float angle = 0;
			float dx = pointB.X - pointA.X;
			float dy = pointA.Y - pointB.Y;
			angle = (float)Math.Atan2(dy, dx);
			return (angle);
		}

		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			mousePressed = false;
			moving = false;
			rotating = false;

			// Transistion into the next state.
			if (state != State.allSet)
			{
				state++;
				helpBox.Rtf = stateHelp[(int)state];
			}
		}

		delegate void SetTextCallback(string text);
		delegate void SetSelectedCallback(int index);
		delegate bool IsCheckedCallback();

		/// <summary>
		/// Main loop updating and rendering the dubins paths.
		/// </summary>
		private void Loop()
		{
			Stopwatch fpsStopwatch = new Stopwatch();
			Bitmap backBuffer = new Bitmap(pictureBox.Width, pictureBox.Height);
			Graphics backGraphics = Graphics.FromImage(backBuffer);

			while (running)
			{
				fpsStopwatch.Restart();

				for (int i = 0; i < paths.Count; i++)
				{
					paths[i].Update();
				}

				// Choose shortest path.
				IsCheckedCallback callbackChecked = () =>
				{ return (checkBoxUseShortest.Checked); };
				bool isChecked = (bool)Invoke(callbackChecked);

				if (isChecked)
				{
					int shortestIndex = 0;
					float minimalDistance = float.PositiveInfinity;
					for (int i = 0; i < paths.Count; i++)
					{
						if (paths[i].Valid && paths[i].Length < minimalDistance)
						{
							shortestIndex = i;
							minimalDistance = paths[i].Length;
						}
					}

					SetSelectedCallback callbackSetSelected = (int index) =>
					{ trajectoryList.SelectedIndex = index; };
					Invoke(callbackSetSelected, new object[] { shortestIndex });
				}

				// Update and render to backbuffer.
				backGraphics.Clear(Color.White);

				Func<int> function = new Func<int>(() =>
					{ return (trajectoryList.SelectedIndex); });
				int selectedIndex = (int)trajectoryList.Invoke(function);
				if (selectedIndex >= 0)
				{
					paths[selectedIndex].Render(backGraphics);
					string lengthString = paths[selectedIndex].Length.ToString();
					SetTextCallback callbackSetText = (string text) =>
						{ labelLength.Text = text; };
					Invoke(callbackSetText, new object[] { lengthString });
				}

				// Display backbuffer.
				g.DrawImage(backBuffer, 0, 0);

				// FPS regulation.
				uint timePerFrame = 1000 / targetFps;
				int difference = (int)(timePerFrame - fpsStopwatch.ElapsedMilliseconds);
				if (difference > 0)
				{
					Thread.Sleep(difference);
				}
			}
		}

		/// <summary>
		/// Resets the state to the first state and deletes the dubins paths.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonReset_Click(object sender, EventArgs e)
		{
			state = State.settingStart;
			helpBox.Rtf = stateHelp[(int)state];
			paths.AsParallel().ForAll(x => x.Start = null);
			paths.AsParallel().ForAll(x => x.Target = null);
		}

		/// <summary>
		/// If the form is about to be closed, abort the loop thread and wait
		/// for it to stop. That way, no form resources are used after the
		/// form is closed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			running = false;
			loopThread.Abort();
			loopThread.Join();
		}

		private void textBoxRadius_TextChanged(object sender, EventArgs e)
		{
			try
			{
				float rMin = (float)Convert.ToDouble(textBoxRadius.Text);
				paths.AsParallel().ForAll(x => x.RMin = rMin);
			}
			catch { };
		}

		private void checkBoxUseShortest_CheckedChanged(object sender, EventArgs e)
		{
			if(checkBoxUseShortest.Checked)
			{
				trajectoryList.Enabled = false;
			}
			else
			{
				trajectoryList.Enabled = true;
			}
		}
	}

	/// <summary>
	/// Represents the different states the software can be in.
	/// </summary>
	enum State
	{
		settingStart,
		settingTarget,
		allSet
	}
}