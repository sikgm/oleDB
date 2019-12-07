using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace sales_management_system
{
    public class DBcomponent
    {
        public static readonly string DBConnectionString = "hogehoge";

        // SELECT すべての行読み取り
        public List<Dictionary<string, string>> ExecSelectQuery(string sql, Dictionary<string,string> parameter)
        {
            OleDbConnection oleConnection = new OleDbConnection(DBConnectionString);
            OleDbCommand oleCommand = new OleDbCommand(sql, oleConnection);

            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            try
            {
                oleConnection.Open();

                foreach (KeyValuePair<string, string> param in parameter)
                {
                    oleCommand.Parameters.AddWithValue(param.Key, param.Value);
                }

                OleDbDataReader dataReader = oleCommand.ExecuteReader();
                for (int i = 0; dataReader.Read() == true; i++)
                {
                    result.Add(new Dictionary<string, string>());
                    for (int k = 0; k < dataReader.FieldCount; k++)
                    {
                        result[i].Add(dataReader.GetName(k), (string)dataReader.GetValue(k));
                    }
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }
            finally
            {
                oleConnection.Close();
            }
            return result;

        }

        // dataGridViewのデータ取得
        public DataTable FetcDGVData(string sql, Dictionary<string, string> parameter)
        {
            OleDbConnection oleConnection = new OleDbConnection(DBConnectionString);
            OleDbCommand oleCommand = new OleDbCommand(sql, oleConnection);

            DataTable dataTable = new DataTable();

            try
            {
                oleConnection.Open();

                foreach (KeyValuePair<string, string> param in parameter)
                {
                    oleCommand.Parameters.AddWithValue(param.Key, param.Value);
                }

                OleDbDataReader dataReader = oleCommand.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }
            finally
            {
                oleConnection.Close();
            }
            return dataTable;
        }

        // INSERT UPDATE DELETE 用
        public int ExecQueryForChangeTable(string sql, Dictionary<string, string> parameter)
        {
            OleDbConnection oleConnection = new OleDbConnection(DBConnectionString);
            OleDbCommand oleCommand = new OleDbCommand(sql, oleConnection);

            int rows = 0;

            try
            {
                oleConnection.Open();

                foreach (KeyValuePair<string, string> param in parameter)
                {
                    oleCommand.Parameters.AddWithValue(param.Key, param.Value);
                }

                rows = oleCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }
            finally
            {
                oleConnection.Close();
            }
            return rows;
        }
    }
}
