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
            double orderTotal = 0.0;

            foreach(ItemLine watch in order.Watches)
            {
                orderTotal = watch.Quantity * watch.Model.Price;
            }

            // Insert order into db
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string cmdText = $"INSERT INTO dbo.orders(CustomerId, OrderDate, OrderTotal) VALUES(@customerId, @orderDate, @orderTotal);"; // INSERT command

            using SqlCommand insertCmd = new SqlCommand(cmdText, connection);
            insertCmd.Parameters.AddWithValue("@customerId", order.User.UserId);
            insertCmd.Parameters.AddWithValue("@orderDate", DateTime.Now);
            insertCmd.Parameters.AddWithValue("@orderTotal", orderTotal);

            try { await insertCmd.ExecuteNonQueryAsync(); } catch (Exception e) { _logger.LogError(e, e.Message); }

            // check to see order was inserted into db
            Guid orderId = Guid.Empty;

            cmdText = $"SELECT OrderId FROM dbo.orders WHERE CustomerId=@customerId;"; // SELECT order

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
                    cmdText = $"INSERT INTO dbo.itemLine(OrderId, WatchId, Quantity) VALUES (@orderId, @watchId, @quantity);";

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

            Guid userId = Guid.Empty;
            List<Guid> orderIds = new List<Guid>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Get user Id
            string cmdText = "SELECT UserId FROM dbo.users WHERE UserEmail=@email;";
            using SqlCommand cmd1 = new SqlCommand(cmdText, connection);
            cmd1.Parameters.AddWithValue("@email", email);
            using SqlDataReader reader1 = await cmd1.ExecuteReaderAsync();

            while (await reader1.ReadAsync())
            {
                userId = reader1.GetGuid(0);
            }

            // Getting orders
            cmdText = $"SELECT OrderId FROM dbo.orders WHERE CustomerId=@userId;";

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
                DateTime orderDate = DateTime.MinValue;
                double orderTotal = 0.0;

                // Get Order info
                cmdText = $"SELECT OrderDate, OrderTotal FROM orders WHERE OrderId=@orderId";

                using SqlCommand orderCmd = new SqlCommand(cmdText, connection);
                orderCmd.Parameters.AddWithValue("@orderId", orderId);
                using SqlDataReader orderReader = await orderCmd.ExecuteReaderAsync();

                while (await orderReader.ReadAsync())
                {
                    orderDate = orderReader.GetDateTime(0);
                    orderTotal = orderReader.GetDouble(1);
                }

                // Get ItemLine info
                cmdText = $"SELECT OrderId, WatchId, Quantity FROM itemLine WHERE OrderId=@orderId";

                using SqlCommand lineCmd = new SqlCommand(cmdText, connection);
                lineCmd.Parameters.AddWithValue("@orderId", orderId);
                using SqlDataReader lineReader = await lineCmd.ExecuteReaderAsync();

                while (await lineReader.ReadAsync())
                {
                    Guid newOrderId = lineReader.GetGuid(0);
                    Guid watchId = lineReader.GetGuid(1);
                    int quantity = lineReader.GetInt32(2);

                    ItemLine tmpLine = new ItemLine(newOrderId, watchId, quantity);

                    itemLines.Add(tmpLine);
                }

                // get list of watches per order
                foreach (var item in itemLines)
                {
                    cmdText = $"SELECT WatchId, WatchBrand, WatchName, Price FROM dbo.watches WHERE WatchId=@watchId";
                    using SqlCommand watchCmd = new SqlCommand(cmdText, connection);
                    watchCmd.Parameters.AddWithValue("@watchId", item.WatchId);
                    using SqlDataReader watchReader = await watchCmd.ExecuteReaderAsync();

                    while (watchReader.Read())
                    {
                        Guid watchId = watchReader.GetGuid(0);
                        string brand = watchReader.GetString(1);
                        string name = watchReader.GetString(2);
                        double price = watchReader.GetDouble(3);

                        Watch tempWatch = new Watch(watchId, brand, name, price);
                        item.Model = tempWatch;
                    }
                }

                orders.Add(new Order(orderId, userId, orderDate, orderTotal, itemLines));
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
            string cmdText = $"SELECT UserId, FirstName, LastName, UserEmail, Address, ZipCode, [State] FROM dbo.user WHERE UserId=@userId;";
            using SqlCommand userCmd = new SqlCommand(cmdText, connection);
            userCmd.Parameters.AddWithValue("@userId", userId);
            using SqlDataReader userReader = await userCmd.ExecuteReaderAsync();

            while (await userReader.ReadAsync())
            {
                // TODO: add mapper?
                user.UserId = userReader.GetGuid(0);
                user.FirstName = userReader.GetString(1);
                user.LastName = userReader.GetString(2);
                user.EmailAddress = userReader.GetString(3);
                user.Address = userReader.GetString(4);
                user.ZipCode = userReader.GetString(5);
                user.State = userReader.GetString(6);
            }

            // Get list of watches in the order
            cmdText = $"SELECT OrderId, WatchID, Quantity FROM dbo.itemLine WHERE OrderId=@orderId;";

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
                cmdText = $"SELECT WatchId, WatchBrand, WatchName, Price FROM dbo.watches WHERE WatchId=@watchId";
                using SqlCommand watchCmd = new SqlCommand(cmdText, connection);
                watchCmd.Parameters.AddWithValue("@watchId", watch.WatchId);
                using SqlDataReader watchReader = await watchCmd.ExecuteReaderAsync();

                while (watchReader.Read())
                {
                    Guid watchId = watchReader.GetGuid(0);
                    string brand = watchReader.GetString(1);
                    string name = watchReader.GetString(2);
                    double price = watchReader.GetDouble(3);

                    Watch tempWatch = new Watch(watchId, brand, name, price);
                    watch.Model = tempWatch;
                }
            }

            OrderWrapper order = new OrderWrapper(user, watches);

            return order;
        }
    }
}