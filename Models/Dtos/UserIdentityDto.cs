namespace Tech_Test_Backend.Models.Dtos
{
    public class UserIdentityDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
