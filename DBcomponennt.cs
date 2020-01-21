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
        public static readonly string DBConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|/システム開発演習_KT-22.accdb;Persist Security Info=True;Jet OLEDB:Database Password=admin";

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
                        var item = dataReader.GetValue(k);
                        if(!(item is string))
                        {
                            item = item.ToString();
                        }
                        result[i].Add(dataReader.GetName(k), (string)item);
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


        // トランザクション用
        public bool ExecQueryForTransaction(List<string> sqls, List<Dictionary<string, string>> parameters)
        {
            OleDbConnection oleConnection = new OleDbConnection(DBConnectionString);

            try
            {
                oleConnection.Open();

                OleDbTransaction transaction = oleConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                OleDbCommand oleCommand = new OleDbCommand()
                {
                    Connection  = oleConnection,
                    Transaction = transaction
                };

                try
                {
                    for(int i = 0;i < sqls.Count; i++)
                    {
                        string sql = sqls[i];
                        Dictionary<string, string> parameter = parameters[i];

                        oleCommand.CommandText = sql;
                        foreach (KeyValuePair<string, string> param in parameter)
                        {
                            oleCommand.Parameters.AddWithValue(param.Key, param.Value);
                        }
                        oleCommand.ExecuteNonQuery();
                        oleCommand.Parameters.Clear();
                    }
                    transaction.Commit();
                }
                catch(Exception exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                return false;
            }
            finally
            {
                oleConnection.Close();
            }

            return true;
        }
    }
}
