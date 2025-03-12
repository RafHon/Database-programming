using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record OrderPositionResponseDTO(int ProductId, string ProductName, double Price, int Amount, double TotalValue);
}
