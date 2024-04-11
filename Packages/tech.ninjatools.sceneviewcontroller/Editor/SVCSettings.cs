using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NinjaTools.NinjaSceneViewController.Editor
{
	[FilePath( "UserSettings/SceneViewController-Settings.asset", FilePathAttribute.Location.ProjectFolder )]
	public class SVCSettings : ScriptableSingleton<SVCSettings>
	{
		[SerializeField] private bool stored_enabled = true;

		public bool enabled
		{
			get => stored_enabled;
			set
			{
				if( stored_enabled != value )
				{
					stored_enabled = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		[SerializeField] private float stored_rotateSpeedLeftRight = 2f; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write
		[SerializeField] private float stored_rotateSpeedUpDown = 1f; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write 

		public float rotateSpeedLeftRight
		{
			get => stored_rotateSpeedLeftRight;
			set
			{
				if( stored_rotateSpeedLeftRight != value )
				{
					stored_rotateSpeedLeftRight = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		public float rotateSpeedUpDown
		{
			get => stored_rotateSpeedUpDown;
			set
			{
				if( stored_rotateSpeedUpDown != value )
				{
					stored_rotateSpeedUpDown = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		[SerializeField] private bool stored_canEnableDisableByShortcut = false; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write

		public bool canEnableDisableByShortcut
		{
			get => stored_canEnableDisableByShortcut;
			set
			{
				if( stored_canEnableDisableByShortcut != value )
				{
					stored_canEnableDisableByShortcut = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		[SerializeField] private KeyCode stored_keyEnableDisable = KeyCode.Slash; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write
		[SerializeField] private KeyCode stored_keyRotateLeft = KeyCode.A; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write 
		[SerializeField] private KeyCode stored_keyRotateRight = KeyCode.D; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write
		[SerializeField] private KeyCode stored_keyRotateUp = KeyCode.W; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write
		[SerializeField] private KeyCode stored_keyRotateDown = KeyCode.S; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write

		public KeyCode keyEnableDisable
		{
			get => stored_keyEnableDisable;
			set
			{
				if( stored_keyEnableDisable != value )
				{
					stored_keyEnableDisable = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		public KeyCode keyRotateLeft
		{
			get => stored_keyRotateLeft;
			set
			{
				if( stored_keyRotateLeft != value )
				{
					stored_keyRotateLeft = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		public KeyCode keyRotateRight
		{
			get => stored_keyRotateRight;
			set
			{
				if( stored_keyRotateRight != value )
				{
					stored_keyRotateRight = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		public KeyCode keyRotateUp
		{
			get => stored_keyRotateUp;
			set
			{
				if( stored_keyRotateUp != value )
				{
					stored_keyRotateUp = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}

		public KeyCode keyRotateDown
		{
			get => stored_keyRotateDown;
			set
			{
				if( stored_keyRotateDown != value )
				{
					stored_keyRotateDown = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}
		
		[SerializeField] private KeyCode stored_keyStrafeUp = KeyCode.Q; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write
		[SerializeField] private KeyCode stored_keyStrafeDown = KeyCode.Z; // required by Unity's magic Singleton - it breaks if you don't add Save(..) on write
		
		public KeyCode keyStrafeUp
		{
			get => stored_keyStrafeUp;
			set
			{
				if( stored_keyStrafeUp != value )
				{
					stored_keyStrafeUp = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}
		public KeyCode keyStrafeDown
		{
			get => stored_keyStrafeDown;
			set
			{
				if( stored_keyStrafeDown != value )
				{
					stored_keyStrafeDown = value;
					Save( true ); // required by Unity's magic Singleton class
				}
			}
		}
	}
}