namespace MintCartWebApi.Service.Auth
{
    public interface IAuthService
    {
        public Task<string> Authenticate(string userEmail, string password);
    }
}
