using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tweetbook.Domain
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatorId { get; set; }
        public IdentityUser CreatedBy { get; set; }
    }
}