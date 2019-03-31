using System.Linq;
using NUnit.Framework;
using ThreeJsExporter.Structures;
using UnityEngine;

namespace Tests
{
    public class ExporterTests
    {
        [Test]
        public void ToUInt16ArrayTest()
        {
            var arr = new int[] {1, 2, 3, 4};
            var expectedResult = new ushort[] {1, 2, 3, 4};
            var actualResult = StructuresConverter.ToUInt16Array(arr);
            
            Debug.Log($"expectedResult : {string.Join(", ", expectedResult)}\n" +
                      $"actualResult : {string.Join(", ", actualResult)}\n");
            Assert.IsTrue(expectedResult.SequenceEqual(actualResult));
        }
        
        [Test]
        public void ToFloatArray2Test()
        {
            var arr = new Vector2[] { new Vector2(1, 2), new Vector2(3, 4) };
            var expectedResult = new float[] {1, 2, 3, 4};
            
            var actualResult = StructuresConverter.ToFloatArray(arr);
            
            Debug.Log($"expectedResult : {string.Join(", ", expectedResult)}\n" +
                      $"actualResult : {string.Join(", ", actualResult)}\n");
            Assert.IsTrue(expectedResult.SequenceEqual(actualResult));
        }
        
        [Test]
        public void ToFloatArray3Test()
        {
            var arr = new Vector3[] { new Vector3(1, 2, 3), new Vector3(4, 5, 6) };
            var expectedResult = new float[] {1, 2, 3, 4, 5, 6};
            
            var actualResult = StructuresConverter.ToFloatArray(arr);
            
            Debug.Log($"expectedResult : {string.Join(", ", expectedResult)}\n" +
                      $"actualResult : {string.Join(", ", actualResult)}\n");
            Assert.IsTrue(expectedResult.SequenceEqual(actualResult));
        }
    }
}
