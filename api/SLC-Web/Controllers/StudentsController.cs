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
    public class StudentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            var dbList = dbClient.GetDatabase("Dbslc-web").GetCollection<Students>("Students").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Students students)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            int LastStudentId = dbClient.GetDatabase("Dbslc-web").GetCollection<Students>("Students").AsQueryable().Count();
            students.StudentId = LastStudentId + 1;

            dbClient.GetDatabase("Dbslc-web").GetCollection<Students>("Students").InsertOneAsync(students);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Students students)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            var filter = Builders<Students>.Filter.Eq("StudentId", students.StudentId);
            var update = Builders<Students>.Update.Set("StudentName", students.StudentName)
                                                    .Set("StudentLastName", students.StudentLastName)
                                                    .Set("StudentContactPhone", students.StudentContactPhone)
                                                    .Set("StudentAddress", students.StudentAddress)
                                                    .Set("StudentEmail", students.StudentEmail)
                                                    .Set("StudentCID", students.StudentCID)
                                                    .Set("StudentDOB", students.StudentDOB)
                                                    .Set("StudentDOJ", students.StudentDOJ);

            dbClient.GetDatabase("Dbslc-web").GetCollection<Students>("Students").UpdateManyAsync(filter, update);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("SLCWebCon"));

            var filter = Builders<Students>.Filter.Eq("StudentId", id);

            dbClient.GetDatabase("Dbslc-web").GetCollection<Students>("Students").DeleteOneAsync(filter);

            return new JsonResult("Deleted Successfully");
        }
    }
}
