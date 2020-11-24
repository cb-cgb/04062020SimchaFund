using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.data;

namespace _04062020SimchaFund.Models
{
    public class ContribViewModel
    {

        public List<Contributor>Contributors { get; set; }
        public Decimal TotalBalance { get; set; }
        public String Message { get; set; }
        

    }
}
