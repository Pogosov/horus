using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.api.Source.Data
{
    public class GetResponse
    {
        public int page { get; set; }

        public int per_page { get; set; }

        public int total { get; set; }

        public int total_pages { get; set; }

        public List<User> data { get; set; }
    }
}
