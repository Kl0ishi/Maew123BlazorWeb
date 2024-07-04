
using Maew123.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IUserRepository _userRepository;

        public ProductTypeController(IProductTypeRepository productTypeRepository, IUserRepository userRepository)
        {
            this._productTypeRepository = productTypeRepository;
            this._userRepository = userRepository;
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<ServiceResponse<ProductType>>> GetProductType(int id)
        {
            var response = new ServiceResponse<ProductType>();

            var Type = await _productTypeRepository.GetProductType(id);
            if (Type == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this ProductType does not exist.";
            }
            else
            {
                response.Data = Type;
                response.Success = true;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
        {
            var response = new ServiceResponse<List<ProductType>>
            {
                Data = await _productTypeRepository.GetProductTypes(),
                Success = true
            };
            return response;
        }

        [HttpPost, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<ProductType>>> CreateProductType(ProductType Type)
        {
            var whoLogin = await getUser();
            Type.InsertBy = whoLogin.Username;
            Type.UpdateBy = whoLogin.Username;
            var response = new ServiceResponse<ProductType>
            {
                Data = await _productTypeRepository.CreateProductType(Type),
                Success = true
            };

            return Ok(response);
        }

        [HttpPut, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<ProductType>>> UpdateProductType(ProductType Type)
        {
            var whoLogin = await getUser();
            Type.UpdateBy = whoLogin.Username;
            var result = new ServiceResponse<ProductType>
            {
                Data = await _productTypeRepository.UpdateProductType(Type),
                Success = true
            };

            if (result.Data == null)
            {
                return Ok(new ServiceResponse<ProductType>
                {
                    Success = false,
                    Message = "ProductType not found."
                });
            }
            return Ok(result);
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProductType(int id)
        {
            var whoLogin = await getUser();
            var result = new ServiceResponse<bool>
            {
                Data = await _productTypeRepository.DeleteProductType(id, whoLogin.Username),
                Success = true
            };

            if (result.Data != true)
            {
                return Ok(new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "ProductType not found."
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
