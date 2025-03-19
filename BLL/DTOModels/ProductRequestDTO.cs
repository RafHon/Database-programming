using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record ProductRequestDTO(string Name, double Price, string Image, bool IsActive, int? GroupID);
}
