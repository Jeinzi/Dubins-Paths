using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DubinsPaths.State
{
	/// <summary>
	/// Manages a list of states and passes user input and update methods
	/// down to the currently active one.
	/// </summary>
	class StateManager
	{
		/**** Variables ****/

		private int currentState;
		private List<State> states;
		
		
		/**** Functions ****/

		public StateManager()
		{
			states = new List<State>();
			currentState = 0;
		}

		/// <summary>
		/// Activates a state.
		/// </summary>
		/// <param name="stateName">The name of the state class.</param>
		/// <returns>Whether the state has been activated successfully.</returns>
		public bool ActivateStateByName(string stateName)
		{
			// Search for a state with the given class name.
			for(int i = 0; i < states.Count; i++)
			{
				string[] typeNames = states[i].GetType().ToString().Split('.');
				string typeName = typeNames[typeNames.Length - 1];
				if(typeName == stateName)
				{
					currentState = i;
					states[i].Init();
					return (true);
				}
			}

			return (false);
		}

		/// <summary>
		/// Updates the currently active state.
		/// </summary>
		public void Update()
		{
			states[currentState].Update();
		}

		public void Render(Graphics g)
		{
			states[currentState].Render(g);
		}

		/// <summary>
		/// Passes the mouse event to the currently active state.
		/// </summary>
		/// <param name="sender">The object triggering the event.</param>
		/// <param name="e">The event arguments related to the event.</param>
		public void MouseDown(object sender, MouseEventArgs e)
		{
			states[currentState].MouseDown(sender, e);
		}

		/// <summary>
		/// Passes the mouse event to the currently active state.
		/// </summary>
		/// <param name="sender">The object triggering the event.</param>
		/// <param name="e">The event arguments related to the event.</param>
		public void MouseUp(object sender, MouseEventArgs e)
		{
			states[currentState].MouseUp(sender, e);
		}

		/// <summary>
		/// Passes the mouse event to the currently active state.
		/// </summary>
		/// <param name="sender">The object triggering the event.</param>
		/// <param name="e">The event arguments related to the event.</param>
		public void MouseMove(object sender, MouseEventArgs e)
		{
			states[currentState].MouseMove(sender, e);
		}

		/// <summary>
		/// Adds a new state to the state manager.
		/// </summary>
		/// <param name="state"></param>
		public void AddState(State state)
		{
			state.StateManager = this;
			if(states.Count == 0)
			{
				state.Init();
			}
			states.Add(state);
		}


		/**** Getter & Setter ****/

		public State ActiveState
		{
			get { return (states[currentState]); }
		}


		/**** Operators ****/

		/// <summary>
		/// Adds a new state to the state manager.
		/// </summary>
		/// <param name="stateManager">The state manager to modify.</param>
		/// <param name="state">The state to add to the state manager.</param>
		/// <returns>A new state manager containing the new state.</returns>
		public static StateManager operator +(StateManager stateManager, State state)
		{
			stateManager.AddState(state);
			return (stateManager);
		}
	}
}
