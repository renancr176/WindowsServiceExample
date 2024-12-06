using InnokuMailSender.Scheduler.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Reflection;
using static Quartz.Logging.OperationName;

namespace InnokuMailSender.Scheduler;

public static class Scheduler
{
    public static void AddJobs(this IServiceCollection services)
    {
        services.AddTransient<SendMailJob>();
    }

    public static void AddScheduler(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJobs();

        services.Configure<QuartzOptions>(options =>
        {
            options.Scheduling.IgnoreDuplicates = true; // default: false
            options.Scheduling.OverWriteExistingData = true; // default: true
        });

        //See examples here https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/microsoft-di-integration.html#di-aware-job-factories
        //Cron Expressions https://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/crontriggers.html
        //Cron expression generator https://crontab.cronhub.io/
        services.AddQuartz(q =>
        {
            #region Config

            q.SchedulerId = Assembly.GetCallingAssembly()?.GetName()?.Name ?? "InnokuMailSender";
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 10;
            });
            q.UseTimeZoneConverter();

            #endregion

            #region SendMailJob

            q.AddJob<SendMailJob>(x => x
                .StoreDurably()
                .WithIdentity(nameof(SendMailJob)));

            q.ScheduleJob<SendMailJob>(trigger => trigger
                .WithDescription($"{nameof(SendMailJob)} on start")
                .WithDailyTimeIntervalSchedule(x =>
                {
                    x.WithInterval(1, IntervalUnit.Second);
                    x.WithRepeatCount(0);
                })
                .StartNow());

            // Every day at 06:00 AM
            q.ScheduleJob<SendMailJob>(trigger => trigger
                .WithDescription($"{nameof(SendMailJob)} at 06:00 AM")
                .WithCronSchedule("0 0 6 ? * * *")
                .StartNow());
            #endregion
        });

        services.AddQuartzHostedService(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });
    }
}