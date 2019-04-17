using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasPrice.Data.Models
{
    public class EthGasPriceDTOModel
    {
        public double fast { get; set; }
        public double fastest { get; set; }
        public double safeLow { get; set; }
        public double average { get; set; }
        public double block_time { get; set; }
        public int blockNum { get; set; }
        public double speed { get; set; }
        public double safeLowWait { get; set; }
        public double avgWait { get; set; }
        public double fastWait { get; set; }
        public double fastestWait { get; set; }
    }
}
