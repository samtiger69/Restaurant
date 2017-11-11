using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Restaurant.Controllers
{
    public class ImageController : BaseController
    {
        
        public HttpResponseMessage Get(Request<int> request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var imageService = ImageService.GetInstance();

                var image = imageService.Get(request.Data);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(image)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return response;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        [Authorize(Roles = "admin, branch_admin")]
        [HttpPost]
        public Response<int> Create(Request<ImageCreate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                ValidateImageCreate(request.Data);
                var imageService = ImageService.GetInstance();

                return imageService.Create(request);
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
                    ErrorCode = new ErrorCode(e.Message, ErrorNumber.GeneralError)
                };
            }
        }

        [Authorize(Roles = "admin, branch_admin")]
        [HttpPost]
        public Response Update(Request<ImageUpdate> request)
        {
            try
            {
                ValidateBaseRequest(request);
                ValidateImageUpdate(request.Data);
                var imageService = ImageService.GetInstance();

                return imageService.Update(request);
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
                    ErrorCode = new ErrorCode(e.Message, ErrorNumber.GeneralError)
                };
            }
        }
    }
}
