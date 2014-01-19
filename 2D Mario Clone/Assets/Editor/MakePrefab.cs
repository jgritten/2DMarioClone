using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class MakePrefab : MonoBehaviour {
    // generate a prefab from the selection

    [MenuItem("Project Tools / Make Prefab")]

    static void CreatePrefab()
    {
        GameObject[] selectedObjects;                                           // selection from the scene view
        selectedObjects = Selection.gameObjects;

        // loop through selection
        foreach (GameObject obj in selectedObjects)
        {
            string name = obj.name;                                             // store the name of selection
            string localPath = "Assets/Meshes/" + name + ".prefab";             // create the path for the prefab 
            
            // check for object inside project path            
            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
            {
                if (EditorUtility.DisplayDialog("Caution", "Prefab already exists. Would you like to overwrite?", "Yes", "No"))     // Yes No statement to override existing prefab
                {
                    CreateNew(localPath, obj);                                  // create new Prefab
                }
            }
            else
            {
                CreateNew(localPath, obj);                                      // create new Prefab
            }
        }
    }


    // Create Prefab using the GameObject and Path given. Then Delete the Mesh, replacing it with the new Prefab
    static void CreateNew(string path, GameObject go)
    {
        Object prefab = PrefabUtility.CreatePrefab(path, go);                          
        AssetDatabase.Refresh();
        DestroyImmediate(go);
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
    }
}
