using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Linq;

namespace TaidanaKage.Arkania.Tweaks
{
    class MainForm : Form
    {
        private ListBox listBoxInfo;
        private Button buttonLocationBrowse;
        private FolderBrowserDialog folderBrowserDialogLocation;
        private Button buttonExit;
        private LinkLabel linkLabelUrl;

        private Game selectedGame;
        private string selectedFolder;

        private Label labelMoney;
        private ComboBox comboBoxMoney;

        private Label labelLE;
        private ComboBox comboBoxLE;

        private Label labelLevelUpLE;
        private ComboBox comboBoxLevelUpLE;

        private Label labelPosUps;
        private ComboBox comboBoxPosUps;

        private Label labelNegUps;
        private ComboBox comboBoxNegUps;

        private Label labelSkillUps;
        private ComboBox comboBoxSkillUps;

        private Label labelAE;
        private ComboBox comboBoxAE;

        private Label labelSpellUps;
        private ComboBox comboBoxSpellUps;

        private Label labelMaxUpsMove;
        private ComboBox comboBoxMaxUpsMove;

        private Button buttonApply;

        // The main entry point for the application.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        // Constructor.
        public MainForm()
        {
            int x = 510;
            this.selectedGame = Game.Unknown;
            this.selectedFolder = "";

            // Main form
            this.Text = "Arkania Tweaks 1.0"; //TODO read this from the assembly?
            this.ClientSize = new Size(1000, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Info box
            this.listBoxInfo = new ListBox();
            this.Controls.Add(listBoxInfo);
            this.listBoxInfo.Location = new Point(10, 10);
            this.listBoxInfo.Size = new Size(490, 480);
            this.listBoxInfo.HorizontalScrollbar = true;

            // Game location
            this.buttonLocationBrowse = new Button();
            this.Controls.Add(buttonLocationBrowse);
            this.buttonLocationBrowse.Location = new Point(x, 10);
            this.buttonLocationBrowse.Size = new Size(60, 25);
            this.buttonLocationBrowse.Text = "Location";
            this.buttonLocationBrowse.Click += new EventHandler(this.ButtonLocationBrowse_Click);

            this.folderBrowserDialogLocation = new FolderBrowserDialog();
            this.folderBrowserDialogLocation.Description =
                "Select the directory where the game was installed.";
            this.folderBrowserDialogLocation.ShowNewFolderButton = false;
            this.folderBrowserDialogLocation.RootFolder = Environment.SpecialFolder.MyComputer;

            // Exit button
            this.buttonExit = new Button();
            this.Controls.Add(buttonExit);
            this.buttonExit.Location = new Point(x, 450);
            this.buttonExit.Size = new Size(60, 25);
            this.buttonExit.Text = "Exit";
            this.buttonExit.Click += new EventHandler(this.ButtonExit_Click);

            // Link to GitHub
            this.linkLabelUrl = new LinkLabel();
            this.Controls.Add(this.linkLabelUrl);
            this.linkLabelUrl.Location = new Point(x + 100, 450);
            this.linkLabelUrl.AutoSize = false;
            this.linkLabelUrl.Size = new Size(300, 25);
            this.linkLabelUrl.TextAlign = ContentAlignment.MiddleLeft;
            this.linkLabelUrl.Text = "GitHub.com/dsx75/ArkaniaTweaks";
            this.linkLabelUrl.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelUrl_LinkClicked);

            AddInfo("Select the game location...");

            //TODO temporary hax
            //this.selectedFolder = @"F:\Steam\steamapps\common\Realms of Arkania";
            //this.selectedGame = Game.BladeOfDestiny;
            //this.selectedFolder = @"F:\Steam\steamapps\common\Realms of Arkania – Star Trail";
            //this.selectedGame = Game.StarTrail;
            //ShowTweakOptions();
        }

        private void AddInfo(String info)
        {
            this.listBoxInfo.Items.Add(info);
            listBoxInfo.TopIndex = listBoxInfo.Items.Count - 1;
            this.listBoxInfo.Refresh();
        }

        // Event handler for the Exit button.
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Event handler for the Location browse button.
        private void ButtonLocationBrowse_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialogLocation.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.selectedFolder = folderBrowserDialogLocation.SelectedPath;
                AddInfo(this.selectedFolder);

