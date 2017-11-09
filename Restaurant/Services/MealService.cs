using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    public class MealService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile MealService _instance = null;

        public static MealService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new MealService();
                }
            }
            return _instance;
        }

        private MealService()
        {
        }

        //public Response<MealInfo> Info(Request<int> request)
        //{
        //    try
        //    {
        //        var response = new Response<MealInfo>
        //        {
        //            ErrorCode = new ErrorCode
        //            {
        //                ErrorMessage = "",
        //                ErrorNumber = ErrorNumber.Success
        //            }
        //        };

        //        ExecuteReader(StoredProcedure.MEAL_INFO, delegate (SqlCommand cmd)
        //        {
        //        }, delegate (SqlDataReader reader)
        //        {
        //            while (reader.Read())
        //            {
        //                response.Data = new MealInfo
        //                {
        //                    Id = GetValue(reader["Id"], 0),
        //                    NameAr = GetValue(reader["NameAr"], ""),
        //                    Name = GetValue(reader["Name"], ""),
        //                    IsActive = GetValue(reader["IsActive"], false),
        //                    MealTypeId = GetValue(reader["MealTypeId"], 0),
        //                    Price = GetValue<decimal>(reader["Price"], 0)
        //                };
        //            }

        //            if (response.Data == null)
        //            {
        //                throw new RestaurantException
        //                {
        //                    ErrorCode = new ErrorCode
        //                    {
        //                        ErrorMessage = "Meal not found",
        //                        ErrorNumber = ErrorNumber.NotFound
        //                    }
        //                };
        //            }

        //            reader.NextResult();
        //            var attributes = new List<Entities.Attribute>();
        //            while (reader.Read())
        //            {
        //                attributes.Add(new Entities.Attribute
        //                {
        //                    Id = GetValue<int>(reader["Id"], 0),
        //                    Name = GetValue<string>(reader["Name"], ""),
        //                    NameAr = GetValue<string>(reader["NameAr"], ""),
        //                    Price = GetValue<decimal>(reader["Price"], 0),
        //                    GroupId = GetValue<int?>(reader["GroupId"], null),
        //                    IsActive = GetValue<bool>(reader["IsActive"], true)
        //                });
        //            }

        //            reader.NextResult();
        //            response.Data.AttributeGroups = new List<AttributeGroup>();
        //            while (reader.Read())
        //            {
        //                var groupId = GetValue<int>(reader["Id"]);
        //                response.Data.AttributeGroups.Add(new AttributeGroup
        //                {
        //                    Id = groupId,
        //                    Name = GetValue<string>(reader["Name"], ""),
        //                    NameAr = GetValue<string>(reader["NameAr"], ""),
        //                    IsActive = GetValue<bool>(reader["IsActive"], true),
        //                    Attributes = attributes.Where(m => m.GroupId == groupId).ToList()
        //                });
        //            }
        //            response.Data.Attributes = attributes.Where(m => m.GroupId == null).ToList();
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

        public Response<List<Meal>> List(Request request)
        {
            try
            {
                var response = new Response<List<Meal>>
                {
                    Data = new List<Meal>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.MEAL_SELECT, delegate (SqlCommand cmd)
                {
                }, delegate (SqlDataReader reader)
                {
                    while (reader.Read())
                    {
                        response.Data.Add(new Meal
                        {
                            Id = GetValue(reader["Id"], 0),
                            NameAr = GetValue(reader["NameAr"], ""),
                            Name = GetValue(reader["Name"], ""),
                            IsActive = GetValue(reader["IsActive"], false),
                            MealTypeId = GetValue(reader["MealTypeId"], 0),
                            Price = GetValue<decimal>(reader["Price"], 0)
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

        public Response<Meal> Create(Request<Meal> request)
        {
            try
            {
                var response = new Response<Meal>
                {
                    Data = request.Data,
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.MEAL_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    cmd.Parameters.AddWithValue("@Price", request.Data.Price);
                    cmd.Parameters.AddWithValue("@MealTypeId", request.Data.MealTypeId);
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data.Id = GetValue<int>(reader["Result"]);
                    }
                });
                if (response.Data.Id == (int)ErrorNumber.Exists)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "A meal with the same name/nameAr already exists in the db",
                            ErrorNumber = ErrorNumber.Exists
                        }
                    };
                Cache.ResetMeals();
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

        public Response Update(Request<MealUpdate> request)
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
                ExecuteReader(StoredProcedure.MEAL_UPDATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", request.Data.Id);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    if (!string.IsNullOrEmpty(request.Data.Name))
                    {
                        cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    }
                    if (!string.IsNullOrEmpty(request.Data.NameAr))
                    {
                        cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    }
                    if (request.Data.MealTypeId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@MealTypeId", request.Data.MealTypeId.Value);
                    }
                    if (request.Data.Price.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@Price", request.Data.Price);
                    }
                    if (request.Data.IsActive.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@IsActive", request.Data.IsActive.Value);
                    }
                    if (request.Data.IsDeleted.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@IsDeleted", request.Data.IsDeleted.Value);
                    }
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        result = GetValue(reader["Result"], ErrorNumber.Success);
                    }
                });
                if (result == ErrorNumber.NotFound)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = string.Format("No such meal with id: {0} exists in the db", request.Data.Id),
                            ErrorNumber = ErrorNumber.NotFound
                        }
                    };
                Cache.ResetMeals();
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