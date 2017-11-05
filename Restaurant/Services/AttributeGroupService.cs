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
    }
}