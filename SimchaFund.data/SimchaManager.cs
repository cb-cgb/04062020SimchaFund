using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;


namespace SimchaFund.data
{
    public class SimchaManager
    {

        private string _conn;

        public SimchaManager(string ConnectionString)
        {
            _conn = ConnectionString;
        }

        public List<Simcha> GetSimchos()
        {
            List<Simcha> simchos = new List<Simcha>();

            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                //query excludes id =-1 as that is the placeholder simchaid for deposits, used in the transaction table, simchaId field
                cmd.CommandText = @"select s.*, isnull(c.ContrCount,0)contrCount, isnull(c.totalAmt,0)totalAmt
                                    from simchos s
                                    left outer join 
                                    (select simchaId,sum(Amount)totalAmt, count(distinct ContrId)contrCount
                                     from Transactions group by simchaId)c 
                                     on s.id = c.simchaId
                                    where id > 0";
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    simchos.Add(new Simcha
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Date = (DateTime)reader["Date"],
                        TotalContributions = (Decimal)reader["totalAmt"],
                        ContributorCount = (int)reader["contrCount"]

                    }); ;
                }
            }

            return simchos;
        }


        public void AddSimcha(Simcha s)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"Insert into Simchos
                                    Select @name, @date";
                cmd.Parameters.AddWithValue("@name", s.Name);
                cmd.Parameters.AddWithValue("@date", s.Date);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int GetContrCount()
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select isnull(count(*), 0)contrCount from Contributor";
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public List<Contributor> GetContributors()
        {
            List<Contributor> contr = new List<Contributor>();
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select c.*,
                                  (select isnull(sum(amount),0) from Transactions where contrId= c.Id)Balance    from Contributor c";

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contr.Add(new Contributor
                    {
                        Id = (int)reader["Id"],
                        First = (String)reader["First"],
                        Last = (String)reader["Last"],
                        Phone = (string)reader["Phone"],
                        Balance = (Decimal)reader["Balance"],
                        AlwaysContribute = (bool)reader["AlwaysContribute"],
                        DateCreated = (DateTime)reader["DateAdded"]

                    });

                }
            }
            return contr;
        }

        public Contributor GetContributorById(int contrId)
        {
            Contributor c = new Contributor();
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select c.*,
                                  (select isnull(sum(amount),0) from Transactions where contrId= c.Id)Balance    from Contributor c
                                   where Id = @contrId";

                cmd.Parameters.AddWithValue("@contrid", contrId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c = new Contributor
                    {
                        Id = (int)reader["Id"],
                        First = (String)reader["First"],
                        Last = (String)reader["Last"],
                        Phone = (string)reader["Phone"],
                        Balance = (Decimal)reader["Balance"],
                        AlwaysContribute = (bool)reader["AlwaysContribute"],
                        DateCreated = (DateTime)reader["DateAdded"]

                    };
                }
            }
            return c;
        }

        public void AddContributor(Contributor c)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"Insert into Contributor (first, last,phone,alwayscontribute)
                                    Select @first, @last,@phone,@alwayscontribute select scope_identity()";
                cmd.Parameters.AddWithValue("@first", c.First);
                cmd.Parameters.AddWithValue("@last", c.Last);
                cmd.Parameters.AddWithValue("@phone", c.Phone);
                cmd.Parameters.AddWithValue("@alwayscontribute", c.AlwaysContribute);

                conn.Open();
                c.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public void UpdateContributor(Contributor c)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"Update Contributor
                                    set first= @first, 
                                        last =@last,
                                        phone = @phone,
                                        alwaysContribute=@alwayscontribute
                                    where Id = @contrId";
                cmd.Parameters.AddWithValue("@first", c.First);
                cmd.Parameters.AddWithValue("@last", c.Last);
                cmd.Parameters.AddWithValue("@phone", c.Phone);
                cmd.Parameters.AddWithValue("@alwayscontribute", c.AlwaysContribute);
                cmd.Parameters.AddWithValue("@contrId", c.Id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public decimal GetTotalBalance()
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"Select isnull(sum(amount),0) from Transactions";
                conn.Open();
                return (decimal)cmd.ExecuteScalar();
            }
        }

        public decimal GetBalance(int contrId)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"Select isnull(sum(amount),0) from Transactions where contrId = @contrid";
                cmd.Parameters.AddWithValue("@contrid", contrId);
                conn.Open();
                return (decimal)cmd.ExecuteScalar();
            }
        }

        public void AddTransaction(Transaction t)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"Insert into Transactions 
                                    Select @simchaId,@contrId ,@amount,@transType,@date";
                cmd.Parameters.AddWithValue("@simchaId", t.SimchaId);
                cmd.Parameters.AddWithValue("@contrId", t.ContrId);
                cmd.Parameters.AddWithValue("@amount", t.Amount);
                cmd.Parameters.AddWithValue("@transType", t.TranType);
                cmd.Parameters.AddWithValue("@date", t.Date);

                conn.Open();
                cmd.ExecuteNonQuery();

            }

        }

        public List<Transaction> GetTransactions(int contrId)
        {
            List<Transaction> t = new List<Transaction>();
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = @"select t.transId,t.contrId, t.simchaId, t.amount, t.Date, 

                                   s.name simchaName, c.First + ' ' + c.Last contrName ,
                                   CASE 
                                        when transType = 'Contribution' then 'Contribution for ' + s.name + ' simcha' 
	                                    else transType 
                                    END as transType                                    
                                    from Transactions t, Simchos s, Contributor c      
                                    where t.contrId = c.Id and t.simchaId = s.Id
                                    and c.id = @contrId";
                cmd.Parameters.AddWithValue("contrId", contrId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    t.Add(new Transaction
                    {
                        TransId = (int)reader["transId"],
                        ContrId = (int)reader["contrId"],
                        SimchaId = (int)reader["simchaId"],
                        Amount = (decimal)reader["amount"],
                        Date = (DateTime)reader["date"],
                        TranType = (String)reader["transType"],
                        ContrName = (String)reader["contrName"],
                        SimchaName = (String)reader["simchaname"]
                    });
                }
            }

            return t;
        }

        public List<Contribution> GetContributions(int simchaId)
        {
            List<Contribution> contr = new List<Contribution>();
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                //pull all the contribuors and indicate if they contributed to the simcha we are looking at
                cmd.CommandText = @"select c.Id contrId, c.First + ' ' + c.Last as contrName, 
                                    c.alwaysContribute, transDate, tr.amount, tr.simchaName , tr.simchaId
                                    
									 
                                 from Contributor c                                       
                                  left outer join 
                                       (select t.contrId,t.simchaId,s.Name simchaName, t.Date transDate, t.amount
					                    from Transactions t, simchos s, contributor contr 
					                    where  t.simchaId = s.Id and t.contrId  = contr.Id					  
                                        and t.transType = 'Contribution'
					                    and t.simchaId = @simchaId
					                    ) tr
                                   on c.Id = tr.contrId";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contr.Add(new Contribution
                    {
                        ContrId = (int)reader["contrId"],
                        ContrName = (String)reader["contrName"],
                        AlwaysInclude = (bool)reader["alwaysContribute"],
                        SimchaName = reader.GetOrNull<String>("simchaName"),
                        SimchaId = reader.GetOrNull<int>("simchaId"),
                        Amount = reader.GetOrNull<Decimal>("amount"),
                        Date = reader.GetOrNull<DateTime>("transDate"),

                    });

                    foreach (Contribution c in contr)
                    {
                        c.Balance = GetBalance(c.ContrId);
                    }
                }
            }
            return contr;
        }

      
        public void UpdateContribution(List<Contribution> contributions, int simchaId)
        {

            String cmdInsert = @"insert into Transactions (contrId, simchaId, amount, transtype)
                                            select @contrId, @simchaId, @amount,   'Contribution'";

            String cmdDelete = @"delete from Transactions where transtype = 'Contribution' and simchaId   = @simchaId";


            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = cmdDelete;// delete all existing contributions for this simcha

                cmd.Parameters.AddWithValue("simchaId", @simchaId);

                conn.Open();
                cmd.ExecuteNonQuery();

                foreach (Contribution c in contributions.Where(c => c.IsContributor)) //for any that are marked as contributing, insert a contribution.
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = cmdInsert;
                    cmd.Parameters.AddWithValue("contrId", c.ContrId);
                    cmd.Parameters.AddWithValue("simchaId", c.SimchaId);
                    cmd.Parameters.AddWithValue("amount", c.Amount);

                    cmd.ExecuteNonQuery();

                }

            }
        }
    }


    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string column)
        {
            object obj = reader[column];
            if (obj == DBNull.Value)
            {
                return default(T);
            }
            return (T)obj;
        }
    }
}