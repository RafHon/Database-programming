using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record UserRequestDTO(string Login, string Password, string Type, int? GroupID);
}
