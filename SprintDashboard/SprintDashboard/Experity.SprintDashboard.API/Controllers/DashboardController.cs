using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Experity.SprintDashboard.API.Services;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using Experity.SprintDashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Experity.SprintDashboard.API.Controllers
{
    [SwaggerTag("Dashboard", Description = "Endpoints to retrieve data related to TargetProcess dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly Func<string, ITeamProvider> _teamProviderFactory;
        private ILogger<DashboardController> _logger { get; }
        private const string PracticeErrorTitle = "An error title specific to your controller.";

        public DashboardController(Func<string, ITeamProvider> teamProviderFactory, ILogger<DashboardController> logger)
        {
            _teamProviderFactory = teamProviderFactory;
            _logger = logger;
        }

        [SwaggerResponse(StatusCodes.Status200OK, typeof(string))]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(IActionResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse))]
        [HttpGet("sprintdashboard/{teams}")]
        public async Task<IActionResult> GetTeams()
        {
            var teamDataProvider = _teamProviderFactory(string.Empty);
            var teams = await new TeamService(teamDataProvider).GetTeamsAsync();

            return Ok(teams);
        }
    }
}