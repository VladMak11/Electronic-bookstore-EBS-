using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models.APIModels
{
    public class City : IndetifiedModel
    {
        public string Name { get; set; }
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
}
