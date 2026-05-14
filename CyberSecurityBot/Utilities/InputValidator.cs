// ============================================================
// File: Utilities/InputValidator.cs
// Purpose: Validates user input to prevent crashes and ensure
//          the chatbot handles empty or invalid input gracefully.
// ============================================================

namespace CyberSecurityBot.Utilities
{
    /// <summary>
    /// Provides validation methods for user input.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Checks if the user input is null, empty, or only whitespace.
        /// </summary>
        /// <param name="input">The user's input string</param>
        /// <returns>True if the input is valid (not empty), false otherwise</returns>
        public static bool IsValidInput(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Normalises user input by trimming whitespace and converting to lowercase
        /// for consistent keyword matching.
        /// </summary>
        /// <param name="input">Raw user input</param>
        /// <returns>Normalised input string</returns>
        public static string NormaliseInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input.Trim().ToLower();
        }
    }
}