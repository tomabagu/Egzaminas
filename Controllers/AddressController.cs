using Egzaminas.Dtos.Requests;
using Egzaminas.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Egzaminas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AddressController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IAddressMapper _mapper;

        public AddressController(ILogger<AddressController> logger,
            IPersonRepository personRepository,
            IAddressRepository addressRepository,
            IAddressMapper mapper)
        {
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
                return NotFound();
            }

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
                return BadRequest("Invalid personId");
            }

            if (person.Address != null)
            {
                return Conflict("Address with this personId already exists");
            }

            var address = _mapper.Map(req);

            if (person.Address == null)
            {
                address.AddressId = Guid.NewGuid();
                //person.AddressId = address.Id;
                person.Address = address;
            }
            _addressRepository.Add(address);
            _personRepository.Update(person);
           
            return Created(nameof(address), new { id = address.AddressId});
        }
        /// <summary>
        /// Atnaujinti vartotojo adreso informaciją
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("UpdateAddress/{addressId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateAddress(Guid addressId, [FromForm] AddressRequestDto req)
        {
            var address = _addressRepository.Get(addressId);
            if (address == null)
            {
                return BadRequest("Invalid addressId");
            }

            _mapper.Project(address, req);
            _addressRepository.Update(address);
            return NoContent();
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
                return NotFound();
            }

            address.City = dto.AddressCity;
            _addressRepository.Update(address);

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
                return NotFound();
            }

            address.Street = dto.AddressStreet;
            _addressRepository.Update(address);

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
                return NotFound();
            }

            address.Number = dto.AddressNumber;
            _addressRepository.Update(address);

            return NoContent();
        }
    }
}
