using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Egzaminas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IPersonRepository _personRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IAddressMapper _mapper;

        public AddressController(ILogger<AddressController> logger,
            IPersonRepository personRepository,
            IAddressRepository addressRepository,
            IAddressMapper mapper)
        {
            _logger = logger;
            _personRepository = personRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gauti asmens adreso duomenis
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpGet("GetAddress/{addressId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAddress(Guid addressId)
        {
            var address = _addressRepository.Get(addressId);
            if (address == null)
            {
                _logger.LogInformation($"Address not found for id: {addressId}");
                return NotFound();
            }
            _logger.LogInformation($"Getting address data found for id: {addressId}");
            var dto = _mapper.Map(address);
            return Ok(dto);
            
        }
        /// <summary>
        /// Sukurti asmens adresą
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("CreateAddress/{personId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult CreateAddress(Guid personId, [FromForm]AddressRequestDto req)
        {
            var person = _personRepository.Get(personId);
            if (person == null)
            {
                _logger.LogInformation($"Person not found for id: {personId}");
                return BadRequest("Invalid personId");
            }

            if (person.Address != null)
            {
                _logger.LogInformation($"Address already exists for personId: {personId}");
                return Conflict("Address with this personId already exists");
            }

            var address = _mapper.Map(req);

            if (person.Address == null)
            {
                address.AddressId = Guid.NewGuid();
                person.Address = address;
            }
            _addressRepository.Add(address);
            _personRepository.Update(person);

            _logger.LogInformation($"New address added with {address.AddressId} for personId: {person.PersonId}");

            return Created(nameof(address), new { id = address.AddressId});
        }
        /// <summary>
        /// Atnaujinti asmens adreso miestą
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdateAddressCity")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateAddressCity([FromForm] UpdateAddressCity dto)
        {

            var address = _addressRepository.Get(dto.AddressId);
            if (address == null)
            {
                _logger.LogInformation($"Address not found for id: {dto.AddressId}");
                return NotFound();
            }

            address.City = dto.AddressCity;
            _addressRepository.Update(address);

            _logger.LogInformation($"Address city updated succesfully for addressId {address.AddressId}");

            return NoContent();
        }
        /// <summary>
        /// Atnaujinti asmens adreso gatvę
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdateAddressStreet")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateAddressStreet([FromForm] UpdateAddressStreet dto)
        {

            var address = _addressRepository.Get(dto.AddressId);
            if (address == null)
            {
                _logger.LogInformation($"Address not found for id: {dto.AddressId}");
                return NotFound();
            }

            address.Street = dto.AddressStreet;
            _addressRepository.Update(address);
            _logger.LogInformation($"Address street updated succesfully for addressId {address.AddressId}");
            return NoContent();
        }
        /// <summary>
        /// Atnaujinti asmens adreso mnumerį
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdateAddressNumber")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateAddressNumber([FromForm] UpdateAddressNumber dto)
        {

            var address = _addressRepository.Get(dto.AddressId);
            if (address == null)
            {
                _logger.LogInformation($"Address not found for id: {dto.AddressId}");
                return NotFound();
            }

            address.Number = dto.AddressNumber;
            _addressRepository.Update(address);
            _logger.LogInformation($"Address number updated succesfully for addressId {address.AddressId}");
            return NoContent();
        }
    }
}
