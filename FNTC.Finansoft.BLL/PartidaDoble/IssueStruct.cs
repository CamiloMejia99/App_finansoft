using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.BLL
{
    #region Issues
    public class IssueStruct
    {
        // public List<KeyValuePair<string, string>> Errors { get; private set; }
        //public List<KeyValuePair<string, string>> Warnings { get; private set; }
        public List<Issue> Errors;
        public List<Issue> Warnings;

        public IssueStruct()
        {
            // Errors = new List<KeyValuePair<string, string>>();
            // Warnings = new List<KeyValuePair<string, string>>();

            Errors = new List<Issue>();
            Warnings = new List<Issue>();
        }

        public void AddError(string error, string key, int index = -1)
        {
            //Errors.Add(new KeyValuePair<string, string>(key, error));
            Errors.Add(new Issue(index, key, error));
        }

        public void AddWarning(string warning, string key, int index = -1)
        {
            //Warnings.Add(new KeyValuePair<string, string>(key, warning));
            Warnings.Add(new Issue(index, key, warning));
        }

        public bool IsValid()
        {
            return Errors.Count == decimal.Zero;
        }
    }
    public static class IssueMessages
    {
        public static string msgNoCostCntr = "Falta centro de costo para {0} en la posición {1}.";
        public static string msgUnbalancedVchr = "Comprobante no esta balanceado";
        public static string msgNoVchrNarration = "Comprobante requiere descripcion";
        public static string msgNoEntryNarration = "La anotacion {0} debe tener descripcion";
        public static string msgRequiereTercero = "LA anotacion {0} requiere tercero";
    }
    #endregion
}
