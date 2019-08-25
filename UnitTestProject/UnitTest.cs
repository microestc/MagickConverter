using MagickConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var service = new MagickService();
            service.Convert(new TaskSetup("1000", @"C:\app\0.pdf", @"C:\app\0.jpeg"));

        }
    }
}
