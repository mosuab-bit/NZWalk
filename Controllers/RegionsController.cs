using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZ_Walk.Data;
using NZ_Walk.Models.Domain;
using NZ_Walk.Models.DTO;
using NZ_Walk.Repositories;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace NZ_Walk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(NZWalksContext dbContext, IRegionRepository regionRepository
            ,IMapper mapper,ILogger<RegionsController> logger)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAllRegions()
        {

            //_logger.LogInformation("GetAll Regions Action Method was invoked");
            //_logger.LogWarning("this is log warnning");
            //_logger.LogError("This is log error");

            try
            {
                //throw new Exception("This is a custom exception");

                // Get Data from Database-DomainModels
                var regions = await _regionRepository.GetAllAsync();

                //here we want to register all regions retrieve from DB and display it as JSON object.
                //we will use system.text to convert domain object to JSON
                _logger.LogInformation($"Finished GetAllRegions request with data:{JsonSerializer.Serialize(regions)} ");

                return Ok(_mapper.Map<List<RegionDTO>>(regions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            
          
        }

        [HttpGet("{id}",Name ="GetRegionById")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            // Get Data from Database-DomainModels
            var region = await _regionRepository.GetIdAsync(id);
            if(region == null)
            {
                return NotFound();
            }

           
            return Ok(_mapper.Map<RegionDTO>(region));
        }

        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //here it is a convert Dto to Domain not need to await
            var regionDomain = _mapper.Map<Region>(addRegionRequestDto);

            regionDomain = await _regionRepository.CreateAsync(regionDomain);

            var regionDto = _mapper.Map<RegionDTO>(regionDomain);

            return CreatedAtAction(nameof(GetRegionById), new RegionDTO { Id = regionDto.Id }, regionDto);
        }

        [HttpPut("{id}",Name ="Update")]
        //[Authorize(Roles = "Writer")]

        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            //Map Dto to Domain
            var regionDomain = _mapper.Map<Region>(updateRegionRequestDto);

            regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);
            if(regionDomain == null)
            {
                return NotFound();
            }


            return Ok(_mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpDelete("{id}",Name ="Delete")]
        //[Authorize(Roles = "Writer")]
        public async  Task<IActionResult> Delete(Guid id)
        {
            var regionDomain = await _regionRepository.DeleteAsync(id);
            if(regionDomain == null)
            {
                return NotFound();
            }

            //here it is optional to convert domain to Dto and return it to user you can 
            //return ok() method empty
            

            return Ok(_mapper.Map<RegionDTO>(regionDomain));

            //if you do'nt convert domain to dto 
            //return Ok();
        }

    }
}
