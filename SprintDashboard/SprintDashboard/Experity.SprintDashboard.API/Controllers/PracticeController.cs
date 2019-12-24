using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using Experity.SprintDashboard.API.Caches;
using Experity.SprintDashboard.API.Middlewares;
using Experity.SprintDashboard.API.Services;
using Experity.SprintDashboard.Data.DTOs;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using Experity.SprintDashboard.Models;
using Microsoft.AspNetCore.Authorization;

namespace Experity.SprintDashboard.API.Controllers
{
    [SwaggerTag("Practice", Description = "Endpoints to retrieve data for template components")]
    [ApiController]
    public class PracticeController : ControllerBase
    {
        private IPracticeEnvironmentCache _practiceCache;
        private readonly Func<string, IClinicProvider> _clinicProviderFactory;
        private ILogger<PracticeController> _logger { get; }
        private const string PracticeErrorTitle = "An error title specific to your controller.";

        public PracticeController(IPracticeEnvironmentCache practiceEnvironmentCache, Func<string, IClinicProvider> clinicProviderFactory, ILogger<PracticeController> logger)
        {
            _practiceCache = practiceEnvironmentCache;
            _clinicProviderFactory = clinicProviderFactory;
            _logger = logger;
        }

        [SwaggerResponse(StatusCodes.Status200OK, typeof(string))]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(IActionResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse))]
        [HttpGet("practices/{practiceName}")]
        public async Task<IActionResult> Get(string practiceName)
        {
            //TODO Remove in your implementation
            //This is an example of a simple fetch.  Controllers are meant to handle routing to other resources
            //For more complex situations place logic in Service class in Service Folder and use controller as pass through.
            var practice = await _practiceCache.GetPracticeAsync(practiceName);
            var clinicDataProvider = _clinicProviderFactory(practice.Environment);

            var clinics = await new ClinicService(clinicDataProvider).GetClinicsAsync(practice.PracticePk);

            return Ok(clinics);
        }

        [Authorize("Bearer")]
        [ServiceFilter(typeof(ValidateTokenFilter))]
        [Description("Gets all clinics for a practice")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(List<ClinicDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))]
        [HttpGet("practices/{practiceName}/clinics")]
        public async Task<IActionResult> GetClinics(string practiceName)
        {
            //TODO Remove in your implementation
            var clinics = new List<ClinicDto>();
            return Ok(clinics);
        }
    }
}
