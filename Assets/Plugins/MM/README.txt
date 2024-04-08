Managers manager is a simple tool to help you orginise your so called "managers". Wiht usage of prefabs you can easily inspect and modify your manager instances from wherever you are in a project.

Usage:
-Attach Manager attribute to your MonoBehaviour class, this will automatically create a prefab out of a game object with that behaviour.
-Open Managers window in Window->Managers, where you can inspect all your managers sorted in a folder-like structure.
-By providing "Path" and "DisplayName" in attributes constructor, you can manipulate where and how newly created instances of that class are shown in a window. 
-By right-clicking an instance in a window, you can change name or path of that specific instance.

Notes:
-To keep hierarchy clean, ONLY ONE manager is allowed per game object (including its children).
-Passing Path and Display name to attribute constructor is not obligatory, natively instances will use class name and will be sorted by associated scene asset.
-Overrides are applied to a prefab automatically, thus it is not recommended to instantiate multiple instances of manager prefab, unless you want each next instance to be bound to the first one.
-You can't add components from a Managers window, to do so you need to do it on a prefab itself.