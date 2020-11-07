using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace SpidometrusTools
{
    public class PrepareDateForSelectOrderTable
    {
        DataTable result =new DataTable();
        private DataSet ds;
        MySqlDataAdapter adapter;
        //private MySqlCommandBuilder commandBuilder;
        static string connectionString = ConfigurationManager.ConnectionStrings["spidometrus-ru"].ConnectionString;
        string sql;
        
        
        public DataTable SelectDataFromDB()
        {
            sql = "SELECT virtuemart_order_id,order_number,order_total,order_status,virtuemart_paymentmethod_id FROM worksite_virtuemart_orders WHERE virtuemart_order_id>1500;";
            sql += "SELECT order_status_code, order_status_name FROM worksite_virtuemart_orderstates;";
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                adapter = new MySqlDataAdapter(sql, connection);

                ds = new DataSet();
                adapter.Fill(ds);
            }
            var tableOfOrderStates = new DataTable(); // таблица статусов
            result = ds.Tables[0];
            tableOfOrderStates = ds.Tables[1];
            
            //перебираем таблицу заказов и заменяем букву статуса на текст
            foreach (DataRow row in result.Rows)
            {
                foreach (DataRow statuses in tableOfOrderStates.Rows)
                {
                    if ((string) statuses[0] == (string) row[3])
                        row[3] = statuses[1];
                }
            }
            result.Columns.Add("paymentName", typeof(string));
            
            //перебираем методы оплаты и заменяем номер метода на текст
            foreach (DataRow row in result.Rows)
            {
                if ((uint) row[4] == 16 ||
                    (uint) row[4] == 17 ||
                    (uint) row[4] == 23 ||
                    (uint) row[4] == 24 ||
                    (uint) row[4] == 25 ||
                    (uint) row[4] == 26)
                    row[5] = "Предоплата";
                if ((uint) row[4] == 15 ||
                    (uint) row[4] == 21)
                    row[5] = "Наложка";
                if ((uint) row[4] == 22)
                    row[5] = "С описью";
            }
            result.Columns.RemoveAt(4);
            return result;
        }
    }
}