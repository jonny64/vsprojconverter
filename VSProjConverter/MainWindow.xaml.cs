using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace VSProjConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            path.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                "\\Visual Studio 2010\\Projects";

        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            var pathDialog = new FolderBrowserDialogEx();
            pathDialog.Description = "Select a folder of solution";
            pathDialog.ShowNewFolderButton = true;
            pathDialog.ShowEditBox = true;
            pathDialog.NewStyle = false;
            pathDialog.SelectedPath = path.Text;
            pathDialog.ShowFullPathInEditBox = true;
            pathDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;

            // Show the FolderBrowserDialog.
            DialogResult result = pathDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path.Text = pathDialog.SelectedPath;
            }

        }

        private void VS2008_Click(object sender, RoutedEventArgs e)
        {
            var projectFiles = Directory.GetFiles(path.Text, "*.csproj", SearchOption.AllDirectories);
            var solutionFiles = Directory.GetFiles(path.Text, "*.sln", SearchOption.TopDirectoryOnly);

            foreach (string solutionFile in solutionFiles)
            {
                string content;
                using (StreamReader sr = new StreamReader(solutionFile))
                {
                    content = sr.ReadToEnd();
                }
                
                content = content.Replace("# Visual Studio 2010", "# Visual Studio 2008")
                    .Replace("Microsoft Visual Studio Solution File, Format Version 11.00", "Microsoft Visual Studio Solution File, Format Version 10.00");

                ReplaceContent(solutionFile, content);
            }

            foreach (string projectFile in projectFiles)
            {
                string content = File.ReadAllText(projectFile);
                content = content.Replace("TargetFrameworkMoniker = \".NETFramework,Version=v2.0\"", "TargetFramework = \"3.5\"")
                    .Replace("<Project ToolsVersion=\"4.0\"", "<Project ToolsVersion=\"3.5\"")
                    .Replace("<ProductVersion>10.0.20506</ProductVersion>", "<ProductVersion>9.0.30729</ProductVersion>")
                    .Replace("\\VisualStudio\\v10.0\\", "\\VisualStudio\\v9.0\\")
                    .Replace("<Import Project=\"$(MSBuildToolsPath)\\Workflow.Targets\" />", "<Import Project=\"$(MSBuildExtensionsPath)\\Microsoft\\Windows Workflow Foundation\\v3.5\\Workflow.Targets\" />");
                
                ReplaceContent(projectFile, content);
            }

            System.Windows.MessageBox.Show("Finished!");
        }

        private static void ReplaceContent(string projectFile, string content)
        {
            try
            {
                string temp = projectFile + ".tmp";
                using (StreamWriter sr = new StreamWriter(temp))
                {
                    sr.Write(content);
                }
                File.Replace(temp, projectFile, projectFile + ".bak");
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }

}
