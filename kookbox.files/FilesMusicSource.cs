using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.files
{
    public class FilesMusicSource : IMusicSource
    {
        public FilesMusicSource()
        {
        }

        public string Name => "Files";
    }
}
