using UnityEngine;
//using System.Collections;
using UnityEditor;
using System.IO;

public class MakeProjectFolder : MonoBehaviour 
{
    // Generate Folders for a New Project

    // menu Item will read the first Static Function
    [MenuItem("Project Tools / Make Folders")]

    static void MakeFolders()
    {
        GenerateFolders();
    }

    static void GenerateFolders()
    {
        string projectPath = Application.dataPath + "/";            // Store Path for the folders so only the Name is needed during creation lines

        // creating folders
        Directory.CreateDirectory(projectPath + "Audio");
        Directory.CreateDirectory(projectPath + "Materials");
        Directory.CreateDirectory(projectPath + "Meshes");
        Directory.CreateDirectory(projectPath + "Fonts");
        Directory.CreateDirectory(projectPath + "Textures");
        Directory.CreateDirectory(projectPath + "Resources");
        Directory.CreateDirectory(projectPath + "Scripts");
        Directory.CreateDirectory(projectPath + "Shaders");
        Directory.CreateDirectory(projectPath + "Packages");
        Directory.CreateDirectory(projectPath + "Physics");

        AssetDatabase.Refresh();
    }
}
