using System;
using System.Collections.Generic;
using System.Drawing;
<<<<<<< HEAD
=======
using System.IO;
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
using System.Windows.Forms;

namespace GradeWiz
{
    public partial class GradeWiz : Form
    {
        private string _moduleCode;
        private Dictionary<int, (double Weighting, string Name)> _componentDetails;
        private MenuStrip _menuStrip;

<<<<<<< HEAD
        private const int TextBoxWidth = 50;
        private const int ButtonWidth = 80;
        private const int ButtonHeight = 30;
        private const int titleSize = 9;

=======
        private const int LabelX = 10;
        private const int TextBoxWidth = 50;
        private const int ComponentSpacing = 40;
        private const int ButtonWidth = 80;
        private const int ButtonHeight = 30;
        private const int StartY = 40;
        private const int LabelSpacing = 40;
        private const int MenuStripHeight = 24;

        // The absolute path to the CSV file
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
        private const string CsvFilePath = @"C:\Users\rmadawho\OneDrive - University College London\ProjectsRA\testData.csv";

        public GradeWiz()
        {
            InitializeComponent();
            this.BackColor = Color.LightGray;
<<<<<<< HEAD
            this.ClientSize = new Size(470, 310);
=======
            this.ClientSize = new Size(400, 350);
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
            _componentDetails = new Dictionary<int, (double Weighting, string Name)>();

            CreateMenuBar();
            PromptForModuleCode();
        }

        private void PromptForModuleCode()
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            var label = new Label
            {
                Text = "Enter Module Code:",
<<<<<<< HEAD
                Font = new Font("Arial", titleSize, FontStyle.Bold), // Set the font to Arial, size 12, bold

                Location = new Point(10, 40),
=======
                Location = new Point(LabelX, StartY),
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
                AutoSize = true
            };
            panel.Controls.Add(label);

            var moduleCodeTextBox = new TextBox
            {
<<<<<<< HEAD
                Location = new Point(160, 40),
=======
                Location = new Point(LabelX + 150, StartY),
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
                Width = TextBoxWidth + 23
            };
            panel.Controls.Add(moduleCodeTextBox);

            var submitButton = new Button
            {
                Text = "Submit",
<<<<<<< HEAD
                Location = new Point(10, 80),
=======
                Location = new Point(LabelX, StartY + ComponentSpacing),
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            submitButton.Click += (sender, e) =>
            {
                _moduleCode = moduleCodeTextBox.Text;
                LoadDataFromCSV(_moduleCode);
            };
            panel.Controls.Add(submitButton);

            Controls.Clear();
            Controls.Add(panel);
            Controls.Add(_menuStrip);
        }

        private void CreateMenuBar()
        {
            _menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");

            var aboutItem = new ToolStripMenuItem("About");
            aboutItem.Click += (sender, e) => ShowAboutDialog();
            fileMenu.DropDownItems.Add(aboutItem);

            var restartItem = new ToolStripMenuItem("Restart");
<<<<<<< HEAD
            restartItem.Click += (sender, e) => RestartApplication();
=======
            restartItem.Click += (sender, e) => {
                // Clear current controls and reset the module code
                Controls.Clear();
                Controls.Add(_menuStrip);
                _moduleCode = string.Empty; // Clear the module code

                // Directly set the title to "GradeWiz ✔"
                Text = "GradeWiz ✔";

                // Show the module code entry prompt
                PromptForModuleCode();
            };

>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
            fileMenu.DropDownItems.Add(restartItem);

            var quitItem = new ToolStripMenuItem("Quit");
            quitItem.Click += (sender, e) => Application.Exit();
            fileMenu.DropDownItems.Add(quitItem);

            _menuStrip.Items.Add(fileMenu);
            MainMenuStrip = _menuStrip;
            Controls.Add(_menuStrip);
        }

