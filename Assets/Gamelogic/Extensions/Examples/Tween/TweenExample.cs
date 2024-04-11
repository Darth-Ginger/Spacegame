using UnityEngine;

namespace Gamelogic.Extensions.Examples
{
	public class TweenExample : GLMonoBehaviour
	{
		[SerializeField] private Transform cube = null;
		[SerializeField] private Transform leftPosition = null;
		[SerializeField] private Transform rightPosition = null;

		public void GoLeft()
		{
			Tween(
				cube.position, 
				leftPosition.transform.position, 
				1, 
				Vector3.Lerp, 
				newPosition => { cube.position = newPosition; });
		}

		public void GoRight()
		{
			Tween(
				cube.position,
				rightPosition.transform.position,
				1,
				Vector3.Lerp,
				newPosition => { cube.position = newPosition; });
		}
	}
}
