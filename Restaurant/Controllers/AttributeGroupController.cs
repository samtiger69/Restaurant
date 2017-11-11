using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Restaurant.Controllers
{
    [Authorize(Roles = "admin, branch_admin")]
    public class AttributeGroupController : BaseController
    {
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
        
        [HttpPost]
        public Response<AttributeGroup> Create(Request<AttributeGroup> request)
        {
            try
            {
                ValidateBaseRequest(request);
                var attributeGroupService = AttributeGroupService.GetInstance();
                TrimNames(request.Data);
                ValidateNames(request.Data);
                return attributeGroupService.Create(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<AttributeGroup>
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<AttributeGroup>
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
        
        [HttpPost]
        public Response Update(Request<AttributeGroup> request)
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
