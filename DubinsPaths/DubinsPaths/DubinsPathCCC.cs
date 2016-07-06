using System;
using System.Windows;

namespace DubinsPaths
{
	class DubinsPathCCC : DubinsPath
	{
		/**** Variables ****/

		private Rotation outerRotation;


		/**** Functions ****/

		public DubinsPathCCC(Rotation outerRotation)
			: base()
		{
			this.outerRotation = outerRotation;
		}

		public override void Update()
		{
			CalculateTrajectory();
		}

		protected override void CalculateTrajectory()
		{
			// ToDo: Implement changed
			// ToDo: Implement Atan2() as in CSC trajectories.
			objects.Clear();
			if (start == null || target == null)
			{
				if (start == null || target == null)
				{
					valid = false;
				}
				return;
			}

			// ToDo: Comment
			float outerRotationAngle = (float)Math.PI / 2;
			if (outerRotation == Rotation.Right)
			{
				outerRotationAngle *= -1;
			}

			// Convert the start and target position.
			Point startPoint = ToWindowsPoint(start.Position);
			Point targetPoint = ToWindowsPoint(target.Position);

			// Calculate position of start circle.
			Vector startVector = new Vector(Math.Cos(start.Angle), -Math.Sin(start.Angle));
			Point startMidpoint = startPoint + RotateVector(startVector, outerRotationAngle) * rMin;

			// Calculate position of target circle.
			Vector targetVector = new Vector(Math.Cos(-target.Angle), Math.Sin(-target.Angle));
			Point targetMidpoint = targetPoint + RotateVector(targetVector, outerRotationAngle) * rMin;

			Vector startToTarget = targetMidpoint - startMidpoint;

			if (startToTarget.Length < 4 * rMin)
			{
				// ToDo: sign of y parameter in Atan2?
				float theta = (float)Math.Acos(startToTarget.Length / 4 / rMin);
				theta += (float)Math.Atan2(startToTarget.Y, startToTarget.X);

				Point transitionMidpoint = new Point();
				transitionMidpoint.X = startMidpoint.X + 2 * rMin * Math.Cos(theta);
				transitionMidpoint.Y = startMidpoint.Y + 2 * rMin * Math.Sin(theta);

				// Calculate tangent points.
				Vector c1c3 = transitionMidpoint - startMidpoint;
				Vector c2c3 = transitionMidpoint - targetMidpoint;
				Point tp1 = startMidpoint + c1c3 / 2;
				Point tp2 = targetMidpoint + c2c3 / 2;

				// Calculate arcs.
				// Start arc.
				float startAngle = start.Angle - outerRotationAngle;
				float startSweepAngle = (float)Math.Atan2(-c1c3.Y / 2, c1c3.X / 2);
				startSweepAngle -= startAngle;
				startSweepAngle = NormalizeAngle(startSweepAngle);
				if (outerRotation == Rotation.Right && startSweepAngle > 0)
				{
					startSweepAngle -= 2 * (float)Math.PI;
				}
				else if (outerRotation == Rotation.Left && startSweepAngle < 0)
				{
					startSweepAngle += 2 * (float)Math.PI;
				}
				objects.Add(new Arc(ToDrawingPoint(startMidpoint), rMin, startAngle, startSweepAngle));

				// Target arc.
				float targetAngle = target.Angle - outerRotationAngle;
				float targetSweepAngle = (float)Math.Atan2(-c2c3.Y / 2, c2c3.X / 2);
				targetSweepAngle -= targetAngle;
				targetSweepAngle = NormalizeAngle(targetSweepAngle);
				if (outerRotation == Rotation.Right && targetSweepAngle < 0)
				{
					targetSweepAngle += 2 * (float)Math.PI;
				}
				else if (outerRotation == Rotation.Left && targetSweepAngle > 0)
				{
					targetSweepAngle -= 2 * (float)Math.PI;
				}
				objects.Add(new Arc(ToDrawingPoint(targetMidpoint), rMin, targetAngle, targetSweepAngle));

				// Transistion arc.
				float transitionStartAngle = startAngle + startSweepAngle + (float)Math.PI;
				float transitionSweepAngle = targetAngle + targetSweepAngle + (float)Math.PI;
				transitionSweepAngle -= transitionStartAngle;
				transitionSweepAngle = NormalizeAngle(transitionSweepAngle);
				while (outerRotation == Rotation.Right && transitionSweepAngle < 0)
				{
					transitionSweepAngle += 2 * (float)Math.PI;
				}
				while (outerRotation == Rotation.Left && transitionSweepAngle > 0)
				{
					transitionSweepAngle -= 2 * (float)Math.PI;
				}
				objects.Add(new Arc(ToDrawingPoint(transitionMidpoint), rMin, transitionStartAngle, transitionSweepAngle));

				valid = true;
			}
			else
			{
				valid = false;
			}
			changed = false;
		}
	}
}