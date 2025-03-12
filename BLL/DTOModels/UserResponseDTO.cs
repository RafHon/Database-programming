using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record UserResponseDTO(int Id, string Login, bool IsActive);
}
