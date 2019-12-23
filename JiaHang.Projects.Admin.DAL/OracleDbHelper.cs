using JiaHang.Projects.Admin.Common;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace JiaHang.Projects.Admin.DAL
{
    public class OracleDbHelper
    {
        //const string connectionString = @"DATA SOURCE=120.79.207.87:1521/orcl;PASSWORD=123456;PERSIST SECURITY INFO=True;USER ID=gao_ming";
        static string connectionString = AppConfig.connectionstring("OracleConnection");

        public static List<T> Query<T>(string cmdstr, List<SqlParameter> parameters = null)
        {
            OracleConnection conn = new OracleConnection(connectionString);
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(cmdstr, conn);
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var par in parameters)
                    {
                        cmd.Parameters.Add(par);
                    }
                }
                OracleDataReader datareader = cmd.ExecuteReader();
                if(!datareader.HasRows)return new List<T>();
                DataTable dataTable = new DataTable();
                dataTable.Load(datareader);
                datareader.Close();
                return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dataTable));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return new List<T>();
        }
        public static DataTable Query(string cmdstr, List<SqlParameter> parameters = null)
        {
            OracleConnection conn = new OracleConnection(connectionString);
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(cmdstr, conn);
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var par in parameters)
                    {
                        cmd.Parameters.Add(par);
                    }
                }
                OracleDataReader datareader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                if (!datareader.HasRows) {
                    return dataTable;
                }
                dataTable.Load(datareader);
                datareader.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return new DataTable();
        }
        
        public static int ExcuteSql(string cmdstr, List<SqlParameter> parameters = null)
        {
            int rows = 0;
            OracleConnection conn = new OracleConnection(connectionString);
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(cmdstr, conn);
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var par in parameters)
                    {
                        cmd.Parameters.Add(par);
                    }
                }
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }
    }
}
