using Terminal.Gui;
using System.IO;
using System;

namespace win32editor
{
    internal static class Helpers
    {
        private static int originalCopylen { get; set; }
        public static void LoadFile()
        {
            try
            {
                if (Editor.fileName != null)
                {
                    string s = "";
                    byte[] buffer = File.ReadAllBytes(Editor.fileName);

                    // BUGBUG: #452 TextView.LoadFile keeps file open and provides no way of closing it 
                    //_textView.LoadFile(fileName);

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        s += (char)buffer[i];
                    }

                    //FUTURE ME: Please set a config options for file types with tabs to spaces.
                    // big problem we on accident formatt peoples files.

                    // maybe the editor has it own formatt. and we save the users original formatting. 
                    // then when we go save the file, we reinstate the original formatting.

                    string removeTabs = s.Replace("\t", "  ");
                    string removeNewLines = removeTabs.Replace("\r\n", "\n");

                    Editor.textField.Text = removeNewLines;
                    originalCopylen = Editor.textField.Text.Length; // not set to the length of removeNewLines because the TextView Element adds to legnth.
                
                    Editor.wind.Title = Editor.fileName;
                    Editor._saved = true;
                    Editor.fileLoaded = true;
                }
            }
            catch (IOException iOException)
            {
                MessageBox.ErrorQuery("Oh No!", iOException.ToString(), "ok");
            }
            catch (Exception err)
            {
                MessageBox.ErrorQuery("Oh No!", $"Unknown Exception {err}", "ok");
            }
        }


        public static void Open()
        {
            var fileSaved = CheckTextState();
            if (!fileSaved)
            {
                var s = MessageBox.Query(5, 5, "Save", "This document has changed since you Opened it. Would you like to save", "yes", "no");
                if (s == 0)
                {
                    try
                    {
                        File.WriteAllText(Editor.fileName, Editor.textField.Text.ToString());
                        Editor._saved = true;
                        Application.RequestStop();
                    }
                    catch (Exception)
                    {
                        MessageBox.ErrorQuery("Oh No!", "Error Opening File", "ok");
                    }
                }
            }

            string checkedDir;
            if (Path.GetDirectoryName(Editor.fileName) != null)
            {
                checkedDir = Path.GetDirectoryName(Editor.fileName);
            }
            else
            {
                checkedDir = Directory.GetCurrentDirectory();
            }
            
            var d = new OpenDialog("Open", "Open a file") { AllowsMultipleSelection = false, DirectoryPath = checkedDir };
            Application.Run(d);

            if (!d.Canceled)
            {
                try
                {
                    Editor.fileName = d.FilePaths[0];
                    LoadFile();
                }
                catch (Exception)
                {
                    MessageBox.ErrorQuery("Oh No!", "File you tried to access was a directory.", "ok");
                    LoadFile();
                }
            }
        }

        private static bool CheckTextState()
        {
            switch (Editor.fileLoaded)
            {
                // this first condition checks to see if there is a file in the context. 
                // checks to see if actual original textview length is greater than the current length of the TextView element.
                case true when originalCopylen != Editor.textField.Text.Length:
                    return false;
                case true:
                    return true;
                default:
                {
                    switch (Editor.textField.Text.Length > 1)
                    {
                        // TODO: try to create new file if user tries to save scratch pad file
                        // this works some how please do not touch -> scratch pad functionality
                        case true when !Editor.fileLoaded:
                            return false;
                        default:
                            return true;
                    }
                }
            }
        }

        public static void DisplayCursorPos()
        { 
            MessageBox.Query(1,5, "CursorPosition", $"Row:{Editor.textField.CurrentRow}\n Column:{Editor.textField.CurrentColumn}","ok");
        }
        
        public static void Quit()
        {
            var fileSaved = CheckTextState();
            if (!fileSaved)
            {
                var s = MessageBox.Query(5, 5, "Save", "This document has changed since you Opened it. Would you like to save", "yes", "no");
                if (s == 0)
                {
                    try
                    {
                        File.WriteAllText(Editor.fileName, Editor.textField.Text.ToString());
                        Editor._saved = true;
                        Application.RequestStop();
                    }
                    catch (Exception)
                    {
                        MessageBox.ErrorQuery("Oh No!", "Error Writing to file", "ok");
                    }
                }
                else if (s == 1)
                {
                    {
                        Application.RequestStop();
                    }
                }
            }
            
        }
        public static void Save()
        {
            if (Editor.fileName != null)
            {
                var s = MessageBox.Query(5, 5, "Save", "Would you like to save", "yes", "no");
                if (s == 0)
                {
                    try
                    {
                        File.WriteAllText(Editor.fileName, Editor.textField.Text.ToString());
                        Editor._saved = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.ErrorQuery("Oh No!", "Error Writing to file", "ok");
                    }
                }
                else
                {
                    LoadFile();
                }
                // BUGBUG: #279 TextView does not know how to deal with \r\n, only \r 
                // As a result files saved on Windows and then read back will show invalid chars.
            } else
            {
                MessageBox.ErrorQuery("save","There is no file to save.", "ok");
            }
        }
    }
}