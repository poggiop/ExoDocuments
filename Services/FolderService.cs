using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;
using Services.Interface;

namespace Services
{
    public class FolderService : IFolderService
    {
        public Folder getCurrentFoler(string path)
        {
            var current = new Folder();
            current.Name = GetName(path);
            current.Childrens = new List<Folder>();
            foreach (var dir in Directory.GetDirectories(path))
            {
                current.Childrens.Add(new Folder() { Name = GetName(dir), Path = dir });
            }
            current.Documents = new List<Document>();
            foreach (var doc in Directory.GetFiles(path, "*.md"))
            {
                current.Documents.Add(new Document() { Name = GetName(doc), Path = doc });
            }

            if (Directory.Exists(path + "\\Index.md"))
            {
                var docMD = Directory.GetFiles(path, "Index.md").First();
                current.MainDocument = new Document() { Name = GetName(docMD), Path = docMD };
            }

            return current;
        }

        private static string GetName(string dir)
        {
            return dir.Split(Path.DirectorySeparatorChar)[dir.Split(Path.DirectorySeparatorChar).Length - 1];
        }
    }
}
