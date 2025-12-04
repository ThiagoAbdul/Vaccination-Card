

namespace Application.Common.Models;

public sealed record ResponseError(string Message, string? Code = null, Dictionary<string, object>? Details = null);
