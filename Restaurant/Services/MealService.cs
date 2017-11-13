using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    /// <summary>
    /// meal database manager
    /// </summary>
    public class MealService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile MealService _instance = null;

        /// <summary>
        /// get singlton
        /// </summary>
        /// <returns>meal service object</returns>
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

        /// <summary>
        /// get meal list
        /// </summary>
        /// <returns>list of meals</returns>
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
                            Price = GetValue<decimal>(reader["Price"], 0),
                            DefaultImageId = GetValue(reader["DefaultImageId"], 0)
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

        /// <summary>
        /// create a meal
        /// </summary>
        /// <returns>created meal id</returns>
        public Response<int> Create(Request<MealCreate> request)
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
                        response.Data = GetValue<int>(reader["Result"]);
                    }
                });
                HandleErrorCode((ErrorNumber)response.Data);

                AddMealRelatedInfo(request, response.Data);

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

        /// <summary>
        /// update a meal
        /// </summary>
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
                HandleErrorCode(result);
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

        private void AddMealRelatedInfo(Request<MealCreate> request, int mealId)
        {
            try
            {
                if (request.Data.AttributeGroups != null && request.Data.AttributeGroups.Count > 0)
                {
                    var mealAttributeGroupService = MealAttributeGroupService.GetInstance();
                    foreach (var id in request.Data.AttributeGroups)
                    {
                        mealAttributeGroupService.Create(new Request<MealAttributeGroupCreate> { UserId = request.UserId, Data = new MealAttributeGroupCreate { MealId = mealId, AttributeGroupId = id } });
                    }
                }

                if (request.Data.Attributes != null && request.Data.Attributes.Count > 0)
                {
                    var mealAttributeService = MealAttributeService.GetInstance();
                    foreach (var id in request.Data.Attributes)
                    {
                        mealAttributeService.Create(new Request<MealAttributeCreate> { UserId = request.UserId, Data = new MealAttributeCreate { AttributeId = id, MealId = mealId } });
                    }
                }


                if(request.Data.MealImages != null && request.Data.MealImages.Count > 0)
                {
                    var imageService = ImageService.GetInstance();
                    foreach (var image in request.Data.MealImages)
                    {
                        imageService.Create(new Request<ImageCreate> { UserId = request.UserId, Data = new ImageCreate { Content = image.Content, IsDefault = image.IsDefualt, SourceId = mealId, SourceType = Enums.SourceType.Meal } });
                    }
                }
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