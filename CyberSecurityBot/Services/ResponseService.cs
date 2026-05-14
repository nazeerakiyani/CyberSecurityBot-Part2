// ============================================================
// File: Services/ResponseService.cs
// Purpose: Core chatbot response engine with sequential progression,
//          proactive engagement, and memory personalisation.
// ============================================================

using System;
using System.Collections.Generic;
using CyberSecurityBot.Delegates;
using CyberSecurityBot.Utilities;

namespace CyberSecurityBot.Services
{
    public class ResponseService
    {
        private readonly Dictionary<string, List<string>> keywordResponses;
        private readonly Dictionary<string, string> keywordVariations;
        private readonly Random random;
        private readonly Dictionary<string, int> lastResponseIndex;
        private readonly Dictionary<string, int> tipsGivenCount;
        private string lastTopic;
        private bool awaitingName;

        public ResponseDelegate? CustomResponseHandler { get; set; }

        public ResponseService()
        {
            random = new Random();
            lastTopic = string.Empty;
            awaitingName = true;
            lastResponseIndex = new Dictionary<string, int>();
            tipsGivenCount = new Dictionary<string, int>();

            keywordResponses = new Dictionary<string, List<string>>()
            {
                {
                    "password",
                    new List<string>()
                    {
                        "Start by using strong, unique passwords for each account. Avoid using personal details like birthdays or pet names in your passwords.",
                        "Use a password manager to generate and store complex passwords. Never reuse passwords across different sites - if one gets breached, all your accounts are at risk.",
                        "Enable two-factor authentication (2FA) wherever possible. It adds an extra layer of security even if your password is compromised. Use apps like Google Authenticator instead of SMS when you can."
                    }
                },
                {
                    "phishing",
                    new List<string>()
                    {
                        "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations like banks or SARS.",
                        "Check the sender's email address carefully. Legitimate companies won't ask for passwords via email. Look for spelling errors and urgent language - these are red flags.",
                        "Hover over links before clicking to see the actual URL. If it looks suspicious, don't click it. When in doubt, contact the organisation directly through their official website or phone number."
                    }
                },
                {
                    "privacy",
                    new List<string>()
                    {
                        "Review your privacy settings on social media regularly. Limit what strangers can see - especially your location, birthdate, and contact details.",
                        "Be careful what you share online. Personal information can be used for identity theft. Think before you post - once it's online, it's hard to remove completely.",
                        "Use private browsing mode when using public computers, and always log out of accounts. Consider using a VPN on public Wi-Fi to encrypt your connection and protect your data."
                    }
                },
                {
                    "scam",
                    new List<string>()
                    {
                        "If an offer seems too good to be true, it probably is. Trust your instincts - scammers prey on greed and fear.",
                        "Never send money to someone you haven't met in person, especially if they contacted you online. Romance scams and investment scams are very common in South Africa.",
                        "Verify unexpected requests by contacting the organisation directly through their official website or phone number. Don't use contact details provided in the suspicious message."
                    }
                },
                {
                    "malware",
                    new List<string>()
                    {
                        "Keep your antivirus software updated and run regular scans. Windows Defender is built-in and free - make sure it's active and updated.",
                        "Don't download software from untrusted websites. Stick to official app stores like Microsoft Store, Google Play, or Apple App Store. Pirated software often contains malware.",
                        "Be wary of USB drives from unknown sources. They can contain malicious software that auto-runs when plugged in. Always scan external devices before opening files."
                    }
                },
                {
                    "browsing",
                    new List<string>()
                    {
                        "Always look for 'https://' and a padlock icon before entering sensitive information on websites. The 's' means the connection is encrypted.",
                        "Keep your browser updated. Updates often include security patches for known vulnerabilities. Enable automatic updates so you don't miss critical fixes.",
                        "Clear your cookies and cache regularly to protect your browsing history and personal data. Consider using privacy-focused browsers like Firefox or Brave for extra protection."
                    }
                }
            };

            keywordVariations = new Dictionary<string, string>()
            {
                {"passcode", "password"}, {"login", "password"}, {"credential", "password"},
                {"pin", "password"}, {"forgot password", "password"}, {"stolen password", "password"},
                {"hack", "password"}, {"crack", "password"}, {"fake email", "phishing"},
                {"spoof", "phishing"}, {"fraud email", "phishing"}, {"suspicious link", "phishing"},
                {"click link", "phishing"}, {"email scam", "phishing"}, {"personal info", "privacy"},
                {"data protection", "privacy"}, {"track me", "privacy"}, {"spy", "privacy"},
                {"private", "privacy"}, {"confidential", "privacy"}, {"fraud", "scam"},
                {"con", "scam"}, {"trick", "scam"}, {"cheat", "scam"}, {"fake", "scam"},
                {"too good", "scam"}, {"lottery", "scam"}, {"prize", "scam"}, {"virus", "malware"},
                {"trojan", "malware"}, {"ransomware", "malware"}, {"spyware", "malware"},
                {"infected", "malware"}, {"antivirus", "malware"}, {"download", "malware"},
                {"internet", "browsing"}, {"website", "browsing"}, {"online", "browsing"},
                {"surf", "browsing"}, {"web", "browsing"}, {"http", "browsing"}, {"padlock", "browsing"}
            };
        }

