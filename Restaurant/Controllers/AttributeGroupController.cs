using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    /// <summary>
    /// attribute group list/create/update
    /// </summary>
    [Authorize(Roles = "admin, branch_admin")]
    public class AttributeGroupController : BaseController
    {
        /// <summary>
        /// get attribute groups
        /// </summary>
        /// <returns>List of attribute groups</returns>
        [HttpPost]
        public Response<List<AttributeGroup>> List(Request<BaseList> request)
        {
            try
            {
                return new Response<List<AttributeGroup>>()
                {
                    Data = Cache.GetAttributeGroups(request),
                    ErrorCode = new ErrorCode
                    {
                        ErrorMessage = "",
                        ErrorNumber = ErrorNumber.Success
                    }
                };
            }
            catch (RestaurantException ex)
            {
                return new Response<List<AttributeGroup>>()
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<List<AttributeGroup>>()
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
        /// create an AttributeGroup and assign it to attributes
        /// </summary>
        /// <returns>created attribute group id</returns>
        [HttpPost]
        public Response<int> Create(Request<AttributeGroupCreate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                ValidateAttributeGroupCreate(request);
                var attributeGroupService = AttributeGroupService.GetInstance();
                return attributeGroupService.Create(request);
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
        /// update attribute group
        /// </summary>
        [HttpPost]
        public Response Update(Request<AttributeGroupUpdate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var attributeGroupService = AttributeGroupService.GetInstance();
                return attributeGroupService.Update(request);
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
