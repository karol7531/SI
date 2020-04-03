using CSP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSPTests
{
    [TestClass]
    public class DomainTests
    {
        [TestMethod]
        public void TestClone()
        {
            byte[] domainValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Domain<byte> domain = new Domain<byte>(domainValues.ToList());
            Domain<byte> domainCloned = domain.Clone();
            domain.values.Remove(3);
            Assert.AreEqual(domainValues.Length, domainCloned.values.Count);
            Assert.AreEqual(9, domainCloned.values.Count);
            Assert.AreEqual(8, domain.values.Count);
            Assert.AreEqual(domainValues.Length - 1, domain.values.Count);
        }
    }
}
