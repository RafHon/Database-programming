using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record ProductResponseDTO(int ID, string Name, double Price, string Image, bool IsActive, string GroupName);
}
