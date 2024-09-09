﻿namespace CustomCADs.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Role { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
