using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Services
{
    public interface IConverter
    {
        CadModel CadToModel(Cad cad, bool firstTime = true);
        Cad ModelToCad(CadModel model, bool firstTime = true);
        Order ModelToOrder(OrderModel model, bool fisrTime = true);
        OrderModel OrderToModel(Order order, bool firstTime = true);
    }
}