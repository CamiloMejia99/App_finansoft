//using .Terceros;
//using Lib.Web.Mvc.JQuery.JqGrid;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace FNTC.Finansoft.UI.Areas.Terceros.Controllers
//{
//    public class AdministradorasController : Controller
//    {
//        public ActionResult Administradoras(JqGridRequest request)
//        {

//            var terceros = new FNTC.Finansoft.Accounting.Terceros.TercerosBLL().Terceros;
//            //int totalRecords = _ordersRepository.GetCount();
//            int totalRecords = terceros.Count;

//            JqGridResponse response = new JqGridResponse()
//            {
//                TotalPagesCount = (int)Math.Ceiling((float)totalRecords / (float)request.RecordsCount),
//                PageIndex = request.PageIndex,
//                TotalRecordsCount = totalRecords
//            };
//            //response.Records.AddRange(from order in _ordersRepository.FindRange(String.Format("{0} {1}", request.SortingName, request.SortingOrder), request.PageIndex * request.RecordsCount, request.RecordsCount)
//            //                          select new JqGridRecord<OrderViewModel>(Convert.ToString(order.Id), new OrderViewModel(order)));

//            response.Records.AddRange(terceros.Select(t => new JqGridRecord<TerceroDTO>(t.NIT, t)));


//            return new JqGridJsonResult() { Data = response };
//        }
     
//    }
//}
