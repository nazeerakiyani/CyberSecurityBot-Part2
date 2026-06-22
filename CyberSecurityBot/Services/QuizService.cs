// ============================================================
// File: Services/QuizService.cs
// Purpose: Manages the cybersecurity quiz mini-game with scoring,
//          immediate feedback, and final results.
// ============================================================

using System;
using System.Collections.Generic;

namespace CyberSecurityBot.Services
{
    /// <summary>
    /// Manages the cybersecurity quiz mini-game.
    /// </summary>
    public class QuizService
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex;
        private int score;
        private bool isActive;

        public bool IsActive => isActive;
        public int CurrentQuestionNumber => currentQuestionIndex + 1;
        public int TotalQuestions => questions.Count;

        /*
         * GeeksforGeeks, 2024. C# List and Dictionary for Quiz Application.
         * [Online]. Available at: https://www.geeksforgeeks.org/c-sharp-list-class/
         * [Accessed 15 June 2026].
         */
        public QuizService()
        {
            questions = new List<QuizQuestion>();
            currentQuestionIndex = 0;
            score = 0;
            isActive = false;
            LoadQuestions();
        }

        /// <summary>
        /// Loads all cybersecurity quiz questions.
        /// </summary>
        private void LoadQuestions()
        {
            questions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "Correct! Reporting phishing emails helps prevent others from falling victim. Never share your password via email."
            ));

            questions.Add(new QuizQuestion(
                "Which of the following is the strongest password?",
                new List<string> { "Password123", "MyDog2024", "Xk9#mP2$vLq@", "YourName2024" },
                2,
                "Correct! A strong password uses a mix of uppercase, lowercase, numbers, and special characters. Avoid personal information."
            ));

            questions.Add(new QuizQuestion(
                "True or False: Two-factor authentication (2FA) makes your account less secure.",
                new List<string> { "True", "False" },
                1,
                "Correct! 2FA adds an extra layer of security. Even if someone gets your password, they still need the second factor."
            ));

            questions.Add(new QuizQuestion(
                "What does 'https://' in a URL indicate?",
                new List<string> { "The website is free", "The connection is encrypted", "The website is fast", "The website is new" },
                1,
                "Correct! HTTPS means the connection between your browser and the website is encrypted, protecting your data."
            ));

            questions.Add(new QuizQuestion(
                "True or False: It's safe to use the same password for all your accounts.",
                new List<string> { "True", "False" },
                1,
                "Correct! Using the same password everywhere is dangerous. If one site is breached, all your accounts are at risk."
            ));

            questions.Add(new QuizQuestion(
                "Which is a common sign of a phishing email?",
                new List<string> { "Perfect spelling and grammar", "Urgent language demanding immediate action", "Sent from a known contact", "Contains your correct name" },
                1,
                "Correct! Phishing emails often use urgency, threats, or prizes to pressure you into acting quickly without thinking."
            ));

            questions.Add(new QuizQuestion(
                "What is malware?",
                new List<string> { "A type of antivirus software", "Harmful software designed to damage or steal data", "A secure browsing mode", "A password manager" },
                1,
                "Correct! Malware includes viruses, trojans, ransomware, and spyware — all designed to harm your device or steal data."
            ));

            questions.Add(new QuizQuestion(
                "True or False: Public Wi-Fi is always safe for online banking.",
                new List<string> { "True", "False" },
                1,
                "Correct! Public Wi-Fi is often unsecured. Hackers can intercept your data. Use a VPN or avoid sensitive transactions."
            ));

            questions.Add(new QuizQuestion(
                "What should you do before clicking a link in an email?",
                new List<string> { "Click immediately to see what it is", "Hover over it to check the actual URL", "Forward it to a friend first", "Reply to the sender asking if it's safe" },
                1,
                "Correct! Hovering reveals the real destination. Scammers disguise malicious links to look legitimate."
            ));

            questions.Add(new QuizQuestion(
                "Which social engineering tactic involves creating a fake scenario to trick you?",
                new List<string> { "Phishing", "Pretexting", "Malware", "Encryption" },
                1,
                "Correct! Pretexting involves creating a false story (pretext) to manipulate you into revealing information or taking action."
            ));

            questions.Add(new QuizQuestion(
                "True or False: Software updates are only for adding new features.",
                new List<string> { "True", "False" },
                1,
                "Correct! Updates often include critical security patches that fix vulnerabilities hackers could exploit."
            ));

            questions.Add(new QuizQuestion(
                "What is the best way to back up important files?",
                new List<string> { "Only on your computer", "On an external drive and cloud storage", "Email them to yourself", "Print them out" },
                1,
                "Correct! The 3-2-1 rule: 3 copies, 2 different media types, 1 offsite. External drive + cloud is a great combination."
            ));
        }

        /// <summary>
        /// Starts a new quiz session.
        /// </summary>
        public string StartQuiz()
        {
            isActive = true;
            currentQuestionIndex = 0;
            score = 0;
            return GetCurrentQuestion();
        }

        /// <summary>
        /// Gets the current question text with options.
        /// </summary>
        public string GetCurrentQuestion()
        {
            if (!isActive || currentQuestionIndex >= questions.Count)
                return "No active quiz. Type 'start quiz' to begin!";

            QuizQuestion q = questions[currentQuestionIndex];
            string result = $"Question {currentQuestionIndex + 1} of {questions.Count}:\n\n{q.Question}\n\n";

            for (int i = 0; i < q.Options.Count; i++)
            {
                string optionLetter = ((char)('A' + i)).ToString();
                result += $"{optionLetter}) {q.Options[i]}\n";
            }

            result += "\nType the letter (A, B, C, or D) or number (1, 2) to answer.";
            return result;
        }

        /// <summary>
        /// Processes the user's answer and returns feedback.
        /// </summary>
        public string SubmitAnswer(string answer)
        {
            if (!isActive)
                return "No active quiz. Type 'start quiz' to begin!";

            int selectedIndex = ParseAnswer(answer);
            if (selectedIndex < 0 || selectedIndex >= questions[currentQuestionIndex].Options.Count)
                return "Please answer with the letter (A, B, C, D) or number (1, 2).";

            QuizQuestion currentQ = questions[currentQuestionIndex];
            bool isCorrect = selectedIndex == currentQ.CorrectAnswerIndex;

            if (isCorrect)
                score++;

            string feedback = isCorrect ? "✅ Correct!" : "❌ Wrong!";
            feedback += $"\n\n{currentQ.Explanation}\n\n";

            currentQuestionIndex++;

            if (currentQuestionIndex >= questions.Count)
            {
                isActive = false;
                feedback += GetFinalResults();
            }
            else
            {
                feedback += GetCurrentQuestion();
            }

            return feedback;
        }

        /// <summary>
        /// Parses user answer input into an index.
        /// </summary>
        private int ParseAnswer(string answer)
        {
            answer = answer.Trim().ToUpper();

            if (answer.Length == 1 && char.IsLetter(answer[0]))
            {
                return answer[0] - 'A';
            }

            if (int.TryParse(answer, out int number))
            {
                return number - 1;
            }

            return -1;
        }

        /// <summary>
        /// Generates final quiz results with feedback.
        /// </summary>
        private string GetFinalResults()
        {
            double percentage = (double)score / questions.Count * 100;
            string message;

            if (percentage >= 90)
                message = "🏆 Outstanding! You're a cybersecurity expert!";
            else if (percentage >= 70)
                message = "🎉 Great job! You know your cybersecurity well!";
            else if (percentage >= 50)
                message = "👍 Good effort! Keep learning to stay safe online.";
            else
                message = "📚 Keep learning! Cybersecurity is important for everyone.";

            return $"=== QUIZ COMPLETE ===\n\nScore: {score}/{questions.Count} ({percentage:F0}%)\n\n{message}\n\nType 'start quiz' to try again!";
        }

        /// <summary>
        /// Cancels the active quiz.
        /// </summary>
        public string CancelQuiz()
        {
            isActive = false;
            return "Quiz cancelled. Type 'start quiz' whenever you're ready!";
        }
    }

    /*
     * C-Sharp Corner, 2023. C# Class and Object Tutorial for Quiz Applications.
     * [Online]. Available at: https://www.c-sharpcorner.com/article/c-sharp-class-and-object/
     * [Accessed 15 June 2026].
     */

    /// <summary>
    /// Represents a single quiz question with options and explanation.
    /// </summary>
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion(string question, List<string> options, int correctAnswerIndex, string explanation)
        {
            Question = question;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            Explanation = explanation;
        }
    }
}