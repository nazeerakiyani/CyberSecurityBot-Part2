// ============================================================
// File: Services/ResponseService.cs
// Purpose: Core chatbot response engine with sequential progression,
//          proactive engagement, memory personalisation, task management,
//          cybersecurity quiz mini-game, and advanced NLP simulation.
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

        private DatabaseService _databaseService;
        private QuizService _quizService;
        private NLPService _nlpService;

        public ResponseDelegate? CustomResponseHandler { get; set; }

        public ResponseService()
        {
            random = new Random();
            lastTopic = string.Empty;
            awaitingName = true;
            lastResponseIndex = new Dictionary<string, int>();
            tipsGivenCount = new Dictionary<string, int>();
            _databaseService = new DatabaseService();
            _quizService = new QuizService();
            _nlpService = new NLPService();

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

        /*
         * Dialzara, 2025. Chatbot Sentiment Analysis: Complete Guide to Implementation and Optimization.
         * [Online]. Available at: https://dialzara.com/blog/step-by-step-guide-to-adding-sentiment-analysis-to-chatbots
         * [Accessed 15 June 2026].
         */

        public string GetResponse(string input, MemoryService memory, SentimentAnalyzer sentimentAnalyzer)
        {
            string normalised = InputValidator.NormaliseInput(input);

            if (string.IsNullOrWhiteSpace(normalised))
            {
                return "I didn't quite understand that. Could you rephrase?";
            }

            // Use NLP to detect intent first
            string intent = _nlpService.DetectIntent(normalised);

            // Handle quiz answers (if quiz is active)
            if (_quizService.IsActive)
            {
                string quizResponse = _quizService.SubmitAnswer(normalised);

                if (quizResponse.Contains("=== QUIZ COMPLETE ==="))
                {
                    _databaseService.LogQuizCompleted(8, 12, 67);
                }
                else
                {
                    _databaseService.LogActivity("Quiz Answer", $"User answered question {_quizService.CurrentQuestionNumber}");
                }
                return quizResponse;
            }

            // Handle NLP-detected intents
            switch (intent)
            {
                case "start_quiz":
                    _databaseService.LogActivity("Quiz Started", "User started cybersecurity quiz");
                    return _quizService.StartQuiz();

                case "cancel_quiz":
                    _databaseService.LogActivity("Quiz Cancelled", "User cancelled the quiz");
                    return _quizService.CancelQuiz();

                case "help":
                    _databaseService.LogActivity("Help Requested", "User asked for help/commands");
                    return _nlpService.GetHelpMessage();

                case "activity_log":
                    _databaseService.LogActivity("Viewed Activity Log", "User requested activity log");
                    return HandleActivityLogCommand();

                case "add_task":
                    _databaseService.LogNLPInteraction("add_task", input);
                    return "What task would you like to add? Please describe it (e.g., 'Review privacy settings').";

                case "view_tasks":
                    _databaseService.LogNLPInteraction("view_tasks", input);
                    return HandleViewTasks();

                case "complete_task":
                    _databaseService.LogNLPInteraction("complete_task", input);
                    return HandleCompleteTask(normalised);

                case "delete_task":
                    _databaseService.LogNLPInteraction("delete_task", input);
                    return HandleDeleteTask(normalised);
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
                if (!string.IsNullOrEmpty(lastTopic))
                {
                    _databaseService.LogKeywordResponse(lastTopic, input);
                }
                return keywordResponse;
            }

            string generalResponse = GetGeneralResponse(normalised, empatheticPrefix);
            if (!string.IsNullOrEmpty(generalResponse))
            {
                return generalResponse;
            }

            return $"{empatheticPrefix}I'm not sure I understand. Can you try rephrasing? I can help with topics like passwords, phishing, privacy, scams, malware, and safe browsing. Or just tell me what you're worried about!";
        }

        /*
         * Stack Overflow Community, 2022. C# chatbot command parsing with string manipulation.
         * Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/71019031/c-sharp-chatbot-command-parsing-with-string-manipulation
         * [Accessed 15 June 2026].
         */

        private string HandleViewTasks()
        {
            List<string> tasks = _databaseService.GetAllTasks();
            if (tasks.Count == 0)
            {
                return "You don't have any tasks yet. Type 'add task' or say 'remind me to...' to create one!";
            }

            string response = "Here are your cybersecurity tasks:\n\n";
            foreach (string task in tasks)
            {
                response += task + "\n\n";
            }
            _databaseService.LogActivity("Viewed Tasks", "User viewed their task list");
            return response.Trim();
        }

        private string HandleCompleteTask(string input)
        {
            int taskId = ExtractTaskId(input);
            if (taskId > 0)
            {
                bool success = _databaseService.MarkTaskAsCompleted(taskId);
                if (success)
                {
                    _databaseService.LogActivity("Task Completed", $"Marked task #{taskId} as completed");
                    return $"Task #{taskId} marked as completed! Great job staying on top of your cybersecurity.";
                }
                return $"Couldn't find task #{taskId}. Type 'show my tasks' to see your task IDs.";
            }
            return "Please specify which task number to complete (e.g., 'complete task 1').";
        }

        private string HandleDeleteTask(string input)
        {
            int taskId = ExtractTaskId(input);
            if (taskId > 0)
            {
                bool success = _databaseService.DeleteTask(taskId);
                if (success)
                {
                    _databaseService.LogActivity("Task Deleted", $"Deleted task #{taskId}");
                    return $"Task #{taskId} deleted successfully.";
                }
                return $"Couldn't find task #{taskId}. Type 'show my tasks' to see your task IDs.";
            }
            return "Please specify which task number to delete (e.g., 'delete task 1').";
        }

        private string HandleActivityLogCommand()
        {
            List<string> activities = _databaseService.GetRecentActivities(10);
            if (activities.Count == 0)
            {
                return "I haven't recorded any activities yet. Try adding a task, taking the quiz, or exploring cybersecurity topics!";
            }

            string response = "Here's a summary of recent actions:\n\n";
            int count = 1;
            foreach (string activity in activities)
            {
                response += $"{count}. {activity}\n";
                count++;
            }

            int totalCount = _databaseService.GetActivityCount();
            if (totalCount > 10)
            {
                response += $"\n... and {totalCount - 10} more actions. Type 'show more' to see all.";
            }

            _databaseService.LogActivity("Viewed Activity Log", "User viewed activity log");
            return response.Trim();
        }

        private int ExtractTaskId(string input)
        {
            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (int.TryParse(words[i], out int id) && id > 0)
                {
                    return id;
                }
            }
            return 0;
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

        /*
         * C-Sharp Corner, 2025. How to Handle Follow-Up Questions and Maintain Context in Chatbots.
         * [Online]. Available at: https://www.c-sharpcorner.com/article/how-to-handle-follow-up-questions-and-maintain-context-in-chatbots-easy-guide/
         * [Accessed 15 June 2026].
         */

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

        /*
         * Microsoft, 2025. Implement sequential conversation flow - Bot Service.
         * Microsoft Learn. [Online]. Available at: https://learn.microsoft.com/en-us/azure/bot-service/bot-builder-dialog-manage-conversation-flow
         * [Accessed 15 June 2026].
         */

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
                { "phishing", "Great progress on phishing! Ready to learn about malware protection, or would you like a quick quiz?" },
                { "privacy", "Excellent work on privacy settings! Want to dive into safe browsing habits next, or shall we do a quick quiz?" },
                { "scam", "You're becoming scam-savvy! Should we explore password security next, or would you like a quiz?" },
                { "malware", "Strong malware knowledge! Want to learn about phishing next, or shall I quiz you?" },
                { "browsing", "Great browsing safety awareness! Ready to learn about privacy settings, or shall we test your knowledge?" }
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

        /*
         * Stack Overflow Community, 2019. Parse returned text for specific set of words or phrases.
         * Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/56091373/parse-returned-text-for-specific-set-of-words-or-phrases
         * [Accessed 15 June 2026].
         */

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