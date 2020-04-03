using CSP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSPTests
{
    [TestClass]
    public class VariableTests
    {
        [TestMethod]
        public void TestClone()
        {
            byte[] domainValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Domain<byte> domain = new Domain<byte>(domainValues.ToList());
            Variable<byte> variable = new Variable<byte>(1, domain, "V");
            Variable<byte> variableCloned = variable.Clone();
            variable.domain.values.Remove(3);
            Assert.AreNotEqual(variable.domain.values.Count, variableCloned.domain.values.Count);
            Assert.AreEqual(variable, variableCloned);
            Assert.AreEqual(variable.domain.values.Count + 1, variableCloned.domain.values.Count);
        }
    }
}