        private void ShowAboutDialog()
        {
            MessageBox.Show(
                "GradeWiz ✔\n\nA simple app that calculates a final module mark based on component marks and weightings.\n\n© 2024 Ricki Angel\nhttps://github.com/TechAngelX\n\n" +
                "Licensed under the GNU General Public License v3.0",
                "About GradeWiz",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void LoadDataFromCSV(string passedModuleCode)
        {
            try
            {
<<<<<<< HEAD
                _componentDetails = CsvLoader.LoadComponentDetails(CsvFilePath, passedModuleCode);
                if (_componentDetails.Count == 0)
=======
                var csvPath = CsvFilePath;
                var lines = File.ReadAllLines(csvPath);
                bool moduleCodeFound = false;

                foreach (var line in lines)
                {
                    var fields = line.Split(',');

                    var moduleCodeInFile = fields[0].ToUpper();
                    var passedCode = passedModuleCode.ToUpper(); // Ensure case insensitivity

                    if (moduleCodeInFile == passedCode)
                    {
                        _componentDetails.Clear();

                        for (int i = 1; i < fields.Length; i += 2)
                        {
                            if (i + 1 < fields.Length)
                            {
                                if (double.TryParse(fields[i], out double weighting))
                                {
                                    _componentDetails[i / 2 + 1] = (weighting, fields[i + 1]);
                                }
                            }
                        }
                        moduleCodeFound = true;
                        break;
                    }
                }

                if (!moduleCodeFound)
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
                {
                    MessageBox.Show($"Module '{passedModuleCode}' not found. Ensure it is a valid CS Dept module code.");
                    ClearScreenAndReturnToModuleCodeEntry();
                }
                else
                {
                    ShowComponentMarkScreen();
<<<<<<< HEAD
                    // CenterTitle(); // Removed the call to CenterTitle
=======
                    CenterTitle();
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from data.csv: {ex.Message}");
                ClearScreenAndReturnToModuleCodeEntry();
            }
        }

        private void ClearScreenAndReturnToModuleCodeEntry()
        {
            Controls.Clear();
            Controls.Add(_menuStrip);
            PromptForModuleCode();
        }

        private void ShowComponentMarkScreen()
        {
<<<<<<< HEAD
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 3,
                RowCount = _componentDetails.Count + 2,
                Padding = new Padding(10)
            };

            var label = new Label
            {
                Text = "Enter mark(s) for each component:\n\n",
                Font = new Font("Arial", titleSize, FontStyle.Bold), // Set the font to Arial, size 12, bold

                AutoSize = true
            };
            panel.Controls.Add(label, 0, 0);
            panel.SetColumnSpan(label, 3);
=======
            var panel = new Panel { Dock = DockStyle.Fill };

            var label = new Label
            {
                Text = "Enter mark(s) for each component:",
                Location = new Point(LabelX, StartY),
                AutoSize = true
            };
            panel.Controls.Add(label);
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8

            var markFields = new TextBox[_componentDetails.Count];
            for (int i = 0; i < _componentDetails.Count; i++)
            {
                int componentNumber = i + 1;
                var (weighting, name) = _componentDetails[componentNumber];

                var componentLabel = new Label
                {
                    Text = $"Component {componentNumber}:",
<<<<<<< HEAD
                    AutoSize = true
                };
                panel.Controls.Add(componentLabel, 0, i + 1);

                var markField = new TextBox
                {
                    Width = TextBoxWidth
                };
                markFields[i] = markField;
                panel.Controls.Add(markField, 1, i + 1);
=======
                    Location = new Point(LabelX, StartY + LabelSpacing + i * ComponentSpacing),
                    AutoSize = true
                };
                panel.Controls.Add(componentLabel);

                var markField = new TextBox
                {
                    Location = new Point(LabelX + 100, StartY + LabelSpacing + i * ComponentSpacing),
                    Width = TextBoxWidth
                };
                markFields[i] = markField;
                panel.Controls.Add(markField);
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8

                var detailLabel = new Label
                {
                    Text = $"{weighting}% - {name}",
<<<<<<< HEAD
                    AutoSize = true
                };
                panel.Controls.Add(detailLabel, 2, i + 1);
=======
                    Location = new Point(LabelX + 170, StartY + LabelSpacing + i * ComponentSpacing),
                    AutoSize = true
                };
                panel.Controls.Add(detailLabel);
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
            }

            var calculateButton = new Button
            {
                Text = "Calculate",
<<<<<<< HEAD
=======
                Location = new Point(LabelX + ButtonWidth + 10, StartY + LabelSpacing + _componentDetails.Count * ComponentSpacing + 10),
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            calculateButton.Click += (sender, e) =>
            {
                try
                {
                    double[] scores = new double[_componentDetails.Count];
                    for (int i = 0; i < _componentDetails.Count; i++)
                    {
                        scores[i] = double.Parse(markFields[i].Text);
                    }
                    ShowResultScreen(scores);
                }
                catch (FormatException)
                {
<<<<<<< HEAD
                    MessageBox.Show("Please enter a number between 0 and 100.");
                }
            };
            panel.Controls.Add(calculateButton, 1, _componentDetails.Count + 1);
=======
                    MessageBox.Show("Please enter valid numbers for the marks.");
                }
            };
            panel.Controls.Add(calculateButton);
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8

