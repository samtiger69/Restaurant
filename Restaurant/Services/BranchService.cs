﻿using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Restaurant.Services
{
    public class BranchService : BaseService
    {
        private static volatile object lockObj;
        private static BranchService _instance = null;

        public static BranchService GetInstance()
        {
            lock (lockObj)
            {
                if(_instance == null)
                {
                    lockObj = _instance = new BranchService();
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

        public Response<Branch> Update(Request<Branch> request)
        {
            try
            {
                var response = new Response<Branch>
                {
                    Data = request.Data
                };
                var result = ErrorNumber.Success;
                ExecuteReader(StoredProcedure.BRANCH_UPDATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", request.Data.Id);
                    if (!string.IsNullOrEmpty(request.Data.Name))
                        cmd.Parameters.AddWithValue("@Name", request.Data.Name);
                    if (!string.IsNullOrEmpty(request.Data.NameAr))
                        cmd.Parameters.AddWithValue("@NameAr", request.Data.NameAr);
                    if (!string.IsNullOrEmpty(request.Data.LocationDescription))
                        cmd.Parameters.AddWithValue("@LocationDescription", request.Data.LocationDescription);
                    if (!string.IsNullOrEmpty(request.Data.Latitude))
                        cmd.Parameters.AddWithValue("@Latitude", request.Data.Latitude);
                    if (!string.IsNullOrEmpty(request.Data.Longitude))
                        cmd.Parameters.AddWithValue("@Longitude", request.Data.Longitude);
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