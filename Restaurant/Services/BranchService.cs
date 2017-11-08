using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    public class BranchService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile BranchService _instance = null;

        public static BranchService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new BranchService();
                }
            }
            return _instance;
        }

        private BranchService()
        {
        }

        public Response<List<Branch>> List(Request request)
        {
            try
            {
                var response = new Response<List<Branch>>
                {
                    Data = new List<Branch>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.BRANCH_SELECT, delegate (SqlCommand cmd)
               {
               }, delegate (SqlDataReader reader)
               {
                   while (reader.Read())
                   {
                       response.Data.Add(new Branch
                       {
                           Id = GetValue<int>(reader["Id"], 0),
                           NameAr = GetValue<string>(reader["NameAr"], ""),
                           Name = GetValue<string>(reader["Name"], ""),
                           LocationDescription = GetValue<string>(reader["LocationDescription"]),
                           IsActive = GetValue<bool>(reader["IsActive"], false),
                           Latitude = GetValue<string>(reader["Latitude"], ""),
                           Longitude = GetValue<string>(reader["Longitude"], "")
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

        public Response<Branch> Create(Request<Branch> request)
        {
            try
            {
                var response = new Response<Branch>
                {
                    Data = request.Data,
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.BRANCH_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    if (!string.IsNullOrEmpty(request.Data.LocationDescription))
                        cmd.Parameters.AddWithValue("@LocationDescription", request.Data.LocationDescription);
                    if (!string.IsNullOrEmpty(request.Data.Latitude))
                        cmd.Parameters.AddWithValue("@Latitude", request.Data.Latitude);
                    if (!string.IsNullOrEmpty(request.Data.Longitude))
                        cmd.Parameters.AddWithValue("@Longitude", request.Data.Longitude);
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
                            ErrorMessage = "A branch with the same name/nameAr already exists in the db",
                            ErrorNumber = ErrorNumber.Exists
                        }
                    };
                Cache.ResetBranches();
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

        public Response Update(Request<Branch> request)
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
                ExecuteReader(StoredProcedure.BRANCH_UPDATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", request.Data.Id);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    if (!string.IsNullOrEmpty(request.Data.Name))
                        cmd.Parameters.AddWithValue("@Name", request.Data.Name.Trim());
                    if (!string.IsNullOrEmpty(request.Data.NameAr))
                        cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr.Trim());
                    if (!string.IsNullOrEmpty(request.Data.LocationDescription))
                        cmd.Parameters.AddWithValue("@LocationDescription", request.Data.LocationDescription.Trim());
                    if (!string.IsNullOrEmpty(request.Data.Latitude))
                        cmd.Parameters.AddWithValue("@Latitude", request.Data.Latitude.Trim());
                    if (!string.IsNullOrEmpty(request.Data.Longitude))
                        cmd.Parameters.AddWithValue("@Longitude", request.Data.Longitude.Trim());
                    if (request.Data.IsActive.HasValue)
                        cmd.Parameters.AddWithValue("@IsActive", request.Data.IsActive);
                    if (request.Data.IsDeleted.HasValue)
                        cmd.Parameters.AddWithValue("@IsDeleted", request.Data.IsDeleted);
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
                            ErrorMessage = string.Format("There isn't a branch with the id: {0} in the db", request.Data.Id),
                            ErrorNumber = ErrorNumber.NotFound
                        }
                    };
                Cache.ResetBranches();
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