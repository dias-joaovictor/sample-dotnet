using Quartz;

public class SampleTask : IJob
{

    private readonly ILogger<SampleTask> _logger;

    public SampleTask(ILogger<SampleTask> logger) 
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Task has been called at " + DateTime.Now);
        return Task.CompletedTask;
    }
}