        public string GetResponse(string input, MemoryService memory, SentimentAnalyzer sentimentAnalyzer)
        {
            string normalised = InputValidator.NormaliseInput(input);

            if (string.IsNullOrWhiteSpace(normalised))
            {
                return "I didn't quite understand that. Could you rephrase?";
            }

            Sentiment sentiment = sentimentAnalyzer.AnalyzeSentiment(input);
            memory.RememberSentiment(sentiment.ToString());
            string empatheticPrefix = sentimentAnalyzer.GetEmpatheticPrefix(sentiment);

            if (IsFollowUpRequest(normalised))
            {
                return HandleFollowUp(empatheticPrefix, memory);
            }

            if (IsConfusedRequest(normalised))
            {
                return HandleConfusion(empatheticPrefix, memory);
            }

            if (awaitingName)
            {
                memory.RememberName(input);
                awaitingName = false;
                return $"{empatheticPrefix}Nice to meet you, {memory.UserMemory.UserName}! I'm here to help you stay safe online. You can ask me about passwords, phishing, privacy, scams, malware, or safe browsing.";
            }

            string topicInterest = ExtractTopicInterest(normalised);
            if (!string.IsNullOrEmpty(topicInterest))
            {
                memory.RememberFavouriteTopic(topicInterest);
                lastTopic = topicInterest;
                return $"{empatheticPrefix}Great! I'll remember that you're interested in {topicInterest}. It's a crucial part of staying safe online.";
            }

            string keywordResponse = GetKeywordResponse(normalised, empatheticPrefix, memory);
            if (!string.IsNullOrEmpty(keywordResponse))
            {
                return keywordResponse;
            }

            string generalResponse = GetGeneralResponse(normalised, empatheticPrefix);
            if (!string.IsNullOrEmpty(generalResponse))
            {
                return generalResponse;
            }

            return $"{empatheticPrefix}I'm not sure I understand. Can you try rephrasing? I can help with topics like passwords, phishing, privacy, scams, malware, and safe browsing. Or just tell me what you're worried about!";
        }

