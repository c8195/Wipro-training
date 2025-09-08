using System;
using System.Data;
using Microsoft.Data;
using Microsoft.Data.SqlClient;

namespace adodemo1
{

    // to get the connection 
    class Program
    {
        static void Main(string[] args)
        {
            // how to use stored procedure in ado.net concept
            string connectionString =
              "Data Source=LAPTOP-QTMJBBT0;Initial Catalog=WiproJuly2025;Encrypt=False;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                // demo for stored procedure with no parameters
                // SqlCommand cmd = new SqlCommand("dispallrecs_emp", connection);
                // cmd.CommandType = CommandType.StoredProcedure;

                // //with parameters
                // // cmd.Parameters.AddWithValue("@empid", 1002);

                // // without parameters
                // SqlDataReader reader = cmd.ExecuteReader();
                // while (reader.Read())
                // {
                //     Console.WriteLine($"ID: {reader["empid"]}, Name: {reader["empname"]}, Salary: {reader["Salary"]}, City: {reader["city"]}");
                // }


                // demo for stored procedure with parameters
                SqlCommand cmd = new SqlCommand("dispempbyid", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@eid", 1002);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["empid"]}, Name: {reader["empname"]}, Salary: {reader["Salary"]}, City: {reader["city"]}");
                }
                else
                {
                    Console.WriteLine("No record found for the given empid.");
                }
            }


         }
    }
}