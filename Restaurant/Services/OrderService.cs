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
                if (response.Data == (int)ErrorNumber.UserDoesNotExist)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "User does not exist",
                            ErrorNumber = ErrorNumber.UserDoesNotExist
                        }
                    };
                if (response.Data == (int)ErrorNumber.BranchDoesNotExist)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "branch does not exist",
                            ErrorNumber = ErrorNumber.BranchDoesNotExist
                        }
                    };

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
                    if (orderMeal.Id == (int)ErrorNumber.OrderDoesNotExist)
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Order does not exist",
                                ErrorNumber = ErrorNumber.OrderDoesNotExist
                            }
                        };
                    if (orderMeal.Id == (int)ErrorNumber.MealDoesNotExist)
                        throw new RestaurantException
                        {
                            ErrorCode = new ErrorCode
                            {
                                ErrorMessage = "Meal does not exist",
                                ErrorNumber = ErrorNumber.MealDoesNotExist
                            }
                        };
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

                        if (orderMeal.Id == (int)ErrorNumber.OrderMealDoesNotExist)
                            throw new RestaurantException
                            {
                                ErrorCode = new ErrorCode
                                {
                                    ErrorMessage = "order meal does not exist",
                                    ErrorNumber = ErrorNumber.OrderMealDoesNotExist
                                }
                            };

                        if (orderMeal.Id == (int)ErrorNumber.AttributeDoesNotExist)
                            throw new RestaurantException
                            {
                                ErrorCode = new ErrorCode
                                {
                                    ErrorMessage = "attribute does not exist",
                                    ErrorNumber = ErrorNumber.AttributeDoesNotExist
                                }
                            };
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
                        result = GetValue<ErrorNumber>(reader["Result"], ErrorNumber.Success);
                    }
                });
                if (result == ErrorNumber.NotFound)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "There isn't a branch with the id: {0} in the db",
                            ErrorNumber = ErrorNumber.NotFound
                        }
                    };
                if (result == ErrorNumber.AccessDenied)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "Access Denied",
                            ErrorNumber = ErrorNumber.AccessDenied
                        }
                    };
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