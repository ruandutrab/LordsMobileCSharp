using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LordsMobile
{
    public class TroopsTier1 
    {
        public int InfT1 { get; set; }
        public int ArchT1 { get; set; }
        public int CavT1 { get; set; }
        public int BalliT1 { get; set; }
    }
    public class Troops
    {

        private State state;

        public Troops()
        {

        }

        

        //private void getAccount()
        //{
        //    if (DB.read("SELECT account FROM accounts WHERE account = '" + this.accountNo + "'").HasRows)
        //        return;
        //    else
        //        addAccount();
        //}
        public TroopsTier1 GetTroops(string account)
        {
            account = "ruandutrab"; // Mock

            using (var conn = new SQLiteConnection("Data Source=MaggotBot.sqlite"))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        infT1,
                        archT1,
                        cavT1,
                        balliT1
                    FROM
                        troopsAvailable
                    WHERE
                        account = @account;
                    ";

                using (var command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@account", account);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Lê a primeira linha (se houver)
                        {
                            return new TroopsTier1
                            {
                                InfT1 = reader.GetInt32(reader.GetOrdinal("infT1")),
                                ArchT1 = reader.GetInt32(reader.GetOrdinal("archT1")),
                                CavT1 = reader.GetInt32(reader.GetOrdinal("cavT1")),
                                BalliT1 = reader.GetInt32(reader.GetOrdinal("balliT1"))
                            };
                        }
                    }
                }

                return null;
            }
        }

        public void AddTroops(string infT1, string archT1, string cavT1, string balliT1)
        {
            string account = "ruandutrab"; // Mock

            using (var conn = new SQLiteConnection("Data Source=MaggotBot.sqlite"))
            {
                conn.Open(); // IMPORTANTE: abrir a conexão

                string query = @"
                    INSERT INTO troopsAvailable (
                        account,
                        infT1, 
                        archT1, 
                        cavT1, 
                        balliT1
                    ) VALUES (
                        @account,
                        @infT1, 
                        @archT1, 
                        @cavT1, 
                        @balliT1
                    );";

                using (var command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@account", account);
                    command.Parameters.AddWithValue("@infT1", ParseIntOrDefault(infT1));
                    command.Parameters.AddWithValue("@archT1", ParseIntOrDefault(archT1));
                    command.Parameters.AddWithValue("@cavT1", ParseIntOrDefault(cavT1));
                    command.Parameters.AddWithValue("@balliT1", ParseIntOrDefault(balliT1));

                    command.ExecuteNonQuery();
                }
            }
        }

        public int ParseIntOrDefault(string input)
        {
            return int.TryParse(input, out int value) ? value : 0;
        }

        public string GetMinTroopType(TroopsTier1 troops)
        {
            var troopValues = new Dictionary<string, int>
            {
                { "InfT1", troops.InfT1 },
                { "ArchT1", troops.ArchT1 },
                { "CavT1", troops.CavT1 },
                { "BalliT1", troops.BalliT1 }
            };

            return troopValues.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
        }

    }

}
