using Egzaminas.Dtos.Results;
using Egzaminas.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Egzaminas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAccountsMapper _map;
        private readonly IAccountRepository _accountRepository;
        public AdminController(ILogger<AdminController> logger, IAccountsMapper map, IAccountRepository accountRepository)
        {
            _logger = logger;
            _map = map;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Ištraukti visų vartotojų duomenis kurių role nėra Admin
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllAccounts")]
        [ProducesResponseType(typeof(List<AccountsResultDto>), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();

            var data = _map.Map(accounts);
            _logger.LogInformation($"[Admin] Getting all acounts");
            return Ok(data);
        }
        /// <summary>
        /// Vartotojo ištrinimas pagal accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public ActionResult DeleteAccount([FromQuery][Required] Guid accountId)
        {
            if (accountId == null)
            {
                throw new ArgumentNullException(nameof(accountId));
            }
            var account = _accountRepository.GetAccountByGuid(accountId);
            if (account == null)
            {
                return BadRequest();
            }
            _accountRepository.Delete(account);
            _logger.LogInformation($"[Admin] Account with accountId: {accountId} deleted");
            return Ok("success");
        }
    }
}
