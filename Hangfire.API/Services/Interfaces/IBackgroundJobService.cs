using Constracts.ScheduleJob;

namespace Hangfire.API.Services.Interfaces
{
    public interface IBackgroundJobService
    {

        IScheduleJobService ScheduleJob { get; }

        string SendEmailContent(string email, string subject, string content, DateTimeOffset enqueueAt);

    }
}
