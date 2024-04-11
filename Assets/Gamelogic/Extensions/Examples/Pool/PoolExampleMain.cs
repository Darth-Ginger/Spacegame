using UnityEngine;
using UnityEngine.UI;

using Gamelogic.Extensions.Algorithms;

namespace Gamelogic.Extensions.Examples
{
	public class PoolExampleMain : GLMonoBehaviour
	{
		[SerializeField] private int poolCapacity = 0;
		[SerializeField] private Button characterPrefab = null;
		[SerializeField] private GameObject poolRoot = null;
		[SerializeField] private Color color1 = Color.black;
		[SerializeField] private Color color2 = Color.white;

		private MonoBehaviourPool<Button> characterPool;
		private IGenerator<Color> skinColor;

		public void Start()
		{
			characterPool = new MonoBehaviourPool<Button>(
				characterPrefab,
				poolRoot,
				poolCapacity,	

				(character) => 
				{
					character.gameObject.SetActive(true);
					character.gameObject.transform.SetAsLastSibling();
					character.image.color = skinColor.Next();
					character.onClick.AddListener(() => characterPool.ReleaseObject(character));	
				},
				
				(character) => 
				{
					character.onClick.RemoveAllListeners();
					character.gameObject.SetActive(false);
				});

			skinColor = Generator
				.UniformRandomFloat()
				.Select(t => Color.Lerp(color1, color2, t));
		}

		public void AddCharacter()
		{
			if(characterPool.IsObjectAvailable)
			{
				characterPool.GetNewObject();
			}
		}
	}
}
