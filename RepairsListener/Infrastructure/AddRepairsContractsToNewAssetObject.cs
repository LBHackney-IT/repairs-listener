using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairsListener.Infrastructure
{
    // The same class is also defined in Asset API to ensure the information sent and received is in the same format
    public class AddRepairsContractsToNewAssetObject
    {
        public Guid EntityId { get; set; }
        public string PropRef { get; set; }
        public bool AddRepairsContracts { get; set; }
    }
}
