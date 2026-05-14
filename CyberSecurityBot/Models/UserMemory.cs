// ============================================================
// File: Models/UserMemory.cs
// Purpose: Stores information about the user that the chatbot
//          remembers across the conversation (name, interests, etc.)
//          Uses automatic properties as required by the module.
// ============================================================

namespace CyberSecurityBot.Models
{
    /// <summary>
    /// Represents the chatbot's memory of the current user.
    /// </summary>
    public class UserMemory
    {
        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's favourite cybersecurity topic.
        /// </summary>
        public string FavouriteTopic { get; set; }

        /// <summary>
        /// Gets or sets the last topic discussed for follow-up questions.
        /// </summary>
        public string LastTopic { get; set; }

        /// <summary>
        /// Gets or sets the last sentiment detected from the user.
        /// </summary>
        public string LastSentiment { get; set; }

        /// <summary>
        /// Clears all stored memory when starting a new conversation.
        /// </summary>
        public void Clear()
        {
            UserName = null;
            FavouriteTopic = null;
            LastTopic = null;
            LastSentiment = null;
        }
    }
}