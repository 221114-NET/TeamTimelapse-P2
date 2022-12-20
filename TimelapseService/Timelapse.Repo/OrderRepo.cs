using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Timelapse.Models;

namespace Timelapse.Repo
{
    public class OrderRepo : IOrderRepo
    {
        private readonly string _connectionString;
        private readonly ILogger<OrderRepo> _logger;

        public OrderRepo(string connectionString, ILogger<OrderRepo> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task PostOrder(OrderWrapper order)
        {
            // Insert order into db
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string cmdText = $"INSERT INTO dbo.Orders(customerId) VALUES(@customerId);"; // INSERT command

            using SqlCommand insertCmd = new SqlCommand(cmdText, connection);
            insertCmd.Parameters.AddWithValue("@customerId", order.User.UserId);

            try { await insertCmd.ExecuteNonQueryAsync(); } catch (Exception e) { _logger.LogError(e, e.Message); }

            // check to see order was inserted into db
            Guid orderId = Guid.Empty;

            cmdText = $"SELECT orderId FROM dbo.Orders WHERE customerId = @customerId;"; // SELECT order

            using SqlCommand selectCmd = new SqlCommand(cmdText, connection);
            selectCmd.Parameters.AddWithValue("@customerId", order.User.UserId);

            using SqlDataReader read = await selectCmd.ExecuteReaderAsync();

            while (await read.ReadAsync())
            {
                orderId = read.GetGuid(0);
            }

            if (orderId != Guid.Empty)
            {
                foreach (ItemLine watch in order.Watches)
                {
                    cmdText = $"INSERT INTO dbo.ItemLine(orderId, watchId, quantity VALUES (@orderId, @watchId, @quantity);";

                    using SqlCommand cmd = new SqlCommand(cmdText, connection);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@watchId", watch.WatchId);
                    cmd.Parameters.AddWithValue("@quantity", watch.Quantity);

                    try { await cmd.ExecuteNonQueryAsync(); } catch (Exception e) { _logger.LogError(e, e.Message); }
                }
            }
            
            await connection.CloseAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders(string email)
        {
            List<Order> orders = new List<Order>();

            Guid userId = new Guid();
            List<Guid> orderIds = new List<Guid>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Get user Id
            string cmdText = "SELECT userId FROM dbo.Users WHERE email = @email;";
            using SqlCommand cmd1 = new SqlCommand(cmdText, connection);
            cmd1.Parameters.AddWithValue("@email", email);
            using SqlDataReader reader1 = await cmd1.ExecuteReaderAsync();

            while (await reader1.ReadAsync())
            {
                userId = reader1.GetGuid(0);
            }

            // Getting orders
            cmdText = $"SELECT orderId FROM Orders WHERE customerId=@userId;";

            using SqlCommand cmd2 = new SqlCommand(cmdText, connection);
            cmd2.Parameters.AddWithValue("@userId", userId);
            using SqlDataReader reader2 = await cmd2.ExecuteReaderAsync();

            while (await reader2.ReadAsync())
            {
                Guid orderId = reader2.GetGuid(0);
                orderIds.Add(orderId);
            }

            _logger.LogInformation(orderIds.ToString());

            // Getting watches per order, to create full Order
            foreach (Guid orderId in orderIds)
            {
                List<ItemLine> itemLines = new List<ItemLine>();
                cmdText = $"SELECT watchId, quantity FROM ItemLine WHERE orderId=@orderId";

                using SqlCommand cmd = new(cmdText, connection);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Guid watchId = reader.GetGuid(0);
                    int quantity = reader.GetInt32(1);

                    ItemLine tmpLine = new ItemLine(orderId, watchId, quantity);

                    itemLines.Add(tmpLine);
                }

                // get list of watches per order
                foreach (var item in itemLines)
                {
                    cmdText = $"SELECT watchId, watchBrand, watchName FROM dbo.Watches WHERE watchId=@watchId";
                    using SqlCommand watchCmd = new SqlCommand(cmdText, connection);
                    watchCmd.Parameters.AddWithValue("@watchId", item.WatchId);
                    using SqlDataReader watchReader = await watchCmd.ExecuteReaderAsync();

                    while (watchReader.Read())
                    {
                        Guid watchId = watchReader.GetGuid(0);
                        string brand = watchReader.GetString(1);
                        string name = watchReader.GetString(1);

                        Watch tempWatch = new Watch(watchId, brand, name);
                        item.Model = tempWatch;
                    }
                }

                orders.Add(new Order(orderId, userId, itemLines));
            }

            await connection.CloseAsync();
            _logger.LogInformation("Executed GetOrder");

            return orders;
        }

        public async Task<OrderWrapper> GetOrderById(Guid userId, Guid orderId)
        {
            List<ItemLine> watches = new List<ItemLine>();
            User user = new User();

            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Get user info
            string cmdText = $"SELECT userId, firstName, lastName, emailAddress, address1, zipCode, state FROM dbo.User WHERE userId=@userId;";
            using SqlCommand userCmd = new SqlCommand(cmdText, connection);
            userCmd.Parameters.AddWithValue("@userId", userId);
            using SqlDataReader userReader = await userCmd.ExecuteReaderAsync();

            while (await userReader.ReadAsync())
            {
                // TODO: add mapper
                user.UserId = userReader.GetGuid(0);
                user.FirstName = userReader.GetString(1);
                user.LastName = userReader.GetString(2);
                user.EmailAddress = userReader.GetString(3);
                user.Address1 = userReader.GetString(4);
                user.ZipCode = userReader.GetInt32(5);
                user.State = userReader.GetString(6);
            }

            // Get list of watches in the order
            cmdText = $"SELECT orderId, watchID, quantity FROM dbo.ItemLine WHERE orderId=@orderId;";

            using SqlCommand orderCmd = new SqlCommand(cmdText, connection);
            orderCmd.Parameters.AddWithValue("@orderId", orderId);
            using SqlDataReader orderReader = await orderCmd.ExecuteReaderAsync();

            while (await orderReader.ReadAsync())
            {
                Guid tempId = orderReader.GetGuid(0);
                Guid watchId = orderReader.GetGuid(1);
                int quantity = orderReader.GetInt32(2);

                ItemLine tmpLine = new ItemLine(orderId, watchId, quantity);

                watches.Add(tmpLine);
            }

            // get list of watches per order
            foreach (var watch in watches)
            {
                cmdText = $"SELECT watchId, watchBrand, watchName FROM dbo.Watches WHERE watchId=@watchId";
                using SqlCommand watchCmd = new SqlCommand(cmdText, connection);
                watchCmd.Parameters.AddWithValue("@watchId", watch.WatchId);
                using SqlDataReader watchReader = await watchCmd.ExecuteReaderAsync();

                while (watchReader.Read())
                {
                    Guid watchId = watchReader.GetGuid(0);
                    string brand = watchReader.GetString(1);
                    string name = watchReader.GetString(1);

                    Watch tempWatch = new Watch(watchId, brand, name);
                    watch.Model = tempWatch;
                }
            }

            OrderWrapper order = new OrderWrapper(user, watches);

            return order;
        }
    }
}