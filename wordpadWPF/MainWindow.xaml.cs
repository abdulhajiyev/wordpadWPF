using System;
using System.Data.Odbc;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Win32;

namespace wordpadWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Txt Documents (*.txt)| *.txt"
            };

            if (ofd.ShowDialog() != true) return;
            string file = ofd.FileName;
            string text = File.ReadAllText(file);
            TextBox.Text = text;
            Path.Content = file;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Text Documents (*.txt)|*.txt",
            };

            if (sfd.ShowDialog() != true) return;
            File.WriteAllText(sfd.FileName, TextBox.Text);
            Path.Content = sfd.FileName;
        }

        private void CutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (TextBox != null) && (TextBox.SelectionLength > 0);
        }

        private void CutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox.Cut();
        }

        private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText();
        }

        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox.Paste();
        }

        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox.Copy();
        }

        private void SelectAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextBox.SelectAll();
            TextBox.Focus();
        }

        private void SelectAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = TextBox != null;
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (AutoSave.IsChecked != true) return;
            File.WriteAllText(Path.Content.ToString(), TextBox.Text);
        }

        private void AutoSave_OnClick(object sender, RoutedEventArgs e)
        {
            var tb = sender as ToggleButton;
            if (TextBox.Text.Length < 0 || Path.Content != string.Empty) return;
            MessageBox.Show("Cannot enable autosave without opening text file", "Warning", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            tb.IsChecked = false;
        }
    }
}