                // Is this Blade of Destiny?
                String exeBladeOfDestiny = this.selectedFolder + Path.DirectorySeparatorChar + "schick.exe";
                if (File.Exists(exeBladeOfDestiny))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(exeBladeOfDestiny);
                    string version = versionInfo.ProductVersion;
                    AddInfo("Realms of Arkania: Blade of Destiny (2013)");
                    AddInfo("version " + version);
                    this.selectedGame = Game.BladeOfDestiny;
                }
                else
                {
                    String exeStarTrail = this.selectedFolder + Path.DirectorySeparatorChar + "schweif.exe";
                    if (File.Exists(exeStarTrail))
                    {
                        var versionInfo = FileVersionInfo.GetVersionInfo(exeStarTrail);
                        string version = versionInfo.ProductVersion;
                        AddInfo("Realms of Arkania: Star Trail (2017)");
                        AddInfo("version " + version);
                        this.selectedGame = Game.StarTrail;
                    }
                }

                if (this.selectedGame == Game.Unknown)
                {
                    AddInfo("No compatible game found. Try again...");
                }
                else
                {
                    this.buttonLocationBrowse.Enabled = false;
                    ShowTweakOptions();
                }

            }
        }

        // The game location was selected, let's show all possible tweaks.
        private void ShowTweakOptions()
        {

            int x = 510;
            int y = 20;
            int yInc = 30;
            int labelWidth = 200;
            Color labelBackground = Color.Transparent;
            int comboWidth = 200;

            // Money (start)
            if (this.selectedGame == Game.StarTrail)
            {
                y += yInc;
                this.labelMoney = new Label();
                this.Controls.Add(this.labelMoney);
                this.labelMoney.Location = new Point(x, y);
                this.labelMoney.Size = new Size(labelWidth, 25);
                this.labelMoney.AutoSize = false;
                this.labelMoney.TextAlign = ContentAlignment.MiddleLeft;
                this.labelMoney.Text = "Money (start):";
                this.labelMoney.BackColor = labelBackground;

                this.comboBoxMoney = new ComboBox();
                this.Controls.Add(comboBoxMoney);
                this.comboBoxMoney.Location = new Point(x + labelWidth + 10, y);
                this.comboBoxMoney.DropDownStyle = ComboBoxStyle.DropDownList;
                this.comboBoxMoney.Size = new Size(comboWidth, 25);
                this.comboBoxMoney.Items.Add("original");
                this.comboBoxMoney.Items.Add("best possible roll");
                this.comboBoxMoney.SelectedIndex = 0;

                this.labelMoney.Height = this.comboBoxMoney.Height;
            }

            // Life Energy (start)
            if (this.selectedGame == Game.BladeOfDestiny)
            {
                y += yInc;
                this.labelLE = new Label();
                this.Controls.Add(this.labelLE);
                this.labelLE.Location = new Point(x, y);
                this.labelLE.Size = new Size(labelWidth, 25);
                this.labelLE.AutoSize = false;
                this.labelLE.TextAlign = ContentAlignment.MiddleLeft;
                this.labelLE.Text = "Life Energy (start):";
                this.labelLE.BackColor = labelBackground;

                this.comboBoxLE = new ComboBox();
                this.Controls.Add(comboBoxLE);
                this.comboBoxLE.Location = new Point(x + labelWidth + 10, y);
                this.comboBoxLE.DropDownStyle = ComboBoxStyle.DropDownList;
                this.comboBoxLE.Size = new Size(comboWidth, 25);
                this.comboBoxLE.Items.Add("original");
                this.comboBoxLE.Items.Add("best possible roll");
                this.comboBoxLE.SelectedIndex = 0;

                this.labelLE.Height = this.comboBoxLE.Height;
            }

            // Life Energy (level up)
            y += yInc;
            this.labelLevelUpLE = new Label();
            this.Controls.Add(this.labelLevelUpLE);
            this.labelLevelUpLE.Location = new Point(x, y);
            this.labelLevelUpLE.Size = new Size(labelWidth, 25);
            this.labelLevelUpLE.AutoSize = false;
            this.labelLevelUpLE.TextAlign = ContentAlignment.MiddleLeft;
            this.labelLevelUpLE.Text = "Life Energy (level up):";
            this.labelLevelUpLE.BackColor = labelBackground;

            this.comboBoxLevelUpLE = new ComboBox();
            this.Controls.Add(comboBoxLevelUpLE);
            this.comboBoxLevelUpLE.Location = new Point(x + labelWidth + 10, y);
            this.comboBoxLevelUpLE.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxLevelUpLE.Size = new Size(comboWidth, 25);
            this.comboBoxLevelUpLE.Items.Add("original");
            this.comboBoxLevelUpLE.Items.Add("best possible roll");
            this.comboBoxLevelUpLE.SelectedIndex = 0;

            this.labelLevelUpLE.Height = this.comboBoxLevelUpLE.Height;

            // Pos Ups
            y += yInc;
            this.labelPosUps = new Label();
            this.Controls.Add(this.labelPosUps);
            this.labelPosUps.Location = new Point(x, y);
            this.labelPosUps.Size = new Size(labelWidth, 25);
            this.labelPosUps.AutoSize = false;
            this.labelPosUps.TextAlign = ContentAlignment.MiddleLeft;
            this.labelPosUps.Text = "Positive attribute points (level up):";
            this.labelPosUps.BackColor = labelBackground;

            this.comboBoxPosUps = new ComboBox();
            this.Controls.Add(comboBoxPosUps);
            this.comboBoxPosUps.Location = new Point(x + labelWidth + 10, y);
            this.comboBoxPosUps.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPosUps.Size = new Size(comboWidth, 25);
            this.comboBoxPosUps.Items.Add("original");
            this.comboBoxPosUps.Items.Add("x2");
            this.comboBoxPosUps.Items.Add("x3");
            this.comboBoxPosUps.SelectedIndex = 0;

            this.labelPosUps.Height = this.comboBoxPosUps.Height;

            // Neg Ups
            y += yInc;
            this.labelNegUps = new Label();
            this.Controls.Add(this.labelNegUps);
            this.labelNegUps.Location = new Point(x, y);
            this.labelNegUps.Size = new Size(labelWidth, 25);
            this.labelNegUps.AutoSize = false;
            this.labelNegUps.TextAlign = ContentAlignment.MiddleLeft;
            this.labelNegUps.Text = "Negative attribute points (level up):";
            this.labelNegUps.BackColor = labelBackground;

            this.comboBoxNegUps = new ComboBox();
            this.Controls.Add(comboBoxNegUps);
            this.comboBoxNegUps.Location = new Point(x + labelWidth + 10, y);
            this.comboBoxNegUps.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxNegUps.Size = new Size(comboWidth, 25);
            this.comboBoxNegUps.Items.Add("original");
            this.comboBoxNegUps.Items.Add("x2");
            this.comboBoxNegUps.Items.Add("x3");
            this.comboBoxNegUps.SelectedIndex = 0;

            this.labelNegUps.Height = this.comboBoxNegUps.Height;

            // Skill Ups
            y += yInc;
            this.labelSkillUps = new Label();
            this.Controls.Add(this.labelSkillUps);
            this.labelSkillUps.Location = new Point(x, y);
            this.labelSkillUps.Size = new Size(labelWidth, 25);
            this.labelSkillUps.AutoSize = false;
            this.labelSkillUps.TextAlign = ContentAlignment.MiddleLeft;
            this.labelSkillUps.Text = "Skill points (level up):";
            this.labelSkillUps.BackColor = labelBackground;

            this.comboBoxSkillUps = new ComboBox();
            this.Controls.Add(comboBoxSkillUps);
            this.comboBoxSkillUps.Location = new Point(x + labelWidth + 10, y);
            this.comboBoxSkillUps.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSkillUps.Size = new Size(comboWidth, 25);
            this.comboBoxSkillUps.Items.Add("original");
            this.comboBoxSkillUps.Items.Add("x2");
            this.comboBoxSkillUps.Items.Add("x3");
            this.comboBoxSkillUps.SelectedIndex = 0;

            this.labelSkillUps.Height = this.comboBoxSkillUps.Height;

            // Arcane Energy (start)
            y += yInc;
            this.labelAE = new Label();
            this.Controls.Add(this.labelAE);
            this.labelAE.Location = new Point(x, y);
            this.labelAE.Size = new Size(labelWidth, 25);
            this.labelAE.AutoSize = false;
            this.labelAE.TextAlign = ContentAlignment.MiddleLeft;
            this.labelAE.Text = "Arcane Energy (start):";
            this.labelAE.BackColor = labelBackground;

            this.comboBoxAE = new ComboBox();
            this.Controls.Add(comboBoxAE);
            this.comboBoxAE.Location = new Point(x + labelWidth + 10, y);
            this.comboBoxAE.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxAE.Size = new Size(comboWidth, 25);
            this.comboBoxAE.Items.Add("original");
            this.comboBoxAE.Items.Add("best possible roll");
            this.comboBoxAE.SelectedIndex = 0;

            this.labelAE.Height = this.comboBoxAE.Height;

            // Spell Ups
            y += yInc;
            this.labelSpellUps = new Label();
            this.Controls.Add(this.labelSpellUps);
            this.labelSpellUps.Location = new Point(x, y);
            this.labelSpellUps.Size = new Size(labelWidth, 25);
            this.labelSpellUps.AutoSize = false;
            this.labelSpellUps.TextAlign = ContentAlignment.MiddleLeft;
            this.labelSpellUps.Text = "Spell points (level up):";
            this.labelSpellUps.BackColor = labelBackground;

            this.comboBoxSpellUps = new ComboBox();
            this.Controls.Add(comboBoxSpellUps);
            this.comboBoxSpellUps.Location = new Point(x + labelWidth + 10, y);
            this.comboBoxSpellUps.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSpellUps.Size = new Size(comboWidth, 25);
            this.comboBoxSpellUps.Items.Add("original");
            this.comboBoxSpellUps.Items.Add("x2");
            this.comboBoxSpellUps.Items.Add("x3");
            this.comboBoxSpellUps.SelectedIndex = 0;

            this.labelSpellUps.Height = this.comboBoxSpellUps.Height;

            // Max Ups MOve
            y += yInc;
            this.labelMaxUpsMove = new Label();
            this.Controls.Add(this.labelMaxUpsMove);
            this.labelMaxUpsMove.Location = new Point(x, y);
            this.labelMaxUpsMove.Size = new Size(labelWidth, 25);
            this.labelMaxUpsMove.AutoSize = false;
            this.labelMaxUpsMove.TextAlign = ContentAlignment.MiddleLeft;
            this.labelMaxUpsMove.Text = "Movable points (level up):";
            this.labelMaxUpsMove.BackColor = labelBackground;

            this.comboBoxMaxUpsMove = new ComboBox();
            this.Controls.Add(comboBoxMaxUpsMove);
            this.comboBoxMaxUpsMove.Location = new Point(x + labelWidth + 10, y);
            this.comboBoxMaxUpsMove.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMaxUpsMove.Size = new Size(comboWidth, 25);
            this.comboBoxMaxUpsMove.Items.Add("original");
            this.comboBoxMaxUpsMove.Items.Add("x2");
            this.comboBoxMaxUpsMove.Items.Add("x3");
            this.comboBoxMaxUpsMove.SelectedIndex = 0;

            this.labelMaxUpsMove.Height = this.comboBoxMaxUpsMove.Height;

            // Apply button
            y += yInc;
            this.buttonApply = new Button();
            this.Controls.Add(buttonApply);
            this.buttonApply.Location = new Point(x, y);
            this.buttonApply.Size = new Size(60, 25);
            this.buttonApply.Text = "Apply";
            this.buttonApply.Click += new EventHandler(this.ButtonApply_Click);

            AddInfo("Choose tweaks and click Apply...");
        }

        // Event handler for the Apply button.
        private void ButtonApply_Click(object sender, EventArgs e)
        {
            if (this.selectedGame == Game.StarTrail)
            {
                this.comboBoxMoney.Enabled = false;
            }
            if (this.selectedGame == Game.BladeOfDestiny)
            {
                this.comboBoxLE.Enabled = false;
            }
            this.comboBoxLevelUpLE.Enabled = false;
            this.comboBoxPosUps.Enabled = false;
            this.comboBoxNegUps.Enabled = false;
            this.comboBoxSkillUps.Enabled = false;
            this.comboBoxAE.Enabled = false;
            this.comboBoxSpellUps.Enabled = false;
            this.comboBoxMaxUpsMove.Enabled = false;
            this.buttonApply.Enabled = false;
            DirSearch(this.selectedFolder);
            //AddInfo("Combo box height = " + this.comboBoxLE.Height);
            this.buttonApply.Text = "Done";
            AddInfo("Done.");
        }

        // Let's recursively find all XML files.
        private void DirSearch(string sDir)
        {
            try
            {
                foreach (string directory in Directory.GetDirectories(sDir))
                {
                    foreach (string file in Directory.GetFiles(directory, "*.xml"))
                    {
                        ParseFile(file);
                    }
                    DirSearch(directory);
                }
            }
            catch (Exception excption)
            {
                AddInfo(excption.Message);
            }
        }

        private void ParseFile(String file)
        {
            //AddInfo("Parsing " + file);
            XDocument xdoc = XDocument.Load(file);

            bool wasChange = false;

            var xmlClassDef = xdoc.Descendants("classdefinition");
            foreach (var xmlClass in xmlClassDef.Elements())
            {
                // We found an archetype definition
                AddInfo("Archetype: " + xmlClass.Element("id").Value);

                // Starting Cash
                if (this.selectedGame == Game.StarTrail)
                {
                    if (this.comboBoxMoney.SelectedIndex > 0)
                    {
                        var xmlMoney = xmlClass.Element("startingCash");
                        if (xmlMoney != null)
                        {
                            int newMoney = BestPossibleRoll(xmlMoney.Value);
                            AddInfo("Money: " + xmlMoney.Value + " --> " + newMoney);
                            xmlMoney.Value = Convert.ToString(newMoney);
                            wasChange = true;
                        }
                    }
                }

                // Life Energy (start)
                if (this.selectedGame == Game.BladeOfDestiny)
                {
                    // In Star Trail starting LE is already maxed.
                    if (this.comboBoxLE.SelectedIndex > 0)
                    {
                        var xmlLE = xmlClass.Element("le");
                        if (xmlLE != null)
                        {
                            int newLE = BestPossibleRoll(xmlLE.Value);
                            AddInfo("LE (start): " + xmlLE.Value + " --> " + newLE);
                            xmlLE.Value = Convert.ToString(newLE);
                            wasChange = true;
                        }
                    }
                }

                // Life Energy (level up)
                if (this.comboBoxLevelUpLE.SelectedIndex > 0)
                {
                    var xmlLevelUpLE = xmlClass.Element("leveluple");
                    if (xmlLevelUpLE != null)
                    {
                        int newLevelUpLE = BestPossibleRoll(xmlLevelUpLE.Value);
                        AddInfo("LE (level up): " + xmlLevelUpLE.Value + " --> " + newLevelUpLE);
                        xmlLevelUpLE.Value = Convert.ToString(newLevelUpLE);
                        wasChange = true;
                    }
                }

                // Pos Ups
                if (this.comboBoxPosUps.SelectedIndex > 0)
                {
                    var xmlPosUps = xmlClass.Element("posups");
                    if (xmlPosUps != null)
                    {
                        Int32.TryParse(xmlPosUps.Value, out int newPosUps);
                        if (this.comboBoxPosUps.SelectedIndex == 1)
                        {
                            newPosUps = 2 * newPosUps;
                        }
                        else if (this.comboBoxPosUps.SelectedIndex == 2)
                        {
                            newPosUps = 3 * newPosUps;
                        }
                        AddInfo("Positive attribute points (level up): " + xmlPosUps.Value + " --> " + newPosUps);
                        xmlPosUps.Value = Convert.ToString(newPosUps);
                        wasChange = true;
                    }
                }

                // Neg Ups
                if (this.comboBoxNegUps.SelectedIndex > 0)
                {
                    var xmlNegUps = xmlClass.Element("negups");
                    if (xmlNegUps != null)
                    {
                        Int32.TryParse(xmlNegUps.Value, out int newNegUps);
                        if (this.comboBoxNegUps.SelectedIndex == 1)
                        {
                            newNegUps = 2 * newNegUps;
                        }
                        else if (this.comboBoxNegUps.SelectedIndex == 2)
                        {
                            newNegUps = 3 * newNegUps;
                        }
                        AddInfo("Negative attribute points (level up): " + xmlNegUps.Value + " --> " + newNegUps);
                        xmlNegUps.Value = Convert.ToString(newNegUps);
                        wasChange = true;
                    }
                }

                // Skill Ups
                if (this.comboBoxSkillUps.SelectedIndex > 0)
                {
                    var xmlSkillUps = xmlClass.Element("skillups");
                    if (xmlSkillUps != null)
                    {
                        Int32.TryParse(xmlSkillUps.Value, out int newSkillUps);
                        if (this.comboBoxSkillUps.SelectedIndex == 1)
                        {
                            newSkillUps = 2 * newSkillUps;
                        }
                        else if (this.comboBoxSkillUps.SelectedIndex == 2)
                        {
                            newSkillUps = 3 * newSkillUps;
                        }
                        AddInfo("Skill points (level up): " + xmlSkillUps.Value + " --> " + newSkillUps);
                        xmlSkillUps.Value = Convert.ToString(newSkillUps);
                        wasChange = true;
                    }
                }

                var xmlMagic = xmlClass.Element("magic");
                if (xmlMagic != null)
                {
                    // AE
                    if (this.comboBoxAE.SelectedIndex > 0)
                    {
                        var xmlAE = xmlMagic.Element("ae");
                        if (xmlAE != null)
                        {
                            int newAE = BestPossibleRoll(xmlAE.Value);
                            AddInfo("AE (start): " + xmlAE.Value + " --> " + newAE);
                            xmlAE.Value = Convert.ToString(newAE);
                            wasChange = true;
                        }
                    }

                    // Spell Ups
                    if (this.comboBoxSpellUps.SelectedIndex > 0)
                    {
                        var xmlSpellUps = xmlMagic.Element("spellups");
                        if (xmlSpellUps != null)
                        {
                            Int32.TryParse(xmlSpellUps.Value, out int newSpellUps);
                            if (this.comboBoxSpellUps.SelectedIndex == 1)
                            {
                                newSpellUps = 2 * newSpellUps;
                            }
                            else if (this.comboBoxSpellUps.SelectedIndex == 2)
                            {
                                newSpellUps = 3 * newSpellUps;
                            }
                            AddInfo("Spell points (level up): " + xmlSpellUps.Value + " --> " + newSpellUps);
                            xmlSpellUps.Value = Convert.ToString(newSpellUps);
                            wasChange = true;
                        }
                    }

                    // Max Ups Move
                    if (this.comboBoxMaxUpsMove.SelectedIndex > 0)
                    {
                        var xmlMaxUpsMove = xmlMagic.Element("maxupsmove");
                        if (xmlMaxUpsMove != null)
                        {
                            Int32.TryParse(xmlMaxUpsMove.Value, out int newMaxUpsMove);
                            if (this.comboBoxMaxUpsMove.SelectedIndex == 1)
                            {
                                newMaxUpsMove = 2 * newMaxUpsMove;
                            }
                            else if (this.comboBoxMaxUpsMove.SelectedIndex == 2)
                            {
                                newMaxUpsMove = 3 * newMaxUpsMove;
                            }
                            AddInfo("Movable points (level up): " + xmlMaxUpsMove.Value + " --> " + newMaxUpsMove);
                            xmlMaxUpsMove.Value = Convert.ToString(newMaxUpsMove);
                            wasChange = true;
                        }
                    }
                }


            }

            if (wasChange)
            {
                AddInfo("Overwriting " + file);
                try
                {
                    xdoc.Save(file);
                }
                catch (Exception e)
                {
                    AddInfo(e.ToString());
                }
            }
        }

        // Calculate the best possible roll (for the specified DSA notation).
        private int BestPossibleRoll(String dsaNotation)
        {
            int result = 0;

            if (Int32.TryParse(dsaNotation, out result))
            {
                // This DSA notation is just a simple number, no conversion is needed.
            }
            else
            {
                string[] separators = { "W", "+" };
                string[] numbers = dsaNotation.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                Int32.TryParse(numbers[0], out int a);
                Int32.TryParse(numbers[1], out int b);

                int c = 0;
                if (numbers.Length > 2)
                {
                    Int32.TryParse(numbers[2], out c);
                }

                result = (a * b) + c;
            }

            return result;
        }

        // Event handler for the GitHub link.
        private void linkLabelUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabelUrl.LinkVisited = true;
            Process.Start("https://github.com/dsx75/ArkaniaTweaks");
        }

    }
}
