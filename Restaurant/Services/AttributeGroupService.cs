using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

        public Response<AttributeGroup> Create(Request<AttributeGroup> request)
        {
            try
            {
                var response = new Response<AttributeGroup>
                {
                    Data = request.Data,
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.ATTRIBUTE_GROUP_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
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
                            ErrorMessage = "An attribute group with the same name/nameAr already exists in the db",
                            ErrorNumber = ErrorNumber.Exists
                        }
                    };
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

        public Response Update(Request<AttributeGroup> request)
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
                if (result == ErrorNumber.NotFound)
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = string.Format("No such attribute group with id: {0} exists in the db", request.Data.Id),
                            ErrorNumber = ErrorNumber.NotFound
                        }
                    };
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