using Quartz;

public class QuartzConfiguration {

    public static void ConfigureQuartz(WebApplicationBuilder builder) {
        Console.Write("Job Configuration");
        ConfigureQuartzWithCron<SampleTask>(builder.Services, "SampleTask", "0/5 * * * * ?");
        ConfigureQuartzWithCron<ProductsTaskProductionTask>(builder.Services, "ProductsTaskProductionTask", "0/5 * * * * ?");

        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }


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
    }

}