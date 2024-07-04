using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecaptchaController : ControllerBase
    {
        private readonly RecaptchaSettings _recaptchaSettings;

        public RecaptchaController(IOptions<RecaptchaSettings> recaptchaSettings)
        {
            _recaptchaSettings = recaptchaSettings.Value;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> VerifyRecaptcha(string recaptchaResponse)
        {
            var httpClient = new HttpClient();
            var secretKey = _recaptchaSettings.SecretKey;
            var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GoogleCaptchaCheckResponseResult>();
                if (result.Success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Captcha verification failed");
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

    }
}
