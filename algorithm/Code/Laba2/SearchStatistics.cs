namespace Laba2;

public class SearchStatistics
{
    public int Steps { get; set; }
    public int DeadEnds { get; set; }
    public int GeneratedStates { get; set; }
    public int StatesInMemory { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public bool TimeLimitExceeded { get; set; }
    public bool MemoryLimitExceeded { get; set; }
}