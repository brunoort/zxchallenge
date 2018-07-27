using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ZxBackend.Models;
using Newtonsoft.Json;
using GeoJSON.Net.Geometry;
using ZxBackend.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using ZxBackend.Data;
using ZxBackend.Models.Utils;
using ZxBackend.Models.Pdv;

namespace ZxBackend.Controllers
{
    [Route("api/[controller]")]
    public class PdvController : Controller
    {
        const string _pdvsCacheKey = "pdvs";
        private IMemoryCache _cache;
        private AppDbContext _db;

        public PdvController(IMemoryCache caching,AppDbContext db)
        {
            _cache = caching;
            _db = db;
        }
        
        // GET api/pdv
        [HttpGet]
        public IEnumerable<Pdv> Get()
        {
            return CachePDVS();
        }

        [HttpGet("closest")]
        public Pdv Closest()
        {
            Pdv result = null;
            
            string latitude = Request.Query["lat"];
            string longitude = Request.Query["lon"];
            
            if (string.IsNullOrEmpty(latitude) || string.IsNullOrEmpty(longitude)) 
            return result;
            
            double lat, lon;
            lat = double.Parse(latitude);
            lon = double.Parse(longitude);
            var testPoint = new Point(new Position(lat, lon));

            //Query to find the closest PDV
            var pdvs = CachePDVS();
            foreach (var pdv in pdvs)
            {
                result = PdvRepository.SearchClosestPDV(result, lat, lon, testPoint, pdv);
            }

            return result;
        }

        

        // GET api/pdv/5
        [HttpGet("{id}")]
        public Pdv Get(int id)
        {
            var pdvs = CachePDVS();
            return pdvs.FirstOrDefault(x => x.Id == id);
        }

        // POST api/pdv
        [HttpPost]
        public UtilResponse Post(JObject item)
        {
            var errors = new List<string>();

            //Validating fields
            if ((int?)item["id"] == null || (int?)item["id"] < 1) errors.Add("Invalid Id");
            if (string.IsNullOrEmpty((string)item["document"])) errors.Add("Invalid CNPJ");            
            if (string.IsNullOrEmpty((string)item["ownerName"])) errors.Add("Invalid Owner Name");
            if (string.IsNullOrEmpty((string)item["tradingName"])) errors.Add("Invalid Trading Name");
            if ((int?)item["deliveryCapacity"] == null) errors.Add("Invalid Capacity");
            var coverageArea = JsonConvert.DeserializeObject<MultiPolygon>(item["coverageArea"]?.ToString());
            if (coverageArea == null) errors.Add("Invalid Coverage Area");
            var address = JsonConvert.DeserializeObject<Point>(item["address"]?.ToString());
            if (address == null) errors.Add("Invalid Address");

            //Validating CNPJ
            var pdvs = CachePDVS();
            if (pdvs.Any(x => x.Document == (string)item["document"])) errors.Add("The CNPJ must be unique within database");

            if (errors.Count > 0)
            {
                return new UtilResponse(false, errors, item);
            }
            else
            {
                var pdv = new Pdv()
                {
                    Id = 0, 
                    TradingName = item["tradingName"].ToString(),
                    OwnerName = item["ownerName"].ToString(),
                    Document = item["document"].ToString(),
                    CoverageArea = item["coverageArea"].ToString(),
                    Address = item["address"].ToString(),
                };
                pdvs.ToList().Add(pdv);
                _cache.Set(_pdvsCacheKey,pdvs);
                _db.Pdvs.Add(pdv);
                _db.SaveChanges();
                return new UtilResponse(true, item);
            }
        }
        
        private IEnumerable<Pdv> CachePDVS()
        {
            var cached = _cache.GetOrCreate(_pdvsCacheKey, entry =>
            {
                return _db.Set<Pdv>().ToList();
            });
            return cached;
        }

    }

    
}
