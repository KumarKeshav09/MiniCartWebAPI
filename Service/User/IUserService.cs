using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Service
{
    public interface IUserService
    {
        public Task<int?> createUserAsync(RegisterUserDto userDto);
        public Task<DBModels.User?> getUserAsync(int id);
    }
}
