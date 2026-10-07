using API_PI_ADM_Clubes.Model.Enums;
using StackExchange.Redis;

namespace API_PI_ADM_Clubes.Application.DTOs
{
    public class UserFilterDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; } 
        public RoleEnum? Role { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class UpdateUserRoleDTO
    {
        public RoleEnum Role { get; set; }
    }

    public class UpdateUserStatusDTO
    {
        public bool IsActive { get; set; }
    }

    // public class CreatUserDTO
    // {
    //     public string Name { get; set; }
    //     public string Email { get; set; }
    //     public string Password { get; set; }
    // }
    // public class CompleteProfileDTO
    // {
    //     public string PhoneNumber { get; set; }
    //     public string Cpf { get; set; }
    //     public DateOnly BirthDate { get; set; }
    // }
    public class UpdateUserDTO
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
    }
    // public class UpdateAvatarDTO
    // {
    //     public IFormFile? AvatarImage { get; set; }
    // }

    public class ResponseUserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public RoleEnum Role { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Provider { get; set; }
    }
    // public class UserDTO
    // {
    //     public Guid Id { get; set; }
    //     public string Name { get; set; }
    //     public string Email { get; set; }
    //     public string? PhoneNumber { get; set; }
    //     public RoleEnum Role { get; set; }
    //     public bool HasPassword { get; set; }
    //     public string? AvatarUrl { get; set; }
    // }
}