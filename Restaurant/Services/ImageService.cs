using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Restaurant.Models.Enums;

namespace Restaurant.Services
{
    /// <summary>
    /// image database manager
    /// </summary>
    public class ImageService : BaseService
    {
        private static object lockObj = new Object();
        private static volatile ImageService _instance = null;

        /// <summary>
        /// singlton
        /// </summary>
        /// <returns>image service object</returns>
        public static ImageService GetInstance()
        {
            if (_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new ImageService();
                }
            }
            return _instance;
        }

        private ImageService()
        {
        }

        /// <summary>
        /// get image by id
        /// </summary>
        /// <returns>image</returns>
        public byte[] Get(int id)
        {
            try
            {
                byte[] content = null;
                ExecuteReader(StoredProcedure.IMAGE_SELECT, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
               , delegate (SqlDataReader reader)
               {
                   if (reader.Read())
                   {
                       content = GetValue<byte[]>(reader["Content"]);
                       //content = (byte[])reader["Content"];
                   }
               });
                return content;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// get image list
        /// </summary>
        /// <returns>list of images</returns>
        public Response<List<Entities.Image>> List(Request request)
        {
            try
            {
                var response = new Response<List<Entities.Image>>
                {
                    Data = new List<Entities.Image>(),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };

                ExecuteReader(StoredProcedure.IMAGE_SELECT, delegate (SqlCommand cmd)
                {
                }, delegate (SqlDataReader reader)
                {
                    while (reader.Read())
                    {
                        response.Data.Add(new Entities.Image
                        {
                            Id = GetValue(reader["Id"], 0),
                            SourceId = GetValue(reader["SourceId"],  0),
                            SourceType = GetValue<SourceType>(reader["SourceType"]),
                            IsDefault = GetValue(reader["IsActive"], false)
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
        /// create an image
        /// </summary>
        /// <returns>created image id</returns>
        public Response<int> Create(Request<ImageCreate> request)
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
                ExecuteReader(StoredProcedure.IMAGE_CREATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@SourceId", request.Data.SourceId);
                    cmd.Parameters.AddWithValue("@Content", request.Data.Content);
                    cmd.Parameters.AddWithValue("@IsDefault", request.Data.IsDefault);
                    cmd.Parameters.AddWithValue("@SourceType", request.Data.SourceType);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                }
               , delegate (SqlDataReader reader)
               {
                   if (reader.Read())
                   {
                       response.Data = GetValue<int>(reader["Result"]);
                   }
               });

                HandleErrorCode((ErrorNumber)response.Data);

                Cache.ResetImages();

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
        /// update an image
        /// </summary>
        public Response Update(Request<ImageUpdate> request)
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

                ExecuteReader(StoredProcedure.IMAGE_UPDATE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@IsDefault", request.Data.IsDefault);
                    cmd.Parameters.AddWithValue("@Id", request.Data.Id);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                }
               , delegate (SqlDataReader reader)
               {
                   if (reader.Read())
                   {
                       result = GetValue<ErrorNumber>(reader["Result"]);
                   }
               });

                HandleErrorCode(result);

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
        /// delete an image
        /// </summary>
        public Response Delete(Request<int> request)
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

                ExecuteReader(StoredProcedure.IMAGE_DELETE, delegate (SqlCommand cmd)
                {
                    cmd.Parameters.AddWithValue("@Id", request.Data);
                    cmd.Parameters.AddWithValue("@UserId", request.UserId);
                }, delegate (SqlDataReader reader)
                {
                    if (reader.Read())
                    {
                        result = GetValue(reader["Result"], ErrorNumber.Success);
                    }
                });

                HandleErrorCode(result);

                Cache.ResetImages();

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