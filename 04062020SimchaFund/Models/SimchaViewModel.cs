using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.data;

namespace _04062020SimchaFund.Models
{
    public class SimchaViewModel
    {

        public List<Simcha>Simchos { get; set; }
        public int TotalContributorCount { get; set; }
        public string Message { get; set; }
        //public string SimchaName { get; set; }
        

    }
}
