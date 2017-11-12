using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    public class MealTypeService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile MealTypeService _instance = null;

        public static MealTypeService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new MealTypeService();
                }
            }
            return _instance;
        }

        private MealTypeService()
        {
        }

        public Response<List<MealType>> List(Request request)
        {
            try
            {
                var response = new Response<List<MealType>>
                {
                    Data = new List<MealType>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.MEAL_TYPE_SELECT, delegate (SqlCommand cmd)
                {
                }, delegate (SqlDataReader reader)
                {
                    while (reader.Read())
                    {
                        response.Data.Add(new MealType
                        {
                            Id = GetValue<int>(reader["Id"], 0),
                            NameAr = GetValue<string>(reader["NameAr"], ""),
                            Name = GetValue<string>(reader["Name"], ""),
                            IsActive = GetValue<bool>(reader["IsActive"], false),
                            BranchId = GetValue<int>(reader["BranchId"], 0),
                            Description = GetValue<string>(reader["Description"], ""),
                            ImageId = GetValue<int>(reader["ImageId"],0)
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

        public Response<int> Create(Request<MealTypeCreate> request)
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

                ExecuteReader(StoredProcedure.MEAL_TYPE_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    cmd.Parameters.AddWithValue("@BranchId", request.Data.BranchId);
                    if (!string.IsNullOrEmpty(request.Data.Description))
                        cmd.Parameters.AddWithValue("@Description", request.Data.Description);
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue<int>(reader["Result"]);
                    }
                });
                HandleErrorCode((ErrorNumber)response.Data);

                var imageService = ImageService.GetInstance();
                imageService.Create(new Request<ImageCreate> { UserId = request.UserId, Data = new ImageCreate { Content = request.Data.ImageContent, IsDefault = true, SourceId = response.Data, SourceType = Enums.SourceType.MealType } });

                Cache.ResetMealTypes();
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

        public Response Update(Request<MealTypeUpdate> request)
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
                ExecuteReader(StoredProcedure.MEAL_TYPE_UPDATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", request.Data.Id);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    if (!string.IsNullOrEmpty(request.Data.Name))
                        cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    if (!string.IsNullOrEmpty(request.Data.NameAr))
                        cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    if (!string.IsNullOrEmpty(request.Data.Description))
                        cmd.Parameters.AddWithValue("@Description", request.Data.Description);
                    if (request.Data.BranchId.HasValue)
                        cmd.Parameters.AddWithValue("@BranchId", request.Data.BranchId.Value);
                    if (request.Data.IsActive.HasValue)
                        cmd.Parameters.AddWithValue("@IsActive", request.Data.IsActive.Value);
                    if (request.Data.IsDeleted.HasValue)
                        cmd.Parameters.AddWithValue("@IsDeleted", request.Data.IsDeleted.Value);
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        result = GetValue(reader["Result"], ErrorNumber.Success);
                    }
                });
                HandleErrorCode(result);
                Cache.ResetMealTypes();
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