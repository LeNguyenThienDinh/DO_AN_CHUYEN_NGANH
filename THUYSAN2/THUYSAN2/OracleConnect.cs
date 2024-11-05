using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace THUYSAN2
{
    internal class OracleConnect
    {
        private string connectionString;
        private OracleConnection _connection;

        public OracleConnect()
        {
            connectionString = "User Id=VUNGNUOI10;Password=vungnuoi10;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=VUNGNUOI10)))";
            _connection = new OracleConnection(connectionString);
        }

        public OracleConnection Connection
        {
            get { return _connection; }
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public DataTable ExecuteQuery(string query, params OracleParameter[] parameters)
        {
            OpenConnection(); // Đảm bảo kết nối được mở

            try
            {
                using (OracleCommand command = new OracleCommand(query, _connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    OracleDataAdapter adapter = new OracleDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                CloseConnection(); // Đóng kết nối
            }
        }

        public int ExecuteProcedureWithOutput(string procedureName, OracleParameter[] parameters)
        {
            OpenConnection();

            try
            {
                using (OracleCommand command = new OracleCommand(procedureName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.ExecuteNonQuery();

                    int outputValue = Convert.ToInt32(parameters[parameters.Length - 1].Value);
                    return outputValue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetDataFromProcedure(string procedureName, params OracleParameter[] parameters)
        {
            OpenConnection();

            try
            {
                using (OracleCommand command = new OracleCommand(procedureName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    OracleDataAdapter adapter = new OracleDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
