// ============================================================
// File: Delegates/ResponseDelegate.cs
// Purpose: Defines a custom delegate for handling chatbot responses.
//          Delegates allow methods to be passed as parameters,
//          making the response system flexible and extensible.
// ============================================================

namespace CyberSecurityBot.Delegates
{
    /// <summary>
    /// Delegate for generating a chatbot response based on user input.
    /// This delegate type can reference any method that takes a string
    /// (user input) and returns a string (bot response).
    /// </summary>
    /// <param name="userInput">The message typed by the user</param>
    /// <returns>The chatbot's response string</returns>
    public delegate string ResponseDelegate(string userInput);
}