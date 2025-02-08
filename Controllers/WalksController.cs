using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZ_Walk.Models.Domain;
using NZ_Walk.Models.DTO;
using NZ_Walk.Repositories;

namespace NZ_Walk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddWalksRequestDto addWalksRequestDto)
        {
            var walkDomain = _mapper.Map<Walk>(addWalksRequestDto);
            await _walkRepository.CreateAsync(walkDomain);

           

            return Ok(_mapper.Map<Walk>(walkDomain));
        }

        [HttpGet]
        //https://localhost:7222/api/Walks?filterOn=name&filterQuery=makara
        public async Task<IActionResult> GetAll([FromQuery]string?filterOn,[FromQuery]string? filterQuery,
           [FromQuery] string? sortBy,[FromQuery]bool? isAscending,
           [FromQuery]int pageNumber=1,int pageSize=1000)
        {
            var walkDomain = await _walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,
                isAscending ?? true,pageNumber,pageSize);
            //create exception to test 
            throw new Exception("This is a new exception");

            return Ok(_mapper.Map<List<WalkDto>>(walkDomain));
        }

        [HttpGet("{id}",Name ="GetById")]
        
        public async Task<IActionResult> GetById(Guid id)
        {
            var walkDomain = await _walkRepository.GetByIdAsync(id);

            if(walkDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walkDomain));
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute]Guid id,UpdateWalkDto updateWalkDto)
        {
            var walkDomain = _mapper.Map<Walk>(updateWalkDto);

            if(walkDomain == null)
            {
                return NotFound();
            }

            walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

            return Ok(_mapper.Map<WalkDto>(walkDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await _walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walkDomain));
        }
    }
}
