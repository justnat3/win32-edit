using Terminal.Gui;
using System.IO;

namespace win32editor
{
    internal class Helpers
    {

        public static void LoadFile()
        {

            if (!Editor._saved)
            {
                MessageBox.ErrorQuery("Not Implemented", "Functionality not yet implemented.", "Ok");
            }


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
                Editor.wind.Title = Editor.fileName;
                Editor._saved = true;
            }
        }


        public static void Open()
        {
            var d = new OpenDialog("Open", "Open a file") { AllowsMultipleSelection = false };
            Application.Run(d);

            if (!d.Canceled)
            {
                Editor.fileName = d.FilePaths[0];
                LoadFile();
            }
        }

        public static void Save()
        {
            if (Editor.fileName != null)
            {
                // BUGBUG: #279 TextView does not know how to deal with \r\n, only \r 
                // As a result files saved on Windows and then read back will show invalid chars.
                File.WriteAllText(Editor.fileName, Editor.textField.Text.ToString());
                Editor._saved = true;
            }
        }
    }
}