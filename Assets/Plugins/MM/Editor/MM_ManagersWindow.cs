using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Scene = UnityEngine.SceneManagement.Scene;
using static UnityEngine.GraphicsBuffer;

namespace MM
{
    public class MM_ManagersWindow : EditorWindow
    {
        private TreeCollection<MM_SceneManagerPair> managers;

        private VisualElement LeftPane;
        private VisualElement RightPane;
        private Dictionary<UnityEngine.Object, Editor> editors;

        private void OnEnable()
        {
            editors = new();
            FetchManagers();
        }

        private void FetchManagers()
        {
            managers = new();
            foreach (var man in managerContainer.Managers)
            {
                if (Type.GetType(man.Type).GetCustomAttribute<Manager>() != null)
                {
                    managers.Add(man, "Scenes/" + man.Scene.name);
                    if (!string.IsNullOrEmpty(man.Path))
                    {
                        managers.Add(man, man.Path);
                    }
                }
            }
        }

        private void OnDisable()
        {
            //Unity does not dispose unused editors if they are created manually which can lead to memory leak.
            //Debugger off because it will always log an error while recompiling, as editor targets are already disposed
            Debug.unityLogger.logEnabled = false;
            foreach (var e in editors.Keys)
                if (e != null) DestroyImmediate(editors[e]);
            editors.Clear();
            Debug.unityLogger.logEnabled = true;
        }

