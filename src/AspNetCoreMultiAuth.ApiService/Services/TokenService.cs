namespace AspNetCoreMultiAuth.ApiService.Services;

public interface ITokenService
{
    ValueTask<CustomToken> GetAsync(string customToken);
}

public class TokenService : ITokenService
{
    public ValueTask<CustomToken> GetAsync(string customToken) => ValueTask.FromResult(new CustomToken { UserId = Guid.NewGuid().ToString() });

}

public class CustomToken
{
    public string UserId { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
