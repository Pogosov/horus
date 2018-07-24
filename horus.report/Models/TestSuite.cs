using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace horus_report.Models
{
    public class TestSuite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}