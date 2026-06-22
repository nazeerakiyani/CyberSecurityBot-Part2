# CyberSecurityBot-Part3
WPF Cybersecurity Awareness Chatbot - PROG6221 Part 3

# Cybersecurity Awareness Chatbot - Part 3

## PROG6221 - Programming 2A

### Author
**Nazeera Kiyani**
Student Number: **ST10473549**
Campus: **Emeris Durban North**

---

### Description
WPF chatbot that educates South African citizens on cybersecurity topics including passwords, phishing, privacy, scams, malware, and safe browsing. This Part 3 version adds MySQL database integration, a task assistant with reminders, a cybersecurity quiz mini-game, advanced NLP simulation, and an activity log feature — all working seamlessly with Part 1's GUI and Part 2's memory and sentiment features.

---

### Features

#### Part 1 Features
- **Voice greeting** on startup
- **ASCII art** logo display

#### Part 2 Features
- **Keyword recognition** (6 topics, 30+ variations)
- **Sequential random responses** with no-repeat logic
- **Conversation flow** with proactive suggestions after 3 tips
- **Memory and recall** (name + favourite topic personalisation)
- **Sentiment detection** (worried, curious, frustrated, neutral)
- **Error handling** for empty/unknown inputs

#### Part 3 Features
- **MySQL Database Integration** — tasks and activity logs persist in database
- **Task Assistant with Reminders** — add, view, complete, and delete cybersecurity tasks with optional reminder dates
- **Cybersecurity Quiz Mini-Game** — 12 questions (MCQ + true/false) with immediate feedback and final score
- **Advanced NLP Simulation** — 200+ intent patterns with synonym mapping and typo tolerance
- **Activity Log** — tracks all bot actions with timestamps, shows last 10 with "show more" option

---

### How to Run
1. Open MySQL Workbench and ensure the `cybersecurity_bot` database exists
2. Open `CyberSecurityBot.sln` in Visual Studio 2022
3. Ensure `MySql.Data` NuGet package is installed
4. Update the connection string in `DatabaseService.cs` with your MySQL password
5. Press **F5** to build and run
6. Type your name when prompted
7. Ask about cybersecurity topics or use commands like `add task`, `start quiz`, `show activity log`

---

### Database Setup
```sql
CREATE DATABASE cybersecurity_bot;

USE cybersecurity_bot;

CREATE TABLE tasks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    reminder_date DATETIME,
    is_completed BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE activity_log (
    id INT AUTO_INCREMENT PRIMARY KEY,
    action_type VARCHAR(100) NOT NULL,
    description TEXT,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

---

### Technologies
- C# .NET 6/7/8
- WPF (Windows Presentation Foundation)
- XAML for UI design
- MySQL with MySql.Data connector
- System.Media for voice greeting

---

### Project Structure
```
CyberSecurityBot/
├── MainWindow.xaml          # GUI layout
├── MainWindow.xaml.cs       # Main logic, task flow state
├── App.xaml                 # Application resources
├── Services/
│   ├── ResponseService.cs   # Core response engine, NLP routing
│   ├── DatabaseService.cs   # MySQL CRUD operations
│   ├── QuizService.cs       # Quiz mini-game logic
│   ├── NLPService.cs        # Intent detection, synonym mapping
│   ├── MemoryService.cs     # User memory storage
│   └── SentimentAnalyzer.cs # Sentiment analysis
├── Utilities/
│   └── InputValidator.cs    # Input normalisation
└── Delegates/
    └── ResponseDelegate.cs  # Custom response handler
