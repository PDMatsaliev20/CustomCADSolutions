﻿using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Core.Models
{
    public class CadQueryModel
    {
        public int TotalCount { get; set; }
        public ICollection<CadModel> Cads { get; set; } = new List<CadModel>();

        public string? Category { get; set; }
        public string? Creator { get; set; }
        public string? SearchName { get; set; }
        public string? SearchCreator { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int CadsPerPage { get; set; } = 1;
        public bool Validated { get; set; } = true;
        public bool Unvalidated { get; set; } = true;
        public CadSorting Sorting { get; set; }
    }
}