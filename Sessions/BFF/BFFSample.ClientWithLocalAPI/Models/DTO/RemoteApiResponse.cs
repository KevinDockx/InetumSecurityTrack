using System.Security.Claims;

namespace BFFSample.ClientWithLocalAPI.Models.DTO;
public class RemoteApiResponse
{
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ClaimData> Claims { get; set; } = [];
}

public class ClaimData
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
} 