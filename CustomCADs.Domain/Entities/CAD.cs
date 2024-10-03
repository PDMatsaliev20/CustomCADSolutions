using CustomCADs.Domain.Enums;
using CustomCADs.Domain.ValueObjects;

namespace CustomCADs.Domain.Entities
{
    public class Cad 
    {
        public int Id { get; set; }
        public required string Name { get; set; } 
        public required string Description { get; set; } 
        public CadStatus Status { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public required Coordinates CamCoordinates { get; set; } 
        public required Coordinates PanCoordinates { get; set; } 
        public Paths Paths { get; set; } = null!;
        public int CategoryId { get; set; } 
        public required Category Category { get; set; } 
        public required string CreatorId { get; set; } 
        public required User Creator { get; set; } 
        public ICollection<Order> Orders { get; set; } = [];
    }
}
