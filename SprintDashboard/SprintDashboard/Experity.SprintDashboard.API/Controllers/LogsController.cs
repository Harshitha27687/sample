using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Experity.SprintDashboard.API.Configuration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Targets;
using NSwag.Annotations;

namespace Experity.SprintDashboard.API.Controllers
{
    [SwaggerTag("Logs", Description = "API to retrieve service logs")]
    [Route("logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;
        private readonly AppSettings _configuration;
        private const string LogError = "An error occured getting logs from memory.";

        public LogsController(ILogger<LogsController> logger, AppSettings configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET logs
        [AllowAnonymous]
        [Description("Gets logged messages")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = LogError)]
        [HttpGet]
        public IActionResult GetLogs()
        {
            if (!_configuration.EnableLoggingEndpoint)
            {
                return NotFound();
            }
            if (LogManager.Configuration == null)
            {
                return BadRequest();
            }
            var target = LogManager.Configuration.FindTargetByName<MemoryTarget>("MemoryLogger");
            if (target == null)
            {
                return BadRequest();
            }
            var messages = target.Logs.Reverse();

            return Ok(string.Join("", messages));
        }

        // GET logs/flush
        [AllowAnonymous]
        [Description("Retrieve and clear logs")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = LogError)]
        [HttpGet("flush")]
        public IActionResult FlushLogs()
        {
            if (!_configuration.EnableLoggingEndpoint)
            {
                return NotFound();
            }
            if (LogManager.Configuration == null)
            {
                return BadRequest();
            }
            var target = LogManager.Configuration.FindTargetByName<MemoryTarget>("MemoryLogger");
            if (target == null)
            {
                return BadRequest();
            }
            var messages = target.Logs.Reverse().ToList();

            target.Logs.Clear();

            return Ok(string.Join("", messages));
        }
    }
}
