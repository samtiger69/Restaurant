using Restaurant.Models;
using System;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    /// <summary>
    /// order delivery database manager
    /// </summary>
    public class OrderDeliveryService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile OrderDeliveryService _instance = null;

        /// <summary>
        /// singlton
        /// </summary>
        /// <returns>order delivery object</returns>
        public static OrderDeliveryService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new OrderDeliveryService();
                }
            }
            return _instance;
        }

        private OrderDeliveryService()
        {
        }

        /// <summary>
        /// create/update order delivery
        /// </summary>
        /// <returns>created/updated order delivery id</returns>
        public Response<int> Save(Request<OrderDeliverySave> request)
        {
            try
            {
                var response = new Response<int>
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.ORDER_DELIVERY_SAVE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    cmd.Parameters.AddWithValue("@OrderId", request.Data.OrderId);
                    cmd.Parameters.AddWithValue("@DeliveryUserId", request.Data.DeliveryUserId);
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue<int>(reader["Result"]);
                    }
                });

                HandleErrorCode((ErrorNumber)response.Data);
                
                return response;
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
    }
}