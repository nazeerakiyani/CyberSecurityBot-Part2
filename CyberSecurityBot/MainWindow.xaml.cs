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
    public partial class MainWindow : Window
    {
        private ResponseService _responseService;
        private MemoryService _memoryService;
        private SentimentAnalyzer _sentimentAnalyzer;

        // Task flow state — stored here to guarantee persistence
        private string _pendingTaskTitle = "";
        private bool _waitingForTaskDescription = false;
        private bool _waitingForReminderDays = false;

        public MainWindow()
        {
            InitializeComponent();

            _responseService = new ResponseService();
            _memoryService = new MemoryService();
            _sentimentAnalyzer = new SentimentAnalyzer();

            DatabaseService db = new DatabaseService();
            ShowBotMessage(db.TestConnectionMessage());

            SendButton.Click += SendButton_Click;
            UserInputTextBox.KeyDown += UserInputTextBox_KeyDown;

            DisplayAsciiArt();
            PlayVoiceGreeting();

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
            catch (Exception)
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

            // ===== TASK FLOW HANDLED DIRECTLY IN MAINWINDOW =====

            // Step 2: User gave task description, ask for reminder days
            if (_waitingForTaskDescription)
            {
                _pendingTaskTitle = input;
                _waitingForTaskDescription = false;
                _waitingForReminderDays = true;
                ShowBotMessage($"Task added: '{input}'. How many days until reminder? (Type a number like 3, or 0 for no reminder)");
                return;
            }

            // Step 3: User gave reminder days — THIS IS THE FIX
            if (_waitingForReminderDays && !string.IsNullOrEmpty(_pendingTaskTitle))
            {
                int days;
                if (int.TryParse(input, out days) && days >= 0)
                {
                    DatabaseService db = new DatabaseService();
                    if (days == 0)
                    {
                        db.AddTask(_pendingTaskTitle, _pendingTaskTitle, null);
                        ShowBotMessage($"Task '{_pendingTaskTitle}' added with no reminder!");
                    }
                    else
                    {
                        DateTime reminderDate = DateTime.Now.AddDays(days);
                        db.AddTask(_pendingTaskTitle, _pendingTaskTitle, reminderDate);
                        ShowBotMessage($"Got it! I'll remind you in {days} days (on {reminderDate:yyyy-MM-dd}). Task '{_pendingTaskTitle}' added successfully!");
                    }
                    db.LogActivity("Task Added", $"Added task: {_pendingTaskTitle} with {(days == 0 ? "no" : days + " day")} reminder");
                    _pendingTaskTitle = "";
                    _waitingForReminderDays = false;
                    return;
                }
                else
                {
                    ShowBotMessage("Please type a number only (like 3, 7, 30) or 0 for no reminder.");
                    return;
                }
            }

            // Normal flow: pass to ResponseService
            string response = _responseService.GetResponse(input, _memoryService, _sentimentAnalyzer);

            // Step 1: Detect when ResponseService wants to start task flow
            if (response.Contains("What task would you like to add?"))
            {
                _waitingForTaskDescription = true;
            }

            // Handle quiz response splitting
            if (response.Contains("=== QUIZ COMPLETE ==="))
            {
                ShowBotMessage(response);
            }
            else if (response.Contains("✅ Correct!") || response.Contains("❌ Wrong!"))
            {
                string[] parts = response.Split(new[] { "\n\n" }, StringSplitOptions.None);
                if (parts.Length >= 2)
                {
                    ShowBotMessage(parts[0] + "\n\n" + parts[1]);
                }
                if (parts.Length > 2)
                {
                    string nextQuestion = string.Join("\n\n", parts, 2, parts.Length - 2);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ShowBotMessage(nextQuestion);
                    }), DispatcherPriority.Background);
                }
            }
            else
            {
                ShowBotMessage(response);
            }
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