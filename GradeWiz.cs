using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GradeWiz
{
    public partial class GradeWiz : Form
    {
        private string _moduleCode;
        private Dictionary<int, double> _componentWeightings;
        private MenuStrip _menuStrip;

        private const int LabelX = 10;
        private const int TextBoxWidth = 50;
        private const int ComponentSpacing = 40;
        private const int ButtonWidth = 80;
        private const int ButtonHeight = 30;
        private const int StartY = 40;
        private const int LabelSpacing = 40;
        private const int MenuStripHeight = 24;

        public GradeWiz()
        {
            InitializeComponent();
            this.BackColor = Color.LightGray;
            this.ClientSize = new Size(400, 350);
            _componentWeightings = new Dictionary<int, double>();

            CreateMenuBar();
            PromptForModuleCode();
        }

        private void PromptForModuleCode()
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            var label = new Label
            {
                Text = "Enter Module Code:",
                Location = new Point(LabelX, StartY),
                AutoSize = true
            };
            panel.Controls.Add(label);

            var moduleCodeTextBox = new TextBox
            {
                Location = new Point(LabelX + 150, StartY),
                Width = TextBoxWidth + 23
            };
            panel.Controls.Add(moduleCodeTextBox);

            var submitButton = new Button
            {
                Text = "Submit",
                Location = new Point(LabelX, StartY + ComponentSpacing),
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

        private void CenterTitle()
        {
            var title = $"GradeWiz - {_moduleCode} ✔";
            Text = title;
            var titleSize = TextRenderer.MeasureText(title, Font);
            var formWidth = ClientSize.Width;
            var titleX = (formWidth - titleSize.Width) / 2;
            this.Text = title;
        }

       
        private void CreateMenuBar()
        {
            _menuStrip = new MenuStrip();  // Initialize the MenuStrip
            var fileMenu = new ToolStripMenuItem("File");

            // About Menu Item
            var aboutItem = new ToolStripMenuItem("About");
            aboutItem.Click += (sender, e) => ShowAboutDialog();
            fileMenu.DropDownItems.Add(aboutItem);

            // Restart Menu Item
            var restartItem = new ToolStripMenuItem("Restart");
            Controls.Clear();
                Controls.Add(_menuStrip);
                PromptForModuleCode();

            // Quit Menu Item
            var quitItem = new ToolStripMenuItem("Quit");
            quitItem.Click += (sender, e) => Application.Exit();
            fileMenu.DropDownItems.Add(quitItem);

            _menuStrip.Items.Add(fileMenu);
            MainMenuStrip = _menuStrip;
            Controls.Add(_menuStrip);  // Add the MenuStrip to the form
        }

              private void ShowAboutDialog()
{
    MessageBox.Show(
        "GradeWiz ✔\n\nA simple app that calculates a final module mark based on component weightings and component marks.\n\n© 2024 Ricki Angel\nhttps://github.com/TechAngelX\n\n" +
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
                var csvPath = Path.Combine(Environment.CurrentDirectory, "data.csv");
                var lines = File.ReadAllLines(csvPath);
                bool moduleCodeFound = false;

                foreach (var line in lines)
                {
                    var fields = line.Split(',');

                    var moduleCodeInFile = fields[0];

                    if (moduleCodeInFile == passedModuleCode)
                    {
                        _componentWeightings.Clear(); // Clear previous data if any

                        for (int i = 1; i < fields.Length; i++)
                        {
                            if (double.TryParse(fields[i], out double weighting))
                            {
                                _componentWeightings[i] = weighting;
                            }
                        }
                        moduleCodeFound = true;
                        break;
                    }
                }

                if (!moduleCodeFound)
                {
                    MessageBox.Show($"Module '{passedModuleCode}' not found. Ensure it is a valid CS Dept module code.");
                    ClearScreenAndReturnToModuleCodeEntry();
                }
                else
                {
                    ShowComponentMarkScreen();
                    CenterTitle();
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
            // Clear the current controls and return to the module code entry screen
            Controls.Clear();
            Controls.Add(_menuStrip);
            PromptForModuleCode();
        }

        private void ShowComponentMarkScreen()
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            // Add the label for entering marks
            var label = new Label
            {
                Text = "Enter mark(s) for each component:",
                Location = new Point(LabelX, StartY),
                AutoSize = true
            };
            panel.Controls.Add(label);

            // Create and add the mark input fields
            var markFields = new TextBox[_componentWeightings.Count];
            for (int i = 0; i < _componentWeightings.Count; i++)
            {
                int componentNumber = i + 1;

                var componentLabel = new Label
                {
                    Text = $"Component {componentNumber} mark ({_componentWeightings[componentNumber]}%):",
                    Location = new Point(LabelX, StartY + LabelSpacing + i * ComponentSpacing),
                    AutoSize = true
                };
                panel.Controls.Add(componentLabel);

                var markField = new TextBox
                {
                    Location = new Point(LabelX + 220, StartY + LabelSpacing + i * ComponentSpacing),
                    Width = TextBoxWidth
                };
                markFields[i] = markField;
                panel.Controls.Add(markField);
            }

            // Add the Calculate button
var calculateButton = new Button
{
    Text = "Calculate",
    Location = new Point(LabelX + ButtonWidth + 10, StartY + LabelSpacing + _componentWeightings.Count * ComponentSpacing + 10),
    Size = new Size(ButtonWidth, ButtonHeight)
};
calculateButton.Click += (sender, e) =>
{
    try
    {
        double[] scores = new double[_componentWeightings.Count];
        for (int i = 0; i < _componentWeightings.Count; i++)
        {
            scores[i] = double.Parse(markFields[i].Text);
        }
        ShowResultScreen(scores);
    }
    catch (FormatException)
    {
        MessageBox.Show("Please enter valid numbers for the marks.");
    }
};
panel.Controls.Add(calculateButton);

// Add the Back button
var backButton = new Button
{
    Text = "Back",
    Location = new Point(LabelX, StartY + LabelSpacing + _componentWeightings.Count * ComponentSpacing + 10), // Align with Calculate button's Y
    Size = new Size(ButtonWidth, ButtonHeight)
};
backButton.Click += (sender, e) => ShowPreviousScreen();
panel.Controls.Add(backButton);

            // Clear the current controls and add the new panel
            Controls.Clear();
            Controls.Add(panel);
            Controls.Add(_menuStrip);
        }

        private void ShowPreviousScreen()
        {
            // Navigate back to the module code entry screen
            PromptForModuleCode(); // Call the method to show the previous screen
        }

        private void ShowResultScreen(double[] scores)
        {
            var panel = new Panel { Dock = DockStyle.Fill };
            var resultLabel = new Label
            {
                Text = $"Total module mark: {CalculateTotalMark(scores):F2}",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(LabelX, StartY),
                AutoSize = true
            };
            panel.Controls.Add(resultLabel);

            for (int i = 0; i < _componentWeightings.Count; i++)
            {
                int componentNumber = i + 1;

                var componentLabel = new Label
                {
                    Text = $"Component {componentNumber} mark ({_componentWeightings[componentNumber]}%):",
                    Location = new Point(LabelX, StartY + LabelSpacing + i * 30),
                    AutoSize = true
                };
                var markLabel = new Label
                {
                    Text = $"{scores[i] * (_componentWeightings[componentNumber] / 100):F2}",
                    Location = new Point(LabelX + 220, StartY + LabelSpacing + i * 30),
                    AutoSize = true
                };

                panel.Controls.Add(componentLabel);
                panel.Controls.Add(markLabel);
            }

            var backButton = new Button
            {
                Text = "Back",
                Location = new Point(LabelX, StartY + LabelSpacing + _componentWeightings.Count * 30 + 10),
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            backButton.Click += (sender, e) => ShowComponentMarkScreen();
            panel.Controls.Add(backButton);

            var restartButton = new Button
            {
                Text = "Restart",
                Location = new Point(LabelX + ButtonWidth + 10, StartY + LabelSpacing + _componentWeightings.Count * 30 + 10),
                Size = new Size(ButtonWidth, ButtonHeight)
            };
            restartButton.Click += (sender, e) => {
                Controls.Clear();
                Controls.Add(_menuStrip);
                PromptForModuleCode();
            };
            panel.Controls.Add(restartButton);

            Controls.Clear();
            Controls.Add(panel);
            Controls.Add(_menuStrip);
        }

        private double CalculateTotalMark(double[] scores)
        {
            double totalMark = 0;
            for (int i = 0; i < _componentWeightings.Count; i++)
            {
                totalMark += scores[i] * (_componentWeightings[i + 1] / 100);
            }
            return totalMark;
        }
    }
}
