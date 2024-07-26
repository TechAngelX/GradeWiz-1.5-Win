using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GradeWiz
{
    public partial class GradeWiz : Form
    {
        private string _moduleCode;
        private Dictionary<int, (double Weighting, string Name)> _componentDetails;
        private MenuStrip _menuStrip;

        private const int TextBoxWidth = 50;
        private const int ButtonWidth = 80;
        private const int ButtonHeight = 30;
        private const int titleSize = 9;

        private const string CsvFilePath = @"C:\Users\rmadawho\OneDrive - University College London\ProjectsRA\testData.csv";

        public GradeWiz()
        {
            InitializeComponent();
            this.BackColor = Color.LightGray;
            this.ClientSize = new Size(470, 310);
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
                Font = new Font("Arial", titleSize, FontStyle.Bold), // Set the font to Arial, size 9, bold
                Location = new Point(10, 40),
                AutoSize = true
            };
            panel.Controls.Add(label);

            var moduleCodeTextBox = new TextBox
            {
                Location = new Point(160, 40),
                Width = TextBoxWidth + 23
            };
            panel.Controls.Add(moduleCodeTextBox);

            var submitButton = new Button
            {
                Text = "Submit",
                Location = new Point(10, 80),
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
            restartItem.Click += (sender, e) => RestartApplication();
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
                _componentDetails = CsvLoader.LoadComponentDetails(CsvFilePath, passedModuleCode);
                if (_componentDetails.Count == 0)
                {
                    MessageBox.Show($"Module '{passedModuleCode}' not found. Ensure it is a valid CS Dept module code.");
                    ClearScreenAndReturnToModuleCodeEntry();
                }
                else
                {
                    ShowComponentMarkScreen();
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
                Font = new Font("Arial", titleSize, FontStyle.Bold), // Set the font to Arial, size 9, bold
                AutoSize = true
            };
            panel.Controls.Add(label, 0, 0);
            panel.SetColumnSpan(label, 3);

            var markFields = new TextBox[_componentDetails.Count];
            for (int i = 0; i < _componentDetails.Count; i++)
            {
                int componentNumber = i + 1;
                var (weighting, name) = _componentDetails[componentNumber];

                var componentLabel = new Label
                {
                    Text = $"Component {componentNumber}:",
                    AutoSize = true
                };
                panel.Controls.Add(componentLabel, 0, i + 1);

                var markField = new TextBox
                {
                    Width = TextBoxWidth
                };
                markFields[i] = markField;
                panel.Controls.Add(markField, 1, i + 1);

                var detailLabel = new Label
                {
                    Text = $"{weighting}% - {name}",
                    AutoSize = true
                };
                panel.Controls.Add(detailLabel, 2, i + 1);
            }

            var calculateButton = new Button
            {
                Text = "Calculate",
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
                    MessageBox.Show("Please enter a number between 0 and 100.");
                }
            };
            panel.Controls.Add(calculateButton, 1, _componentDetails.Count + 1);

            var backButton = new Button
            {
                Text = "Back",
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            backButton.Click += (sender, e) => PromptForModuleCode();
            panel.Controls.Add(backButton, 0, _componentDetails.Count + 1);

            Controls.Clear();
            Controls.Add(panel);
            Controls.Add(_menuStrip);
        }

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

            var resultLabel = new Label
            {
                Text = $"Total module mark: {CalculateTotalMark(scores):F2}",
                Font = new Font("Arial", 15, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };
            panel.Controls.Add(resultLabel, 0, 0);
            panel.SetColumnSpan(resultLabel, 2);

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

        private double CalculateTotalMark(double[] scores)
        {
            double totalMark = 0;
            foreach (var component in _componentDetails)
            {
                var (weighting, _) = component.Value;
                int index = component.Key - 1;
                totalMark += scores[index] * (weighting / 100);
            }
            return totalMark;
        }

        private void RestartApplication()
        {
            Application.Restart();
        }
    }
}
