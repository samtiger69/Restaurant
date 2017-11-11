using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    public class MealAttributeService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile MealAttributeService _instance = null;

        public static MealAttributeService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new MealAttributeService();
                }
            }
            return _instance;
        }

        private MealAttributeService()
        {
        }

        public Response<List<MealAttribute>> List(Request request)
        {
            try
            {
                var response = new Response<List<MealAttribute>>
                {
                    Data = new List<MealAttribute>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.MEAL_ATTRIBUTE_SELECT, delegate (SqlCommand cmd)
                {
                }, delegate (SqlDataReader reader)
                {
                    while (reader.Read())
                    {
                        response.Data.Add(new MealAttribute
                        {
                            Id = GetValue(reader["Id"], 0),
                            MealId = GetValue(reader["MealId"], 0),
                            AttributeId = GetValue(reader["AttributeId"], 0)
                        });
                    }
                });
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

        public Response<int> Create(Request<MealAttributeCreate> request)
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

                ExecuteReader(StoredProcedure.MEAL_ATTRIBUTE_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@MealId", request.Data.MealId);
                    cmd.Parameters.AddWithValue("@AttributeId", request.Data.AttributeId);
                }, delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue(reader["Result"], 0);
                    }
                });

                HandleErrorCode((ErrorNumber)response.Data);
                Cache.ResetMealAttributes();
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

        public Response Delete(Request<MealAttributeCreate> request)
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

                ExecuteReader(StoredProcedure.MEAL_ATTRIBUTE_DELETE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@AttributeId", request.Data.AttributeId);
                    cmd.Parameters.AddWithValue("@MealId", request.Data.MealId);
                }, delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        result = GetValue(reader["Result"], ErrorNumber.Success);
                    }
                });

                HandleErrorCode(result);

                Cache.ResetMealAttributes();
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