// ============================================================
// File: Models/ChatMessage.cs
// Purpose: Represents a single message in the chat conversation,
//          storing whether it was sent by the user or the bot.
// ============================================================

using System;

namespace CyberSecurityBot.Models
{
    /// <summary>
    /// Represents a single message in the chat conversation.
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// The text content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// True if the message was sent by the user, false if by the bot.
        /// </summary>
        public bool IsUserMessage { get; set; }

        /// <summary>
        /// The timestamp when the message was sent.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Creates a new chat message.
        /// </summary>
        /// <param name="content">The message text</param>
        /// <param name="isUserMessage">True for user, false for bot</param>
        public ChatMessage(string content, bool isUserMessage)
        {
            Content = content;
            IsUserMessage = isUserMessage;
            Timestamp = DateTime.Now;
        }
    }
}