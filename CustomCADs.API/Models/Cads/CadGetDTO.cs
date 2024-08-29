﻿using CustomCADs.API.Models.Others;

namespace CustomCADs.API.Models.Cads
{
    public class CadGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string CreationDate { get; set; } = null!;
        public decimal Price { get; set; }
        public string CadPath { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public double[] Coords { get; set; } = [];
        public CoordinatesDTO CamCoordinates { get; set; } = null!;
        public CoordinatesDTO PanCoordinates { get; set; } = null!;
        public int Fov { get; set; } = 90;
        public string Status { get; set; } = null!;
        public int OrdersCount { get; set; }
        public CategoryDTO Category { get; set; } = null!;
    }
}
