using System;
using System.Text;

namespace FNTC.Finansoft.Accounting.DTO.Terceros.ViewModels
{
    public class CustomSearchViewModel
    {
        #region Properties
        public string Nit { get; set; }

        public string NombreComercial { get; set; }

        public string NOMBRE { get; set; }

        #endregion

        #region Methods
        public string GetFilterExpression()
        {
            StringBuilder filterExpressionBuilder = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(Nit))
                filterExpressionBuilder.Append(String.Format("Id = {0} AND ", Nit));
            if (!String.IsNullOrWhiteSpace(NombreComercial))
                filterExpressionBuilder.Append(String.Format("Name = \"{0}\" AND ", NombreComercial));
            if (NOMBRE != null)
                filterExpressionBuilder.Append(String.Format("SupplierId = {0} AND ", NOMBRE));

            if (filterExpressionBuilder.Length > 0)
                filterExpressionBuilder.Remove(filterExpressionBuilder.Length - 5, 5);
            return filterExpressionBuilder.ToString();
        }
        #endregion
    }
}
