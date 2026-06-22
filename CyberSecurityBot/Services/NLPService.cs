// ============================================================
// File: Services/NLPService.cs
// Purpose: Natural Language Processing simulation for the chatbot.
//          Recognises user commands across multiple phrasings, synonyms,
//          and variations with typo tolerance for engaging interaction.
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityBot.Services
{
    /*
     * GeeksforGeeks, 2024. C# String Manipulation for Natural Language Processing.
     * [Online]. Available at: https://www.geeksforgeeks.org/c-sharp-string-class/
     * [Accessed 15 June 2026].
     */

    /// <summary>
    /// Simulates NLP by detecting user intent from various phrasings,
    /// synonyms, and partial matches with typo tolerance.
    /// </summary>
    public class NLPService
    {
        private readonly Dictionary<string, List<string>> intentPatterns;
        private readonly Dictionary<string, List<string>> synonyms;

        public NLPService()
        {
            // Synonym mapping for advanced matching
            synonyms = new Dictionary<string, List<string>>()
            {
                { "task", new List<string> { "task", "todo", "to-do", "to do", "item", "job", "chore", "assignment" } },
                { "reminder", new List<string> { "reminder", "remind", "alert", "notification", "prompt", "nudge" } },
                { "quiz", new List<string> { "quiz", "test", "exam", "challenge", "game", "trivia", "assessment" } },
                { "show", new List<string> { "show", "display", "view", "see", "list", "check", "look", "find", "get", "give me" } },
                { "add", new List<string> { "add", "create", "make", "new", "set", "setup", "schedule", "plan", "organise", "organize" } },
                { "delete", new List<string> { "delete", "remove", "erase", "clear", "cancel", "get rid of", "drop", "eliminate" } },
                { "complete", new List<string> { "complete", "finish", "done", "mark", "check off", "tick off", "accomplish", "fulfill" } },
                { "help", new List<string> { "help", "assist", "support", "guide", "explain", "what can you do", "how to" } },
                { "activity", new List<string> { "activity", "log", "history", "actions", "what happened", "summary", "record", "track" } }
            };

            intentPatterns = new Dictionary<string, List<string>>()
            {
                {
                    "add_task",
                    new List<string>()
                    {
                        "add task", "new task", "create task", "make task",
                        "remind me to", "set a reminder for", "set reminder for",
                        "don't forget to", "do not forget to", "help me remember to",
                        "I need to", "I have to", "I should", "schedule",
                        "plan to", "want to remember", "need reminder for",
                        "add a task", "create a new task", "set up task",
                        "set task reminder", "add reminder", "new reminder",
                        "create reminder", "remind me about", "remind me",
                        "add a todo", "create todo", "new todo",
                        "set a todo", "make a reminder", "add a job",
                        "I must", "I want to", "I would like to",
                        "can you remind me", "please remind me", "remind me that",
                        "don't let me forget", "help me not forget",
                        "put on my list", "add to my list", "put on task list"
                    }
                },
                {
                    "view_tasks",
                    new List<string>()
                    {
                        "show my tasks", "view tasks", "list tasks", "my tasks",
                        "what tasks do I have", "show reminders", "what do I need to do",
                        "show my reminders", "view my tasks", "list my tasks",
                        "what are my tasks", "show pending tasks", "show completed tasks",
                        "what's on my list", "what is on my list", "check my tasks",
                        "see my tasks", "display tasks", "task list",
                        "my reminders", "task reminders", "show task reminders",
                        "what reminders do I have", "view reminders", "list reminders",
                        "check reminders", "see reminders", "show my task reminders",
                        "what do i need to do", "what do I need to do",
                        "show me my tasks", "show me my reminders", "any tasks",
                        "any reminders", "do I have tasks", "do I have reminders",
                        "what's my todo list", "show todo list", "view todo",
                        "check my todo", "see my todo list", "what jobs do I have",
                        "anything on my list", "whats on my task list"
                    }
                },
                {
                    "complete_task",
                    new List<string>()
                    {
                        "complete task", "mark task done", "done task", "finish task",
                        "task is done", "task is complete", "task finished",
                        "mark as complete", "mark as done", "I finished task",
                        "I completed task", "task completed", "task done",
                        "mark task", "complete", "finish",
                        "task 1 done", "task 2 done", "task 3 done",
                        "check off task", "tick off task", "cross off task",
                        "I did task", "I finished", "all done with task",
                        "task is finished", "mark my task", "mark it done"
                    }
                },
                {
                    "delete_task",
                    new List<string>()
                    {
                        "delete task", "remove task", "get rid of task", "cancel task",
                        "erase task", "clear task", "remove reminder", "delete reminder",
                        "I don't need task", "I do not need task", "remove this task",
                        "delete task 1", "delete task 2", "remove task 1",
                        "get rid of reminder", "clear reminder", "drop task",
                        "eliminate task", "remove from list", "delete from list",
                        "I want to delete", "please remove task", "take off my list"
                    }
                },
                {
                    "start_quiz",
                    new List<string>()
                    {
                        "start quiz", "quiz me", "take quiz", "test me",
                        "quiz", "test my knowledge", "give me a quiz", "I want a quiz",
                        "quiz me on", "test me on", "quiz time", "let's quiz",
                        "start a quiz", "begin quiz", "quiz please", "can I quiz",
                        "I want to test my knowledge", "challenge me", "quiz challenge",
                        "can you quiz me", "let me quiz", "quiz now",
                        "give me a test", "start test", "begin test",
                        "I want a challenge", "cybersecurity quiz", "security quiz",
                        "ask me questions", "test my cybersecurity", "quiz game"
                    }
                },
                {
                    "cancel_quiz",
                    new List<string>()
                    {
                        "cancel quiz", "stop quiz", "end quiz", "quit quiz",
                        "I don't want to quiz", "I do not want to quiz",
                        "exit quiz", "leave quiz", "stop the quiz",
                        "no more quiz", "end the quiz", "I'm done with quiz",
                        "quit test", "stop test", "exit test", "end test"
                    }
                },
                {
                    "activity_log",
                    new List<string>()
                    {
                        "show activity log", "what have you done", "what did you do",
                        "show log", "activity log", "what have you done for me",
                        "what have I done", "show my activity", "view activity log",
                        "what actions have you taken", "what did we do", "activity history",
                        "show history", "what happened", "log of actions",
                        "what have you been doing", "show me the log", "view log",
                        "show me activity", "view my activity", "check activity log",
                        "what's my history", "show my history", "track my actions",
                        "what did I do", "what have I accomplished", "show progress"
                    }
                },
                {
                    "help",
                    new List<string>()
                    {
                        "help", "what can you do", "commands", "features",
                        "what do you do", "how do I use you", "what are your features",
                        "show commands", "list commands", "what can I ask",
                        "how does this work", "what do you offer", "capabilities",
                        "what topics", "what can I learn", "show me what you can do",
                        "what are you", "who are you", "what is your purpose",
                        "how to use", "user guide", "instructions", "tutorial",
                        "what should I ask", "what do you know", "help me"
                    }
                },
                {
                    "cybersecurity_topic",
                    new List<string>()
                    {
                        "password", "phishing", "privacy", "scam", "malware", "browsing",
                        "passcode", "login", "credential", "pin", "hack", "fake email",
                        "fraud", "virus", "trojan", "ransomware", "spyware", "antivirus",
                        "internet safety", "online safety", "cybersecurity", "security",
                        "two factor", "2fa", "encryption", "firewall", "backup",
                        "social engineering", "data breach", "identity theft", "hacker",
                        "secure password", "strong password", "password manager",
                        "email security", "safe browsing", "online privacy", "digital security"
                    }
                }
            };
        }

        /*
         * Stack Overflow Community, 2022. C# fuzzy string matching for chatbot commands.
         * Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/2344320/comparing-strings-with-tolerance
         * [Accessed 15 June 2026].
         */

        /// <summary>
        /// Detects user intent from input text using exact matching,
        /// synonym expansion, and fuzzy partial matching.
        /// </summary>
        /// <param name="input">User's message</param>
        /// <returns>The detected intent, or "unknown" if no match</returns>
        public string DetectIntent(string input)
        {
            string normalised = input.ToLower().Trim();

            // First: exact pattern matching
            foreach (var intent in intentPatterns)
            {
                foreach (string pattern in intent.Value)
                {
                    if (normalised.Contains(pattern))
                    {
                        return intent.Key;
                    }
                }
            }

            // Second: synonym expansion matching
            foreach (var intent in intentPatterns)
            {
                foreach (string pattern in intent.Value)
                {
                    string expandedPattern = ExpandSynonyms(pattern);
                    if (normalised.Contains(expandedPattern) || expandedPattern.Contains(normalised))
                    {
                        return intent.Key;
                    }
                }
            }

            // Third: fuzzy partial matching (typo tolerance)
            foreach (var intent in intentPatterns)
            {
                foreach (string pattern in intent.Value)
                {
                    if (FuzzyMatch(normalised, pattern))
                    {
                        return intent.Key;
                    }
                }
            }

            return "unknown";
        }

        /// <summary>
        /// Expands a pattern using synonyms for broader matching.
        /// </summary>
        private string ExpandSynonyms(string pattern)
        {
            string result = pattern;
            foreach (var syn in synonyms)
            {
                foreach (string word in syn.Value)
                {
                    if (result.Contains(word))
                    {
                        // Replace with the first synonym (canonical form)
                        result = result.Replace(word, syn.Key);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Fuzzy matching with typo tolerance using Levenshtein-like approach.
        /// </summary>
        private bool FuzzyMatch(string input, string pattern)
        {
            string[] inputWords = input.Split(' ');
            string[] patternWords = pattern.Split(' ');

            int matchCount = 0;
            foreach (string pWord in patternWords)
            {
                if (pWord.Length <= 2) continue; // Skip short words

                foreach (string iWord in inputWords)
                {
                    if (iWord.Length <= 2) continue;

                    // Exact match or substring match
                    if (iWord == pWord || iWord.Contains(pWord) || pWord.Contains(iWord))
                    {
                        matchCount++;
                        break;
                    }

                    // Typo tolerance: 1 character difference for short words
                    if (pWord.Length <= 5 && LevenshteinDistance(iWord, pWord) <= 1)
                    {
                        matchCount++;
                        break;
                    }

                    // Typo tolerance: 2 character difference for longer words
                    if (pWord.Length > 5 && LevenshteinDistance(iWord, pWord) <= 2)
                    {
                        matchCount++;
                        break;
                    }
                }
            }

            // If 60% of pattern words match, consider it a match
            int significantPatternWords = patternWords.Count(w => w.Length > 2);
            if (significantPatternWords == 0) return false;

            return (double)matchCount / significantPatternWords >= 0.6;
        }

        /// <summary>
        /// Calculates Levenshtein distance between two strings for typo tolerance.
        /// </summary>
        private int LevenshteinDistance(string s1, string s2)
        {
            int n = s1.Length;
            int m = s2.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (s2[j - 1] == s1[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

        /// <summary>
        /// Extracts a task description from reminder-style input.
        /// </summary>
        public string ExtractTaskDescription(string input)
        {
            string[] prefixes = {
                "remind me to", "set a reminder for", "set reminder for",
                "don't forget to", "do not forget to", "help me remember to",
                "I need to", "I have to", "I should", "schedule",
                "plan to", "want to remember", "need reminder for",
                "can you remind me to", "please remind me to",
                "don't let me forget to", "help me not forget to",
                "put on my list to", "add to my list to"
            };

            string lower = input.ToLower();
            foreach (string prefix in prefixes)
            {
                if (lower.Contains(prefix))
                {
                    int index = lower.IndexOf(prefix) + prefix.Length;
                    return input.Substring(index).Trim();
                }
            }

            return input;
        }

        /// <summary>
        /// Extracts topic from quiz request (e.g., "quiz me on phishing" → "phishing").
        /// </summary>
        public string ExtractQuizTopic(string input)
        {
            string[] topicWords = { "password", "phishing", "privacy", "scam", "malware", "browsing" };
            string lower = input.ToLower();

            foreach (string topic in topicWords)
            {
                if (lower.Contains(topic))
                {
                    return topic;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks if input is a simple quiz answer (A, B, C, D, 1, 2).
        /// </summary>
        public bool IsQuizAnswer(string input)
        {
            string trimmed = input.Trim().ToUpper();
            if (trimmed.Length == 1)
            {
                return char.IsLetter(trimmed[0]) || char.IsDigit(trimmed[0]);
            }
            return false;
        }

        /// <summary>
        /// Gets a help message listing all available commands with examples.
        /// </summary>
        public string GetHelpMessage()
        {
            return @"🤖 Here are all the things I can help you with:

📚 CYBERSECURITY TOPICS:
   • Ask about: passwords, phishing, privacy, scams, malware, safe browsing
   • Say 'tell me more' or 'explain more' for follow-up tips
   • I remember your favourite topic and personalise responses!

📝 TASK ASSISTANT:
   • 'add task' → I'll help you create a cybersecurity task
   • 'remind me to update my password' → Direct reminder creation
   • 'show my tasks' or 'what do I need to do' → View your tasks
   • 'complete task 1' → Mark a task as done
   • 'delete task 1' → Remove a task
   • Tasks are saved to a database with reminder dates!

🎯 QUIZ:
   • 'start quiz' or 'quiz me' or just 'quiz' → Test your knowledge
   • 'cancel quiz' → Stop the quiz anytime
   • 12 questions covering phishing, passwords, malware, and more
   • Immediate feedback and final score with personalised advice!

📊 ACTIVITY LOG:
   • 'show activity log' or 'what have you done' → See our conversation history
   • Tracks tasks, quizzes, and topic discussions with timestamps

😊 OTHER FEATURES:
   • I can detect your mood and respond with empathy!
   • I remember your name and favourite topics
   • Type 'help' anytime to see this list again

What would you like to explore first?";
        }
    }
}