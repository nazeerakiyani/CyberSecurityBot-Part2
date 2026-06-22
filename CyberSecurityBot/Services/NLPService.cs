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
    /// Simulates NLP by detecting user intent from various phrasings.
    /// </summary>
    public class NLPService
    {
        private readonly Dictionary<string, List<string>> intentPatterns;

        public NLPService()
        {
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
                        "create reminder", "remind me about", "remind me"
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
                        "any reminders", "do I have tasks", "do I have reminders"
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
                        "task 1 done", "task 2 done", "task 3 done"
                    }
                },
                {
                    "delete_task",
                    new List<string>()
                    {
                        "delete task", "remove task", "get rid of task", "cancel task",
                        "erase task", "clear task", "remove reminder", "delete reminder",
                        "I don't need task", "I do not need task", "remove this task",
                        "delete task 1", "delete task 2", "remove task 1"
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
                        "can you quiz me", "let me quiz", "quiz now"
                    }
                },
                {
                    "cancel_quiz",
                    new List<string>()
                    {
                        "cancel quiz", "stop quiz", "end quiz", "quit quiz",
                        "I don't want to quiz", "I do not want to quiz",
                        "exit quiz", "leave quiz", "stop the quiz",
                        "no more quiz", "end the quiz"
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
                        "what have you been doing", "show me the log", "view log"
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
                        "what are you", "who are you", "what is your purpose"
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
                        "two factor", "2fa", "encryption", "firewall", "backup"
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
        /// Detects user intent from input text.
        /// </summary>
        /// <param name="input">User's message</param>
        /// <returns>The detected intent, or "unknown" if no match</returns>
        public string DetectIntent(string input)
        {
            string normalised = input.ToLower().Trim();

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

            // Check for partial matches (if input contains key words from patterns)
            foreach (var intent in intentPatterns)
            {
                foreach (string pattern in intent.Value)
                {
                    string[] patternWords = pattern.Split(' ');
                    int matchCount = 0;
                    foreach (string word in patternWords)
                    {
                        if (word.Length > 2 && normalised.Contains(word))
                        {
                            matchCount++;
                        }
                    }
                    // If 70% of words match, consider it a match
                    if (patternWords.Length > 0 && (double)matchCount / patternWords.Length >= 0.7)
                    {
                        return intent.Key;
                    }
                }
            }

            return "unknown";
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
                "plan to", "want to remember", "need reminder for"
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
        /// Gets a help message listing all available commands.
        /// </summary>
        public string GetHelpMessage()
        {
            return @"Here are all the things I can help you with:

**Cybersecurity Topics:**
- Ask about: passwords, phishing, privacy, scams, malware, safe browsing
- Say 'tell me more' or 'explain more' for follow-up tips

**Task Assistant:**
- 'add task' or 'remind me to...' - Add a cybersecurity task
- 'show my tasks' or 'what do I need to do' - View your tasks
- 'complete task 1' - Mark a task as done
- 'delete task 1' - Remove a task

**Quiz:**
- 'start quiz' or 'quiz me' or just 'quiz' - Test your knowledge
- 'cancel quiz' - Stop the quiz

**Activity Log:**
- 'show activity log' or 'what have you done' - See what we've done

**General:**
- Say 'help' anytime to see this list again
- I can detect your mood and respond with empathy!";
        }
    }
}