using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace FNTC.Finansoft.UI.Areas.ControlErrores.Controllers
{
    public class Log
    {
        private string Path = "";


        public Log(string Path= @"c:\finasoft\log")  //ruta provicional para guardar log
        {
            this.Path = Path;
        }

        public void Add(string sLog)
        {
            CreateDirectory();
            string nombre = GetNameFile();
            string cadena = "";

            cadena += DateTime.Now + " - " + sLog + Environment.NewLine;

            StreamWriter sw = new StreamWriter(Path + "/" + nombre, true);
            sw.Write(cadena);
            sw.Close();

        }

        #region HELPER
        private string GetNameFile()
        {
            string nombre = "";

            nombre = "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".txt";

            return nombre;
        }

        private void CreateDirectory()
        {
            try
            
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);


            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion
    }
}