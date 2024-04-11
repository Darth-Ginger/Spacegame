// Copyright Gamelogic (c) http://www.gamelogic.co.za

using Gamelogic.Extensions.Internal;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// Provides a generic implementation of a singleton-like pattern for <see cref="MonoBehaviour"/> classes.
	/// This class automatically searches for an existing instance in the scene or logs an error if none or more than one
	/// are found.
	/// </summary>
	/// <typeparam name="T">The type of the Singleton class derived from <see cref="MonoBehaviour"/>.</typeparam>
	/// <remarks> Singletons usually manage their own creation, but it is common in Unity projects to have a singleton
	/// already placed in the scene. In this sense, this class really checks whether an instance can be used as a
	/// singleton, and then provides access to the instance.
	///
	/// If you want to create a singleton that is not already placed in the scene, you can use put
	/// <see cref="SingletonPrefabSpawner{T}"/> in the scene instead. 
	/// </remarks>
	[Version(1)]
	[AddComponentMenu("Gamelogic/Extensions/Singleton")]
	public class Singleton<T> : GLMonoBehaviour 
		where T : MonoBehaviour
	{
		#region  Properties
		/// <summary>
		/// Returns the instance of this singleton.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (instance != null)
				{
					return instance;
				}
				
				var instances = FindObjectsOfType<T>();
				
				if(instances.Length == 0)
				{
					Debug.LogError($"An instance of {typeof(T)} is needed in the scene, but there is none.");
				}
				else if (instances.Length > 1)
				{
					Debug.LogError($"There is more than one instance of {typeof(T)} in the scene. Only one instance is allowed.");
				}
				else
				{
					instance = instances[0];
				}

				return instance;
			}
		}
		#endregion

		#region Private Fields
		protected static T instance;
		#endregion

		#region Unity Messages
		[Version(2, 6)]
		private void OnDestroy() => instance = null;
		#endregion
	}
	
	[Version(2, 6)]
	public class SingletonPrefabSpawner<T> : GLMonoBehaviour 
		where T : MonoBehaviour
	{
		[SerializeField] private T prefab = null;
		[SerializeField] private bool dontDestroyOnLoad = false;

		public void Awake()
		{
			if (Singleton<T>.Instance == null)
			{
				Instantiate(prefab);
			}
			
			/*	Why outside the if statement above? This is for the situation
				where the prefab _is_ placed in the scene (probably manually)
				and DontDestroyOnLoad has not been called, as is typical. 
			*/
			if (dontDestroyOnLoad)
			{
				DontDestroyOnLoad(Singleton<T>.Instance.gameObject);
			}
			
			Assert.IsNotNull(Singleton<T>.Instance);
		}
	}
}
