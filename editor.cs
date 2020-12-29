using System.IO;
using Terminal.Gui;


namespace win32editor
{
    internal class EditorProcess
    {

        public static string fileName { get; set; }
        public static bool _saved { get; set; } = true;  // assume the file is saved and check back later to see if the file has been updated. 
        public static Window wind = new Window("Untitled") { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() };
        public static TextView textField = new TextView { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() };
        public static MenuBar menu = new MenuBar(new[] {
                new MenuBarItem ("_File", new[] {
                    new MenuItem ("_Open", "", () => { Helpers.Open(); }, null, null, Key.O | Key.CtrlMask),
                    new MenuItem ("_Save", "", () => Helpers.Save(), null, null, Key.S | Key.CtrlMask),
                    null,
                    new MenuItem ("_Quit", "", () => Application.RequestStop(), null, null, Key.Q | Key.CtrlMask),
                    new MenuItem("_lineNumber", "", () => CurrLine(), null, null, Key.L | Key.CtrlMask)
                }),
            });
        public static void Main(string[] args)
        {
            Application.Init();
            CaptureArgs(args);

            //Create Mouse-Accessed menubar to save, open, load, files.
            
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
                    Helpers.LoadFile();
                }
                else
                {
                    fileName = "Untitled";
                }
            }
        }

        private static void CurrLine()
        {
            var d = new Label(textField.CurrentRow.ToString()) { X = 0, Y = 0, Width = Dim.Percent(100), Height = Dim.Percent(10) };
            wind.Add(d);
        }
        
    }
}