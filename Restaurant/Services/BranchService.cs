﻿using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restaurant.Services
{
    /// <summary>
    /// branch database manager
    /// </summary>
    public class BranchService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile BranchService _instance = null;

        /// <summary>
        /// singlton
        /// </summary>
        /// <returns>branch service object</returns>
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

        /// <summary>
        /// get branches
        /// </summary>
        /// <returns>list of branches</returns>
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

        /// <summary>
        /// create a branch
        /// </summary>
        /// <returns>created branch id</returns>
        public Response<int> Create(Request<BranchCreate> request)
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

                ExecuteReader(StoredProcedure.BRANCH_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                    cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    if (!string.IsNullOrEmpty(request.Data.LocationDescription))
                        cmd.Parameters.AddWithValue("@LocationDescription", request.Data.LocationDescription.Trim());
                    if (!string.IsNullOrEmpty(request.Data.Latitude))
                        cmd.Parameters.AddWithValue("@Latitude", request.Data.Latitude.Trim());
                    if (!string.IsNullOrEmpty(request.Data.Longitude))
                        cmd.Parameters.AddWithValue("@Longitude", request.Data.Longitude.Trim());
                },
                delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        response.Data = GetValue<int>(reader["Result"]);
                    }
                });
                HandleErrorCode((ErrorNumber)response.Data);
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

        /// <summary>
        /// update a branch
        /// </summary>
        public Response Update(Request<BranchUpdate> request)
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
                HandleErrorCode(result);
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