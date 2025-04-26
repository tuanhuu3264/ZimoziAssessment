using Zimozi.Assessment.BusinessModels.RequestModels.Login;
using Zimozi.Assessment.BusinessModels.ResponseModels;
using Zimozi.Assessment.BusinessModels.ResponseModels.Login;

namespace Zimozi.Assessment.Services.UserService
{
    public interface IUserService
    {
       public Task<IBusinessResult<LoginResponse>> Login(LoginModel loginModel);
    }
}
