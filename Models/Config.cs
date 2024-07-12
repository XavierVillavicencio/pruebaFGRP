﻿namespace pruebaFGRP.Models
{
    public class Config
    {
        public int Id { get; set; }
        public required string Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
