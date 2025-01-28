using Hangfire.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ScheduleJob;
using System.ComponentModel.DataAnnotations;

namespace Hangfire.API.Controllers
{
    [ApiController]
    [Route("api/schedule-jobs")]
    public class ScheduleJobController: ControllerBase
    {
        private IBackgroundJobService _backgroundJobService;

        public ScheduleJobController(IBackgroundJobService backgroundJobService)
        {
            _backgroundJobService = backgroundJobService;
        }
        [HttpPost]
        [Route(template: "send-email-reminder-checkout-order")]
        public IActionResult SendReminderCheckoutOrderEmail([FromBody] ReminderCheckoutOrderDto model)
        {
            var jobId = _backgroundJobService.SendEmailContent(model.email, model.subject, model.emailContent,
                model.enqueueAt);

            return Ok(jobId);
        }

        [HttpDelete]
        [Route("delete/jobId/{id}")]
        public IActionResult DeleteJobId([Required] string id)
        {
            var result = _backgroundJobService.ScheduleJob.Delete(id);

            if (result)
            {
                return Ok(new { message = $"Job {id} deleted successfully" });
            }

            return NotFound(new { message = $"Job {id} not found or could not be deleted" });
        }

    }
}
