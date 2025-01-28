using Constracts.ScheduleJob;
using Constracts.Services;
using Hangfire.API.Services.Interfaces;
using Shared.Services.Email;
using ILogger = Serilog.ILogger;
namespace Hangfire.API.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private IScheduleJobService _scheduleJobService;

        private ISMTPEmailServices _mailServices;

        private ILogger _logger;

        public IScheduleJobService ScheduleJob => _scheduleJobService;

        public BackgroundJobService(IScheduleJobService scheduleJobService, ISMTPEmailServices mailService, ILogger logger)
        {
            _scheduleJobService = scheduleJobService;
            _mailServices = mailService;
            _logger = logger;
        }
        public string SendEmailContent(string email, string subject, string emailContent, DateTimeOffset enqueueAt)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = email,
                Body = emailContent,
                Subject = subject
            };

            try
            {
                var jobId = _scheduleJobService.Schedule(() => SendEmailAsyncWrapper(emailRequest), enqueueAt);
                _logger.Information($"jobId {jobId} will execute at {enqueueAt}");
                return jobId;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        public Task SendEmailAsyncWrapper(MailRequest emailRequest)
        {
            return _mailServices.SendEmailAsync(emailRequest);
        }
    }
}
