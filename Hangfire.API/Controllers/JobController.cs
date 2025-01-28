
using Constracts.ScheduleJob;
using Microsoft.AspNetCore.Mvc;
using Serilog;
namespace Hangfire.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController: ControllerBase
    {
        private IScheduleJobService _jobService;
        private Serilog.ILogger _logger;

        public JobController(IScheduleJobService jobService, Serilog.ILogger logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [HttpPost]
        [Route(template: "[action]")]
        public IActionResult Welcome()
        {
            var jobId = _jobService.Enqueue(() => ResponseWelcome("Welcome to Hangfire API"));
            return Ok($"Job ID: {jobId} - Enqueue Job");
        }

        [HttpPost]
        [Route(template: "[action]")]
        public IActionResult DelayedWelcome()
        {
            var seconds = 5;
            var jobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"),
                delay: TimeSpan.FromSeconds(seconds));
            return Ok($"Job ID: {jobId} - Enqueue Job");
        }

        [HttpPost]
        [Route(template: "[action]")]
        public IActionResult WelcomeAt()
        {
            var enqueueAt = DateTimeOffset.UtcNow.AddSeconds(10);
            var jobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"),
                enqueueAt);
            return Ok($"Job ID: {jobId} - Enqueue Job");
        }

        [HttpPost]
        [Route(template: "[action]")]
        public IActionResult ConfirmedWelcome()
        {
            const int timeInSeconds = 5;
            var parentJobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"),
                delay: TimeSpan.FromSeconds(5));

            var jobId = _jobService.ContinueQueueWith(parentJobId,
                () => ResponseWelcome("Welcome message is sent"));

            return Ok($"Job ID: {jobId} - Enqueue Job");
        }


        [NonAction]
        public void ResponseWelcome(string text)
        {
            _logger.Information(text);
        }
    }
}
