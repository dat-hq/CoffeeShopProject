using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DAO
{
    public class DataProvider
    {
        private static DataProvider instance; 

        private string connectionSTR =
             @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set => instance = value;
        }

        internal DataTable ExecuteQuery()
        {
            throw new NotImplementedException();
        }

        private DataProvider()
        { }

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach(string items in listPara)
                    {
                        if (items.Contains('@'))
                        {
                            command.Parameters.AddWithValue(items, parameter[i]);
                        }
                    }
                }


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            };
            return data;
        }

        /*public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    //@idBill , @idFood , @count
                
                    command.Parameters.Add(new SqlParameter("@idBill", parameter[0]));
                    command.Parameters.Add(new SqlParameter("@idFood", parameter[1]));
                    command.Parameters.Add(new SqlParameter("@count", parameter[2]));
                    //string[] listPara = query.Split(' ');
                    //int i = 0;
                    //foreach (string items in listPara)
                    //{
                    //    if (items.Contains('@'))
                    //    {
                    //        command.Parameters.AddWithValue(items, parameter[i]);
                    i++;
                    //    }
                    //}
                }

                data = command.ExecuteNonQuery();
                connection.Close();
            };
            return data;
        }
        */

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }

            return data;
        }
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string items in listPara)
                    {
                        if (items.Contains('@'))
                        {
                            command.Parameters.AddWithValue(items, parameter[i]);
                        }
                    }
                }
                data = command.ExecuteScalar();
                connection.Close();
            };
            return data;
        }
    }
}
