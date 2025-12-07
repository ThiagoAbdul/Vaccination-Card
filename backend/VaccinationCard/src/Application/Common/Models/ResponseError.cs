namespace Application.Common.Models;

public class ResponseError(string Message, string? Code = null, Dictionary<string, object>? Details = null, IEnumerable<string>? errors = null)
{
    public string Message { get; set; } = Message;
    public string? Code { get; set; } = Code;
    public Dictionary<string, object>? Details { get; set; } = Details;
    public IEnumerable<string>? Errors { get; set; } = errors;
}
