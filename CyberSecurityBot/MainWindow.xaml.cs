using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using CyberSecurityBot.Services;
using CyberSecurityBot.Utilities;

namespace CyberSecurityBot
{
    /// <summary>
    /// Main window for the Cybersecurity Awareness Chatbot GUI.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ResponseService _responseService;
        private MemoryService _memoryService;
        private SentimentAnalyzer _sentimentAnalyzer;

        public MainWindow()
        {
            InitializeComponent();

            _responseService = new ResponseService();
            _memoryService = new MemoryService();
            _sentimentAnalyzer = new SentimentAnalyzer();

            SendButton.Click += SendButton_Click;
            UserInputTextBox.KeyDown += UserInputTextBox_KeyDown;

            DisplayAsciiArt();
            PlayVoiceGreeting();

            // Welcome message - first user input will be treated as name
            ShowBotMessage("Welcome! I'm your Cybersecurity Awareness Assistant. What's your name?");
        }

        private void DisplayAsciiArt()
        {
            string asciiArt = @"
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
            AsciiArtTextBlock.Text = asciiArt;
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.Play();
            }
            catch (Exception )
            {
                ShowBotMessage("[Voice greeting unavailable]");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInputTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput()
        {
            string input = UserInputTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                ShowBotMessage("Please type something before sending.");
                return;
            }

            ShowUserMessage(input);
            UserInputTextBox.Clear();

            string response = _responseService.GetResponse(input, _memoryService, _sentimentAnalyzer);
            ShowBotMessage(response);
        }

        private void ShowUserMessage(string message)
        {
            Border bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0, 132, 255)),
                CornerRadius = new CornerRadius(15, 15, 0, 15),
                Padding = new Thickness(12),
                Margin = new Thickness(50, 5, 10, 5),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            bubble.Child = text;
            ChatStackPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        private void ShowBotMessage(string message)
        {
            Border bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(69, 69, 69)),
                CornerRadius = new CornerRadius(15, 15, 15, 0),
                Padding = new Thickness(12),
                Margin = new Thickness(10, 5, 50, 5),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            bubble.Child = text;
            ChatStackPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ChatScrollViewer.ScrollToEnd();
            }), DispatcherPriority.Render);
        }
    }
}