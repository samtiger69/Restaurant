using Restaurant.Entities;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class AttributeController : BaseController
    {
        [Authorize]
        public Response<List<Entities.Attribute>> List(Request<BaseListModel> request)
        {
            try
            {
                return new Response<List<Entities.Attribute>>()
                {
                    Data = Cache.GetAttributes(request.Data),
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response<Entities.Attribute> Create(Request<Entities.Attribute> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
                var attributeService = AttributeService.GetInstance();
                TrimNames(request.Data);
                ValidateNames(request.Data);
                return attributeService.Create(request);
            }
            catch (RestaurantException ex)
            {
                return new Response<Entities.Attribute>
                {
                    Data = null,
                    ErrorCode = ex.ErrorCode
                };
            }
            catch (Exception e)
            {
                return new Response<Entities.Attribute>
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Response Update(Request<UpdateAttributeModel> request)
        {
            try
            {
                if (request == null || request.Data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
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
