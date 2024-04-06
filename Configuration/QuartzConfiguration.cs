using Quartz;

public class QuartzConfiguration {

    public static void ConfigureQuartz(WebApplicationBuilder builder) {
        ConfigureQuartzWithCron<SampleTask>(builder.Services, "SampleTask", "0/5 * * * * ?");
        ConfigureQuartzWithCron<ProductsTaskProductionTask>(builder.Services, "ProductsTaskProductionTask", "0/5 * * * * ?");
    }

    // private static void DefineQuartzWithCron(IServiceCollectionQuartzConfigurator quartz, Type jobType, string jobName, string cronExpression)
    // {
    //     JobKey jobKey = new JobKey(jobName);
    //     quartz.AddJob(jobType, opts => opts.WithIdentity(jobKey));

    //     quartz.AddTrigger(options => 
    //             options.ForJob(jobKey)
    //                 .WithIdentity(jobName + "-Trigger")
    //                 .WithCronSchedule(cronExpression));
    // }

    public static void ConfigureQuartzWithCron<TJob>(IServiceCollection services, string jobName, string cronExpression)
        where TJob : class, IJob
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey(jobName);
            q.AddJob<TJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-Trigger")
                .WithCronSchedule(cronExpression));
        });

        // Ensure Quartz Hosted Service is added to manage the job lifecycle
        // services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

}