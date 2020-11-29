using System;

namespace SimchaFund.data
{
    public class Simcha
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime Date { get; set; }
        public Decimal TotalContributions { get; internal set; }
        public int ContributorCount { get; internal set; }
    }

    public class Contributor
    {
        public int Id { get; set; }
        public String First { get; set; }
        public String Last { get; set; }
        public String Name => First + ' ' + Last;
         public  String Phone { get; set; }
        public Decimal Balance { get; set; }
        public  bool AlwaysContribute { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class Transaction
    {
        public int TransId { get; set; }
        public int SimchaId { get; set; }
        public int ContrId { get; set; }
        public Decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public String TranType { get; set; }
        public String SimchaName { get; set; }
        public String ContrName { get; set; }
    }

    public class Contribution
    {
        
        public int ContrId { get; set; }
        public int SimchaId { get; set; }
        public String ContrName { get; set; }
        public String SimchaName { get; set; }
        public Decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool AlwaysInclude { get; set; }
        public Decimal Balance { get; set; }
        //public bool IsContributor => ContrId != 0 && SimchaId != 0;
        public bool IsContributor { get; set; }
    }
}
