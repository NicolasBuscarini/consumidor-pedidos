using Microsoft.AspNetCore.Mvc;
using ConsumidorPedidos.Model.Response;

namespace ConsumidorPedidos.Controllers
{
    public abstract class BaseController(ILogger<BaseController> logger) : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger = logger;

        protected IActionResult CreateResponse<T>(List<T> data, MetaData meta, string actionName, object routeValues) where T : class
        {
            var links = new List<LinkInfo>();

            var selfUrl = Url.Action(actionName, routeValues);
            if (!string.IsNullOrEmpty(selfUrl))
            {
                links.Add(new LinkInfo("self", selfUrl, "GET"));
            }

            if (meta.CurrentPage < meta.TotalPages)
            {
                var nextUrl = Url.Action(actionName, new { pageNumber = meta.CurrentPage + 1, pageSize = meta.ItemsPerPage });
                if (!string.IsNullOrEmpty(nextUrl))
                {
                    links.Add(new LinkInfo("next", nextUrl, "GET"));
                }
            }

            if (meta.CurrentPage > 1)
            {
                var previousUrl = Url.Action(actionName, new { pageNumber = meta.CurrentPage - 1, pageSize = meta.ItemsPerPage });
                if (!string.IsNullOrEmpty(previousUrl))
                {
                    links.Add(new LinkInfo("previous", previousUrl, "GET"));
                }
            }

            var response = new BaseResponse<List<T>>
            {
                Data = data,
                Meta = meta,
                Links = links
            };

            return Ok(response);
        }


        protected IActionResult HandleNotFound<T>(string message) where T : class
        {
            _logger.LogError(message);
            return NotFound(new BaseResponse<List<T>> { Error = new ErrorResponse(404) { Message = message } });
        }

        protected IActionResult HandleServerError<T>(string message) where T : class
        {
            _logger.LogError(message);
            return StatusCode(500, new BaseResponse<T> { Error = new ErrorResponse(500) { Message = message } });
        }

        protected IActionResult HandleServerError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, new BaseResponse<bool> { Error = new ErrorResponse(500) { Message = message } });
        }
    }
}