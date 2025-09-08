// using System;
// using System.Data;
// using Microsoft.Data;
// using Microsoft.Data.SqlClient;

// namespace adodemo1
// {

//     // to get the connection 
//     class Program
//     {
//         static void Main(string[] args)
//         {
//             // string connectionString =
//             //  "Data Source=LAPTOP-QTMJBBT0;Initial Catalog=WiproJuly2025;Encrypt=False;Integrated Security=True";

//             // using (SqlConnection connection = new SqlConnection(connectionString))
//             // {
//             //     connection.Open();
//             //     Console.WriteLine("Connection opened successfully.");

//             //     connection.Close();
//             //     Console.WriteLine("Connection closed.");
//             // }

//             // demo for sqlcommand
//             // string connectionString =
//             //   "Data Source=LAPTOP-QTMJBBT0;Initial Catalog=WiproJuly2025;Encrypt=False;Integrated Security=True";

//             // using (SqlConnection connection = new SqlConnection(connectionString))
//             // {
//             //     connection.Open();
//             //     SqlCommand cmd = new SqlCommand
//             //     ("select * from Employee  where empid>1002 order by empname", connection);

//             //     SqlDataReader reader = cmd.ExecuteReader();
//             //     while (reader.Read())
//             //     {
//             //         Console.WriteLine($"ID: {reader["empid"]}, Name: {reader["city"]}, Salary: {reader["Salary"]}");
//             //     }

//             // }


//             // sqlcommand demo
//             // string connectionString =
//             //   "Data Source=LAPTOP-QTMJBBT0;Initial Catalog=WiproJuly2025;Encrypt=False;Integrated Security=True";
//             // using (SqlConnection connection = new SqlConnection(connectionString))
//             // {
//             //     connection.Open();
//             //     SqlCommand cmd = new SqlCommand
//             //     ("insert into Employee values(1005,'Ravi',50000,'Bangalore')", connection);

//             //     int rowsAffected = cmd.ExecuteNonQuery();
//             //     Console.WriteLine($"{rowsAffected} row(s) inserted successfully.");
//             // }


//             // sqlcommand with multiple statements

//             // string connectionString =
//             // "Data Source=LAPTOP-QTMJBBT0;Initial Catalog=WiproJuly2025;Encrypt=False;Integrated Security=True";

//             // using (SqlConnection connection = new SqlConnection(connectionString))
//             // {
//             //     connection.Open();

//             //     string sql = @" insert into Employee values(1006,'Ravi','mysore',1000);
//             //                   update Employee set empname='Rajesh' where empid=1005);
//             //                   delete from employee where empid=1002; ";

//             //     using (SqlCommand command = new SqlCommand(sql, connection))
//             //     {
//             //         int affectedRows = command.ExecuteNonQuery();
//             //         Console.WriteLine($"{affectedRows} rows affected.");
//             //     }
//             // }

//             // sqldataadapter demo
//             // string connectionString =
//             //   "Data Source=LAPTOP-QTMJBBT0;Initial Catalog=WiproJuly2025;Encrypt=False;Integrated Security=True";

//             // using (SqlConnection connection = new SqlConnection(connectionString))
//             // {
//             //     connection.Open();

//             //     string sql = "select * from Employee";

//             //     using (SqlCommand command = new SqlCommand(sql, connection))
//             //     {
//             //         SqlDataAdapter adapter = new SqlDataAdapter(command);
//             //         DataSet ds = new DataSet();
//             //         // Fill the DataSet with data from the Employee table   
//             //         adapter.Fill(ds, "Employee");

//             //         // Display the data in the console
//             //         Console.WriteLine("Employee Data:");        
//             //         foreach (DataRow row in ds.Tables["Employee"].Rows)
//             //         {
//             //             Console.WriteLine($"ID: {row["empid"]}, Name: {row["empname"]}, Salary: {row["Salary"]}, City: {row["city"]}");
//             //         }   
//             //     }
//             // }

//             // dataset demo

//             DataTable t1 = new DataTable("customer");
//             t1.Columns.Add("custid", typeof(int));
//             t1.Columns.Add("custname", typeof(string));
//             t1.Columns.Add("custSalary", typeof(int));
//             t1.Rows.Add(101, "John", 5000);
//             t1.Rows.Add(102, "Jane", 6000);
//             t1.Rows.Add(103, "Doe", 7000);

//             DataTable t2 = new DataTable("orders");
//             t2.Columns.Add("orderid", typeof(int));
//             t2.Columns.Add("custid", typeof(int));
//             t2.Columns.Add("orderdate", typeof(DateTime));
//             t2.Rows.Add(1, 101, DateTime.Now);
//             t2.Rows.Add(2, 102, DateTime.Now.AddDays(-1));
//             t2.Rows.Add(3, 103, DateTime.Now.AddDays(-2));
//             t2.Rows.Add(4, 101, DateTime.Now.AddDays(-3));
//             t2.Rows.Add(5, 102, DateTime.Now.AddDays(-4));

//             DataSet ds = new DataSet();

//             ds.Tables.Add(t1);
//             ds.Tables.Add(t2);

// foreach (DataTable t in ds.Tables)
// {
//     Console.WriteLine($"--- Table: {t.TableName} ---");

//     // Print column headers
//     foreach (DataColumn column in t.Columns)
//     {
//         Console.Write($"{column.ColumnName}\t");
//     }
//     Console.WriteLine();

//     // Print each row
//     foreach (DataRow row in t.Rows)
//     {
//         foreach (var item in row.ItemArray)
//         {
//             Console.Write($"{item}\t");
//         }
//         Console.WriteLine();
//     }

//     Console.WriteLine(); // Add spacing between tables
// }


//         }
//     }
// }


