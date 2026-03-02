namespace TaskPlaApplication.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime GeneratedTime { get; set; }
        public TimeSpan ExecutionDuration { get; set; }
        public int ItemsProcessed { get; set; }
        public string OutputFile { get; set; } = string.Empty;
    }
}