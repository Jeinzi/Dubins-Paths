using System;
using System.Windows;

namespace DubinsPaths
{
	class DubinsPathCSC : DubinsPath
	{
		/**** Variables ***/

		private Rotation startRotation;
		private Rotation targetRotation;


		/**** Functions ****/

		public DubinsPathCSC(Rotation startRotation, Rotation targetRotation)
			: base()
		{
			this.startRotation = startRotation;
			this.targetRotation = targetRotation;
		}

		/// <summary>
		/// Recalculates the trajectory, if neccessary.
		/// </summary>
		public override void Update()
		{
			CalculateTrajectory();
		}

		/// <summary>
		/// Recalculates the trajectory, if the start or the end condition
		/// has been changed.
		/// </summary>
		protected override void CalculateTrajectory()
		{
			objects.Clear();
			if (!changed || start == null || target == null)
			{
				if (start == null || target == null)
				{
					valid = false;
				}
				return;
			}

			// Set the radius of the start and the target circle.
			// If the start and target rotations are not equal, one radius is
			// made negative, resulting in an angled tangent.
			// (in relation to the direct connection between the points)
			float rStart = rMin;
			float rTarget = rMin;
			if (startRotation != targetRotation)
			{
				rTarget *= -1;
			}

			// During the calculation, some vectors need to be rotated by
			// Pi/2 depending on the direction of their corresponding circles.
			double startRotationAngle = Math.PI / 2;
			if (startRotation == Rotation.Right)
			{
				startRotationAngle *= -1;
			}
			double targetRotationAngle = Math.PI / 2;
			if (targetRotation == Rotation.Right)
			{
				targetRotationAngle *= -1;
			}

			// Convert the start and target position.
			Point startPoint = ToWindowsPoint(start.Position);
			Point targetPoint = ToWindowsPoint(target.Position);

			// Find the midpoints of the start and target circles by rotating
			// the start and target tangents by (+-)Pi/2.
			// If the circle is turing left, the tangent is rotated by Pi/2 (left),
			// if the circle is turing right, the tangent is rotated by -Pi/2 (right).
			// Note that startVector and targetVector have length one!
			Vector startVector = new Vector(Math.Cos(start.Angle), -Math.Sin(start.Angle));
			Point startMidpoint = startPoint + RotateVector(startVector, startRotationAngle) * rMin;
			Vector targetVector = new Vector(Math.Cos(target.Angle), -Math.Sin(target.Angle));
			Point targetMidpoint = targetPoint + RotateVector(targetVector, targetRotationAngle) * rMin;

			Vector startToTarget = targetMidpoint - startMidpoint;

			// ToDo: Something wrong here. If the distance of RSL or LSR circles
			// gets less than 2 * rMin (-> circles intersecting) no tangent can
			// be calculated => havoc
			if (startRotation != targetRotation &&
				startToTarget.Length <= 2 * rMin)
			{
				valid = false;
				return;
			}

			// tangentAngle is the angle between the direct connection of the
			// circles and the tangent between them. If the circles are both
			// rotating in the same direction, (rStart - rTarget) is zero and
			// tangentAngle also evaluates to 0.
			// If they are rotating in opposite direction, (rStart - rTarget)
			// is equal to 2 * rMin and the tangent becomes inclined.
			float tangentAngle = (float)Math.Acos((rStart - rTarget) / startToTarget.Length);
			if (startRotation == Rotation.Left) tangentAngle *= -1;
			// Normalize the vector connection both circles.
			Vector startToTargetN = startToTarget;
			startToTargetN.Normalize();

			// By rotating the direct connection of the circles by tangentAngle,
			// the vector n normal to the tangent is created.
			Vector n = RotateVector(startToTargetN, tangentAngle);
			// By adding the difference in radii to the connection of the two
			// circles in in direction of n, the vector leading from one tangent
			// point to the other is created.
			Vector tangentPointConnection = startToTarget + (rTarget - rStart) * n;

			// Now the tangent points can be calculated.
			Point tp1 = startMidpoint + n * rMin;
			Point tp2 = tp1 + tangentPointConnection;


			// Start arc.
			// Make sure startAngle is within (-pi, pi) just like the return
			// value of Atan2().
			double arcStartAngle = (start.Angle - startRotationAngle);
			if (arcStartAngle > Math.PI) arcStartAngle -= Math.PI * 2;
			if (arcStartAngle < -Math.PI) arcStartAngle += Math.PI * 2;
			// By using Atan2() on the vector n (leading to tp1) and substracting
			// the startAngle, one gets a signed angle between the two directions.
			double startSweepAngle = Math.Atan2(-n.Y, n.X) - arcStartAngle;
			// Change the angle to fit the direction of the circle.
			if (startRotation == Rotation.Right && startSweepAngle > 0)
				startSweepAngle -= Math.PI * 2;
			else if (startRotation == Rotation.Left && startSweepAngle < 0)
				startSweepAngle += Math.PI * 2;
			Arc startArc = new Arc(ToDrawingPoint(startMidpoint), rMin, arcStartAngle, startSweepAngle);


			// Target arc.
			if (targetRotation != startRotation) n.Negate();

			arcStartAngle = (target.Angle - targetRotationAngle);
			if (arcStartAngle > Math.PI) arcStartAngle -= Math.PI * 2;
			if (arcStartAngle < -Math.PI) arcStartAngle += Math.PI * 2;

			double targetSweepAngle = arcStartAngle - Math.Atan2(-n.Y, n.X);
			if (targetRotation == Rotation.Right && targetSweepAngle > 0)
				targetSweepAngle -= Math.PI * 2;
			else if (targetRotation == Rotation.Left && targetSweepAngle < 0)
				targetSweepAngle += Math.PI * 2;
			Arc targetArc = new Arc(ToDrawingPoint(targetMidpoint), rMin, Math.Atan2(-n.Y, n.X), targetSweepAngle);

			// Add geometricalobjects to the list.
			Line line = new Line(ToDrawingPoint(tp1), ToDrawingPoint(tp2));
			objects.Add(line);
			objects.Add(startArc);
			objects.Add(targetArc);

			valid = true;
		}
	}
}
