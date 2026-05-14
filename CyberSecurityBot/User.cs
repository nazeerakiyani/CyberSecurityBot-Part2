using System;

namespace CyberSecurityBot
{
    public class User
    {
        public string Name { get; set; } = "Guest";

        // Simple validation for Part 1 POE
        public bool IsValidName(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && input.Length >= 2;
        }
    }
}