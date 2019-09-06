using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirRouteFinder;
using AirRouteFinder.Controllers;
using AirRouteFinder.Models;

namespace AirRouteFinder.Tests.Controllers
{
    [TestClass]
    public class RouteControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            RouteController controller = new RouteController();
            // Act
            string result = controller.Get("YYZ","YVR");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("YYZ -> JFK -> LAX -> YVR", result);
        }

        [TestMethod]
        public void GetNoRoute()
        {
            // Arrange
            RouteController controller = new RouteController();
            // Act
            string result = controller.Get("YYZ", "ORD");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("No Route", result);
        }

        [TestMethod]
        public void GetInvalidOrigin()
        {
            // Arrange
            RouteController controller = new RouteController();
            // Act
            string result = controller.Get("XXX", "ORD");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Origin", result);
        }

        [TestMethod]
        public void GetInvalidDestination()
        {
            // Arrange
            RouteController controller = new RouteController();
            // Act
            string result = controller.Get("ORD", "XXX");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Destination", result);
        }
    }
}
