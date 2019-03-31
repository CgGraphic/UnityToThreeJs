using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using ThreeJsExporter.Structures;
using UnityEngine;
using Mesh = ThreeJsExporter.Structures.Mesh;
using UnityScene = UnityEngine.SceneManagement.Scene;
using ThreeJsScene = ThreeJsExporter.Structures.Scene;

namespace ThreeJsExporter
{
    public static class Exporter
    {
        private static string GENERATOR_NAME = "Gabenwithrock.UnityToThreeJsExporter";
        private static IJsonSerializer jsonSerializer;
        private const bool INCLUDE_INACTIVE = false;
        private static readonly float[] MATRIX_IDENTITY = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
        private static Dictionary<UnityEngine.Mesh, Geometry> meshesDictionary;

        static Exporter()
        {
//            jsonSerializer = new UnityJsonSerializer(); // DOESN'T support object[] polymorph serialization
            jsonSerializer = new NewtonsoftJsonSerializer();
        }
        
        public static string ExportSceneToJson(UnityScene scene)
        {
            var resScene = new ThreeJsScene();
            // BUG: float Version serialization precision 4.3f => 4.300000190734863, try another JSON serializer,
            // which can specify float serialization length
            resScene.Metadata = new Metadata(4.5f, "Object", GENERATOR_NAME);
            meshesDictionary = PrepareGeometriesMap(scene, INCLUDE_INACTIVE);
            
            resScene.Geometries = meshesDictionary.Values.ToArray();
            resScene.Object = PrepareSceneHierarchy(scene, INCLUDE_INACTIVE);
            
            return jsonSerializer.ToJson(resScene);
        }

        private static Dictionary<UnityEngine.Mesh, Geometry> PrepareGeometriesMap(UnityScene scene, bool includeInactive)
        {
            var meshFilters = UnityExporterHelper.GetComponentsInScene<MeshFilter>(scene, includeInactive);
            var distinctMeshes = meshFilters.Select(x => x.sharedMesh).Distinct();

            var res = new Dictionary<UnityEngine.Mesh, Geometry>();
            foreach (var mesh in distinctMeshes)
            {
                var geometry = StructuresConverter.ToBufferGeometry(mesh);
                res.Add(mesh, geometry);
            }

            return res;
        }

        private static SceneHierarchy PrepareSceneHierarchy(UnityScene scene, bool includeInactive)
        {
            var sceneGuid = Guid.NewGuid().ToString();
            var res = new SceneHierarchy(sceneGuid, scene.name, MATRIX_IDENTITY);

            var rootGameObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                var rootObject = rootGameObjects[i];
                if (!includeInactive && !rootObject.activeInHierarchy)
                    continue;

                FillChildrenRecursive(rootObject.transform, res.Children);
            }
            return res;
        }

        private static void FillChildrenRecursive(Transform rootObject, List<object> resChildren)
        {
            // rootObject.localToWorldMatrix is not in parent's world space
            var trs = Matrix4x4.TRS(rootObject.localPosition, rootObject.localRotation, rootObject.localScale); // TODO: slow, need optimization 
            var matrixArray = UnityExporterHelper.MatrixToArray(trs);
            
            var filter = rootObject.GetComponent<MeshFilter>();
            if (filter)
            {
                UnityEngine.Mesh sharedMesh = filter.sharedMesh;
                var meshObject = new Mesh(
                    Guid.NewGuid().ToString("D"),
                    sharedMesh.name,
                    matrixArray,
                    filter == null ? string.Empty : meshesDictionary[sharedMesh].Uuid
                );
            
                resChildren.Add(meshObject);
            }
            
            var camera = rootObject.GetComponent<Camera>();
            if (camera)
            {
                var cameraMatrixArray = GetCameraMatrixArray(matrixArray);
                
                var cameraObj = new PerspectiveCamera(
                    Guid.NewGuid().ToString("D"),
                    rootObject.name,
                    cameraMatrixArray,
                    camera.fieldOfView,
                    camera.nearClipPlane,
                    camera.farClipPlane);
                
                resChildren.Add(cameraObj);
            }
            
            var newRoot = new Object3D(
                Guid.NewGuid().ToString("D"),
                rootObject.name,
                matrixArray);
            
            resChildren.Add(newRoot);
            
            for (int i = 0; i < rootObject.childCount; i++)
            {
                var child = rootObject.GetChild(i);
                FillChildrenRecursive(child, newRoot.Children);
            }
        }

        /// <summary>
        /// Rotates camera by 180 degrees around local UP (inverse lookAt direction)
        /// </summary>
        private static float[] GetCameraMatrixArray(float[] matrixArray)
        {
            var res = new float[16];
            matrixArray.CopyTo(res, 0);

            // X 
            res[0] *= -1;
            res[1] *= -1;
            res[2] *= -1;
            res[3] *= -1;
            
            // Z
            res[8]  *= -1;
            res[9]  *= -1;
            res[10] *= -1;
            res[11] *= -1;
            
            return res;
        }
    }
}
