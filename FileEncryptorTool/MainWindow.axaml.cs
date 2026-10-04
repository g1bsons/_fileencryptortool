using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace FileEncryptorTool;

public partial class MainWindow : Window
{
    // Stores the path for the file chosen by the user 
    private string selectedFile = "";

    // This allows the GUI to access the encryption methods
    private FileEncryption fileEncryption;

    public MainWindow()
    {
        // Starts the controls created in the mainwindow.axaml file
        InitializeComponent();
        // This creates the the object that gets used when the encrypt and decrypt button
        fileEncryption = new FileEncryption();
    }

    // This is the select file method
    private async void SelectFileButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // Opens OS file selection window
        var files = await StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select a File",
                AllowMultiple = false
            });

        // Check if a file was chosen
        if (files.Count > 0)
        {
            // Saves the file path
            selectedFile = files[0].Path.LocalPath;

            // Display the chosen file in the GUI
            SelectedFileText.Text =
                Path.GetFileName(selectedFile);
        }
    }

    // Method that runs when the encrypt file button is selected
    private async void EncryptButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // Ensure a file was selected
        if (selectedFile == "")
        {
            await ShowMessage("Please select a file first.");
            return;
        }

        // Ensure a password was entered
        if (PasswordTextBox.Text == "")
        {
            await ShowMessage("Please enter a password.");
            return;
        }

        // Prompts where the file should be stored
        var file = await StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions
            {
                Title = "Save Encrypted File",
                SuggestedFileName =
                    Path.GetFileName(selectedFile) + ".encrypted"
            });

        // Ends the process if the window is closed before the process ends 
        if (file == null)
        {
            return;
        }

        // User chooses file location
        string outputFile = file.Path.LocalPath;

        // Sends original file, output and password to the FileEncryption class 
        try
        {
            fileEncryption.EncryptFile(
                selectedFile,
                outputFile,
                PasswordTextBox.Text);

            // Encryption that the file was finished
            await ShowMessage(
                "The file was encrypted successfully.");
        }
        catch (Exception ex)
        {
            // If something went wrong
            await ShowMessage(
                "Encryption failed:\n\n" + ex.Message);
        }
    }

    // This the decrypt method that runs 
    private async void DecryptButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (selectedFile == "")
        {
            await ShowMessage("Please select a file first.");
            return;
        }

        if (PasswordTextBox.Text == "")
        {
            await ShowMessage("Please enter a password.");
            return;
        }

        string suggestedName =
            GetDecryptedFileName(selectedFile);

        var file = await StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions
            {
                Title = "Save Decrypted File",
                SuggestedFileName = suggestedName
            });

        if (file == null)
        {
            return;
        }

        string outputFile = file.Path.LocalPath;

        try
        {
            fileEncryption.DecryptFile(
                selectedFile,
                outputFile,
                PasswordTextBox.Text);

            await ShowMessage(
                "The file was decrypted successfully.");
        }
        catch (CryptographicException)
        {
            await ShowMessage(
                "Decryption failed. " +
                "The password may be incorrect " +
                "or the file may be damaged.");
        }
        catch (Exception ex)
        {
            await ShowMessage(
                "Decryption failed:\n\n" + ex.Message);
        }
    }

    private string GetDecryptedFileName(string fileName)
    {
        if (fileName.EndsWith(".encrypted"))
        {
            return fileName.Substring(
                0,
                fileName.Length - ".encrypted".Length);
        }

        return fileName + ".decrypted";
    }

    // Messages to the user 
    private async Task ShowMessage(string message)
    {
        Window messageWindow = new Window
        {
            Title = "File Encryptor",
            Width = 400,
            Height = 200
        };

        Button button = new Button
        {
            Content = "OK",
            Width = 100,
            HorizontalAlignment =
                Avalonia.Layout.HorizontalAlignment.Center
        };

        TextBlock text = new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(20)
        };

        StackPanel panel = new StackPanel
        {
            Spacing = 20
        };

        panel.Children.Add(text);
        panel.Children.Add(button);

        messageWindow.Content = panel;

        button.Click += delegate
        {
            messageWindow.Close();
        };

        await messageWindow.ShowDialog(this);
    }
}