using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record OrderResponseDTO(int Id, double TotalValue, bool IsPaid, DateTime Date);
}
