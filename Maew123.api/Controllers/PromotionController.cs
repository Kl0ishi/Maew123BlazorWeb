using Azure;
using Maew123.Api.Utilities;
using Maew123.Models;

using Maew123.Models.InputedValues;
using Maew123.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IUserRepository _userRepository;

        public PromotionController(IPromotionRepository promotionRepository, IUserRepository userRepository)
        {
            this._promotionRepository = promotionRepository;
            this._userRepository = userRepository;
        }

        [HttpGet]
        [Route("[action]/{promotionId}")]
        public async Task<ActionResult<ServiceResponse<PromotionDto>>> GetPromotion(int promotionId)
        {
            var response = new ServiceResponse<PromotionDto>();
                
            var promotionDto = await _promotionRepository.GetPromotionDto(promotionId);
            if (promotionDto == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this Promotion does not exist.";
            }
            else
            {
                response.Data = promotionDto;
                response.Success = true;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<PromotionDto>>>> GetPromotions()
        {
            var response = new ServiceResponse<List<PromotionDto>>();

            var promotionsDto = await _promotionRepository.GetPromotions();
            if (promotionsDto == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this Promotion does not exist.";
            }
            else
            {
                response.Data = promotionsDto;
                response.Success = true;
            }

            return Ok(response);
        }

        [HttpGet, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<PromotionDto>>>> GetPromotionsAdmin()
        {
            var response = new ServiceResponse<List<PromotionDto>>
            {
                Data = await _promotionRepository.GetPromotionsAdmin(),
                Success = true
            };
            return response;
        }

        [HttpPost, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<PromotionDto>>> CreatePromotion(PromotionDto promotionDto)
        {
            var whoLogin = await getUser();

            var promotion = new Promotion();
            promotion.GetFromDto(promotionDto);

            promotion.InsertBy = whoLogin.Username;
            promotion.UpdateBy = whoLogin.Username;
            var response = new ServiceResponse<PromotionDto>
            {
                Data = await _promotionRepository.CreatePromotion(promotion),
                Success = true
            };
            return Ok(response);
        }

        //เอาไว้ใส่เป็น dropdowns
        [HttpGet, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<List<Promotion>>>> GetPromotionTypes()
        {
            var promotionTypes = PromotionTypeValues.promotionTypes;
            var response = new ServiceResponse<List<Promotion>>
            {
                Data = promotionTypes,
                Success = true
            };
            
            return response;
        }

        [HttpPut, Authorize(Roles = "Admin,Employee")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<PromotionDto>>> UpdatePromotion(PromotionDto promotionDto)
        {
            var whoLogin = await getUser();

            var promotion = new Promotion();
            promotion.GetFromDto(promotionDto);
            promotion.PromotionId = promotionDto.PromotionId;

            promotion.UpdateBy = whoLogin.Username;
            var response = new ServiceResponse<PromotionDto>
            {
                Data = await _promotionRepository.UpdatePromotion(promotion),
                Success = true
            };
            return Ok(response);
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        [Route("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeletePromotion(int id)
        {
            var whoLogin = await getUser();
            var result = new ServiceResponse<bool>
            {
                Data = await _promotionRepository.DisablePromotion(id, whoLogin.Username!),
                Success = true
            };

            if (result.Data != true)
            {
                return Ok(new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Promotion not found."
                });
            }

            return Ok(result);
        }

        //คำนวณ หน้าที่ตามtypes


        private async Task<User> getUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userRepository.GetUserById(int.Parse(userId!));
        }
    }
}
