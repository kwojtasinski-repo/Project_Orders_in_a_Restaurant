using Microsoft.AspNetCore.Mvc;
using Restaurant.API.DTO;
using Restaurant.ApplicationLogic.Interfaces;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController
        (
            IEmailOptions emailOptions
        )
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new
            {
                emailOptions.Login,
                Password  = string.Empty,
                emailOptions.SmtpClient,
                emailOptions.SmtpPort,
                emailOptions.Email
            });
        }

        [HttpPut]
        public async Task<IActionResult> Put(SettingsDto dto)
        {
            emailOptions.Login = dto.Login;
            emailOptions.Password = dto.Password;
            emailOptions.SmtpPort = dto.SmtpPort;
            emailOptions.SmtpClient = dto.SmtpClient;
            emailOptions.Email = dto.Email;
            return NoContent();
        }
    }
}
