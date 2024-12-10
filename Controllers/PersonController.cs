using Egzaminas.Dtos.Requests;
using Egzaminas.Interfaces;
using Egzaminas.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Egzaminas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IPersonMapper _mapper;
        private readonly IAccountService _service;

        public PersonController(ILogger<PersonController> logger,
            IPersonRepository repository,
            IJwtService jwtService,
            IPersonMapper mapper,
            IAccountService service)
        {
            _logger = logger;
            _repository = repository;
            _jwtService = jwtService;
            _mapper = mapper;
            _service = service;
        }

        [HttpPost("SignUp")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreatePerson(PersonRequestDto req)
        {
            var per = _repository.Get(req.AccountId);
            if (per == null)
                per.Id = Guid.NewGuid();
            _mapper.Project(per, req);
            var userId = _repository.Update(per);

            return Created("", new { id = userId });
        }
    }
}
