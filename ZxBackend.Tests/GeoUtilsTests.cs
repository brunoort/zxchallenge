using GeoJSON.Net.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ZxBackend.Utils;

namespace ZxBackend.Tests
{
    [TestClass]
    public class GeoUtilsTests
    {
        [TestMethod]
        public void IsPointInPolygonFalseTest()
        {
            //Arrange
            var point = new Point(new Position(2.5, 2.5));
            var polygon = new Point[4]{
                new Point(new Position(1, 1)),
                new Point(new Position(1, 2)),
                new Point(new Position(2, 2)),
                new Point(new Position(2 ,1)),
            };
            
            // Act
            var response = GeoUtils.IsPointInPolygon(polygon, point);

            // Assert
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void IsPointInPolygonTrueTest()
        {
            //Arrange
            var point = new Point(new Position(1.2, 1.2));
            var polygon = new Point[4]{
                new Point(new Position(1, 1)),
                new Point(new Position(1, 2)),
                new Point(new Position(2, 2)),
                new Point(new Position(2 ,1)),
            };

            // Act
            var response = GeoUtils.IsPointInPolygon(polygon, point);

            // Assert
            Assert.IsTrue(response);
        }
    }
}
