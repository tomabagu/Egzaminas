using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _account;
        private readonly ILoginService _login;
        private readonly IJwtService _jwt;
        private readonly IAccountRepository _accountRepository;
        private readonly IPersonMapper _personMapper;
        public AccountController(IAccountService account, ILoginService login, IJwtService jwt, IAccountRepository accountRepository, IPersonMapper personMapper)
        {
            _account = account;
            _login = login;
            _jwt = jwt;
            _accountRepository = accountRepository;
            _personMapper = personMapper;
        }
        /// <summary>
        /// Vartotojo registracija
        /// </summary>
        /// <param name="accountRequestDto"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Signup([FromQuery] AccountRequestDto accountRequestDto)
        {
            try
            {
                Account account = _account.SignupNewAccount(accountRequestDto.UserName, accountRequestDto.Password);
                return Created(nameof(account), account);
            }
            catch (Exception ex)
            {
                var errors = new List<string> { ex.Message };
                return BadRequest(new { Errors = errors });
            }
        }
        /// <summary>
        /// Vartotojo prisijungimas, jwt tokeno, accountId, role gavimas
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login([FromQuery][Required] string username, [FromQuery][Required] string password)
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            bool log = _login.Login(username, password, out string role, out string accountId);
            if (log)
            {
                return Ok(new{ Token = _jwt.GetJwtToken(username, role, accountId), AccountId = accountId, Role = role});
            }
            else
            {
                return BadRequest("Login failed");
            }
        }
        /// <summary>
        /// Vartotojo asmens ir adreso informacija
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("GetData/{accountId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetData(Guid accountId)
        {
            var account = _accountRepository.GetAccountByGuid(accountId);
            if (account == null)
            {
                return NotFound();
            }
            var data = _personMapper.Map(account);
            return Ok(data);
        }
        /// <summary>
        /// Vartotojo ištrinimas
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteAccount([FromQuery][Required] Guid accountId)
        {
            var account = _accountRepository.GetAccountByGuid(accountId);
            if (account == null)
            {
                return BadRequest();
            }
            _accountRepository.Delete(account);
            return Ok("success");
        }
    }
}
