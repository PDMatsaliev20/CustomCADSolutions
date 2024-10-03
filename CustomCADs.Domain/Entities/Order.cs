using CustomCADs.Domain.Enums;

namespace CustomCADs.Domain.Entities
{
    public class Order 
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; } 
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public bool ShouldBeDelivered { get; set; }
        public required string ImagePath { get; set; } 
        public int CategoryId { get; set; }
        public required Category Category { get; set; } 
        public required string BuyerId { get; set; } 
        public required User Buyer { get; set; } 
        public string? DesignerId { get; set; }
        public User? Designer { get; set; } 
        public int? CadId { get; set; }
        public Cad? Cad { get; set; }        
    }
}
