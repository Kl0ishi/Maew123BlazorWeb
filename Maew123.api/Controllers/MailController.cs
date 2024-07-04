using Maew123.Api.Services.MailService;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maew123.Api.Controllers
{
    //Controller นี้ไม่ใช้
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            this._mailService = mailService;
        }

        [HttpPost]
        [Route("SendMail")]
        public bool SendMail(MailData mailData)
        {
            return _mailService.SendMail(mailData);
        }
    }
}