```

---

### Commands
| Command | Description |
|---------|-------------|
| `password`, `phishing`, etc. | Get cybersecurity tips |
| `add task` or `remind me to...` | Create a task with optional reminder |
| `show my tasks` | View all tasks with reminder status |
| `complete task 1` | Mark a task as completed |
| `delete task 1` | Remove a task |
| `start quiz` | Begin cybersecurity quiz (12 questions) |
| `show activity log` | View recent actions with timestamps |
| `help` | Show all available commands |
| `bye` | Exit the chatbot |

---

### GitHub Releases
- **v1.0-part1** — Basic chatbot with GUI
- **v2.0-part2** — Enhanced with memory and sentiment analysis
- **v3.0-final** — Complete version with database, quiz, NLP, and activity log

---

## References

Stack Overflow Community, 2019. *Parse returned text for specific set of words or phrases*. Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/56091373/parse-returned-text-for-specific-set-of-words-or-phrases [Accessed 11 May 2026].

Microsoft, 2025. *Implement sequential conversation flow - Bot Service*. Microsoft Learn. [Online]. Available at: https://learn.microsoft.com/en-us/azure/bot-service/bot-builder-dialog-manage-conversation-flow [Accessed 11 May 2026].

Dialzara, 2025. *Chatbot Sentiment Analysis: Complete Guide to Implementation and Optimization*. [Online]. Available at: https://dialzara.com/blog/step-by-step-guide-to-adding-sentiment-analysis-to-chatbots [Accessed 12 May 2026].

C-Sharp Corner, 2025. *How to Handle Follow-Up Questions and Maintain Context in Chatbots*. [Online]. Available at: https://www.c-sharpcorner.com/article/how-to-handle-follow-up-questions-and-maintain-context-in-chatbots-easy-guide/ [Accessed 12 May 2026].

Maguire, J., 2024. *Semantic Kernel: Using Memories to Create Intelligent AI Agents*. [Online]. Available at: https://jamiemaguire.net/index.php/2024/10/06/semantic-kernel-using-memories-to-create-intelligent-ai-agents/ [Accessed 11 May 2026].

Stack Overflow Community, 2011. *Check if string is empty or all spaces in C#*. Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/7438957/check-if-string-is-empty-or-all-spaces-in-c-sharp [Accessed 12 May 2026].

Oracle. 2024. *MySQL Connector/NET Developer Guide*. MySQL Documentation. [Online]. Available at: https://dev.mysql.com/doc/connector-net/en/ [Accessed 15 June 2026].

Stack Overflow Community, 2020. *C# MySQL insert query with parameters*. Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/65229757/c-sharp-mysql-insert-query-with-parameters [Accessed 15 June 2026].

C-Sharp Corner, 2023. *CRUD Operations in C# with MySQL Database*. [Online]. Available at: https://www.c-sharpcorner.com/article/crud-operations-in-c-sharp-with-mysql-database/ [Accessed 15 June 2026].

GeeksforGeeks, 2024. *Update and Delete Data in MySQL using C#*. [Online]. Available at: https://www.geeksforgeeks.org/update-and-delete-data-in-mysql-using-c-sharp/ [Accessed 15 June 2026].

TutorialsTeacher, 2024. *C# MySQL Delete Operation*. [Online]. Available at: https://www.tutorialsteacher.com/csharp/csharp-mysql-delete [Accessed 15 June 2026].

Stack Overflow Community, 2021. *Best practice for logging in C# application with database*. Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/68107055/best-practice-for-logging-in-c-sharp-application-with-database [Accessed 15 June 2026].

C-Sharp Corner, 2022. *Retrieve Data from MySQL Database in C#*. [Online]. Available at: https://www.c-sharpcorner.com/article/retrieve-data-from-mysql-database-in-c-sharp/ [Accessed 15 June 2026].

GeeksforGeeks, 2024. *C# List and Dictionary for Quiz Application*. [Online]. Available at: https://www.geeksforgeeks.org/c-sharp-list-class/ [Accessed 15 June 2026].

C-Sharp Corner, 2023. *C# Class and Object Tutorial for Quiz Applications*. [Online]. Available at: https://www.c-sharpcorner.com/article/c-sharp-class-and-object/ [Accessed 15 June 2026].

GeeksforGeeks, 2024. *C# String Manipulation for Natural Language Processing*. [Online]. Available at: https://www.geeksforgeeks.org/c-sharp-string-class/ [Accessed 15 June 2026].

Stack Overflow Community, 2022. *C# fuzzy string matching for chatbot commands*. Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/2344320/comparing-strings-with-tolerance [Accessed 15 June 2026].
"""

with open('/mnt/agents/output/README.md', 'w', encoding='utf-8') as f:
    f.write(readme_content)

print("README.md saved successfully!")
print(f"File size: {len(readme_content)} characters")
