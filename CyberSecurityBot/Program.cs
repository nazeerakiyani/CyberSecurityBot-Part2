using System;
using System.Windows;

namespace CyberSecurityBot
{
    /// <summary>
    /// Application entry point. Starts the WPF GUI instead of console.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application app = new Application();
            app.Run(new MainWindow());
        }
    }
}