using AutoMapper;
using DAM.Application.Contracts;
using DAM.Application.DTO.Requests;
using DAM.Domain.Entities;
using DAM.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DAM.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly IOrganization _organizationRepository;

        private readonly IMapper _mapper;

        public OrganizationController(IOrganization organizationRepository, 
            IMapper mapper,
            ILogger<OrganizationController> logger)
        {
            _logger = logger;
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            return Ok(organizations);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            var organization = await _organizationRepository.GetByIdAsync(Id);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(Guid Id, OrganizationRequest organizationRequest)
        {
            var organization =  _mapper.Map<OrganizationRequest, Organization>(organizationRequest);

            await _organizationRepository.UpdateAsync(organization);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrganizationRequest organizationRequest)
        {
            //Magic values to be replaced by the constants / Session Variables
            var createdBy = "Sethu";
            var createdOn = DateTime.UtcNow;
            var organization = _mapper.Map<OrganizationRequest, Organization>(organizationRequest);

            organization.CreatedBy = createdBy;
            organization.CreatedOn = createdOn;

            await _organizationRepository.AddAsync(organization);
            return CreatedAtAction("Get", new {Id =  organization.Id}, organization);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var organization = await _organizationRepository.GetByIdAsync(Id);
            if (organization == null)
            {
                return NotFound();
            }
            await _organizationRepository.RemoveByIdAsync(organization.Id);
            return Ok(organization);
        }
    }
}