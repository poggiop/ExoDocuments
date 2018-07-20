using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;

namespace Services.Interface
{
    public interface IFolderService
    {

        Folder getCurrentFoler(String path);
        
    }
}
