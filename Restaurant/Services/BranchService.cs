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

        public async Task<Response<List<Branch>>> List(Request request)
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
                
                await ExecuteReader(StoredProcedure.BRANCH_SELECT, delegate (SqlCommand cmd)
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
    }
}