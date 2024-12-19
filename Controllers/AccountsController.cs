using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Egzaminas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _account;
        private readonly ILoginService _login;
        private readonly IJwtService _jwt;
        private readonly IAccountRepository _accountRepository;
        private readonly IPersonMapper _personMapper;
        public AccountController(ILogger<AccountController> logger, IAccountService account, ILoginService login, IJwtService jwt, IAccountRepository accountRepository, IPersonMapper personMapper)
        {
            _logger = logger;
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
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromQuery] AccountRequestDto accountRequestDto)
        {
            try
            {
                Account account = _account.SignupNewAccount(accountRequestDto.UserName, accountRequestDto.Password);
                _logger.LogInformation($"Registration success for username: {accountRequestDto.UserName}");
                return Created(nameof(account), account.AccountId);
            }
            catch (Exception ex)
            {
                var errors = new List<string> { ex.Message };
                _logger.LogError($"Registration errors: {errors}");
                return BadRequest(new { Errors = errors });
            }
        }
        /// <summary>
        /// Vartotojo prisijungimas, jwt tokeno, accountId, role gavimas
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login([FromQuery][Required] string username, [FromQuery][Required] string password)
        {
            bool log = _login.Login(username, password, out string role, out string accountId);
            if (log)
            {
                _logger.LogInformation($"Login success for username: {username} accountId: {accountId}");
                return Ok(new{ Token = _jwt.GetJwtToken(username, role, accountId), AccountId = accountId, Role = role});
            }
            else
            {
                _logger.LogInformation($"Login failed for user: {username}");
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
            if (accountId == null)
            {
                throw new ArgumentNullException(nameof(accountId));
            }
            var account = _accountRepository.GetAccountByGuid(accountId);
            if (account == null)
            {
                _logger.LogInformation($"Data not found for {accountId}");
                return NotFound();
            }
            _logger.LogInformation($"Get account data success for accountId {accountId}");
            var data = _personMapper.Map(account);
            return Ok(data);
        }
    }
}
