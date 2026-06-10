using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Shared
{
    public class Errors
    {
        public static class DeviceErrors
        {
            public static Error DeviceNotFound = new Error("Device not found.", "not_found");
            public static Error DeviceExists = new Error("Device already exists.", "already_exists");
        }
        public static class DeviceModelErrors
        {
            public static Error DeviceModelNotFound = new Error("\"Device model not found.", "not_found");
        }

        public static class UserErrors
        {
            public static Error UserExists = new Error("User already exists.", "already_exists");
            public static Error InvalidCredentials = new Error("Invalid credentials.", "invalid_credentials");
        }
    }
}
