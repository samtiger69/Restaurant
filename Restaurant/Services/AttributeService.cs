using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    /// <summary>
    /// atribute database manager
    /// </summary>
    public class AttributeService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile AttributeService _instance = null;

        /// <summary>
        /// singlton
        /// </summary>
        /// <returns>attribute service object</returns>
        public static AttributeService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new AttributeService();
                }
            }
            return _instance;
        }

        private AttributeService()
        {
        }

        /// <summary>
        /// get attribute
        /// </summary>
        /// <returns>list of attributes</returns>
        public Response<List<Entities.Attribute>> List(Request request)
        {
            try
            {
                var response = new Response<List<Entities.Attribute>>
                {
                    Data = new List<Entities.Attribute>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                 ExecuteReader(StoredProcedure.ATTRIBUTE_SELECT, delegate (SqlCommand cmd)
                {
                }, delegate (SqlDataReader reader)
                {
                    while (reader.Read())
                    {
                        response.Data.Add(new Entities.Attribute
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

        /// <summary>
        /// create an attribute
        /// </summary>
        /// <returns>created attribute id</returns>
        public Response<int> Create(Request<AttributeCreate> request)
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

                ExecuteReader(StoredProcedure.ATTRIBUTE_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    if (request.Data.GroupId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@GroupId", request.Data.GroupId.Value);
                    }
                    cmd.Parameters.AddWithValue("@Price", request.Data.Price);
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue<int>(reader["Result"]);
                    }
                });
                HandleErrorCode((ErrorNumber)response.Data);
                Cache.ResetAttributes();
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
        /// update an attribute
        /// </summary>
        public Response Update(Request<AttributeUpdate> request)
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
                ExecuteReader(StoredProcedure.ATTRIBUTE_UPDATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", request.Data.Id);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    if (!string.IsNullOrEmpty(request.Data.Name))
                    {
                        cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    }
                    if(!string.IsNullOrEmpty(request.Data.NameAr))
                    {
                        cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    }
                    if (request.Data.GroupId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@GroupId", request.Data.GroupId.Value);
                    }
                    if (request.Data.Price.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@Price", request.Data.Price.Value);
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
                Cache.ResetAttributes();
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