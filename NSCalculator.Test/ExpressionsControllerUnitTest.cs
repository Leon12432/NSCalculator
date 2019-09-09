using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSCalculator.Controllers;
using NSCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSCalculator.Test
{
    // Unit test voor de controller. Test statuscodes en responses bij een bepaalde invoer.
    [TestClass]
    public class ExpressionsControllerUnitTest
    {
        ExpressionsController controller;
        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<NSCalculatorContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;
            var context = new NSCalculatorContext(options);
            controller = new ExpressionsController(context);
        }

        [TestMethod]
        public void ExpressionShouldEvaluate()
        {
            var result = controller.EvaluateExpression("5*3");

            Assert.AreEqual("15", result.Result.Value.ExpressionResult);
        }

        [TestMethod]
        public void ExpressionShouldReturnBadRequest()
        {
            var result = controller.EvaluateExpression("5*");

            Assert.AreEqual(400, ((StatusCodeResult)result.Result.Result).StatusCode);
        }

        [TestMethod]
        public async Task ExpressionShouldStore()
        {
            var result = controller.GetExpressions();

            Assert.AreEqual(0, ((IEnumerable<Expression>)((ObjectResult)result.Result.Result).Value).Count());

            await controller.EvaluateExpression("1+1");

            var result2 = controller.GetExpressions();

            Assert.AreEqual(1, ((IEnumerable<Expression>)((ObjectResult)result2.Result.Result).Value).Count());
        }
    }
}