            var backButton = new Button
            {
                Text = "Back",
<<<<<<< HEAD
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            backButton.Click += (sender, e) => PromptForModuleCode();
            panel.Controls.Add(backButton, 0, _componentDetails.Count + 1);
=======
                Location = new Point(LabelX, StartY + LabelSpacing + _componentDetails.Count * ComponentSpacing + 10),
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            backButton.Click += (sender, e) => ShowPreviousScreen();
            panel.Controls.Add(backButton);
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8

            Controls.Clear();
            Controls.Add(panel);
            Controls.Add(_menuStrip);
        }

<<<<<<< HEAD
      private void ShowResultScreen(double[] scores)
{
    var panel = new TableLayoutPanel
    {
        Dock = DockStyle.Fill,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        ColumnCount = 2,
        RowCount = _componentDetails.Count + 2, // Adjusted row count to fit the button panel
        Padding = new Padding(10)
    };

    // Result label
    var resultLabel = new Label
    {
        Text = $"Total module mark: {CalculateTotalMark(scores):F2}",
        Font = new Font("Arial", 15, FontStyle.Bold),
        ForeColor = Color.Black,
        AutoSize = true
    };
    panel.Controls.Add(resultLabel, 0, 0);
    panel.SetColumnSpan(resultLabel, 2);

    // Component details
    for (int i = 0; i < _componentDetails.Count; i++)
    {
        int componentNumber = i + 1;
        var (weighting, name) = _componentDetails[componentNumber];

        var componentLabel = new Label
        {
            Text = $"Component {componentNumber} ({weighting}% - {name}):",
            AutoSize = true
        };
        panel.Controls.Add(componentLabel, 0, i + 1);

        var markLabel = new Label
        {
            Text = $"{scores[i] * (weighting / 100):F2}",
            AutoSize = true
        };
        panel.Controls.Add(markLabel, 1, i + 1);
    }

    // Create a FlowLayoutPanel for the buttons
    var buttonPanel = new FlowLayoutPanel
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.LeftToRight, // Align buttons horizontally
        AutoSize = true,
        Padding = new Padding(10),
        WrapContents = false // Keep buttons on the same line
    };

    var backButton = new Button
    {
        Text = "Back",
        Size = new Size(ButtonWidth, ButtonHeight)
    };
    backButton.Click += (sender, e) => ShowComponentMarkScreen();
    buttonPanel.Controls.Add(backButton);

    var restartButton = new Button
    {
        Text = "Restart",
        Size = new Size(ButtonWidth, ButtonHeight)
    };
    restartButton.Click += (sender, e) => RestartApplication();
    buttonPanel.Controls.Add(restartButton);

    var quitButton = new Button
    {
        Text = "Quit",
        Size = new Size(ButtonWidth, ButtonHeight)
    };
    quitButton.Click += (sender, e) => Application.Exit();
    buttonPanel.Controls.Add(quitButton);

