using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ThreeJsExporter.Editor
{
    public static class ExporterMenu 
    {
        [MenuItem("Tools/UnityToThreeJS/Export %&#P")]
        [DidReloadScripts]
        public static void Export()
        {
            var sw = new Stopwatch();
            sw.Start();
            
            var activeScene = EditorSceneManager.GetActiveScene();
            var sceneJson = Exporter.ExportSceneToJson(activeScene);

            var assetsPath = Application.dataPath;
            var resPath = Path.Combine(assetsPath, "..", "Result", "ExportedScene.js");
            File.WriteAllText(resPath, sceneJson);
            
            sw.Stop();
            
            Debug.Log($"Scene was exported in {sw.ElapsedMilliseconds}ms\nPath: {resPath}\n" + sceneJson);
        }
    }
}
