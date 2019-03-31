using System.Collections.Generic;
using UnityEngine;

namespace ThreeJsExporter.Structures
{
    public static class UnityExporterHelper
    {
        public static List<T> GetComponentsInScene<T>(UnityEngine.SceneManagement.Scene scene, bool includeInactive) where T: Component
        {
            var res = new List<T>();
            var rootObjects = scene.GetRootGameObjects();
            for (var i = 0; i < rootObjects.Length; i++)
            {
                var rootObject = rootObjects[i];
                var components = rootObject.GetComponentsInChildren<T>(includeInactive);
                res.AddRange(components);
            }

            return res;
        }

        public static float[] MatrixToArray(Matrix4x4 mat)
        {
            // slow performance
//            var res = new[]
//            {
//                mat.m00, mat.m10, mat.m20, mat.m30,
//                mat.m01, mat.m11, mat.m21, mat.m31,
//                mat.m02, mat.m12, mat.m22, mat.m32,
//                mat.m03, mat.m13, mat.m23, mat.m33,
//            };

// fast performance
            var res = new float[16];
            res[0]  = mat.m00;
            res[4]  = mat.m01;
            res[8]  = mat.m02;
            res[12] = mat.m03;
            
            res[1]  = mat.m10;
            res[5]  = mat.m11;
            res[9]  = mat.m12;
            res[13] = mat.m13;
            
            res[2]  = mat.m20;
            res[6]  = mat.m21;
            res[10] = mat.m22;
            res[14] = mat.m23;
            
            res[3]  = mat.m30;
            res[7]  = mat.m31;
            res[11] = mat.m32;
            res[15] = mat.m33;

            return res;
        }
    }
}