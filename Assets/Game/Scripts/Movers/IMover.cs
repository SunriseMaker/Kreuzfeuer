using UnityEngine;

namespace Game
{
	public interface IMover
	{
		/// <summary> Move character. </summary>
		/// <returns>True if position changed.</returns>
		bool Move(float forwardAmount, float rightAmount, float upAmount, float timeAmount);

		/// <summary> Move character towards position. </summary>
		/// <returns>True if position changed.</returns>
		bool MoveTowards(Vector3 targetPosition, float timeAmount);

		bool CanMove { get; }

		bool CanFly { get; }
		
		void RotateTowards(Vector3 targetPosition, bool instantly);
	}
}