    // Add the button panel to the TableLayoutPanel
    panel.Controls.Add(buttonPanel, 0, _componentDetails.Count + 1);
    panel.SetColumnSpan(buttonPanel, 2); // Span across both columns

    Controls.Clear();
    Controls.Add(panel);
    Controls.Add(_menuStrip);
}

=======
        private void ShowPreviousScreen()
        {
            PromptForModuleCode();
        }

        private void ShowResultScreen(double[] scores)
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            var resultLabel = new Label
            {
                Text = $"Total module mark: {CalculateTotalMark(scores):F2}",
                Font = new Font("Arial", 15, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(LabelX, StartY),
                AutoSize = true
            };
            panel.Controls.Add(resultLabel);

            for (int i = 0; i < _componentDetails.Count; i++)
            {
                int componentNumber = i + 1;
                var (weighting, name) = _componentDetails[componentNumber];

                var componentLabel = new Label
                {
                    Text = $"Component {componentNumber} mark ({weighting}% - {name}):",
                    Location = new Point(LabelX, StartY + LabelSpacing + i * 30),
                    AutoSize = true
                };
                var markLabel = new Label
                {
                    Text = $"{scores[i] * (weighting / 100):F2}",
                    Location = new Point(LabelX + 220, StartY + LabelSpacing + i * 30),
                    AutoSize = true
                };

                panel.Controls.Add(componentLabel);
                panel.Controls.Add(markLabel);
            }

            var backButton = new Button
            {
                Text = "Back",
                Location = new Point(LabelX, StartY + LabelSpacing + _componentDetails.Count * 30 + 10),
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            backButton.Click += (sender, e) => ShowComponentMarkScreen();
            panel.Controls.Add(backButton);

            var restartButton = new Button
            {
                Text = "Restart",
                Location = new Point(LabelX + ButtonWidth + 10, StartY + LabelSpacing + _componentDetails.Count * 30 + 10),
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            restartButton.Click += (sender, e) =>
            {
                Controls.Clear();
                Controls.Add(_menuStrip);
                _moduleCode = string.Empty; // Clear the module code
                Text = "GradeWiz ✔"; // Directly set the title
                PromptForModuleCode();
            };
            panel.Controls.Add(restartButton);

            var quitButton = new Button
            {
                Text = "Quit",
                Location = new Point(LabelX + 2 * (ButtonWidth + 10), StartY + LabelSpacing + _componentDetails.Count * 30 + 10),
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            quitButton.Click += (sender, e) => Application.Exit();
            panel.Controls.Add(quitButton);

            Controls.Clear();
            Controls.Add(panel);
            Controls.Add(_menuStrip);
        }
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8

        private double CalculateTotalMark(double[] scores)
        {
            double totalMark = 0;
<<<<<<< HEAD
            foreach (var component in _componentDetails)
            {
                var (weighting, _) = component.Value;
                int index = component.Key - 1;
                totalMark += scores[index] * (weighting / 100);
=======
            for (int i = 0; i < _componentDetails.Count; i++)
            {
                var (weighting, _) = _componentDetails[i + 1];
                totalMark += scores[i] * (weighting / 100);
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
            }
            return totalMark;
        }

<<<<<<< HEAD
        private void RestartApplication()
        {
            Application.Restart();
=======
        private void CenterTitle()
        {
            var title = string.IsNullOrEmpty(_moduleCode)
                ? "GradeWiz ✔"
                : $"GradeWiz ✔ - {_moduleCode.ToUpper()}";
            Text = title;

            // Center title (if needed)
            var titleSize = TextRenderer.MeasureText(title, Font);
            var formWidth = ClientSize.Width;
            var titleX = (formWidth - titleSize.Width) / 2;
            // Optionally update title alignment if needed
            this.Text = title;
>>>>>>> e3c647ee8a49cab599220ad0db4ba3dce7b955d8
        }
    }
}
