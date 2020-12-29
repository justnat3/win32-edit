using System.IO;
using Terminal.Gui;

//struct Buffer
//{
//    private string     bufferName;
//    private int        numChars;
//    private int        numlines;
//    private DateTime   fileTime;
//    private bool       _isModified;

//    public Buffer(string bufferName, int numChars, int numlines, DateTime fileTime, bool _isModified)
//    {
//        this.bufferName  = bufferName;
//        this.numChars    = numChars;
//        this.numlines    = numlines;
//        this.fileTime    = fileTime;
//        this._isModified = _isModified;
//    }
//}
    

namespace win32editor
{
    public static class EditorProcess
    {

        private static string fileName;
        private static bool _saved = true;
        private static Window wind = new Window() { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() };
        private static TextView textField = new TextView { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() };

        public static void Main(string[] args)
        {
            Application.Init();
            //create function for args
            

            //Create Mouse-Accessed menubar to save, open, load, files.
            var menu = new MenuBar(new[] {
                new MenuBarItem ("_File", new[] {
                    new MenuItem ("_New", "", New),
                    new MenuItem ("_Open", "", () => Open(), null, null, Key.O | Key.CtrlMask),
                    new MenuItem ("_Save", "", () => Save(), null, null, Key.S | Key.CtrlMask),
                    null,
                    new MenuItem ("_Quit", "", () => Application.RequestStop(), null, null, Key.Q | Key.CtrlMask),
                    new MenuItem("_lineNumber", "", () => CurrLine(), null, null, Key.L | Key.CtrlMask)
                }),
            }); ; ;
            textField.ColorScheme = Colors.Dialog;
            menu.ColorScheme = Colors.Dialog;
            wind.Add(textField);
            wind.Add(menu);
            Application.Top.Add(wind);
            Application.Run();
            
        }

        public static void CaptureArgs(string[] args)
        {
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    fileName = args[0];
                    LoadFile();
                }
            }

        }

        private static void New()
        {
            wind.Title = fileName = "Untitled";

        }

        private static void CurrLine()
        {
            var d = new Label(textField.CurrentRow.ToString()) { X = 0, Y = 0, Width = Dim.Percent(100), Height = Dim.Percent(10)};
            wind.Add(d);
        }
        private static void LoadFile ()
        {
            
            if (!_saved) {
                MessageBox.ErrorQuery ("Not Implemented", "Functionality not yet implemented.", "Ok");
            }
            

            if (fileName != null) {
                string s = "";
                byte[] buffer = File.ReadAllBytes(fileName);

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
                
                textField.Text = removeNewLines;
                wind.Title = fileName;
                _saved = true;
            }
        }
        
     
        private static void Open ()
        {
            var d = new OpenDialog ("Open", "Open a file") { AllowsMultipleSelection = false };
            Application.Run (d);

            if (!d.Canceled) {
                fileName = d.FilePaths [0];
                LoadFile ();
            }
        }

        private static void Save ()
        {
            if (fileName != null) {
                // BUGBUG: #279 TextView does not know how to deal with \r\n, only \r 
                // As a result files saved on Windows and then read back will show invalid chars.
                File.WriteAllText (fileName, textField.Text.ToString());
                _saved = true;
            }
        }
    }
}