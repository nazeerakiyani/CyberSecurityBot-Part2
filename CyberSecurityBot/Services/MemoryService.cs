// ============================================================
// File: Services/MemoryService.cs
// Purpose: Manages the chatbot's memory of user information.
//          Stores and recalls details like name, interests, and
//          conversation context for personalised interactions.
// ============================================================

using CyberSecurityBot.Models;

namespace CyberSecurityBot.Services
{
    /// <summary>
    /// Manages user memory for personalised chatbot interactions.
    /// </summary>
    public class MemoryService
    {
        /// <summary>
        /// Gets the current user's memory store.
        /// </summary>
        public UserMemory UserMemory { get; private set; }

        /// <summary>
        /// Initialises a new memory service with empty user memory.
        /// </summary>
        public MemoryService()
        {
            UserMemory = new UserMemory();
        }

        /// <summary>
        /// Stores the user's name in memory.
        /// </summary>
        /// <param name="name">The user's name</param>
        public void RememberName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                UserMemory.UserName = name.Trim();
            }
        }

        /// <summary>
        /// Stores the user's favourite cybersecurity topic.
        /// </summary>
        /// <param name="topic">The topic name</param>
        public void RememberFavouriteTopic(string topic)
        {
            if (!string.IsNullOrWhiteSpace(topic))
            {
                UserMemory.FavouriteTopic = topic.Trim().ToLower();
            }
        }

        /// <summary>
        /// Stores the last discussed topic for follow-up handling.
        /// </summary>
        /// <param name="topic">The topic key</param>
        public void RememberLastTopic(string topic)
        {
            UserMemory.LastTopic = topic;
        }

        /// <summary>
        /// Stores the last detected sentiment.
        /// </summary>
        /// <param name="sentiment">The sentiment string</param>
        public void RememberSentiment(string sentiment)
        {
            UserMemory.LastSentiment = sentiment;
        }

        /// <summary>
        /// Returns a personalised greeting using the user's name if known.
        /// </summary>
        /// <returns>Personalised greeting string</returns>
        public string GetPersonalisedGreeting()
        {
            if (!string.IsNullOrWhiteSpace(UserMemory.UserName))
            {
                return $"Hello, {UserMemory.UserName}! Welcome back to the Cybersecurity Awareness Bot.";
            }
            return "Hello! Welcome to the Cybersecurity Awareness Bot.";
        }

        /// <summary>
        /// Returns a personalised tip based on the user's favourite topic.
        /// </summary>
        /// <returns>Personalised tip string</returns>
        public string GetPersonalisedTip()
        {
            if (!string.IsNullOrWhiteSpace(UserMemory.FavouriteTopic))
            {
                return $"As someone interested in {UserMemory.FavouriteTopic}, you might want to explore more tips on this topic!";
            }
            return string.Empty;
        }

        /// <summary>
        /// Clears all user memory (for restarting conversation).
        /// </summary>
        public void ClearMemory()
        {
            UserMemory.Clear();
        }
    }
}