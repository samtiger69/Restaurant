using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Restaurant.Services
{
    public class OrderDeliveryService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile OrderDeliveryService _instance = null;

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

        //public Response<List<Branch>> List(Request request)
        //{
        //    try
        //    {
        //        var response = new Response<List<Branch>>
        //        {
        //            Data = new List<Branch>(),
        //            ErrorCode = new ErrorCode
        //            {
        //                ErrorMessage = "",
        //                ErrorNumber = ErrorNumber.Success
        //            }
        //        };

        //        ExecuteReader(StoredProcedure.ORDER_DELIVERY_SELECT, delegate (SqlCommand cmd)
        //        {
        //        }, delegate (SqlDataReader reader)
        //        {
        //            while (reader.Read())
        //            {
        //                response.Data.Add(new Branch
        //                {
        //                    Id = GetValue<int>(reader["Id"], 0),
        //                    NameAr = GetValue<string>(reader["NameAr"], ""),
        //                    Name = GetValue<string>(reader["Name"], ""),
        //                    LocationDescription = GetValue<string>(reader["LocationDescription"]),
        //                    IsActive = GetValue<bool>(reader["IsActive"], false),
        //                    Latitude = GetValue<string>(reader["Latitude"], ""),
        //                    Longitude = GetValue<string>(reader["Longitude"], "")
        //                });
        //            }
        //        });
        //        return response;
        //    }
        //    catch (RestaurantException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

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