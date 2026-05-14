// ============================================================
// File: Utilities/AsciiArt.cs
// Purpose: Stores and provides the ASCII art logo for the chatbot.
//          This adds visual branding to the GUI.
// ============================================================

namespace CyberSecurityBot.Utilities
{
    /// <summary>
    /// Provides ASCII art for the Cybersecurity Awareness Bot.
    /// </summary>
    public static class AsciiArt
    {
        /// <summary>
        /// Returns the ASCII art logo as a multi-line string.
        /// </summary>
        public static string GetLogo()
        {
            return @"
   ____      _                       _                 
  / ___|   _| |__   ___  _   _ _ __ | |__   ___ _ __   
 | |  | | | | '_ \ / _ \| | | | '_ \| '_ \ / _ \ '__|  
 | |__| |_| | |_) | (_) | |_| | |_) | | | |  __/ |     
  \____\__, |_.__/ \___/ \__, | .__/|_| |_|\___|_|     
       |___/             |___/|_|                      
   ____ _           _   _                             
  / ___| |__   __ _| |_| |_ ___ _ __                  
 | |   | '_ \ / _` | __| __/ _ \ '__|                 
 | |___| | | | (_| | |_| ||  __/ |                    
  \____|_| |_|\__,_|\__|\__\___|_|                    
  CYBERSECURITY AWARENESS ASSISTANT
";
        }
    }
}