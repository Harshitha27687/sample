using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Experity.SprintDashboard.API.Streams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using Experity.SprintDashboard.Data.DTOs;

namespace Experity.SprintDashboard.API.Controllers
{
    [SwaggerTag("Info", Description = "Version Information Controller")]
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IStreamProvider _streamProvider;
        private const string infoError = "An error occurred when attempting to get the version information.";
        private ILogger<InfoController> _logger { get; }

        public InfoController(ILogger<InfoController> logger, IStreamProvider streamProvider)
        {
            _streamProvider = streamProvider;
            _logger = logger;
        }

        [AllowAnonymous]
        [Description("Gets the current version info")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(VersionInfoDto))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = infoError)]
        [HttpGet, Route("GetVersionInfo")]
        public IActionResult GetVersionInfo()
        {
            var versionInfo = new VersionInfoDto();
            using (var reader = new StreamReader(_streamProvider.GetStream()))
            {
                var version = reader.ReadLine();
                versionInfo.Version = version;
            }
            versionInfo.VersionBase = versionInfo.Version.Substring(0, 4);

            _logger.LogInformation("Finished GetVersionInfo");
            return Ok(versionInfo);

        }

        [AllowAnonymous]
        [Description("Performs a basic health check")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = infoError)]
        [HttpGet, Route("HealthCheck")]
        public IActionResult HealthCheck()
        {

            var response = "Ok";
            return Ok(response);

        }
    }
}
