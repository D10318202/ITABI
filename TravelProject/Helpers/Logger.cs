using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TravelProject.Helpers
{
    public class Logger
    {
        private const string _savePath = "C:\\Csharp\\log\\log.log";
        public static void WriteLog(string moduleName, Exception ex)
        {
            string content = $@"---------\r\n
                                {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}
                                {moduleName}
                                {ex.ToString()}
                                ---------\r\n
                                {Environment.NewLine}";
            File.AppendAllText(_savePath, content);
        }
    }
}