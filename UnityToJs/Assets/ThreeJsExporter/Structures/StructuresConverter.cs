using System;
using UnityEngine;

namespace ThreeJsExporter.Structures
{
    public static unsafe class StructuresConverter
    {
        public static Geometry ToBufferGeometry(UnityEngine.Mesh mesh)
        {
            var uuid = Guid.NewGuid().ToString();
            var positionFloats = ToFloatArray(mesh.vertices);
            var normalFloats   = ToFloatArray(mesh.normals);
            var uvFloats       = ToFloatArray(mesh.uv);
            var indices        = ToUInt16Array(mesh.triangles);
            
            var geometry = new GeometryAttributes(
                new GeometryAttribute("Float32Array", 3, positionFloats), 
                 new GeometryAttribute("Float32Array", 3, normalFloats), 
                     new GeometryAttribute("Float32Array", 2, uvFloats));
            
            var res = new Geometry(uuid, mesh.name, "BufferGeometry", geometry, new GeometryIndexAttribute("Uint16Array", indices));
            return res;
        }

        public static ushort[] ToUInt16Array(int[] array)
        {
            if (array == null || array.Length == 0)
                return null;

            var res = new ushort[array.Length];
            fixed (ushort* shortArrPtr = res)
            fixed (int* intArrPtr = array)
            {
                var src = intArrPtr;
                var dst = shortArrPtr;

                var end = src + array.Length;

                while (src < end)
                {
                    *dst = (ushort) *src;
                    ++dst;
                    ++src;
                }
            }
            
            return res;
        }


        public static float[] ToFloatArray(Vector2[] array)
        {
            if (array == null || array.Length == 0)
                return null;

            var res = new float[array.Length * 2];
            fixed (float* floatArrPtr = res)
            fixed (Vector2* vectorArrPtr = array)
            {
                var bytesToCopy = res.Length * sizeof(float);
                // Copy! May be performance critical
                Buffer.MemoryCopy(vectorArrPtr, floatArrPtr, 
                    bytesToCopy, bytesToCopy);
            }
            
            return res;
        }
        
        public static float[] ToFloatArray(Vector3[] array)
        {
            if (array == null || array.Length == 0)
                return null;

            var res = new float[array.Length * 3];
            fixed (float* floatArrPtr = res)
            fixed (Vector3* vectorArrPtr = array)
            {
                var bytesToCopy = res.Length * sizeof(float);
                // Copy! May be performance critical
                Buffer.MemoryCopy(vectorArrPtr, floatArrPtr, 
                    bytesToCopy, bytesToCopy);
            }
            
            return res;
        }
    }
}