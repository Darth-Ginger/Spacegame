using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NinjaTools.NinjaSceneViewController.Editor
{
	[InitializeOnLoad]
	public class SceneViewController
	{
		private static bool rotateLeft = false, rotateRight = false, rotateUp = false, rotateDown = false;
		private static bool strafeUp = false, strafeDown = false;

		private static bool _WentDown( KeyCode kc )
		{
			return (Event.current is Event e
			        && e.keyCode == kc
			        && e.type == EventType.KeyDown);
		}

		private static bool _WentUp( KeyCode kc )
		{
			return (Event.current is Event e
			        && e.keyCode == kc
			        && e.type == EventType.KeyUp);
		}

		private static void _ToggleBoolUsingKeyState( KeyCode kc, Action<bool> changeBool )
		{
			if( Event.current is Event e && e.keyCode == kc )
			{
				if( e.type == EventType.KeyDown )
					changeBool( true );
				else if( e.type == EventType.KeyUp )
					changeBool( false );
			}
		}

		static SceneViewController()
		{
			EditorApplication.update += InEditorUpdate;

			SceneView.duringSceneGui += view =>
			{
				if( SVCSettings.instance.canEnableDisableByShortcut && _WentDown( SVCSettings.instance.keyEnableDisable ) )
				{
					Debug.Log( (SVCSettings.instance.enabled ? "Dis" : "En") + "abling SceneView keyboard-Controller" );
					SVCSettings.instance.enabled = !SVCSettings.instance.enabled;
				}

				if( SVCSettings.instance.enabled )
				{
					if( Event.current != null )
					{
						_ToggleBoolUsingKeyState( SVCSettings.instance.keyRotateLeft, b => rotateLeft = b );
						_ToggleBoolUsingKeyState( SVCSettings.instance.keyRotateRight, b => rotateRight = b );
						_ToggleBoolUsingKeyState( SVCSettings.instance.keyRotateUp, b => rotateUp = b );
						_ToggleBoolUsingKeyState( SVCSettings.instance.keyRotateDown, b => rotateDown = b );
						
						_ToggleBoolUsingKeyState( SVCSettings.instance.keyStrafeUp, b => strafeUp = b );
						_ToggleBoolUsingKeyState( SVCSettings.instance.keyStrafeDown, b => strafeDown = b );
					}
				}
			};
		}

		private static bool changeInProgress => rotateLeft || rotateRight || rotateUp || rotateDown
		                                        || strafeUp || strafeDown;

		private static Vector3 actualPos;
		private static Quaternion actualRot;

		private static Vector3 unityOriginalForwards;

		//private static Quaternion growingRot;
		private static Quaternion growingRotY, growingRotX;

		public static void InEditorUpdate()
		{
			var v = SceneView.lastActiveSceneView;
			var s = SVCSettings.instance;

			if( !changeInProgress )
			{
				var unityOriginalPivot = v.pivot;
				actualPos = v.camera.transform.position;
				unityOriginalForwards = unityOriginalPivot - actualPos;
				actualRot = Quaternion.LookRotation( unityOriginalForwards, Vector3.up );
				growingRotY = growingRotX = Quaternion.identity;
			}

			//Debug.Log( "Tick editord" );
			if( rotateLeft )
			{
				var extraRot = Quaternion.AngleAxis( s.rotateSpeedLeftRight * Time.deltaTime * -1, Vector3.up );
				growingRotY *= extraRot;
			}

			if( rotateRight )
			{
				var extraRot = Quaternion.AngleAxis( s.rotateSpeedLeftRight * Time.deltaTime, Vector3.up );
				growingRotY *= extraRot;
			}

			if( rotateUp )
			{
				var localRight = growingRotY * Vector3.right;
				var extraRot = Quaternion.AngleAxis( s.rotateSpeedUpDown * Time.deltaTime * -1f, localRight );
				growingRotX *= extraRot;
			}

			if( rotateDown )
			{
				var localRight = growingRotY * Vector3.right;
				var extraRot = Quaternion.AngleAxis( s.rotateSpeedUpDown * Time.deltaTime, localRight );
				growingRotX *= extraRot;
			}

			if( strafeUp )
			{
				actualPos += Time.deltaTime * Vector3.up;
			}

			if( strafeDown )
			{
				actualPos += Time.deltaTime * Vector3.down;
			}

			if( changeInProgress )
			{
				var newPivot = actualPos + (growingRotY * growingRotX) * unityOriginalForwards;

				v.pivot = newPivot;
				v.rotation = growingRotY * actualRot * growingRotX;
			}
		}
	}
}