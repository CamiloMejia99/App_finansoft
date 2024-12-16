using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DAL.Tools
{
    public class NumberFormat
    {
        public decimal Numero { get; set; }
        public int ParteDecimal { get; set; }

        public NumberFormat(decimal numero, int parteDecimal)
        {
            this.Numero = numero;
            this.ParteDecimal = parteDecimal;
        }

        public string GetFormatNumber()
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            string valor = "";
            try
            {
                valor = Numero.ToString("N" + ParteDecimal, formato);
            }
            catch (Exception ex)
            {
            }
            return valor;
        }

        List<int> FixMe(List<List<int>> myList)
        {
            List<int> newList = new List<int>();
            if (myList.Count != 0)
            { // imperative code
                foreach (List<int> item in myList)
                {
                    foreach (int element in item)
                    {
                        newList.Add(element);
                    }
                }
            }
            else
            {  // functional code
                newList = myList.Select(x => x.Count).ToList();
            }

            // sorting hierarchy:
            //   1. desc by x % 5
            //   2. desc by x
            return newList.OrderByDescending(x => x % 5 + x / (newList.Max() * 2)).ToList();
        }

        //List<int> FixMe2(List<List<int>> myList)
        //{

        //    List<int> newList = new List<int>();
        //    if (myList.Count % 2 == 0)
        //    { // imperative code
        //        foreach (List<int> item in myList)
        //        {
        //            foreach (int element in item)
        //            {
        //                newList = newList.Add(element);
        //            }
        //        }
        //    }
        //    else
        //    {  // functional code
        //        newList = myList.Select(x => x).ToList();
        //    }

        //    // sorting hierarchy:
        //    //   1. desc by x % 5
        //    //   2. desc by x
        //    return newList.OrderBy(x => x % 5 + x / (newList.Max() * 2)).ToList();
        //}



    }
}
