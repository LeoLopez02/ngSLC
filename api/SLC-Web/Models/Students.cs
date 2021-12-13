using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLC_Web.Models
{
    public class Students
    {
        public ObjectId Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentContactPhone { get; set; }
        public string StudentAddress { get; set; }
        public string StudentEmail { get; set; }
        public string StudentCID { get; set; }
        public DateTime StudentDOB { get; set; }
        public DateTime StudentDOJ { get; set; }
    }
}
