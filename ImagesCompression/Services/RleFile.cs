using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesCompression.Services
{
    class RleFile
    {
        private List<byte> file;
        public RleFile(RleHeader header, IEnumerable<byte> red, IEnumerable<byte> green, IEnumerable<byte> blue)
        {
            
            file.AddRange(header.Header);
            file.AddRange(red);
            file.AddRange(green);
            file.AddRange(blue);
        }

    }
}
