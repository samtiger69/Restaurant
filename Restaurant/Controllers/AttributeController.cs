using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    /// <summary>
    /// attribute list/create/update
    /// </summary>
    [Authorize(Roles = "admin, branch_admin")]
    public class AttributeController : BaseController
    {
        /// <summary>
        /// get attribute list
        /// </summary>
        /// <returns>list of attributes</returns>
        [HttpPost]
        public Response<List<Entities.Attribute>> List(Request<BaseList> request)
        {
            try
            {
                return new Response<List<Entities.Attribute>>()
                {
                    Data = Cache.GetAttributes(request),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };
            }
            catch (RestaurantException ex)
            {
                return new Response<List<Entities.Attribute>>()
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<List<Entities.Attribute>>()
                {
                    Data = null,
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = e.Message,
                        ErrorNumber = ErrorNumber.GeneralError
                    }
                };
            }
        }

        /// <summary>
        /// create attribute
        /// </summary>
        /// <returns>created attribute id</returns>
        [HttpPost]
        public Response<int> Create(Request<AttributeCreate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                ValidateAttributeCreate(request);
                var attributeService = AttributeService.GetInstance();
                return attributeService.Create(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<int>
                {
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<int>
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = e.Message,
                        ErrorNumber = ErrorNumber.GeneralError
                    }
                };
            }
        }

        /// <summary>
        /// update attribute
        /// </summary>
        [HttpPost]
        public Response Update(Request<AttributeUpdate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var attributeService = AttributeService.GetInstance();
                return attributeService.Update(request);
            }
            catch (RestaurantException ex)
            {
                return new Response
                {
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = e.Message,
                        ErrorNumber = ErrorNumber.GeneralError
                    }
                };
            }
        }
    }
}
