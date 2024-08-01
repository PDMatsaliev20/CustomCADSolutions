﻿using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.OrderConstants;
using static CustomCADs.Domain.DataConstants.CategoryConstants;

namespace CustomCADs.API.Models.Orders
{
    public class OrderImportDTO
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(IdMin, IdMax, ErrorMessage = RangeErrorMessage)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public bool ShouldBeDelivered { get; set; } = false;
    }
}
