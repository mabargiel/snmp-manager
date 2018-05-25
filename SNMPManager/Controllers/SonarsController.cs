using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Sonars.Exceptions;
using SNMPManager.Core.Sonars.Repositories;
using SNMPManager.Models;

namespace SNMPManager.Controllers
{
    [Produces("application/json")]
    [Route("api/sonars")]
    public class SonarsController : Controller
    {
        private readonly SonarsRepository _sonarsRepository;
        private readonly IMapper _mapper;

        public SonarsController(SonarsRepository sonarsRepository, IMapper mapper)
        {
            _sonarsRepository = sonarsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetSonars()
        {
            var result = _sonarsRepository.GetAllSonars()
                .Select(x => _mapper.Map<SonarDto>(x));

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}", Name="GetSonar")]
        public IActionResult GetSonar(string id)
        {
            var guid = Guid.ParseExact(id, "N");
            var sonar = _sonarsRepository.GetSonar(guid);
            var result = _mapper.Map<SonarDto>(sonar);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSonar([FromBody] CreateSonarDto sonarDto)
        {
            if (sonarDto == null)
                return BadRequest("Post body could not be deserialized or was null");

            var sonar = _mapper.Map<Sonar>(sonarDto);
            sonar.IsActive = true;

            _sonarsRepository.CreateSonar(sonar);
            await _sonarsRepository.SaveChangesAsync();

            return CreatedAtRoute("GetSonar", new {Controller = "Sonars", id = sonar.Id.ToString("N")},
                _mapper.Map<SonarDto>(sonar));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> ReplaceSonar(string id, [FromBody] CreateSonarDto sonarDto)
        {
            if (sonarDto == null)
                return BadRequest("Put body could not be deserialized or was null");

            var sonar = _mapper.Map<Sonar>(sonarDto);
            sonar.Id = Guid.ParseExact(id, "N");
            sonar.IsActive = true;

            try
            {
                _sonarsRepository.UpdateSonar(sonar);

                await _sonarsRepository.SaveChangesAsync();
                var result = _mapper.Map<SonarDto>(sonar);
                return CreatedAtRoute("GetSonar", new { Controller = "Sonars", id }, result);
            }
            catch (SonarMissingException)
            {
                return NotFound();
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateSonar(string id, [FromBody] JsonPatchDocument<CreateSonarDto> sonarPatch)
        {
            if (sonarPatch == null)
                return BadRequest("Patch body could not be deserialized or was null");

            var sonarId = Guid.ParseExact(id, "N");
            var sonar = _sonarsRepository.GetSonar(sonarId);

            if (sonar == null)
                return NotFound();

            var toPatch = new CreateSonarDto
            {
                Title = sonar.Title,
                IpAddress = sonar.IpAddress,
                Mib = sonar.Mib,
                IsActive = true
            };

            sonarPatch.ApplyTo(toPatch);

            var patchedSonar = _mapper.Map<Sonar>(toPatch);

            try
            {
                _sonarsRepository.UpdateSonar(patchedSonar);
                await _sonarsRepository.SaveChangesAsync();

                return CreatedAtRoute("GetSonar", new { Controller = "Sonars", id }, _mapper.Map<SonarDto>(sonar));
            }
            catch (SonarMissingException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSonar(string id)
        {
            var sonarId = Guid.ParseExact(id, "N");

            try
            {
                _sonarsRepository.RemoveSonar(sonarId);
                await _sonarsRepository.SaveChangesAsync();

                return Ok();
            }
            catch (SonarMissingException)
            {
                return NotFound();
            }
        }
    }
}