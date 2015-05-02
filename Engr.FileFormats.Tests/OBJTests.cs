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
            var obj = new OBJFile("Models/cube.obj");
            Assert.AreEqual(24, obj.Vertices.Count);
            Assert.AreEqual(24, obj.TextureVertices.Count);
            Assert.AreEqual(24, obj.Normals.Count);
            Assert.AreEqual(3, obj.LoadedMaterials.Count);
            Assert.AreEqual(1, obj.Materials.Count);
            Assert.AreEqual("DEFAULT_MTL", obj.Materials[0].Name);
            Assert.AreEqual(0.695, obj.Materials[0].Kd.X, 0.001);
            Assert.AreEqual(0.743, obj.Materials[0].Kd.Y, 0.001);
            Assert.AreEqual(0.790, obj.Materials[0].Kd.Z, 0.001);
        }
        [TestMethod]
        public void LoadCapsule()
        {
            var obj = new OBJFile("Models/capsule.obj");
            Assert.AreEqual(5252, obj.Vertices.Count);
            Assert.AreEqual(5252, obj.TextureVertices.Count);
            Assert.AreEqual(5252, obj.Normals.Count);
            Assert.AreEqual(1, obj.Materials.Count);
            Assert.AreEqual("material0", obj.Materials.First().Name);
        }

        [TestMethod]
        public void LoadShips()
        {
            //http://pamargames.com/free-3d-models-game-assets/
            var ship1 = new OBJFile("Models/Ship1/Ship.obj");
            Assert.AreEqual(1073, ship1.Vertices.Count);
            Assert.AreEqual(1316, ship1.TextureVertices.Count);
            Assert.AreEqual(1276, ship1.Normals.Count);
            Assert.AreEqual(1076, ship1.Faces.Count);

            var ship2 = new OBJFile("Models/Ship2/Ship.obj");
            Assert.AreEqual(657, ship2.Vertices.Count);
            Assert.AreEqual(788, ship2.TextureVertices.Count);
            Assert.AreEqual(751, ship2.Normals.Count);
            Assert.AreEqual(712, ship2.Faces.Count);

            var ship3 = new OBJFile("Models/Ship3/Ship.obj");
            Assert.AreEqual(539, ship3.Vertices.Count);
            Assert.AreEqual(677, ship3.TextureVertices.Count);
            Assert.AreEqual(627, ship3.Normals.Count);
            Assert.AreEqual(546, ship3.Faces.Count);
        }

    }
}