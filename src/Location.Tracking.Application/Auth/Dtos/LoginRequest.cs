using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Auth.Dtos
{
    internal class LoginRequest
    {
        [Required]
        [EmailAddress]
        internal string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        internal string Password { get; set; } = string.Empty;
    }
}
