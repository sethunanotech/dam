using AutoMapper;
using DAM.Application.Contracts;
using DAM.Application.DTO.Requests;
using DAM.Domain.Entities;
using DAM.Persistence.Repositories;
using Hangfire;
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

        // Variables 
        private readonly string _cacheKey = "organization";
        //End

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
            _logger.LogInformation("Request for All Organization");

            var organizations = await _organizationRepository.GetAllAsync(true, _cacheKey);
            return Ok(organizations);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            _logger.LogInformation($"Request Organization for {Id}");
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
            if (organizationRequest == null)
            {
                _logger.LogInformation("The Put request is contains null");
                return BadRequest("The request parameter is null");
            }

            if (!Guid.TryParse(Id.ToString(), out _)) {
                _logger.LogInformation($"The input value is not valid {Id}");
                return BadRequest(Id);
            }

            try
            {
                //Map the incoming request object with the expected Organization entity
                var organization = _mapper.Map<OrganizationRequest, Organization>(organizationRequest);

                organization.LastModifiedBy = "Sethu";
                organization.LastModifiedOn = DateTime.UtcNow;
                await _organizationRepository.UpdateAsync(organization, true, _cacheKey);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
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

            await _organizationRepository.AddAsync(organization, true, _cacheKey);
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
            await _organizationRepository.RemoveByIdAsync(organization.Id, true, _cacheKey);
            return Ok(organization);
        }
    }
}