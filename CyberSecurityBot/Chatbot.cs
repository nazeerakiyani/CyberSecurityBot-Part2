using System;
using System.Media;
using System.Threading;

namespace CyberSecurityBot
{
    public class Chatbot
    {
        private string? userName;

        public void Start()
        {
            PlayGreeting();   // 🔊 MUST RUN FIRST
            ShowHeader();     // 🎨 SHOW LOGO AFTER SOUND

            Console.Write("\nEnter your name: ");
            userName = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.Write("Please enter a valid name: ");
                userName = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=======================================");
            Console.WriteLine($"   Welcome, {userName}!");
            Console.WriteLine("=======================================");
            Console.WriteLine("You can ask me about:");
            Console.WriteLine("- Password safety");
            Console.WriteLine("- Phishing scams");
            Console.WriteLine("- Safe browsing");
            Console.WriteLine("\nType 'exit' to quit anytime.");
            Console.ResetColor();

            RunChat();
        }

        private void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.Load();
                player.PlaySync(); // ensures it actually finishes playing
            }
            catch
            {
                Console.WriteLine("(Audio file missing or not found)");
            }
        }

        private void ShowHeader()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("==================================================");
            Console.WriteLine("      CYBERSECURITY AWARENESS BOT");
            Console.WriteLine("==================================================");
            Console.WriteLine("");

            Console.WriteLine("              .-\"\"\"\"\"-.");
            Console.WriteLine("             /  _   _  \\");
            Console.WriteLine("            |  (o) (o)  |");
            Console.WriteLine("            |     ^     |");
            Console.WriteLine("            |  \\_____/  |");
            Console.WriteLine("             \\         /");
            Console.WriteLine("              '-.___.-'");

            Console.WriteLine("");
            Console.WriteLine("         Stay Safe Online!");
            Console.WriteLine("==================================================");

            Console.ResetColor();

            Thread.Sleep(1000); // small pause for effect
        }

        private void RunChat()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nYou: ");
                Console.ResetColor();

                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Bot: Please enter something valid.");
                    Console.ResetColor();
                    continue;
                }

                if (input.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Bot: Goodbye " + userName + "! Stay safe 👋");
                    Console.ResetColor();
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Bot: " + ResponseHandler(input));
                Console.ResetColor();
            }
        }

        private string ResponseHandler(string input)
        {
            input = input.ToLower();

            if (input.Contains("how are you"))
                return "I'm doing great! I'm here to help you stay safe online.";

            if (input.Contains("purpose"))
                return "My purpose is to educate you about cybersecurity.";

            if (input.Contains("password"))
                return "Use strong passwords with letters, numbers, and symbols.";

            if (input.Contains("phishing"))
                return "Phishing is when scammers trick you into giving personal info.";

            if (input.Contains("safe browsing"))
                return "Avoid clicking suspicious links and always check URLs.";

            if (input.Contains("scam"))
                return "Scams try to trick you into giving money or personal information.";

            if (input.Contains("virus"))
                return "A virus is harmful software that can damage your computer.";

            if (input.Contains("hello"))
                return $"Hello {userName}! I'm your cybersecurity assistant.";

            return "I didn’t understand that. Try asking about passwords, phishing, or safe browsing.";
        }
    }
}