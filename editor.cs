using System;
using System.IO;
using System.Net.NetworkInformation;
using NStack;
using Terminal.Gui;

namespace win32editor
{
    internal class Editor
    {
        public static bool fileLoaded { get; set; } = false;
        public static string fileName { get; set; }
        public static bool _saved { get; set; } = true;  // assume the file is saved and check back later to see if the file has been updated. 

        public static readonly Window wind = new Window("Untitled") { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() };
        public static readonly TextView textField = new TextView { X = 0, Y = 1, Width = Dim.Fill(), Height = Dim.Fill() };
        private static readonly MenuBar menu = new MenuBar(new[] {
                new MenuBarItem ("_File", new[] {
                    new MenuItem ("_Open", "", Helpers.Open, null, null, Key.O | Key.CtrlMask),
                    new MenuItem ("_Save", "", Helpers.Save, null, null, Key.S | Key.CtrlMask),
                    null,
                    new MenuItem ("_Quit", "", Helpers.Quit, null, null, Key.Q | Key.CtrlMask),
                    new MenuItem("_lineNumber", "", Helpers.DisplayCursorPos, null, null, Key.P | Key.AltMask)
                }),
            });

        
        public static void Main(string[] args)
        {
            Application.Init();
            CaptureArgs(args);

            textField.ColorScheme = Colors.Dialog;
            menu.ColorScheme = Colors.Dialog;
            wind.Add(textField);
            wind.Add(menu);
            Application.Top.Add(wind);
            Application.Run();
        }

        private static void CaptureArgs(string[] args)
        {
            switch (args.Length > 0)
            {
                case true when File.Exists(args[0]):
                    fileName = args[0];
                    Helpers.LoadFile();
                    break;
                case true:
                    try
                    {
                        File.Create(fileName);
                    }
                    catch
                    {
                        Console.WriteLine("Could not create file");
                    }

                    break;
            }
        }
    }  
}