using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Folder
    {

        public string Name { get; set; }

        public string Path { get; set; }

        public List<Folder> Childrens { get; set; }

        public List<Document> Documents { get; set; }

        public Document MainDocument { get; set; }

    }
}
