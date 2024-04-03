namespace MintCartWebApi.Service.Auth
{
    public interface IAuthService
    {
        public Task<(string userToken, int userId)?> Authenticate(string userEmail, string password);
    }
}
