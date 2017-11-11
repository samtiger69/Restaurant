using Restaurant.Models;
using System;
using System.Data.SqlClient;
using static Restaurant.Models.Enums;

namespace Restaurant.Services
{
    public class OrderService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile OrderService _instance = null;

        public static OrderService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new OrderService();
                }
            }
            return _instance;
        }

        private OrderService()
        {
        }

        public Response<int> Create(Request<OrderCreate> request)
        {
            try
            {
                var response = new Response<int>
                {
                    Data = 0,
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.ORDER_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.Data.UserId);
                    cmd.Parameters.AddWithValue("@BranchId", request.Data.BranchId);
                    cmd.Parameters.AddWithValue("@Status", OrderStatus.New);
                    cmd.Parameters.AddWithValue("@IsPickUp", request.Data.IsPickUp);
                    if (request.Data.IsPickUp)
                    {
                        cmd.Parameters.AddWithValue("@Area", request.Data.Address.Area);
                        cmd.Parameters.AddWithValue("@Street", request.Data.Address.Street);
                        cmd.Parameters.AddWithValue("@Building", request.Data.Address.Building);
                        cmd.Parameters.AddWithValue("@Floor", request.Data.Address.Floor);
                        cmd.Parameters.AddWithValue("@Office", request.Data.Address.OfficeNumber);
                    }
                    if (!string.IsNullOrEmpty(request.Data.Notes))
                        cmd.Parameters.AddWithValue("@Notes", request.Data.Notes);
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue<int>(reader["Result"]);
                    }
                });

                HandleErrorCode((ErrorNumber)response.Data);

                if (!request.Data.IsPickUp)
                {
                    ExecuteNonQuery(StoredProcedure.ORDER_ADDRESS_CREATE, delegate (SqlCommand cmd)
                    {
                        cmd.Parameters.AddWithValue("@OrderId", request.Data);
                        cmd.Parameters.AddWithValue("@Area", request.Data.Address.Area);
                        cmd.Parameters.AddWithValue("@Street", request.Data.Address.Street);
                        cmd.Parameters.AddWithValue("@Building", request.Data.Address.Building);
                        cmd.Parameters.AddWithValue("@Floor", request.Data.Address.Floor);
                        cmd.Parameters.AddWithValue("@OfficeNumber", request.Data.Address.OfficeNumber);
                    });
                }

                foreach (var orderMeal in request.Data.OrderMeals)
                {
                    ExecuteReader(StoredProcedure.ORDER_MEAL_CREATE, delegate (SqlCommand cmd)
                    {
                        cmd.Parameters.AddWithValue("@OrderId", response.Data);
                        cmd.Parameters.AddWithValue("@MealId", orderMeal.MealId);
                        cmd.Parameters.AddWithValue("@Quantity", orderMeal.Quantity);
                    },
                    delegate (SqlDataReader reader)
                    {
                        if (reader.Read())
                        {
                            orderMeal.Id = GetValue<int>(reader["Result"]);
                        }
                    });
                    HandleErrorCode((ErrorNumber)orderMeal.Id);
                }

                foreach (var orderMeal in request.Data.OrderMeals)
                {
                    foreach (var orderMealAttribute in orderMeal.AttributeId)
                    {
                        ExecuteReader(StoredProcedure.ORDER_MEAL_ATTRIBUTE_CREATE, delegate (SqlCommand cmd)
                        {
                            cmd.Parameters.AddWithValue("@OrderMealId", orderMeal.Id);
                            cmd.Parameters.AddWithValue("@AttributeId", orderMealAttribute);
                        },
                        delegate (SqlDataReader reader)
                        {
                            if (reader.Read())
                            {
                                orderMeal.Id = GetValue<int>(reader["Result"]);
                            }
                        });
                        HandleErrorCode((ErrorNumber)orderMeal.Id);
                    }
                }

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

        public Response Update(Request<OrderUpdate> request)
        {
            try
            {
                var response = new Response
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };
                var result = ErrorNumber.Success;
                ExecuteReader(StoredProcedure.ORDER_UPDATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@OrderId", request.Data.OrderId);
                    cmd.Parameters.AddWithValue("@Status", request.Data.Status);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);

                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        result = GetValue(reader["Result"], ErrorNumber.Success);
                    }
                });
                HandleErrorCode(result);
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