using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Controller.WindowComponentController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.WindowComponentController.Tests
{
    [TestClass()]
    public class SlimeNodeViewControllerTests
    {
        [TestMethod()]
        public void GetColourForNodeTest()
        {
            var slimeCoveredFood = new FoodSourceNode(1, 1, 1);
            var food = new FoodSourceNode(2,2,2);
            var slimeCoveredNormal = new Node(3,3,3);
            var normal = new Node(4,4,4);
            var controller = new SlimeNodeViewController(new List<Node>() {slimeCoveredFood, slimeCoveredNormal});
            Assert.AreEqual(SlimeNodeViewController.SlimeFoodSourceColour, controller.GetColourForNode(slimeCoveredFood));
            Assert.AreEqual(SlimeNodeViewController.FoodSourceColour, controller.GetColourForNode(food));

            Assert.AreEqual(SlimeNodeViewController.SlimeNodeColour, controller.GetColourForNode(slimeCoveredNormal));
            Assert.AreEqual(SlimeNodeViewController.NormalNodeColour, controller.GetColourForNode(normal));
        }

        [TestMethod()]
        public void GetSizeForNodeTest()
        {
            var food = new FoodSourceNode(2, 2, 2);
            var normal = new Node(4, 4, 4);
            var controller = new SlimeNodeViewController(new List<Node>());

            Assert.AreEqual(SlimeNodeViewController.FoodSourcePointSize, controller.GetSizeForNode(food));
            Assert.AreEqual(SlimeNodeViewController.NonFoodSourcePointSize, controller.GetSizeForNode(normal));
        }
    }
}
