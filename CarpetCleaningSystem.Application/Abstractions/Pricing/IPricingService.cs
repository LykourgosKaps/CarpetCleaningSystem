using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Abstractions.Services
{
    public interface IPricingService
    {
        decimal CalculateItemPrice(ItemPricingInput input);

        decimal CalculateOrderTotal(IEnumerable<ItemPricingInput> items);
    }

}
