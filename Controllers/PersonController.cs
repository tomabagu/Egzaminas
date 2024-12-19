using Egzaminas.Dtos.Requests;
using Egzaminas.Dtos.Results;
using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Egzaminas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Egzaminas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly IPersonMapper _mapper;
        private readonly IAccountService _service;
        private readonly Guid _accountId;

        public PersonController(ILogger<PersonController> logger,
            IPersonRepository personRepository,
            IAccountRepository accountRepository,
            IJwtService jwtService,
            IPersonMapper mapper,
            IAccountService service)
        {
            _logger = logger;
            _personRepository = personRepository;
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _service = service;
        }
        /// <summary>
        /// Gauti asmens informaciją
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("GetPerson/{personId}")]
        [Authorize]
        [ProducesResponseType(typeof(PersonResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPerson(Guid personId)
        {
            var person = _personRepository.Get(personId);
            if (person == null)
            {
                _logger.LogInformation($"Person not found for personId: {personId}");
                return NotFound();
            }
            _logger.LogInformation($"Getting person data for personId: {personId}");
            var dto = _mapper.Map(person);
            return Ok(dto);
            
        }
        /// <summary>
        /// Pridėti asmens informaciją
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("CreatePerson/{accountId}")]
        [Authorize]
        [ProducesResponseType(typeof(Person), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CreatePerson(Guid accountId, [FromForm]PersonRequestDto req)
        {
            try {
                if (accountId == null)
                {
                    throw new ArgumentNullException(nameof(accountId));
                }
                var account = _accountRepository.GetAccountByGuid(accountId);
                if (account == null)
                {
                    _logger.LogInformation($"Account not found for accoundId: {accountId}");
                    return BadRequest("Invalid AccountId");
                }

                if (account.Person != null)
                {
                    _logger.LogInformation($"Failed to create person because person already exists: {account.Person.PersonId}");
                    return Conflict("Person with this accountId already exists");
                }

                var person = _mapper.Map(req);

                if (account.Person == null)
                {
                    account.Person = person;
                }

                _personRepository.Add(person);
                _logger.LogInformation($"Person created succesfully for accountId: {accountId}");
                return Created(nameof(person), new { id = person.PersonId});
            } catch (Exception ex)
            {
                var errors = new List<string> { ex.Message };
                _logger.LogInformation($"Create person failed, validation errors: {errors}");
                return BadRequest(new { Errors = errors });
            }
        }
        /// <summary>
        /// Gauti asmens nuotrauką
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("GetPhoto/{personId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPhoto(Guid personId)
        {
            var person = _personRepository.Get(personId);
            if (person == null)
            {
                _logger.LogInformation($"Person not found for id: {personId}");
                return NotFound();
            }
            _logger.LogInformation($"Getting person image for personId: {personId}");
            return File(person.PersonImageBytes, "image/jpeg");
        }
        /// <summary>
        /// Atnaujinti asmens vardą
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdatePersonName")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePersonName([FromForm] UpdatePersonNameDto dto)
        {

            var person = _personRepository.Get(dto.PersonId);
            if (person == null)
            {
                _logger.LogInformation($"Person not found for id: {dto.PersonId}");
                return NotFound();
            }

            person.Name = dto.PersonName;
            _personRepository.Update(person);
            _logger.LogInformation($"Person name updated succesfully for personId {person.PersonId}");
            return NoContent();
        }
        /// <summary>
        /// Atnaujinti asmens pavardę
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdatePersonSurname")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePersonSurname([FromForm] UpdatePersonSurnameDto dto)
        {

            var person = _personRepository.Get(dto.PersonId);
            if (person == null)
            {
                _logger.LogInformation($"Person not found for id: {dto.PersonId}");
                return NotFound();
            }

            person.Surname = dto.PersonSurname;
            _personRepository.Update(person);
            _logger.LogInformation($"Person surname updated succesfully for personId {person.PersonId}");
            return NoContent();
        }
        /// <summary>
        /// Atnaujinti asmens asmens kodą
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdatePersonCode")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePersonCode([FromForm] UpdatePersonCodeDto dto)
        {

            try
            {
                var person = _personRepository.Get(dto.PersonId);
                if (person == null)
                {
                    _logger.LogInformation($"Person not found for id: {dto.PersonId}");
                    return NotFound();
                }

                person.PersonCode = dto.PersonCode;
                _personRepository.Update(person);
                _logger.LogInformation($"Person code updated succesfully for personId {person.PersonId}");
                return NoContent();
            }
            catch (Exception ex)
            {
                var errors = new List<string> { ex.Message };
                _logger.LogInformation($"Failed to update Person code, validation errors: {errors}");
                return BadRequest(new { Errors = errors });
            }
        }
        /// <summary>
        /// Atnaujinti asmens paštą
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdatePersonEmail")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePersonEmail([FromForm] UpdatePersonEmailDto dto)
        {

            var person = _personRepository.Get(dto.PersonId);
            if (person == null)
            {
                _logger.LogInformation($"Person not found for id: {dto.PersonId}");
                return NotFound();
            }

            person.Email = dto.PersonEmail;
            _personRepository.Update(person);
            _logger.LogInformation($"Person email updated succesfully for personId {person.PersonId}");
            return NoContent();
        }
        /// <summary>
        /// Atnaujinti asmens nuotrauką
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdatePersonImage")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePersonImage([FromForm] UpdatePersonImageDto dto)
        {

            var person = _personRepository.Get(dto.PersonId);
            if (person == null)
            {
                _logger.LogInformation($"Person not found for id: {dto.PersonId}");
                return NotFound();
            }

            person.PersonImageBytes = ImageService.ResizeImage(dto.PersonImage);
            _personRepository.Update(person);
            _logger.LogInformation($"Person image updated succesfully for personId {person.PersonId}");
            return NoContent();
        }
    }
}
