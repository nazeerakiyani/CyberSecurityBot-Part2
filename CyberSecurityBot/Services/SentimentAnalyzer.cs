// ============================================================
// File: Services/SentimentAnalyzer.cs
// Purpose: Detects the user's emotional sentiment from their input
//          and categorises it as Worried, Curious, Frustrated, or Neutral.
//          This allows the chatbot to respond with empathy.
// ============================================================

using System.Collections.Generic;

namespace CyberSecurityBot.Services
{
    /// <summary>
    /// Represents the possible emotional states detected from user input.
    /// </summary>
    public enum Sentiment
    {
        Worried,
        Curious,
        Frustrated,
        Neutral
    }

    /// <summary>
    /// Analyses user messages to detect emotional sentiment using keyword matching.
    /// Uses a generic collection (Dictionary&lt;Sentiment, List&lt;string&gt;&gt;) to store keywords.
    /// </summary>
    public class SentimentAnalyzer
    {
        // Generic collection: Dictionary with Sentiment as key, List of keywords as value
        private readonly Dictionary<Sentiment, List<string>> sentimentKeywords;

        /// <summary>
        /// Initialises the sentiment analyser with keyword dictionaries.
        /// </summary>
        public SentimentAnalyzer()
        {
            sentimentKeywords = new Dictionary<Sentiment, List<string>>()
            {
                {
                    Sentiment.Worried,
                    new List<string>()
                    {
                        "worried", "scared", "afraid", "nervous", "anxious",
                        "terrified", "fear", "concerned", "uneasy", "panic"
                    }
                },
                {
                    Sentiment.Curious,
                    new List<string>()
                    {
                        "curious", "wonder", "interested", "want to know",
                        "tell me more", "how does", "what is", "explain"
                    }
                },
                {
                    Sentiment.Frustrated,
                    new List<string>()
                    {
                        "frustrated", "annoyed", "angry", "mad", "stupid",
                        "hate", "useless", "doesn't work", "confused", "lost"
                    }
                }
            };
        }

        /// <summary>
        /// Analyses the user's input and returns the detected sentiment.
        /// </summary>
        /// <param name="userInput">The user's message</param>
        /// <returns>The detected Sentiment enum value</returns>
        public Sentiment AnalyzeSentiment(string userInput)
        {
            string normalised = userInput.ToLower();

            foreach (var pair in sentimentKeywords)
            {
                foreach (string keyword in pair.Value)
                {
                    if (normalised.Contains(keyword))
                    {
                        return pair.Key;
                    }
                }
            }

            return Sentiment.Neutral;
        }

        /// <summary>
        /// Returns an empathetic prefix based on the detected sentiment.
        /// </summary>
        /// <param name="sentiment">The detected sentiment</param>
        /// <returns>An empathetic response prefix string</returns>
        public string GetEmpatheticPrefix(Sentiment sentiment)
        {
            return sentiment switch
            {
                Sentiment.Worried => "It's completely understandable to feel that way. ",
                Sentiment.Curious => "Great question! ",
                Sentiment.Frustrated => "I understand this can be frustrating. Let me help. ",
                Sentiment.Neutral => "",
                _ => ""
            };
        }
    }
}