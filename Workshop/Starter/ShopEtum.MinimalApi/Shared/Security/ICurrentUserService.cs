namespace ShopEtum.MinimalApi.Shared.Security;

public interface ICurrentUserService
{
    string UserId { get; }
}