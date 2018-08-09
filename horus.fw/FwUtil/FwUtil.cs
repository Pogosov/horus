using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.FwUtil
{
    public class FwUtil
    {
        public static void DeleteFilesAndSubDirectories(string directoryPath)
        {
            Directory.GetDirectories(directoryPath).ToList().ForEach(d => Directory.Delete(d, true));
            Directory.GetFiles(directoryPath).ToList().ForEach(f => File.Delete(f));
        }
    }
}
