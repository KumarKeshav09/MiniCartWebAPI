using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MintCartWebApi.Common;
using MintCartWebApi.Data;
using MintCartWebApi.Helper;
using MintCartWebApi.LoggerService;
using MintCartWebApi.ModelDto;
using MintCartWebApi.Utilities;
using System;

namespace MintCartWebApi.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(ApplicationDbContext context, ILoggerManager logger
            , IMapper mapper, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<int?> createUserAsync(RegisterUserDto userDto)
        {
            try
            {
                var exUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userDto.UserEmail && u.UserPhone ==userDto.UserPhone);
                if (exUser != null)
                {
                    _logger.LogWarning($"user already exist by {exUser.UserEmail} mail id");
                    return null;
                }
                var filePath = "/";
                if (userDto.ProfileImage != null && userDto.ProfileImage.Length > 0)
                {
                    filePath = await FileHelper.SaveFileAsync(userDto.ProfileImage, _webHostEnvironment.WebRootPath);
                }
                var newUser = new DBModels.User();
                newUser.ProfileImageUrl = filePath;
                newUser.UserEmail = userDto.UserEmail;
                newUser.UserPhone = userDto.UserPhone;
                newUser.UserName = userDto.UserName;
                newUser.UserRole = Convert.ToInt32(Enums.Roles.User);
                newUser.CreatedDate = DateTime.Now;
                newUser.UpdatedDate = DateTime.Now;
                var _pepper = _configuration["AppSettings:pepper"];
                var _iteration = _configuration["AppSettings:iteration"];
                newUser.PasswordSalt = PasswordHasher.GenerateSalt();
                newUser.PasswordHash = PasswordHasher.ComputeHash(userDto.Password, newUser.PasswordSalt, _pepper, Convert.ToInt32(_iteration));

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                _logger.LogInfo($"user data is successfully save ,with userId{newUser.UserId}");
                return newUser.UserId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"method createUserAsync {userDto.ToString()} , {ex.Message}");
                throw;
            }
        }
    }
}
