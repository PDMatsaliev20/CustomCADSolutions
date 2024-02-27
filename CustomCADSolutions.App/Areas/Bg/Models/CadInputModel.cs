using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.App.Areas.Bg.Models
{
    public class CadInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително!")]
        [StringLength(CadConstants.NameMaxLength, 
            MinimumLength = CadConstants.NameMinLength, 
            ErrorMessage = "Дължината на името трябва да бъде между {2} и {1} символа")]
        [Display(Name = "Име")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Категорията е задължителна!")]
        [Display(Name = "Категория")]
        public Category Category { get; set; } = null!;

        [Required(ErrorMessage = "3D Моделът е задължителен!")]
        [Display(Name = "3D Модел")]
        public IFormFile CadFile { get; set; } = null!;
        
        [Range(CadConstants.XMin, CadConstants.XMax, ErrorMessage = "{0} трябва да бъде между {2} и {1}")]
        public short X { get; set; }
        
        [Range(CadConstants.YMin, CadConstants.YMax, ErrorMessage = "{0} трябва да бъде между {2} и {1}")]
        public short Y { get; set; }

        [Range(CadConstants.ZMin, CadConstants.ZMax, ErrorMessage = "{0} трябва да бъде между {2} и {1}")]
        public short Z { get; set; }

        [Range(0, CadConstants.SpinFactorMax * 100, ErrorMessage = "{0} трябва да бъде между {2} и {1}")]
        [Display(Name = "Скорост")]
        public int SpinFactor { get; set; }

        [RegularExpression("[xyz]", ErrorMessage = "{0} трябва да бъде x, y или z")]
        [Display(Name = "Ос на въртеж")]
        public char? SpinAxis { get; set; }
    }
}
