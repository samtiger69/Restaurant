using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    public class MealAttributeGroupService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile MealAttributeGroupService _instance = null;
        public static MealAttributeGroupService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new MealAttributeGroupService();
                }
            }
            return _instance;
        }
        private MealAttributeGroupService()
        {
        }

        public Response<List<MealAttributeGroup>> List(Request request)
        {
            try
            {
                var response = new Response<List<MealAttributeGroup>>
                {
                    Data = new List<MealAttributeGroup>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.MEAL_ATTRIBUTE_GROUP_SELECT, delegate (SqlCommand cmd)
                {
                }, delegate (SqlDataReader reader)
                {
                    while (reader.Read())
                    {
                        response.Data.Add(new MealAttributeGroup
                        {
                            Id = GetValue(reader["Id"], 0),
                            MealId = GetValue(reader["MealId"], 0),
                            AttributeGroupId = GetValue(reader["AttributeGroupId"], 0)
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

        public Response<int> Create(Request<MealAttributeGroupCreate> request)
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

                ExecuteReader(StoredProcedure.MEAL_ATTRIBUTE_GROUP_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@MealId", request.Data.MealId);
                    cmd.Parameters.AddWithValue("@AttributeGroupId", request.Data.AttributeGroupId);
                }, delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue(reader["Result"], 0);
                    }
                });

                HandleErrorCode((ErrorNumber)response.Data);
                Cache.ResetMealAttributeGroups();
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

        public Response Delete(Request<MealAttributeGroupCreate> request)
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

                ExecuteReader(StoredProcedure.MEAL_ATTRIBUTE_GROUP_DELETE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@AttributeGroupId", request.Data.AttributeGroupId);
                    cmd.Parameters.AddWithValue("@MealId", request.Data.MealId);
                }, delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        result = GetValue(reader["Result"], ErrorNumber.Success);
                    }
                });

                HandleErrorCode(result);

                Cache.ResetMealAttributeGroups();
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