using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DubinsPaths.State
{
	/// <summary>
	/// Serves as a base class for all states.
	/// </summary>
	abstract class State
	{
		/**** Variables ****/

		protected string helpString;
		protected static StateManager stateManager;
		protected static Form1 form;
		protected bool mousePressed;
		protected bool rotating;
		protected bool moving;
		protected static uint selectionDistance;
		protected static List<DubinsPaths.DubinsPath> paths;
		protected static DirectionalPoint startPoint;
		protected static DirectionalPoint targetPoint;


		/**** Functions ****/

		protected State(Form1 form)
		{
			State.form = form;

			// Define variables.
			mousePressed = false;
			rotating = false;
			moving = false;
			selectionDistance = 30;
			paths = new List<DubinsPath>();

			// Create dubins paths.
			paths = new List<DubinsPaths.DubinsPath>();
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Right, Rotation.Right));
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Right, Rotation.Left));
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Left, Rotation.Right));
			paths.Add(new DubinsPaths.DubinsPathCSC(Rotation.Left, Rotation.Left));
			paths.Add(new DubinsPaths.DubinsPathCCC(Rotation.Right));
			paths.Add(new DubinsPaths.DubinsPathCCC(Rotation.Left));
		}

		public virtual void Init()
		{
			form.SetHelpText(helpString);
		}

		public virtual void Update()
		{
			for (int i = 0; i < paths.Count; i++)
			{
				paths[i].Update();
			}

			// Choose shortest path.
			bool isChecked = form.UseShortestIsChecked();

			if (isChecked)
			{
				int indexShortest = 0;
				float minimalDistance = float.PositiveInfinity;
				for (int i = 0; i < paths.Count; i++)
				{
					if (paths[i].Valid && paths[i].Length < minimalDistance)
					{
						indexShortest = i;
						minimalDistance = paths[i].Length;
					}
				}
				form.SetSelectedPath(indexShortest);
			}
		}

		public virtual void Render(Graphics g)
		{
			int selectedIndex = form.GetSelectedPath();
			if (selectedIndex >= 0)
			{
				paths[selectedIndex].Render(g);
				string lengthString = paths[selectedIndex].Length.ToString();
				form.SetPathLength(lengthString);
			}
		}

		public virtual void MouseDown(object sender, MouseEventArgs e)
		{
			mousePressed = true;
		}

		public virtual void MouseUp(object sender, MouseEventArgs e)
		{
			mousePressed = false;
			moving = false;
			rotating = false;
		}

		public virtual void MouseMove(object sender, MouseEventArgs e) { }


		protected float AngleBetweenPoints(Point pointA, Point pointB)
		{
			float angle = 0;
			float dx = pointB.X - pointA.X;
			float dy = pointA.Y - pointB.Y;
			angle = (float)Math.Atan2(dy, dx);
			return (angle);
		}


		/**** Getter & Setter ****/

		/// <summary>
		/// A string explaining the possible actions the user may perform
		/// in the given state. Formatted as RTF for use with a RichTextBox.
		/// </summary>
		public virtual string Help
		{
			get { return (helpString); }
		}

		/// <summary>
		/// The minimum turning radius used for calculating the dubins paths.
		/// </summary>
		public float RMin
		{
			get { return (paths[0].RMin); }
			set
			{
				if (value > 0)
				{
					paths.AsParallel().ForAll(x => x.RMin = value);
				}
			}
		}

		/// <summary>
		/// Gets or sets the currently used state manager.
		/// </summary>
		public StateManager StateManager
		{
			get { return (stateManager); }
			set { stateManager = value; }
		}
	}
}