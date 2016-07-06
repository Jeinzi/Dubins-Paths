using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using DubinsPaths.State;

namespace DubinsPaths
{
	public partial class Form1 : Form
	{
		/**** Variables ****/

		private bool running;
		private uint targetFps;
		private Graphics g;
		private Thread loopThread;
		private StateManager stateManager;

		// Define delegates used for external
		// cross-thread calls on control elements.
		delegate void SetTextDelegate(string text);
		delegate void SetIntegerDelegate(int index);
		delegate int ReturnIntegerDelegate();
		delegate bool ReturnBoolDelegate();


		/**** Functions ****/

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// Define variables and constants.
			running = true;
			targetFps = 30;
			
			// Create objects.
			g = pictureBox.CreateGraphics();
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;			

			// State manager.
			stateManager = new StateManager();
			stateManager += new StateSettingStart(this);
			stateManager += new StateSettingTarget(this);
			stateManager += new StateAllSet(this);

			// Gui.
			trajectoryList.SelectedIndex = 0;
			textBoxRadius.Text = stateManager.ActiveState.RMin.ToString();

			// Start looping thread.
			loopThread = new Thread(Loop);
			loopThread.Start();
		}

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

				// Update and render to backbuffer.
				backGraphics.Clear(Color.White);
				stateManager.Update();
				stateManager.Render(backGraphics);

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
		/// Sets the text for the help box displayed to the user.
		/// </summary>
		/// <param name="text">The help text.</param>
		public void SetHelpText(string text)
		{
			if(!helpBox.InvokeRequired)
			{
				// If the calling method and the help box live within the same
				// thread, the text may be set directly.
				// This code may fail, if the provided text is not formatted
				// as proper rtf.
				try
				{
					helpBox.Rtf = text;
				}
				catch { }
			}
			else
			{
				// If the method is called from a different thread as the one
				// the help box is created for, it has to be recursivly invoked
				// for that thread.
				SetTextDelegate myDelegate = SetHelpText;
				helpBox.Invoke(myDelegate, new object[] { text });
			}
		}

		/// <summary>
		/// Sets the length of the dubins path.
		/// </summary>
		/// <param name="length">The length of the path.</param>
		public void SetPathLength(string length)
		{
			if (!labelLength.InvokeRequired)
			{
				labelLength.Text = length;
			}
			else
			{
				// If the method is called from a different thread as the one
				// the help box is created for, it has to be recursivly invoked
				// for that thread.
				SetTextDelegate myDelegate = SetPathLength;
				labelLength.Invoke(myDelegate, new object[] { length });
			}
		}

		/// <summary>
		/// Returns whether the shortest dubins path
		/// should always be the one displayed.
		/// </summary>
		/// <returns></returns>
		public bool UseShortestIsChecked()
		{
			// Same game as in SetHelpText - if neccessary, the method is
			// invoked in the thread the the control element was created for.
			if (!checkBoxUseShortest.InvokeRequired)
			{
				return (checkBoxUseShortest.Checked);
			}
			else
			{
				ReturnBoolDelegate myDelegate = UseShortestIsChecked;
				bool isChecked = (bool)helpBox.Invoke(myDelegate);
				return (isChecked);
			}
		}

		/// <summary>
		/// Returns the index of the currently selected dubins path.
		/// </summary>
		/// <returns></returns>
		public int GetSelectedPath()
		{
			// See SetHelpText for detailled explanation.
			if (!trajectoryList.InvokeRequired)
			{
				return (trajectoryList.SelectedIndex);
			}
			else
			{
				ReturnIntegerDelegate myDelegate = GetSelectedPath;
				int index = (int)trajectoryList.Invoke(myDelegate);
				return (index);
			}
		}

		/// <summary>
		/// Sets the currently selected dubins path.
		/// </summary>
		/// <returns>
		/// The index of the dubins path within the trajectory list to be selected.
		/// </returns>
		public void SetSelectedPath(int index)
		{
			// See SetHelpText for detailed explanation.
			if (!trajectoryList.InvokeRequired)
			{
				trajectoryList.SelectedIndex = index;
			}
			else
			{
				SetIntegerDelegate myDelegate = SetSelectedPath;
				trajectoryList.Invoke(myDelegate, new object[] { index });
			}
		}

		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			stateManager.MouseDown(sender, e);
		}

		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			stateManager.MouseMove(sender, e);
		}

		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			stateManager.MouseUp(sender, e);
		}

		/// <summary>
		/// Resets the state to the initial state.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonReset_Click(object sender, EventArgs e)
		{
			stateManager.ActivateStateByName("StateSettingStart");
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
				stateManager.ActiveState.RMin = rMin;
			}
			catch { };
		}

		/// <summary>
		/// If the checkbox is toggled, the trajectory list is activated or
		/// deactivated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
}