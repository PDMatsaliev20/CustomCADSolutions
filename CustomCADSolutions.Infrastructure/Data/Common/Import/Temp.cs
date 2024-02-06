using CustomCADSolutions.Infrastructure.Data.Common.Import;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.Text.Json;

namespace CustomCADSolutions.Infrastructure.Data.Common
{
    public class Temp
    {
        public static Cad[] ImportCads()
        {
            string json = File.ReadAllText("categories.json");
            CadDTO[] cadDTOs = JsonSerializer.Deserialize<CadDTO[]>(json)!;
            Cad[] cads = new Cad[cadDTOs.Length];

            for (int i = 0; i < cadDTOs.Length; i++)
            {
                cads[i] = new Cad()
                {
                    Id = i + 1,
                    Name = cadDTOs[i].Name,
                    Url = cadDTOs[i].Url,
                    Category = (Category)(i / 15 + 1),
                };
            }

            return cads;
        }
    }
}
