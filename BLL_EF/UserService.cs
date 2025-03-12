using BLL.DTOModels;
using BLL.ServiceInterfaces;
using DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF
{
    public class UserService : IUserService
    {
        private readonly WebStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(WebStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserResponseDTO Login(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user == null)
                throw new Exception("User not found");

            if (user.Password != password)
                throw new Exception("Incorrect password");

            return new UserResponseDTO(user.ID, user.Login, user.IsActive);
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }

}
