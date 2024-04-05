using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SBG.SpeedScript
{
	public class ImportSetupWindow : EditorWindow
	{
		[InitializeOnLoadMethod]
		public static void OnInitialize()
		{
			var config = ScriptBuilder.LoadConfig();
			if (config.DisplayedInstallDialog) return;

			ShowWindow();
		}

		public static void ShowWindow()
		{
			var window = ScriptableObject.CreateInstance(typeof(ImportSetupWindow)) as ImportSetupWindow;
			window.titleContent = new GUIContent("Setup SpeedScript");
			window.minSize = new Vector2(450, 160);
			window.maxSize = new Vector2(450, 160);
			window.ShowUtility();
		}

		private void OnEnable()
		{
			AssemblyReloadEvents.beforeAssemblyReload += BeforeReload;
		}

		private void OnDisable()
		{
			AssemblyReloadEvents.beforeAssemblyReload -= BeforeReload;
		}

		private void BeforeReload()
		{
			Close();
		}

		private void OnGUI()
		{
			GUIStyle labelStyle = new GUIStyle(EditorStyles.helpBox);
			labelStyle.fontSize = 14;

			EditorGUILayout.Separator();

			EditorGUILayout.LabelField("It seems you just imported SpeedScript! Do you want to configure your settings now?", labelStyle);

			EditorGUILayout.Separator();

			if (GUILayout.Button("1. Setup Folders", GUILayout.Height(30)))
			{
				FolderStructureEditorWindow.ShowWindow();
			}

			if (GUILayout.Button("2. Setup Templates", GUILayout.Height(30)))
			{
				TemplateSettingsEditorWindow.ShowWindow();
			}

			if (GUILayout.Button("3. Finish Setup (Close)", GUILayout.Height(30)))
			{
				var config = ScriptBuilder.LoadConfig();
				config.DisplayedInstallDialog = true;
				ScriptBuilder.SaveConfig(config);
				Close();
			}
		}
	}
}