        private void CreateGUI()
        {
            var root = rootVisualElement;

            var splitView = new TwoPaneSplitView(0, 350, TwoPaneSplitViewOrientation.Horizontal);
            splitView.StretchToParentSize();
            root.Add(splitView);

            LeftPane = new VisualElement() { name = "LeftPane" };
            FillLeftPane("");

            splitView.Add(LeftPane);

            RightPane = new VisualElement() { name = "RightPane" };
            FillRightPane(null);
            splitView.Add(RightPane);

            string scriptPath = AssetDatabase.FindAssets("t:Script MM_ManagersWindow")[0];
            string scriptFolder = System.IO.Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(scriptPath));
            string ussFilePath = System.IO.Path.Combine(scriptFolder, "MM_WindowStyles.uss");
            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussFilePath);
            root.styleSheets.Add(style);
        }

        private void FillLeftPane(string path, MM_SceneManagerPair inspectedPrefab = null)
        {
            LeftPane.Clear();
            var navigationBar = new VisualElement() { name = "NavigationBar" };
            LeftPane.Add(navigationBar);

            var rootNav = new NavigationBarElement(() => FillLeftPane("")) { text = "root" };
            rootNav.AddToClassList("first");
            navigationBar.Add(rootNav);

            string[] folders = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string navPath = "";
            for (int i = 0; i < folders.Length; i++)
            {
                navPath += "/" + folders[i];
                string pathCopy = navPath;
                var nav = new NavigationBarElement(() => FillLeftPane(pathCopy)) { text = folders[i] };
                navigationBar.Add(nav);
                nav.SendToBack();
            }
            if (inspectedPrefab != null)
            {
                var nav = new NavigationBarElement() { text = String.IsNullOrEmpty(inspectedPrefab.DisplayName) ? Type.GetType(inspectedPrefab.Type).Name : inspectedPrefab.DisplayName };
                navigationBar.Add(nav);
                nav.SendToBack();

                var scroll = new ScrollView(ScrollViewMode.Vertical);
                LeftPane.Add(scroll);
                DrawHierarchy(inspectedPrefab.Prefab, scroll, 0);
            }
            else
            {
                List<System.Object> list = new List<System.Object>();
                list.AddRange(managers.GetSubPaths(path));
                list.AddRange(managers.Get(path));

                ListView listView = new ListView(list, 30, () => new VisualElement(), (elem, index) =>
                {
                    elem.Clear();
                    elem.RemoveFromClassList("folder-element");
                    elem.RemoveFromClassList("prefab-element");
                    if (list[index] is MM_SceneManagerPair)
                    {
                        var go = list[index] as MM_SceneManagerPair;
                        elem.AddToClassList("prefab-element");
                        elem.Add(new Image() { image = EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "Prefab Icon") as Texture });
                        elem.Add(new Label(String.IsNullOrEmpty(go.DisplayName) ? Type.GetType(go.Type).Name : go.DisplayName));
                        if (go.Prefab.transform.childCount > 0)
                            elem.Add(new Image() { image = EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "tab_next") as Texture, name = "tab-next" });
                        elem.AddManipulator(CreateContextualMenu(go, path));
                    }
                    else
                    {
                        elem.AddToClassList("folder-elemnt");
                        elem.Add(new Image() { image = EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "Folder Icon") as Texture });
                        elem.Add(new Label(list[index] as string));
                    }
                })
                {
                    reorderable = false,
                    showFoldoutHeader = false,
                    showAddRemoveFooter = false,
                    showBorder = false,
                    showBoundCollectionSize = false
                };
                listView.selectionChanged += (x) =>
                {
                    if (x.First() is string)
                        FillLeftPane(path + "/" + (x.First() as string));
                    else
                    {
                        FillRightPane((x.First() as MM_SceneManagerPair).Prefab);
                        if ((x.First() as MM_SceneManagerPair).Prefab.transform.childCount > 0)
                            FillLeftPane(path, x.First() as MM_SceneManagerPair);
                    }
                };

                LeftPane.Add(listView);
            }
            var refresh = new Button(() =>
            {
                FetchManagers();
                FillLeftPane("");
            })
            { name = "RefreshhButton", tooltip = "Refresh if managers window hasn't fetched newly created instances" };
            LeftPane.Add(refresh);
        }
        private void FillRightPane(GameObject go)
        {
            RightPane.Clear();
            foreach (var e in editors.Values)
                DestroyImmediate(e);
            editors.Clear();
            if (go == null)
            {
                RightPane.Add(new Label("No manager selected"));
                return;
            }

            editors.Add(go, Editor.CreateEditor(go));
            Dictionary<Editor, bool> foldouts = new();
            var scroll = new ScrollView(ScrollViewMode.Vertical);
            RightPane.Add(scroll);
            scroll.Add(new IMGUIContainer(() =>
            {
                EditorGUI.BeginChangeCheck();
                editors[go].DrawHeader();
                foreach (var c in go.GetComponents<Component>())
                {
                    if ((c.hideFlags & HideFlags.HideInInspector) != 0) continue;
                    if (!editors.TryGetValue(c, out var edi))
                    {
                        edi = Editor.CreateEditor(c);
                        editors.Add(c, edi);
                        foldouts.Add(edi, true);
                    }
                    foldouts[edi] = EditorGUILayout.InspectorTitlebar(foldouts[edi], c);
                    if (foldouts[edi])
                        editors[c].OnInspectorGUI();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(go);
                }
            }));
            scroll.Add(new Button(() =>
            {
                var rootOfPrefab = go.transform.root.gameObject;
                Debug.Log(rootOfPrefab);
                EditorGUIUtility.PingObject(rootOfPrefab);
                Selection.activeObject = rootOfPrefab;
            })
            { name = "NavigatePrefabButton", text = "Navigate to prefab" });
        }

        private void DrawHierarchy(GameObject gameObject, VisualElement parentElement, float indent)
        {
            CustomFoldout foldout = new CustomFoldout(true, indent);
            if (parentElement is CustomFoldout)
                (parentElement as CustomFoldout).Add(foldout);
            else
                parentElement.Add(foldout);

            var elementImage = new Image() { name = "prefab-image" };
            var elementTexture = EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "Prefab Icon");
            elementImage.image = elementTexture as Texture;

            foldout.titleContainer.Add(elementImage);
            foldout.titleContainer.Add(new Label(gameObject.name));
            if (gameObject.transform.childCount == 0) foldout.titleContainer.Query<Image>(name: "arrow").First().visible = false;

            foldout.titleContainer.RegisterCallback<PointerDownEvent>(e =>
            {
                FillRightPane(gameObject);
            });

            foreach (Transform child in gameObject.transform)
            {
                DrawHierarchy(child.gameObject, foldout, indent + 15);
            }
        }
        private IManipulator CreateContextualMenu(MM_SceneManagerPair manager, string currentPath)
        {
            ContextualMenuManipulator menu = new(
                    menuEvent =>
                    {
                        menuEvent.menu.AppendAction(
                        "Change instance path", actionEvent =>
                        {
                            var window = new TextFieldPopup(s =>
                            {
                                manager.Path = s;
                                managerContainer.SaveData();
                                FetchManagers();
                                FillLeftPane(s);
                            }, "Path", manager.Path);
                            UnityEditor.PopupWindow.Show(new Rect(actionEvent.eventInfo.mousePosition, Vector2.zero), window);
                            window.editorWindow.rootVisualElement.Q<TextField>().Q<VisualElement>(name: "unity-text-input").Focus();
                        });
                        menuEvent.menu.AppendAction(
                        "Change instance display name", actionEvent =>
                        {
                            var window = new TextFieldPopup(s =>
                            {
                                manager.DisplayName = s;
                                managerContainer.SaveData();
                                FetchManagers();
                                FillLeftPane(currentPath);
                            }, "Name", manager.DisplayName);
                            UnityEditor.PopupWindow.Show(new Rect(actionEvent.eventInfo.mousePosition, Vector2.zero), window);
                            window.editorWindow.rootVisualElement.Q<TextField>().Q<VisualElement>(name: "unity-text-input").Focus();
                        });
                    }
                    );
            return menu;
        }

        [MenuItem("Window/Managers")]
        public static void ShowWindow()
        {
            GetWindow<MM_ManagersWindow>("Managers", true, typeof(SceneView));
        }


        #region Initialization


        private static MM_ManagersContainer managerContainer;
        private static Dictionary<MM_SceneManagerPair, GameObject> managersToTrack;



        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            managerContainer ??= MM_ManagersContainer.instance;

            ObjectFactory.componentWasAdded += OnComponentAdded;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorSceneManager.sceneClosing += OnSceneClosed;
            EditorApplication.update += TrackPrefabs;
        }
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            managerContainer ??= MM_ManagersContainer.instance;
            EditorApplication.delayCall += EvaluateAllScenes;
        }


        private static void TrackPrefabs()
        {
            if (managersToTrack == null) return;
            for (int i = managersToTrack.Count - 1; i >= 0; i--)
            {
                var prefab = managersToTrack.ElementAt(i).Value;
                var man = managersToTrack.ElementAt(i).Key;
                if (prefab == null) //Game object was deleted
                {
                    var actualPrefab = man.Prefab;
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(actualPrefab));
                    managersToTrack.Remove(man);
                    managerContainer.Managers.Remove(man);
                    managerContainer.SaveData();
                    AssetDatabase.Refresh();
                    continue;
                }
                if (PrefabUtility.HasPrefabInstanceAnyOverrides(prefab, false)) //Game object has overrides
                {
                    if (prefab.GetComponent(Type.GetType(man.Type)) == null) //Manager component was deleted
                    {
                        PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                        var actualPrefab = man.Prefab;
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(actualPrefab));
                        managersToTrack.Remove(man);
                        managerContainer.Managers.Remove(man);
                        managerContainer.SaveData();
                        AssetDatabase.Refresh();
                        continue;
                    }
                    else //Overrides need to applied
                    {
                        PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.AutomatedAction);
                    }
                }
            }
        }


        private static void OnComponentAdded(Component obj)
        {
            if (obj.GetType().GetCustomAttribute<Manager>() != null) //Added component is a manager
            {
                managerContainer ??= MM_ManagersContainer.instance;
                if (obj.GetComponentsInChildren<Component>().Where(c => c.GetType().GetCustomAttribute<Manager>() != null).Count() > 1
                    || obj.GetComponentsInParent<Component>().Where(c => c.GetType().GetCustomAttribute<Manager>() != null).Count() > 1)
                {
                    Debug.LogError($"Game object {obj.gameObject.name}, or one of its children or parent already contains one manager");
                    DestroyImmediate(obj);
                }
                else
                {
                    CreateManagerPrefab(obj);
                }
                managerContainer.SaveData();
            }
        }


        private static void CreateManagerPrefab(Component obj)
        {
            if (!AssetDatabase.IsValidFolder("Assets/MM"))
                AssetDatabase.CreateFolder("Assets", "MM");
            if (!AssetDatabase.IsValidFolder("Assets/MM/Managers"))
                AssetDatabase.CreateFolder("Assets/MM", "Managers");
            GameObject prefab;
            if (AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/MM/Managers/{obj.GetType().Name}.prefab") != null)
            {
                int count = 1;
                while (AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/MM/Managers/{obj.GetType().Name}{count}.prefab") != null)
                    count++;
                prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(obj.gameObject, $"Assets/MM/Managers/{obj.GetType().Name}{count}.prefab", InteractionMode.AutomatedAction);
            }
            else
            {
                prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(obj.gameObject, $"Assets/MM/Managers/{obj.GetType().Name}.prefab", InteractionMode.AutomatedAction);
            }

            MM_SceneManagerPair pair = new()
            {
                Prefab = prefab,
                Scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(obj.gameObject.scene.path),
                DisplayName = obj.GetType().GetCustomAttribute<Manager>().DisplayName,
                Path = obj.GetType().GetCustomAttribute<Manager>().Path,
                Type = obj.GetType().AssemblyQualifiedName
            };
            managerContainer.Managers.Add(pair);
            managersToTrack.Add(pair, obj.gameObject);
            EditorSceneManager.MarkSceneDirty(obj.gameObject.scene);
        }


        private static void EvaluateAllScenes()
        {
            int sceneCount = EditorSceneManager.sceneCount;
            managersToTrack = new();
            for (int i = 0; i < sceneCount; i++)
            {
                EvaluateScene(EditorSceneManager.GetSceneAt(i));
            }
        }


        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            managerContainer ??= MM_ManagersContainer.instance;
            EvaluateScene(scene, mode == OpenSceneMode.Single);
        }


        private static void OnSceneClosed(Scene scene, bool removingScene)
        {
            if (managersToTrack == null) return;
            var trackersToRemove = managersToTrack.Keys.Where(k => managersToTrack[k].scene == scene).ToArray();
            foreach (var man in trackersToRemove)
                managersToTrack.Remove(man);
        }



        private static void EvaluateScene(Scene scene, bool clearTracking = false)
        {
            if (clearTracking) managersToTrack = new();

            var managerTypes = TypeCache.GetTypesWithAttribute<Manager>();
            var componentsOfManagerType = scene.GetRootGameObjects().SelectMany(x => managerTypes.SelectMany(y => x.GetComponentsInChildren(y)));

            for (int i = managerContainer.Managers.Count - 1; i >= 0; i--) //Remove managers of that scene that reffer to a non-manager type, and unpack its instance
            {
                if (AssetDatabase.GetAssetPath(managerContainer.Managers[i].Scene) == scene.path && !managerTypes.Any(y => y == Type.GetType(managerContainer.Managers[i].Type)))
                {
                    Debug.LogWarning($"Manager attribute has been removed from type '{managerContainer.Managers[i].Type}'. Prefab of that type is being unpacked and removed, " +
                    $"make sure to load other scenes containing objects of types from which manager attribute was removed, in order for them to be properly unpacked");
                    PrefabUtility.UnpackAllInstancesOfPrefab(managerContainer.Managers[i].Prefab, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(managerContainer.Managers[i].Prefab));
                    managerContainer.Managers.RemoveAt(i);
                    EditorSceneManager.MarkSceneDirty(scene);
                }
            }

            var managersOfThatScene = managerContainer.Managers.Where(x => AssetDatabase.GetAssetPath(x.Scene) == scene.path);

            foreach (var obj in componentsOfManagerType)
            {
                if (obj.GetComponents(obj.GetType()).Length > 1)//Multiple instances of that type on an object
                {
                    Debug.LogError($"Game object {obj.gameObject.name} already contains manager of type {obj.GetType().Name}");
                    DestroyImmediate(obj);
                    continue;
                }
                var correspondingPrefab = managersOfThatScene.FirstOrDefault(m => m.Prefab == PrefabUtility.GetCorrespondingObjectFromSource(obj.gameObject));
                if (correspondingPrefab != null) //This game object is a manager prefab
                {
                    if (correspondingPrefab.Type != obj.GetType().AssemblyQualifiedName) //...but not of this type
                    {
                        Debug.LogError($"Game object {obj.gameObject.name} already contains one manager");
                        DestroyImmediate(obj);
                        continue;
                    }
                    managersToTrack.Add(correspondingPrefab, obj.gameObject);
                }
                else //This game object is not a manager prefab
                {
                    if (componentsOfManagerType.Where(x => x != null && x != obj).Any(x => x.transform.IsChildOf(obj.transform) || obj.transform.IsChildOf(x.transform)))
                    //...but one of its children or parents is
                    {
                        Debug.LogError($"Game object {obj.gameObject.name}, or one of its children or parent already contains one manager");
                        DestroyImmediate(obj);
                        continue;
                    }
                    else //Not a prefab, is singular, no parent or children with managers
                    {
                        CreateManagerPrefab(obj);
                    }
                }
            }

            managerContainer.SaveData();
            AssetDatabase.Refresh();
        }
        #endregion
    }

    internal class NavigationBarElement : VisualElement
    {
        private string _text;
        public string text { get => _text; set { _text = value; this.Q<Label>().text = value; } }
        public NavigationBarElement()
        {
            generateVisualContent += GenerateContext;
            AddToClassList("nav-element");
            focusable = true;
            Add(new Label(text));
        }
        public NavigationBarElement(Action clickEvet)
        {
            generateVisualContent += GenerateContext;
            AddToClassList("nav-element");
            focusable = true;
            Add(new Label(text));
            RegisterCallback<PointerDownEvent>(e => clickEvet.Invoke());
        }

        private void GenerateContext(MeshGenerationContext ctx)
        {
            var painter = ctx.painter2D;
            painter.lineWidth = 1;
            painter.strokeColor = resolvedStyle.borderTopColor;
            painter.fillColor = resolvedStyle.backgroundColor;
            painter.lineJoin = LineJoin.Round;
            painter.BeginPath();
            painter.MoveTo(contentRect.position);
            painter.LineTo(contentRect.position + new Vector2(contentRect.width, 0));
            painter.LineTo(contentRect.position + new Vector2(contentRect.width + 10, contentRect.height / 2));
            painter.LineTo(contentRect.position + new Vector2(contentRect.width, contentRect.height));
            painter.LineTo(contentRect.position + new Vector2(0, contentRect.height));
            painter.ClosePath();
            painter.Fill();
            painter.BeginPath();
            painter.MoveTo(contentRect.position + new Vector2(contentRect.width, 0));
            painter.LineTo(contentRect.position + new Vector2(contentRect.width + 10, contentRect.height / 2));
            painter.LineTo(contentRect.position + new Vector2(contentRect.width, contentRect.height));
            painter.Stroke();
        }
    }

    internal class TextFieldPopup : PopupWindowContent
    {
        private TextField textField;
        private Action<string> action;
        private string labelName;
        private string initialValue;
        public TextFieldPopup(Action<string> action, string labelName, string initialValue)
        {
            this.action = action;
            this.labelName = labelName;
            this.initialValue = initialValue;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 50);
        }

        public override void OnGUI(Rect rect)
        {

        }

        public override void OnOpen()
        {
            textField = new TextField() { label = labelName };
            textField.value = initialValue;
            var button = new Button(() => { action.Invoke(textField.value); editorWindow.Close(); }) { text = "OK" };
            var root = editorWindow.rootVisualElement;
            root.Add(textField);
            root.Add(button);
            root.name = "Popup";
            string scriptPath = AssetDatabase.FindAssets("t:Script MM_ManagersWindow")[0];
            string scriptFolder = System.IO.Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(scriptPath));
            string ussFilePath = System.IO.Path.Combine(scriptFolder, "MM_WindowStyles.uss");
            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussFilePath);
            root.styleSheets.Add(style);

            textField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return)
                {
                    action.Invoke(textField.value);
                    evt.StopPropagation();
                    evt.PreventDefault();
                    editorWindow.Close();
                }
            });
        }
    }

    public class MM_AssetsModification : AssetModificationProcessor
    {
        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
        {
            if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(SceneAsset))
            {
                var man = MM_ManagersContainer.instance;
                for (int i = man.Managers.Count - 1; i >= 0; i--)
                {
                    if (AssetDatabase.GetAssetPath(man.Managers[i].Scene) == path)
                    {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(man.Managers[i].Prefab));
                    }
                }
                man.SaveData();
            }
            else if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(GameObject))
            {
                var man = MM_ManagersContainer.instance;
                for (int i = man.Managers.Count - 1; i >= 0; i--)
                {
                    if (AssetDatabase.GetAssetPath(man.Managers[i].Prefab) == path)
                    {
                        PrefabUtility.UnpackAllInstancesOfPrefab(man.Managers[i].Prefab, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                        man.Managers.RemoveAt(i);
                    }
                }
                man.SaveData();
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }

    internal class CustomFoldout : VisualElement
    {
        private Texture expandedTex;
        private Texture collapsedTex;
        private Texture expandedFocusTex;
        private Texture collapsedFocusTex;

        private bool _value = true;
        public bool value { get => _value; set { _value = value; ToggleFoldout(); } }

        public VisualElement titleContainer { get; private set; }

        private Image foldArrow;
        public VisualElement content { get; private set; }

        public CustomFoldout(bool value, float indent)
        {
            _value = value;
            expandedTex = EditorGUIUtility.Load("IN foldout on") as Texture;
            collapsedTex = EditorGUIUtility.Load("IN foldout") as Texture;
            expandedFocusTex = EditorGUIUtility.Load("IN foldout focus on") as Texture;
            collapsedFocusTex = EditorGUIUtility.Load("IN foldout focus") as Texture;

            content = new VisualElement() { name = "content-container" };
            titleContainer = new VisualElement() { name = "title-container", focusable = true };
            titleContainer.style.flexDirection = FlexDirection.Row;
            titleContainer.style.paddingLeft = indent;
            foldArrow = new Image() { image = _value ? expandedTex : collapsedTex, name = "arrow" };
            foldArrow.style.width = 13;
            foldArrow.RegisterCallback<PointerDownEvent>(e => { foldArrow.image = _value ? expandedFocusTex : collapsedFocusTex; e.StopImmediatePropagation(); e.PreventDefault(); });
            foldArrow.RegisterCallback<PointerUpEvent>(e => { this.value = !this.value; });
            titleContainer.Add(foldArrow);

            Insert(0, titleContainer);
            base.Add(content);

            content.style.display = _value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public new void Add(VisualElement element)
        {
            content.Add(element);
        }

        private void ToggleFoldout()
        {
            content.style.display = _value ? DisplayStyle.Flex : DisplayStyle.None;
            foldArrow.image = _value ? expandedTex : collapsedTex;
        }
    }
}