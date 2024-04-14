﻿namespace CustomCADSolutions.Core.Mappings.CadDTOs
{
    public class CadQueryDTO
    {
        public int TotalCount { get; set;}

        public ICollection<CadExportDTO> Cads { get; set; } = Array.Empty<CadExportDTO>();
    }
}
