using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSCalculator.Utility;

namespace NSCalculator.Test
{
    // Unit tests voor de ExpressionEvaluator. 
    // Handig om dit van te voren al te maken en hiervanuit het algoritme schrijven en aanpassen
    [TestClass]
    public class ExpressionEvaluatorUnitTest
    {
        [TestMethod]
        public void TestBasicExpression()
        {
            var result = ExpressionEvaluator.EvaluateExpression("1+2");

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestAdvancedExpression()
        {
            var result = ExpressionEvaluator.EvaluateExpression("1+2*3");

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void TestMoreAdvancedExpression()
        {
            var result = ExpressionEvaluator.EvaluateExpression("1+3*5-6+6/2*2-4*4/2-1*2");

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void TestExpressionWithNegative()
        {
            var result = ExpressionEvaluator.EvaluateExpression("5*-3");

            Assert.AreEqual(-15, result);
        }

        [TestMethod]
        public void TestDecimalExpression()
        {
            var result = ExpressionEvaluator.EvaluateExpression("3/2*2+.2");

            Assert.AreEqual(3.2, result);
        }

        [TestMethod]
        public void TestPowerExpression()
        {
            var result = ExpressionEvaluator.EvaluateExpression("2+3^3+2");

            Assert.AreEqual(31, result);
        }
    }
}
