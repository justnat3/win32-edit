using Terminal.Gui;
using System.IO;
using System;

namespace win32editor
{
    internal class Helpers
    {
        private static int originalCopylen { get; set; }
        public static void LoadFile()
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


        public static void Open()
        {
            var fileSaved = CheckTextState();
            if (!fileSaved)
            {
                var s = MessageBox.Query(5, 5, "Save", "This document has changed since you Opened it. Would you like to save", "yes", "no");
                if (s == 0)
                {
                    File.WriteAllText(Editor.fileName, Editor.textField.Text.ToString());
                    Editor._saved = true;
                    Application.RequestStop();
                }
            }

            var d = new OpenDialog("Open", "Open a file") { AllowsMultipleSelection = false };
            Application.Run(d);

            if (!d.Canceled)
            {
                Editor.fileName = d.FilePaths[0];
                LoadFile();
            }
        }

        public static bool CheckTextState()
        {
            
            // this first condition checks to see if there is a file in the context. 
            if (Editor.fileLoaded)
            {
                // checks to see if actual original textview length is greater than the current length of the TextView element.
                if (originalCopylen != Editor.textField.Text.Length)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                // TODO: try to create new file if user tries to save scratch pad file
                // this works some how please do not touch -> scratch pad functionality
                if (Editor.textField.Text.Length > 1 && !Editor.fileLoaded)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static void Quit()
        {
            var fileSaved = CheckTextState();
            if (!fileSaved)
            {
                var s = MessageBox.Query(5, 5, "Save", "This document has changed since you Opened it. Would you like to save", "yes", "no");
                if (s == 0)
                {
                    File.WriteAllText(Editor.fileName, Editor.textField.Text.ToString());
                    Editor._saved = true;
                    Application.RequestStop();
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
                    File.WriteAllText(Editor.fileName, Editor.textField.Text.ToString());
                    Editor._saved = true;
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