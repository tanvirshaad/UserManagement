namespace UserManagement.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime RegistrationTime { get; set; }

        // Add this property to fix the error  
        public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
