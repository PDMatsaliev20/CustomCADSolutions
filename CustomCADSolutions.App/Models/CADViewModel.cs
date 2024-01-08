using CustomCADSolutions.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.App.Models
{
    public class CADViewModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string CreatedOn { get; set; }
        public string Url { get; set; }
    }
}
