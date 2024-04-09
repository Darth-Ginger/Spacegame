using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;

public class ChildClassCreator : EditorWindow
{
    private string childClassName = "NewTileType";
    private string baseClassName = "Tile";

    [MenuItem("Tools/Child Class Creator")]
    public static void ShowWindow()
    {
        GetWindow<ChildClassCreator>("Child Class Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        baseClassName = EditorGUILayout.TextField("Base Class Name", baseClassName);
        childClassName = EditorGUILayout.TextField("Child Class Name", childClassName);

        if (GUILayout.Button("Create Child Class"))
        {
            CreateChildClassFile(baseClassName, childClassName);
        }
    }

    private void CreateChildClassFile(string baseClass, string childClass)
    {
        string filePath = Path.Combine(Application.dataPath, "Scripts", $"{childClass}.cs");

        // Ensure the directory exists
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        StringBuilder classContent = new StringBuilder();

        classContent.AppendLine("using UnityEngine;");
        classContent.AppendLine("");
        classContent.AppendLine($"public class {childClass} : {baseClass}");
        classContent.AppendLine("{");

        // Include abstract methods with empty bodies
        System.Type type = System.Type.GetType($"{baseClass}, Assembly-CSharp");
        if (type != null)
        {
            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.IsAbstract)
                .ToArray();

            foreach (var methodInfo in methodInfos)
            {
                classContent.AppendLine($"    public override {methodInfo.ReturnType.Name} {methodInfo.Name}()");
                classContent.AppendLine("    {");
                classContent.AppendLine("        // Implement abstract method");
                classContent.AppendLine("    }");
                classContent.AppendLine();
            }
        }

        classContent.AppendLine("}");

        File.WriteAllText(filePath, classContent.ToString());

        AssetDatabase.Refresh();
    }
}
