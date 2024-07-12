namespace pruebaFGRP.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public int? Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
