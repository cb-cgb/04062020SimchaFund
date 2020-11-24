using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.data;

namespace _04062020SimchaFund.Models
{
    public class ContributionViewModel
    {

        public List<Contribution>Contributions { get; set; }
        public String SimchaName { get; set; }
        public int SimchaId { get; set; }
        public String Message { get; set; }
        

    }
}
