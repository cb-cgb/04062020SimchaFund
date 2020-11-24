using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.data;

namespace _04062020SimchaFund.Models
{
    public class TransViewModel
    {

        public List<Transaction>Transactions { get; set; }
        public String  Name  { get; set; }
        public Decimal Balance { get; set; }
        

    }
}
