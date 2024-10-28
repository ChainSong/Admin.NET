namespace Admin.NET.MAUI;

public static class TaskHelpers
{
    public static void FireAndForget(this Task task, bool configureAwait = false)
    {
        task.ConfigureAwait(configureAwait);
    }
}