using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Engr.FileFormats.OBJ;
using Engr.FileFormats.STL;
using Engr.Maths.Vectors;
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
        [TestMethod]
        public void Test()
        {
            var stlIn = new STLFile()
            {
                Header = "Testing",
            };
            stlIn.Facets.Add(new Facet(Vect3f.UnitX, Vect3f.UnitX, Vect3f.UnitX, Vect3f.UnitX));
            stlIn.Facets.Add(new Facet(Vect3f.UnitY, Vect3f.UnitY, Vect3f.UnitY, Vect3f.UnitY));
            stlIn.Facets.Add(new Facet(Vect3f.UnitZ, Vect3f.UnitZ, Vect3f.UnitZ, Vect3f.UnitZ));

            Assert.AreEqual(stlIn.Header, "Testing");
            Assert.AreEqual(stlIn.Facets.Count, 3);

            var ms = new MemoryStream();
            stlIn.Save(ms, true);

            ms.Position = 0;

            var stlOut = new STLFile(ms);

            Assert.AreEqual(stlIn.Header, stlOut.Header);
            Assert.AreEqual(stlIn.Facets.Count, stlOut.Facets.Count);

           


            //stl.Facets.Add();



            //using (var md5 = MD5.Create())
            //{
            //    using (var stream = File.OpenRead(filename))
            //    {
            //        md5.ComputeHash(stream);
            //    }
            //}
        }

    }
}