        private bool IsFollowUpRequest(string input)
        {
            string[] followUpPhrases = { "another", "more", "explain", "tell me more", "give me another", "what else", "anything else", "continue", "next tip" };
            foreach (string phrase in followUpPhrases)
            {
                if (input.Contains(phrase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsConfusedRequest(string input)
        {
            string[] confusionPhrases = {
                "confused", "don't understand", "do not understand", "don't get it", "what do you mean",
                "explain that", "can you explain", "not sure", "clarify", "why", "how does that work",
                "what does that mean", "i don't know what that means"
            };
            foreach (string phrase in confusionPhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }

        private string HandleFollowUp(string prefix, MemoryService memory)
        {
            string topic = !string.IsNullOrEmpty(memory.UserMemory.FavouriteTopic)
                ? memory.UserMemory.FavouriteTopic
                : lastTopic;

            if (!string.IsNullOrEmpty(topic) && keywordResponses.ContainsKey(topic))
            {
                List<string> responses = keywordResponses[topic];
                int index = GetSequentialIndex(topic, responses.Count);
                string response = responses[index];

                if (!tipsGivenCount.ContainsKey(topic))
                    tipsGivenCount[topic] = 0;
                tipsGivenCount[topic]++;

                if (tipsGivenCount[topic] >= 3)
                {
                    tipsGivenCount[topic] = 0;
                    string proactiveSuggestion = GetProactiveSuggestion(topic);
                    return $"{prefix}Here's the next tip on {topic}: {response}\n\n{proactiveSuggestion}";
                }

                return $"{prefix}Here's the next tip on {topic}: {response}";
            }

            return $"{prefix}I'd be happy to share more! Which topic would you like to know more about: passwords, phishing, privacy, scams, malware, or safe browsing?";
        }

        private string HandleConfusion(string prefix, MemoryService memory)
        {
            string topic = !string.IsNullOrEmpty(memory.UserMemory.FavouriteTopic)
                ? memory.UserMemory.FavouriteTopic
                : lastTopic;

            var explanations = new Dictionary<string, string>()
            {
                { "password", "Let me break it down simply: a strong password is like a unique key for each of your online accounts. It should be at least 12 characters, use a mix of letters, numbers, and symbols, and never be reused. Think of it like having a different key for every door in your house - if someone steals one key, they can't open all your doors." },
                { "phishing", "Here's the simple version: phishing is when scammers send fake emails or messages that look real, trying to trick you into giving them personal info. Imagine someone dressing up as a police officer to steal your wallet - they look official but they're not. Always check the sender's real email address and never click suspicious links." },
                { "privacy", "In simple terms: your online privacy means controlling who can see your personal information. It's like choosing who you tell your home address to. Review your social media settings regularly and don't overshare - scammers use personal details to guess passwords or answer security questions." },
                { "scam", "Basically: if someone online promises something too good to be true, asks for money upfront, or pressures you to act fast - it's probably a scam. In South Africa, common scams include fake lottery wins, romance scams, and investment schemes. Stop, think, and verify before sending any money or information." },
                { "malware", "Simply put: malware is harmful software that can steal your data or damage your device. It's like a virus that makes you sick, but for your computer. Keep your antivirus updated, only download from trusted sources, and never plug in USB drives you found lying around - they can be infected on purpose." },
                { "browsing", "The basics: always check for 'https://' and a padlock before entering passwords or card details. The 's' means the connection is encrypted (scrambled so hackers can't read it). Update your browser regularly and clear your cookies/cache to remove tracking data that websites store about you." }
            };

            if (!string.IsNullOrEmpty(topic) && explanations.ContainsKey(topic))
            {
                return $"{prefix}{explanations[topic]}";
            }

            return $"{prefix}No problem, let me explain differently. Which topic are you asking about: passwords, phishing, privacy, scams, malware, or safe browsing?";
        }

        private int GetSequentialIndex(string topic, int count)
        {
            if (!lastResponseIndex.ContainsKey(topic))
            {
                lastResponseIndex[topic] = -1;
            }

            int lastIndex = lastResponseIndex[topic];
            int newIndex = (lastIndex + 1) % count;

            lastResponseIndex[topic] = newIndex;
            return newIndex;
        }

        private string GetProactiveSuggestion(string currentTopic)
        {
            var suggestions = new Dictionary<string, string>()
            {
                { "password", "You've learned a lot about passwords! Would you like to explore phishing awareness next, or shall I quiz you on what you've learned so far?" },
                { "phishing", "Great progress on phishing! Ready to learn about malware protection, or would you like me to test your knowledge with a quick quiz?" },
                { "privacy", "Excellent work on privacy settings! Want to dive into safe browsing habits next, or shall we do a quick quiz to reinforce what you've learned?" },
                { "scam", "You're becoming scam-savvy! Should we explore password security next, or would you like a quiz to test your scam-spotting skills?" },
                { "malware", "Strong malware knowledge! Want to learn about phishing next, or shall I quiz you on protecting your devices?" },
                { "browsing", "Great browsing safety awareness! Ready to learn about privacy settings, or shall we test your knowledge with a quiz?" }
            };

            if (suggestions.ContainsKey(currentTopic))
            {
                return suggestions[currentTopic];
            }

            return "You've covered a lot on this topic! What would you like to explore next: passwords, phishing, privacy, scams, malware, or safe browsing?";
        }

        private string GetPersonalisation(string currentTopic, MemoryService memory)
        {
            if (!string.IsNullOrEmpty(memory.UserMemory.FavouriteTopic) &&
                memory.UserMemory.FavouriteTopic != currentTopic)
            {
                return $"As someone interested in {memory.UserMemory.FavouriteTopic}, this is also important: ";
            }
            return string.Empty;
        }

        private string ExtractTopicInterest(string input)
        {
            string[] interestPhrases = { "interested in", "like", "want to know about", "tell me about", "love", "favourite", "favorite" };

            foreach (string phrase in interestPhrases)
            {
                if (input.Contains(phrase))
                {
                    foreach (string topic in keywordResponses.Keys)
                    {
                        if (input.Contains(topic))
                        {
                            return topic;
                        }
                    }
                }
            }
            return string.Empty;
        }

        private string GetKeywordResponse(string input, string prefix, MemoryService memory)
        {
            foreach (var pair in keywordResponses)
            {
                if (input.Contains(pair.Key))
                {
                    lastTopic = pair.Key;
                    List<string> responses = pair.Value;
                    int index = GetSequentialIndex(pair.Key, responses.Count);
                    string selectedResponse = responses[index];

                    string personalisation = GetPersonalisation(pair.Key, memory);

                    return $"{prefix}{personalisation}{selectedResponse}";
                }
            }

            foreach (var variation in keywordVariations)
            {
                if (input.Contains(variation.Key))
                {
                    string mainTopic = variation.Value;
                    if (keywordResponses.ContainsKey(mainTopic))
                    {
                        lastTopic = mainTopic;
                        List<string> responses = keywordResponses[mainTopic];
                        int index = GetSequentialIndex(mainTopic, responses.Count);
                        string selectedResponse = responses[index];

                        string personalisation = GetPersonalisation(mainTopic, memory);

                        return $"{prefix}{personalisation}{selectedResponse}";
                    }
                }
            }

            return string.Empty;
        }

        private string GetGeneralResponse(string input, string prefix)
        {
            if (input.Contains("safe") || input.Contains("protect") || input.Contains("secure"))
            {
                return $"{prefix}Staying safe online is all about being aware. Keep your software updated, use strong passwords, be careful with emails from unknown senders, and always verify before clicking links. Which of these would you like to know more about?";
            }

            if (input.Contains("help") || input.Contains("what can you do") || input.Contains("what do you do") || input.Contains("purpose"))
            {
                return $"{prefix}I'm here to help you stay safe online! I can give you tips on passwords, phishing, privacy, scams, malware, and safe browsing. Just ask me about any of these topics, or tell me what you're worried about.";
            }

            if (input.Contains("how are you") || input.Contains("how do you do"))
            {
                return $"{prefix}I'm doing great, thanks for asking! I'm ready to help you learn about cybersecurity. What would you like to know about today?";
            }

            if (input.Contains("thank") || input.Contains("thanks"))
            {
                return $"{prefix}You're very welcome! I'm glad I could help. Stay safe online, and feel free to come back anytime you have questions.";
            }

            if (input.Contains("bye") || input.Contains("goodbye") || input.Contains("see you"))
            {
                return $"{prefix}Goodbye! Remember to stay vigilant online. Your cybersecurity matters!";
            }

            return string.Empty;
        }

        public void ResetConversation()
        {
            awaitingName = true;
            lastTopic = string.Empty;
            lastResponseIndex.Clear();
            tipsGivenCount.Clear();
        }
    }
}