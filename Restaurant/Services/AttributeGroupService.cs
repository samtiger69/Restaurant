using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    public class AttributeGroupService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile AttributeGroupService _instance = null;

        public static AttributeGroupService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new AttributeGroupService();
                }
            }
            return _instance;
        }

        private AttributeGroupService()
        {
        }

        public Response<List<AttributeGroup>> List(Request request)
        {
            try
            {
                var response = new Response<List<AttributeGroup>>
                {
                    Data = new List<AttributeGroup>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.ATTRIBUTE_GROUP_SELECT, delegate (SqlCommand cmd)
                {
                }, delegate (SqlDataReader reader)
                {
                    while (reader.Read())
                    {
                        response.Data.Add(new AttributeGroup
                        {
                            Id = GetValue<int>(reader["Id"], 0),
                            NameAr = GetValue<string>(reader["NameAr"], ""),
                            Name = GetValue<string>(reader["Name"], ""),
                            IsActive = GetValue<bool>(reader["IsActive"], false)
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

        public Response<int> Create(Request<AttributeGroupCreate> request)
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

                ExecuteReader(StoredProcedure.ATTRIBUTE_GROUP_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue<int>(reader["Result"]);
                    }
                });

                HandleErrorCode((ErrorNumber)response.Data);

                if(request.Data.Attributes != null && request.Data.Attributes.Count > 0)
                {
                    var attributeService = AttributeService.GetInstance();
                    foreach (var attributeId in request.Data.Attributes)
                    {
                        attributeService.Update(new Request<AttributeUpdate> { UserId = request.UserId, Data = new AttributeUpdate { GroupId = response.Data, Id = attributeId } });
                    }
                    Cache.ResetAttributes();
                }

                Cache.ResetAttributeGroups();
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

        public Response Update(Request<AttributeGroupUpdate> request)
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
                ExecuteReader(StoredProcedure.ATTRIBUTE_GROUP_UPDATE, delegate (SqlCommand cmd)
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
                        result = GetValue<ErrorNumber>(reader["Result"]);
                    }
                });
                HandleErrorCode(result);

                if(request.Data.AttributesToAdd != null && request.Data.AttributesToAdd.Count > 0)
                {
                    var attributeService = AttributeService.GetInstance();
                    foreach (var attributeId in request.Data.AttributesToAdd)
                    {
                        attributeService.Update(new Request<AttributeUpdate> { UserId = request.UserId, Data = new AttributeUpdate { GroupId = request.Data.Id, Id = attributeId } });
                    }
                    Cache.ResetAttributes();
                }

                if (request.Data.AttributesToRemove != null && request.Data.AttributesToRemove.Count > 0)
                {
                    var attributeService = AttributeService.GetInstance();
                    foreach (var attributeId in request.Data.AttributesToRemove)
                    {
                        attributeService.Update(new Request<AttributeUpdate> { UserId = request.UserId, Data = new AttributeUpdate { GroupId = -1, Id = attributeId } });
                    }
                    Cache.ResetAttributes();
                }

                Cache.ResetAttributeGroups();
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