using Restaurant.Entities;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class BaseController : ApiController
    {
        #region Shared
        protected void ValidateNames(BasicEntity entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.Name))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "Name cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };

                if (string.IsNullOrEmpty(entity.NameAr))
                    throw new RestaurantException
                    {
                        ErrorCode = new ErrorCode
                        {
                            ErrorMessage = "NameAr cannot be empty",
                            ErrorNumber = ErrorNumber.EmptyRequiredField
                        }
                    };
            }
            catch (RestaurantException ex)
            {
                throw ex;
            }
        }

        protected void TrimNames(BasicEntity entity)
        {
            entity.Name = entity.Name.Trim();
            entity.NameAr = entity.NameAr.Trim();
        }
        #endregion
    }
}
