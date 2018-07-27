using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZxBackend.Models;
using ZxBackend.Models.Pdv;

namespace ZxBackend.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Pdvs.Any())
            {
                return; //DB has been seeded
            }
            var rawObj = JObject.Parse(System.IO.File.ReadAllText(@"pdvs.json"));
            var pdvs = (JArray)rawObj["pdvs"];
            foreach (var item in pdvs)
            {
                var pdv = new Pdv()
                {
                    Id = 0, // Avoid explict id inserting in identity column
                    TradingName = (string)item["tradingName"],
                    OwnerName = (string)item["ownerName"],
                    Document = (string)item["document"],
                    CoverageArea = (string)item["coverageArea"],
                    Address = (string)item["address"],
                };
                context.Pdvs.Add(pdv);
            }
            context.SaveChanges();
        }
    }
}
