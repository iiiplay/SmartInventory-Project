using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient; // 統一使用 MySQL 命名空間
using SmartInventory.Models;
namespace SmartInventory.Data
{
    public static class MySqlDbHelper
    {
        private static string? connStr;

        // 靜態建構函式：在類別第一次被使用時自動執行
        static MySqlDbHelper()
        {
            var builder = new ConfigurationBuilder()     
            .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            // 從設定檔讀取連線字串
            connStr = config.GetConnectionString("TiDB");
        }

        public static bool CheckAccount(string userName, string password)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                // 建議使用 SQL 語法進行驗證，確保只有帳號密碼都正確時才回傳 True
                string sql = "SELECT * FROM Accounts WHERE Name = @UserName AND Password = @Password";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password); // 注意：實務上密碼需加密比對
                                                                        // ExecuteScalar 回傳查詢結果的第一欄第一列

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }                   
                }
            }           
        }

        // 建立資料表 (修正語法)
        public static void InitDb()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                // 使用 MySQL 語法：AUTO_INCREMENT 與 DOUBLE
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        Id INT NOT NULL AUTO_INCREMENT,
                        Name VARCHAR(255),
                        Category VARCHAR(100),
                        Quantity INT,
                        Price DOUBLE,
                        PRIMARY KEY (Id)
                    );";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result);
                }
            }
        }

        // 新增資料
        public static void InsertProduct(Product p)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "INSERT INTO Products(Name, Category, Quantity, Price) VALUES(@Name, @Category, @Quantity, @Price);";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", p.Name);
                    cmd.Parameters.AddWithValue("@Category", p.Category);
                    cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                    cmd.Parameters.AddWithValue("@Price", (double)p.Price);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Product> GetAllProducts()
        {
            var result = new List<Product>();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM Products";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()!,
                                Category = reader["Category"].ToString()!,
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDecimal(reader["Price"])
                            });
                        }
                    }
                }
            }
            return result;
        }

        public static void DeleteProduct(Product p)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM Products WHERE Id = @Id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteAllProducts()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM Products";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProduct(Product p)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = @"UPDATE Products SET 
                               Name = @Name, 
                               Category = @Category, 
                               Quantity = @Quantity, 
                               Price = @Price 
                               WHERE Id = @Id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", p.Name);
                    cmd.Parameters.AddWithValue("@Category", p.Category);
                    cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                    cmd.Parameters.AddWithValue("@Price", (double)p.Price);
                    cmd.Parameters.AddWithValue("@Id", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}