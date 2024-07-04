using Azure;


using Maew123.Models;

using Maew123.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCatagoryController : ControllerBase
    {
        private readonly IProductCatagoryRepository _catagoryRepository;
        private readonly IUserRepository _userRepository;

        public ProductCatagoryController(IProductCatagoryRepository catagoryRepository, IUserRepository userRepository)
        {
            this._catagoryRepository = catagoryRepository;
            this._userRepository = userRepository;
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<ServiceResponse<ProductCatagory>>> GetCatagory(int id) 
        {
            var response = new ServiceResponse<ProductCatagory>();

            var catagory = await _catagoryRepository.GetCatagory(id);
            if (catagory == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this Catagory does not exist.";
            }
            else
            {
                response.Data = catagory;
                response.Success = true;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<ProductCatagory>>>> GetCatagories()
        {
            var response = new ServiceResponse<List<ProductCatagory>>
            {
                Data = await _catagoryRepository.GetCatagories(),
                Success = true
            };
            return response;
        }

        [HttpPost, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<ProductCatagory>>> CreateCatagory(ProductCatagory catagory)
        {
            var whoLogin = await getUser();
            catagory.InsertBy = whoLogin.Username;
            catagory.UpdateBy = whoLogin.Username;
            var response = new ServiceResponse<ProductCatagory>
            {
                Data = await _catagoryRepository.CreateCatagory(catagory),
                Success = true
            };
                
            return Ok(response);
        }

        [HttpPut, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<ProductCatagory>>> UpdateCatagory(ProductCatagory catagory)
        {
            var whoLogin = await getUser();
            catagory.UpdateBy = whoLogin.Username;
            var result = new ServiceResponse<ProductCatagory>
            {
                Data = await _catagoryRepository.UpdateCatagory(catagory),
                Success = true
            };
           
            if(result.Data == null)
            {
                return Ok(new ServiceResponse<ProductCatagory>
                {
                    Success = false,
                    Message = "Catagory not found."
                });
            }
            return Ok(result);
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCatagory(int id)
        {
            var whoLogin = await getUser();
            var result = new ServiceResponse<bool>
            {
                Data = await _catagoryRepository.DeleteCatagory(id, whoLogin.Username!),
                Success = true
            };

            if (result.Data != true)
            {
                return Ok(new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Catagory not found."
                });
            }

            return Ok(result);
        }


        private async Task<User> getUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userRepository.GetUserById(int.Parse(userId!));
        }
    }
}
