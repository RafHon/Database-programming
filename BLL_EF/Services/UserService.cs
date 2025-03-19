using BLL.DTOModels;
using BLL.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF.Services
{
    public class UserService : IUserService
    {
        private readonly WebstoreContext _context;
        private static string loggedUser;

        public UserService(WebstoreContext context)
        {
            _context = context;
        }

        public UserLoginResponseDTO Login(UserLoginRequestDTO loginRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == loginRequest.Login && u.Password == loginRequest.Password);
            if(user == null)
            {
                loggedUser = string.Empty;
            }
            else
            {
                loggedUser = user.Login;
            }
            return user != null ? new UserLoginResponseDTO("token") : null;
        }

        public void Logout(int userID)
        {
            loggedUser = "";
        }
    }
}
