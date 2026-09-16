namespace Daymark.Api.Models;

public record DaymarkTask(int Id, string Title, string Tag, string Time, bool Completed);

public record CreateTaskRequest(string Title, string? Tag, string? Time);

public record UpdateTaskRequest(string? Title, string? Tag, string? Time, bool? Completed);
