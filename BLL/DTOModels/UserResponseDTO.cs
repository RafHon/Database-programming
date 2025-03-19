using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record UserResponseDTO(int ID, string Login, string Type, bool IsActive, string GroupName);
}
