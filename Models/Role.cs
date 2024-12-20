using System;

namespace RestApi.Models
{
    public class Role
    {
        public long RoleId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
