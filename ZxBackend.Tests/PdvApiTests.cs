using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using ZxBackend.Controllers;
using ZxBackend.Data;

namespace ZxBackend.Tests
{
    [TestClass]
    public class PdvApiTests
    {
        [TestMethod]
        public void TestCreatePdvInvalidId()
        {
            //Arrange
            var controller = InstantiateController();
            var obj = MockValidPdv();
            obj["id"] = null;

            // Act
            var response = controller.Post(obj);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Count > 0);
        }
        
        [TestMethod]
        public void TestCreatePdvEmptyAddress()
        {
            //Arrange
            var controller = InstantiateController();
            var obj = MockValidPdv();
            obj["address"] = null;

            // Act
            var response = controller.Post(obj);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Count > 0);
        }


        [TestMethod]
        public void TestCreatePdvSuccess()
        {
            //Arrange
            var controller = InstantiateController();
            var obj = MockValidPdv();

            // Act
            var response = controller.Post(obj);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.IsTrue(response.Errors.Count == 0);
        }

        private JObject MockValidPdv()
        {
            return JObject.Parse(@"{
	            ""id"": 51, 
	            ""tradingName"": ""Adega da Cerveja - Teste"",
	            ""ownerName"": ""Pedro Melo"",
	            ""document"": ""1234567891011/0001"",
	            ""coverageArea"": { 
	              ""type"": ""MultiPolygon"", 
	              ""coordinates"": [
		            [[[30, 20], [45, 40], [10, 40], [30, 20]]], 
		            [[[15, 5], [40, 10], [10, 20], [5, 10], [15, 5]]]
	              ]
	            },
	            ""address"": { 
	              ""type"": ""Point"",
	              ""coordinates"": [-46.57421, -21.785741]
	            },
	            ""deliveryCapacity"": 5
            }");
        }

        private static PdvController InstantiateController()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseInMemoryDatabase("zxbackendinmemorydb");
            var db = new AppDbContext(optionsBuilder.Options);
            var controller = new PdvController(cache,db);
            return controller;
        }

    }
}
