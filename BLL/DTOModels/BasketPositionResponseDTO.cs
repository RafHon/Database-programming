using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public record BasketPositionResponseDTO(int ProductId, int Amount, string ProductName, double Price);
}
