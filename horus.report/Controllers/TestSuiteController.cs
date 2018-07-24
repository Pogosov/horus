using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using horus_report.Models;

namespace horus_report.Controllers
{
    [RoutePrefix("api/testsuites")]
    public class TestSuiteController : ApiController
    {
        private TestSuite[] TestSuites = new TestSuite[]
        {
            new TestSuite { Id = 1, Name = "Tomato Soup", StartTime = DateTime.Now, EndTime = DateTime.Now + TimeSpan.FromHours(1) },
            new TestSuite { Id = 2, Name = "Yo-yo", StartTime = DateTime.Now, EndTime = DateTime.Now + TimeSpan.FromHours(1) },
            new TestSuite { Id = 3, Name = "Hammer", StartTime = DateTime.Now, EndTime = DateTime.Now + TimeSpan.FromHours(1) }
        };

        [Route("")]
        [HttpGet]
        public IEnumerable<TestSuite> GetAllTestSuites()
        {
            return TestSuites;
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetTestSuite(int id)
        {
            var testSuite = TestSuites.FirstOrDefault((p) => p.Id == id);
            if (testSuite == null)
            {
                return NotFound();
            }

            return Ok(testSuite);
        }

        [Route("list/{page?}/{pageSize?}")]
        [HttpGet]
        public IHttpActionResult GetTestSuites(int page, int pageSize)
        {
            var testSuites = TestSuites.Where((p) => p.Id == page || p.Id == pageSize);
            if (testSuites == null)
            {
                return NotFound();
            }

            return Ok(testSuites);
        }
    }
}
