using System.Diagnostics;
namespace XYDEditor
{
    public class BuildTools
    {
        public static string processCommand(string command, string argument)
        {
            ProcessStartInfo start = new ProcessStartInfo(command, argument);
            string res = "";
            start.CreateNoWindow = true;
            start.ErrorDialog = true;
            start.UseShellExecute = true;

            if (start.UseShellExecute){
                start.RedirectStandardOutput = false;
                start.RedirectStandardError = false;
                start.RedirectStandardInput = false;
            } else {
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.RedirectStandardInput = true;
                start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
                start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            }

            Process p = Process.Start(start);

            if (!start.UseShellExecute) {
                if (!p.StandardOutput.EndOfStream)
                    res = p.StandardOutput.ReadToEnd();
                if (!p.StandardError.EndOfStream)
                    res += "(error: " + p.StandardError.ReadToEnd() + ")";
                if (res != "")
                    UnityEngine.Debug.Log(res);
            }

            p.WaitForExit();
            p.Close();

            return res;
        }
    }
}