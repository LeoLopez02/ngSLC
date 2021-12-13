using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SLC_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLC_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            var dbList = dbClient.GetDatabase("Dbslc-web").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            int LastEmployeeId = dbClient.GetDatabase("Dbslc-web").GetCollection<Employee>("Employee").AsQueryable().Count();
            employee.EmployeeId = LastEmployeeId + 1;

            dbClient.GetDatabase("Dbslc-web").GetCollection<Employee>("Employee").InsertOneAsync(employee);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", employee.EmployeeId);
            var update = Builders<Employee>.Update.Set("EmployeeName", employee.EmployeeName)
                                                    .Set("EmployeeLastName", employee.EmployeeLastName)
                                                    .Set("EmployeeContactPhone", employee.EmployeeContactPhone)
                                                    .Set("EmployeeAddress", employee.EmployeeAddress)
                                                    .Set("EmployeeEmail", employee.EmployeeEmail)
                                                    .Set("EmployeeCID", employee.EmployeeCID)
                                                    .Set("EmployeeDOB", employee.EmployeeDOB)
                                                    .Set("EmployeeDOJ", employee.EmployeeDOJ);

            dbClient.GetDatabase("Dbslc-web").GetCollection<Employee>("Employee").UpdateManyAsync(filter, update);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);

            dbClient.GetDatabase("Dbslc-web").GetCollection<Employee>("Employee").DeleteOneAsync(filter);

            return new JsonResult("Deleted Successfully");
        }
    }
}
