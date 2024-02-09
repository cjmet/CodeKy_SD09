using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using DataLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace ConsoleToWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Returning from TestController Get Method";
            // return DataLibrary.IProductRepository.GetAllProducts();
        }
    }
}
