// ThreeJS documentation
// https://github.com/mrdoob/three.js/wiki/JSON-Object-Scene-format-4

using System;
using System.Collections.Generic;

namespace ThreeJsExporter.Structures
{
    [Serializable]
    public struct Scene
    {
        public Metadata Metadata;
        public Geometry[] Geometries;
        public SceneHierarchy Object;
    }

    #region [Assets]

    [Serializable]
    public struct Metadata
    {
        public float Version;
        public string Type;
        public string Generator;

        public Metadata(float version, string type, string generator)
        {
            Version = version;
            Type = type;
            Generator = generator;
        }
    }
    
    #region [Geometry]

    [Serializable]
    public struct Geometry
    {
        public string Uuid;
        public string Name;
        public string Type;
        public GeometryData Data;
        

        public Geometry(string uuid, string name, string type, GeometryAttributes attributes, GeometryIndexAttribute index)
        {
            Uuid = uuid;
            Name = name;
            Type = type;
            Data = new GeometryData(attributes, index);
        }
    }

    public struct GeometryData
    {
        public GeometryAttributes Attributes;
        public GeometryIndexAttribute Index;

        public GeometryData(GeometryAttributes attributes, GeometryIndexAttribute index)
        {
            Attributes = attributes;
            Index = index;
        }
    }
    
    [Serializable]
    public struct GeometryAttributes
    {
        public GeometryAttribute Position;
        public GeometryAttribute Normal;
        public GeometryAttribute Uv;
        
        public GeometryAttributes(GeometryAttribute position, GeometryAttribute normal, GeometryAttribute uv)
        {
            Position = position;
            Normal = normal;
            Uv = uv;
        }
    }
    
    [Serializable]
    public struct GeometryAttribute
    {
        public int ItemSize;
        public string Type;
        public float[] Array;

        public GeometryAttribute(string type, int itemSize, float[] array)
        {
            ItemSize = itemSize;
            Type = type;
            Array = array;
        }
    }
    
    [Serializable]
    public struct GeometryIndexAttribute
    {
        public string Type;
        public ushort[] Array;

        public GeometryIndexAttribute(string type, ushort[] array)
        {
            Type = type;
            Array = array;
        }
    }
    
    #endregion
    
    // TODO: materials, textures, images
    
    #endregion

    [Serializable]
    public class SceneHierarchy: Object3D
    {
        public SceneHierarchy(string uuid, string name, float[] matrix): base(uuid, name, matrix)
        {
            Uuid = uuid;
            Matrix = matrix;
            Type = "Object3D";
        }
    }
    
    [Serializable]
    public class Mesh: Object3D
    {
        public string Geometry;

        public Mesh(string uuid, string name, float[] matrix, string geometry) : base(uuid, name, matrix)
        {
            Geometry = geometry;
        }
    }
    
    public class PerspectiveCamera: Object3D
    {
        public float Fov;
        public float Near;
        public float Far;

        public PerspectiveCamera(string uuid, string name, float[] matrix, float fov, float near, float far) : base(uuid, name, matrix)
        {
            Fov = fov;
            Near = near;
            Far = far;
        }
    }
    
    public class Object3D
    {
        public string Uuid;
        public string Name;
        public string Type;
        public float[] Matrix; // Matrix4x4 => float[16]
        public List<object> Children = new List<object>();

        public Object3D(string uuid, string name, float[] matrix)
        {
            Uuid = uuid;
            Name = name;
            Matrix = matrix;
            Type = GetType().Name;
        }
    }
}