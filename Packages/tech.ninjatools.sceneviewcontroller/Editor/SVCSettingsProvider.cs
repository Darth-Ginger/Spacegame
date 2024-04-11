using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UIElements.Toggle;

namespace NinjaTools.NinjaSceneViewController.Editor
{
	public static class SVCSettingsProvider
	{
		[SettingsProvider]
		public static SettingsProvider Create()
		{
			var provider = new SettingsProvider( "Project/SceneViewController", SettingsScope.Project )
			{
				label = "Scene-View Controller",
				// activateHandler is called when the user clicks on the Settings item in the Settings window.
				activateHandler = ( searchContext, rootElement ) =>
				{
					lastPaintedRootElement = rootElement;
					_RefreshSettingsWindow();
				},
				keywords = new HashSet<string>( new[] { "SVC", "SceneView", "Controller" } )
			};

			provider.inspectorUpdateHandler = () =>
			{
				_RefreshSettingsWindow();
			};

			return provider;
		}

		private static VisualElement lastPaintedRootElement;

		private static void _RefreshSettingsWindow()
		{
			_RefreshSettingsWindow( lastPaintedRootElement );
		}

		private static int _AddTextBlockWithURLs( VisualElement localRoot, int uniqueID, string[] texts, string[] links, bool isPara = false )
		{
			var textBlock = localRoot._Local_ReuseOrAppendNew( "kText-"+uniqueID, () => new VisualElement() { style =
			{
				flexDirection = FlexDirection.Row,
				paddingBottom = isPara ? 10 : 0,
			} } );
			
			var colorHyperlinkPlain = EditorGUIUtility.isProSkin
				? new Color( 0.8f, 0.8f, 1f )
				: Color.white;
			var colorHyperlinkHover = EditorGUIUtility.isProSkin
				? new Color( 0.5f, 0.5f, 1f )
				: Color.blue;

			for( int i = 0; i < texts.Length; i++ )
			{
				var text = texts[i];
				var url = links[i];
				var isLink = url != null;

				if( isLink )
				{
					var lButton = textBlock._Local_ReuseOrAppendNew( "kbtn-" + i, () => new Label()
					{
						text = text,
						style =
						{
							color = colorHyperlinkPlain, borderBottomColor = colorHyperlinkPlain,
							borderBottomWidth = 1,
							flexGrow = 0,
						}
					}, newLabel =>
					{
						newLabel.RegisterCallback<MouseUpEvent>( evt =>
						{
							Debug.Log( "Item-" + i + "user clicked URL = " + url );
							Application.OpenURL( url );
						} );
						newLabel.RegisterCallback<MouseEnterEvent>( evt => newLabel.style.color = colorHyperlinkHover );
						newLabel.RegisterCallback<MouseLeaveEvent>( evt => newLabel.style.color = colorHyperlinkPlain );
					} );
				}
				else
				{
					textBlock._Local_ReuseOrAppendNew( "kText-"+i, () => new Label()
					{
						text = text
					} );
				}
			}
			
			return uniqueID + 1;
		}
		private static void _RefreshSettingsWindow( VisualElement rootElement )
		{
			var settings = SVCSettings.instance;

			var bgColor_TextSections = EditorGUIUtility.isProSkin
				? new Color( 0.15f, 0.15f, 0.15f, 0.5f )
				: new Color( 0.5f, 0.5f, 0.5f, 0.5f );

			var mainScrollview = rootElement._Local_ReuseOrAppendNew( "kOuterScroll", () => new ScrollView() );
			var mainContentsInsideScrollView = mainScrollview._Local_ReuseOrAppendNew( "kScrollContents", () => new VisualElement() );
			

			mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kTitleMain", () => new Label() { text = "Scene-View Controller", style =
			{
				fontSize = 20, unityFontStyleAndWeight = FontStyle.Bold,
				marginBottom = 10,
			} } );
			
			
			mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kTitle_About", () => new Label()
			{
				text = "About / Upgrades", style =
				{
					fontSize = 16, unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10,
				}
			} );
			var sectionAbout = mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kSectionAbout", () => new VisualElement() { style = { paddingLeft = 20, paddingRight = 20, paddingBottom = 10 } } );
			{
				int uid = 1;
				uid = _AddTextBlockWithURLs( sectionAbout, uid, new[]
				{
					"For more Unity tools (including advanced Editor plugins), see: ",
					"https://ninjatools.tech"
				}, new[]
				{
					null,
					"https://ninjatools.tech"
				}, true );
				
				uid = _AddTextBlockWithURLs( sectionAbout, uid, new[]
				{
					"More from this publisher:",
					"https://assetstore.unity.com/publishers/9057"
				}, new[]
				{
					null,
					"https://prf.hn/l/wzywqNn"
				}, true );
				
				uid = _AddTextBlockWithURLs( sectionAbout, uid, new[]
				{
					"Support:",
					"https://discord.gg/",
					"[NinjaTools]"
				}, new[]
				{
					null,
					"https://discord.gg/eXyw57YxCw",
					null
				} );
			}

			mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kTitle_Status", () => new Label()
			{
				text = "Status", style =
				{
					fontSize = 16, unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10,
				}
			} );
			var sectionEnabling = mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kSectionEnabling", () => new VisualElement() { style = { paddingLeft = 20, paddingRight = 20, paddingBottom = 10 } } );
			{
				{
					var rowEnabled = sectionEnabling._Local_ReuseOrAppendNew( "kEnabled", () => new VisualElement() { style = { flexDirection = FlexDirection.Row } } );
					rowEnabled._Local_ReuseOrAppendNew( "kName", () => new Label() { text = "Scene Control enabled?" } );
					var tEnabled = rowEnabled._Local_ReuseOrAppendNew( "kValue", () => new Toggle() { } );
					tEnabled.SetValueWithoutNotify( settings.enabled );
					tEnabled.RegisterValueChangedCallback( evt => { settings.enabled = evt.newValue; } );

					var lStatusMessage = sectionEnabling._Local_ReuseOrAppendNew( "kInfo", () => new Label()
					{
						style =
						{
							whiteSpace = WhiteSpace.Normal, fontSize = 14,
							marginBottom = 5, marginLeft = 10,
							backgroundColor = bgColor_TextSections,
						}
					} );
					if( SVCSettings.instance.enabled )
						lStatusMessage.text = "Scene-view control is ACTIVE";
					else
						lStatusMessage.text = "Scene-view control DISABLED:\nThis asset has no effect (enable it using the tickbox above, or by hitting the key: " + SVCSettings.instance.keyEnableDisable + " if you've enabled keyboard-based on/off)";
				}

				var metaRowKeyboardBasedEnablement = sectionEnabling._Local_ReuseOrAppendNew( "kSubSectionKeyEnabling", () => new VisualElement()
				{
					style =
					{
						flexDirection = FlexDirection.Row,
						marginTop = 10,
					}
				} );
				{
					var rowEnableKeyEnabling = metaRowKeyboardBasedEnablement._Local_ReuseOrAppendNew( "kEnableable", () => new VisualElement() { style = { flexDirection = FlexDirection.Row } } );
					rowEnableKeyEnabling._Local_ReuseOrAppendNew( "kName", () => new Label() { text = "Key can enable/disable?", style = { unityTextAlign = TextAnchor.MiddleLeft } } );
					var tEnabled = rowEnableKeyEnabling._Local_ReuseOrAppendNew( "kValue", () => new Toggle() { } );
					tEnabled.SetValueWithoutNotify( settings.canEnableDisableByShortcut );
					tEnabled.RegisterValueChangedCallback( evt => { settings.canEnableDisableByShortcut = evt.newValue; } );

					var rowEnablingKey = metaRowKeyboardBasedEnablement._Local_ReuseOrAppendNew( "kEnablingKey", () => new VisualElement() { style = { flexDirection = FlexDirection.Row } } );
					rowEnablingKey._Local_ReuseOrAppendNew( "kName", () => new Label() { text = "Key:", style = { unityTextAlign = TextAnchor.MiddleLeft } } );
					var keyField = rowEnablingKey._Local_ReuseOrAppendNew( "kV", () => new EnumField( SVCSettings.instance.keyEnableDisable ) { style = { minWidth = 30f } } );
					keyField.RegisterValueChangedCallback( evt => SVCSettings.instance.keyEnableDisable = (KeyCode)evt.newValue );
				}
			}

			mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kTitle_Rotating", () => new Label()
			{
				text = "Rotation", style =
				{
					fontSize = 16, unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10,
				}
			} );
			var sectionRotating = mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kSectionRotating", () => new VisualElement() { style = { paddingLeft = 20, paddingRight = 20 } } );
			{
				sectionRotating._Local_ReuseOrAppendNew( "kInstruction", () => new Label()
				{
					text = "Press and hold one of these keys while the Scene view is focussed to rotate the SceneView camera in that direction",
					style =
					{
						whiteSpace = WhiteSpace.Normal,
						marginBottom = 10,
						fontSize = 14,
					}
				} );

				var subSection_leftRight = sectionRotating._Local_ReuseOrAppendNew( "kSubSectionLR", () => new VisualElement()
				{
					style =
					{
						flexDirection = FlexDirection.Row,
					}
				} );

				var blockSpeedLR = subSection_leftRight._Local_ReuseOrAppendNew( "kSpeedLR", () => new VisualElement()
				{
					style =
					{
						flexDirection = FlexDirection.Row,
						marginLeft = 5, marginRight = 5,
						alignItems = Align.Center,
					}
				} );
				blockSpeedLR._Local_ReuseOrAppendNew( "kName", () => new Label() { text = "Speed left/right: ", style = { width = 100f } } );
				var speedLRField = blockSpeedLR._Local_ReuseOrAppendNew( "kSpeed", () => new FloatField() );
				speedLRField.SetValueWithoutNotify( settings.rotateSpeedLeftRight );
				speedLRField.RegisterValueChangedCallback( evt => settings.rotateSpeedLeftRight = evt.newValue );

				var blockKeysLR = subSection_leftRight._Local_ReuseOrAppendNew( "kKeysLR", () => new VisualElement()
				{
					style =
					{
						flexDirection = FlexDirection.Column,
						marginLeft = 5, marginRight = 5,
					}
				} );
				{
					var keyNames = new string[] { "left", "right" };
					var keyGetters = new KeyCode[] { settings.keyRotateLeft, settings.keyRotateRight };
					var keySetters = new Action<KeyCode>[] { code => settings.keyRotateLeft = code, code => settings.keyRotateRight = code };
					for( int i = 0; i < keyNames.Length; i++ )
					{
						var singleRow = blockKeysLR._Local_ReuseOrAppendNew( "kItem-" + keyNames[i], () => new VisualElement() { style = { flexDirection = FlexDirection.Row } } );

						singleRow._Local_ReuseOrAppendNew( "kName", () => new Label() { text = "Rotate-" + keyNames[i], style = { width = 85f } } );

						var keyField = singleRow._Local_ReuseOrAppendNew( "kV", () => new EnumField( keyGetters[i] ) { style = { minWidth = 30f } } );
						var i1 = i; // necessary to workaround C#'s horrible 'feature' that 'i' will get randomly corrupted at runtime otherwise
						keyField.RegisterValueChangedCallback( evt => keySetters[i1]( (KeyCode)evt.newValue ) );
					}
				}

				sectionRotating._Local_ReuseOrAppendNew( "kDivider-1", () => new VisualElement()
				{
					style =
					{
						borderTopColor = Color.black, borderTopWidth = 1f,
						marginTop = 4, marginBottom = 4,
					}
				} );

				var subSection_UpDown = sectionRotating._Local_ReuseOrAppendNew( "kSubSectionUD", () => new VisualElement()
				{
					style =
					{
						flexDirection = FlexDirection.Row,
					}
				} );

				var blockSpeedUD = subSection_UpDown._Local_ReuseOrAppendNew( "kSpeedUD", () => new VisualElement()
				{
					style =
					{
						flexDirection = FlexDirection.Row,
						marginLeft = 5, marginRight = 5,
						alignItems = Align.Center,
					}
				} );
				blockSpeedUD._Local_ReuseOrAppendNew( "kName", () => new Label() { text = "Speed up/down: ", style = { width = 100f } } );
				var speedUDField = blockSpeedUD._Local_ReuseOrAppendNew( "kSpeed", () => new FloatField() );
				speedUDField.SetValueWithoutNotify( settings.rotateSpeedUpDown );
				speedUDField.RegisterValueChangedCallback( evt => settings.rotateSpeedUpDown = evt.newValue );

				var blockKeysUD = subSection_UpDown._Local_ReuseOrAppendNew( "kKeysUD", () => new VisualElement()
				{
					style =
					{
						flexDirection = FlexDirection.Column,
						marginLeft = 5, marginRight = 5,
					}
				} );
				{
					var keyNames = new string[] { "up", "down" };
					var keyGetters = new KeyCode[] { settings.keyRotateUp, settings.keyRotateDown };
					var keySetters = new Action<KeyCode>[] { code => settings.keyRotateUp = code, code => settings.keyRotateDown = code };
					for( int i = 0; i < keyNames.Length; i++ )
					{
						var singleRow = blockKeysUD._Local_ReuseOrAppendNew( "kItem-" + keyNames[i], () => new VisualElement() { style = { flexDirection = FlexDirection.Row } } );

						singleRow._Local_ReuseOrAppendNew( "kName", () => new Label() { text = "Rotate-" + keyNames[i], style = { width = 85f } } );

						var keyField = singleRow._Local_ReuseOrAppendNew( "kV", () => new EnumField( keyGetters[i] ) { style = { minWidth = 30f } } );
						var i1 = i; // necessary to workaround C#'s horrible 'feature' that 'i' will get randomly corrupted at runtime otherwise
						keyField.RegisterValueChangedCallback( evt => keySetters[i1]( (KeyCode)evt.newValue ) );
					}
				}
			}

			mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kTitle_Warnings", () => new Label()
			{
				text = "Warnings", style =
				{
					fontSize = 16, unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10,
				}
			} );

			var sectionWarnings = mainContentsInsideScrollView._Local_ReuseOrAppendNew( "kSectionWarnings", () => new VisualElement() { style = { paddingLeft = 20, paddingRight = 20 } } );
			{
				sectionWarnings._Local_ReuseOrAppendNew( "kControls-1", () => new Label()
				{
					style = { whiteSpace = WhiteSpace.Normal, backgroundColor = bgColor_TextSections, marginBottom = 5 },
					text = "After releasing a rotation key, wait 0.1 seconds for Unity to finish the 'animation' of scene-camera, or you'll get briefly incorrect orientation" +
					       "(Unity's internal animation routine gets confused for a few frames if you don't give it a chance to finish)"
				} );
				sectionWarnings._Local_ReuseOrAppendNew( "kControls-2", () => new Label()
				{
					style = { whiteSpace = WhiteSpace.Normal, backgroundColor = bgColor_TextSections, marginBottom = 5 },
					text = "Rotation keys can be held down, but not at same time as using Unity's own cursor-key movement, this is due to how the Unity Editor handles" +
					       "key input while in SceneView (may be changed in a future release, currently all attempts caused other problems with Unity Editor)"
				} );
			}
		}

		/// <summary>
		/// Cloned from the Unity AssetStorePublishers' Toolbox: more available free here: https://github.com/adamgit/PublishersFork/blob/main/README.md
		/// </summary>
		/// <param name="localRoot"></param>
		/// <param name="kName"></param>
		/// <param name="initialSetup"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		private static T _Local_ReuseOrAppendNew<T>( this VisualElement localRoot, string kName, Func<T> initialSetup, Action<T> postConstructionInitializer = null ) where T : VisualElement
		{
			var block = localRoot.Q<T>( kName );
			if( block == null )
			{
				block = initialSetup();
				block.name = kName;
				if( postConstructionInitializer != null ) postConstructionInitializer( block );
				localRoot.Add( block );
			}
			else if( block.style.display == DisplayStyle.None )
				block.style.display = DisplayStyle.Flex;

			return block;
		}
	}
}