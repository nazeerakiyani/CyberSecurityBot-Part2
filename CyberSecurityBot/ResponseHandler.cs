using System;

namespace CyberSecurityBot
{
    public class ResponseHandler
    {
        public string GetResponse(string input)
        {
            input = input.ToLower();

            if (input.Contains("hello") || input.Contains("hi"))
                return "Hello! I'm your cybersecurity assistant.";

            else if (input.Contains("how are you"))
                return "I'm doing great! I'm here to help you stay safe online.";

            else if (input.Contains("purpose") || input.Contains("what can you do") || input.Contains("what can you tell me"))
                return "I can teach you about cybersecurity topics like passwords, phishing, scams, safe browsing, 2FA, updates, and VPN.";

            else if (input.Contains("cyber") || input.Contains("security"))
                return "Cybersecurity is about protecting your devices and personal information.";

            // NEW RESPONSES ADDED HERE:
            else if (input.Contains("2fa") || input.Contains("two factor"))
                return "2FA adds extra security—use an authenticator app, not SMS when possible.";

            else if (input.Contains("update") || input.Contains("updates"))
                return "Always keep your software, apps, and OS updated to patch security vulnerabilities.";

            else if (input.Contains("vpn"))
                return "VPN encrypts your internet connection, hiding your activity from hackers on public WiFi.";

            // YOUR ORIGINAL RESPONSES (UNCHANGED):
            else if (input.Contains("password"))
                return "Use strong passwords with letters, numbers, and symbols.";

            else if (input.Contains("phishing") || input.Contains("phising"))
                return "Phishing is when scammers trick you into giving personal info.";

            else if (input.Contains("safe browsing") || input.Contains("browse"))
                return "Avoid suspicious links and always check websites.";

            else if (input.Contains("scam"))
                return "Scams try to trick you into giving money or personal info.";

            else if (input.Contains("virus"))
                return "A virus is harmful software that can damage your computer.";

            else
                return "I didn't understand. Try asking about cybersecurity.";
        }
    }
}