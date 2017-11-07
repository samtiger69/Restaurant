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

        //        ExecuteReader(StoredProcedure.BRANCH_SELECT, delegate (SqlCommand cmd)
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