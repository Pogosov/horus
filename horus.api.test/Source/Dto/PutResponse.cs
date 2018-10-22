using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.api.Source.Dto
{
    public class PutResponse
    {
        public string name { get; set; }

        public string job { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
