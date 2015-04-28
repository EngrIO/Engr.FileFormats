using Engr.FileFormats.STL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engr.FileFormats.Tests
{
    [TestClass]
    public class STLTests
    {
        [TestMethod]
        public void LoadBinary()
        {
            var stl = new STLFile("Models/cube_binary.stl");
            Assert.AreEqual(STLType.Binary, stl.STLType);
            Assert.AreEqual(12, stl.Facets.Count);
            Assert.AreEqual("solid TESTCUBE", stl.Header);
        }

        [TestMethod]
        public void LoadASCII()
        {
            var stl = new STLFile("Models/cube_ascii.stl");
            Assert.AreEqual(STLType.ASCII, stl.STLType);
            Assert.AreEqual(12, stl.Facets.Count);
            Assert.AreEqual("TESTCUBE", stl.Header);
        }
    }
}
