using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zimozi.Assessment.BusinessModels.RequestModels.Login;
using Zimozi.Assessment.BusinessModels.ResponseModels;
using Zimozi.Assessment.BusinessModels.ResponseModels.Login;
using Zimozi.Assessment.Models;
using Zimozi.Assessment.Repository.UserRepository;

namespace Zimozi.Assessment.Services.UserService
{
    public class UserService : IUserService
    {   
        private ILogger<UserService> _logger;
        private IUserRepository _userRepository;
        private IConfiguration _configuration;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration) { 
         _userRepository = userRepository;
         _configuration = configuration;
         _logger = logger;
        }
        public async  Task<IBusinessResult<LoginResponse>> Login(LoginModel loginModel)
        {
            try
            {
                var user = await  _userRepository.Login(loginModel.Email, loginModel.Password);
                
                if (user == null)
                {
                    return new BusinessResult<LoginResponse>("There is not found any user", 401, null);
                }

                var token = GenerateJwtToken(user);

                return new BusinessResult<LoginResponse>("Login successfully", 200, new LoginResponse() { Token = token });

            } catch (Exception ex)
            {
                _logger.LogError(DateTime.Now.ToShortDateString() + " - UserService - Error: " + ex.Message);
                return new BusinessResult<LoginResponse>(ex.Message, 500, null);
            }
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user?.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
