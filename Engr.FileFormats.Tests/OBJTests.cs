using System.Linq;
using Engr.FileFormats.OBJ;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engr.FileFormats.Tests
{
    [TestClass]
    public class OBJTests
    {
        [TestMethod]
        public void LoadCube()
        {
            var obj = new OBJFile("Models/cube.obj", new FileMaterialLoader("Models/cube.mtl"));
            Assert.AreEqual(24, obj.Vertices.Count);
            Assert.AreEqual(24, obj.TextureVertices.Count);
            Assert.AreEqual(24, obj.Normals.Count);
            Assert.AreEqual(1, obj.Materials.Count);
            Assert.AreEqual(3, obj.LoadedMaterials.Count);
            Assert.AreEqual("DEFAULT_MTL", obj.Materials.First().Name);
        }
        [TestMethod]
        public void LoadCapsule()
        {
            var obj = new OBJFile("Models/capsule.obj", new FileMaterialLoader("Models/capsule.mtl"));
            //Assert.AreEqual(24, obj.Vertices.Count);
            //Assert.AreEqual(24, obj.TextureVertices.Count);
            //Assert.AreEqual(24, obj.Normals.Count);
            //Assert.AreEqual(1, obj.Materials.Count);
            //Assert.AreEqual("DEFAULT_MTL", obj.Materials.First().Name);
        }
    }
}