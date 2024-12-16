//using AutoMapper;
//using .Terceros;
////using FNTC.Finansoft.Accounting.DAL;
////using FNTC.Finansoft.Accounting.DAL.Terceros;
//using FNTC.Finansoft.UI.Areas.Terceros.ViewModels.JqGrid;
//using Lib.Web.Mvc.JQuery.JqGrid;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;

//namespace FNTC.Finansoft.UI.Areas.Terceros.Controllers
//{
//    public class Terceros2Controller : Controller
//    {
//        public int MyProperty { get; set; }
//        public Terceros2Controller()
//        {

//        }
//        // GET: Terceros/Default
//        public ActionResult Index()
//        {
//            //  var terceros = new FNTC.Finansoft.Accounting.Terceros.TercerosBLL().Terceros;
//            //  return View(terceros);
//            return View("IndexTerceros");
//        }

//        [OutputCache(Duration = 100)]
//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult Terceros(JqGridRequest request)
//        {

//            var terceros = new FNTC.Finansoft.Accounting.Terceros.TercerosBLL().Terceros;
//            var tercerosVM = new List<TerceroViewModel>();
//            //convertir a ViewMOdel
//            //var tercerosVM = terceros.Select(t => new TerceroViewModel(t));
//            #region Mapper
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<Tercero2DTO, TerceroViewModel>();
//            });
//            var mapper = config.CreateMapper();
//            #endregion

//            foreach (var item in terceros)
//            {
//                var terceroVM = mapper.Map<TerceroViewModel>(item);
//                tercerosVM.Add(terceroVM);
//            }

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

//            response.Records.AddRange(tercerosVM.Select(t => new JqGridRecord<TerceroViewModel>(t.NIT, t)));


//            return new JqGridJsonResult() { Data = response };
//        }

//        //[AcceptVerbs(HttpVerbs.Post)]
//        //public ActionResult Terceros(JqGridRequest request, CustomSearchViewModel viewModel)
//        //{
//        //    var terceros = new TercerosRepository();

//        //    string filterExpression = String.Empty;
//        //    if (request.Searching)
//        //    {
//        //        if (request.SearchingFilter != null)
//        //            filterExpression = GetFilter(request.SearchingFilter.SearchingName, request.SearchingFilter.SearchingOperator, request.SearchingFilter.SearchingValue);
//        //        else if (request.SearchingFilters != null)
//        //        {
//        //            StringBuilder filterExpressionBuilder = new StringBuilder();
//        //            string groupingOperatorToString = request.SearchingFilters.GroupingOperator.ToString();
//        //            foreach (JqGridRequestSearchingFilter searchingFilter in request.SearchingFilters.Filters)
//        //            {
//        //                filterExpressionBuilder.Append(GetFilter(searchingFilter.SearchingName, searchingFilter.SearchingOperator, searchingFilter.SearchingValue));
//        //                filterExpressionBuilder.Append(String.Format(" {0} ", groupingOperatorToString));
//        //            }
//        //            if (filterExpressionBuilder.Length > 0)
//        //                filterExpressionBuilder.Remove(filterExpressionBuilder.Length - groupingOperatorToString.Length - 2, groupingOperatorToString.Length + 2);
//        //            filterExpression = filterExpressionBuilder.ToString();
//        //        }
//        //        else if (viewModel != null)
//        //            filterExpression = viewModel.GetFilterExpression();
//        //    }

//        //    //Required workaround for grouping (it passes column name instead of index)
//        //    string sortingName = request.SortingName.Replace("Category", "CategoryId");

//        //    int totalRecordsCount = terceros.GetCount(filterExpression);

//        //    JqGridResponse response = new JqGridResponse()
//        //    {
//        //        TotalPagesCount = (int)Math.Ceiling((float)totalRecordsCount / (float)request.RecordsCount),
//        //        PageIndex = request.PageIndex,
//        //        TotalRecordsCount = totalRecordsCount,
//        //        //Footer data
//        //        //UserData = new { QuantityPerUnit = "Avg Unit Price:", UnitPrice = _productsRepository.Find(filterExpression).Average(p => p.UnitPrice) }
//        //        UserData = new { QuantityPerUnit = "algun texto", UnitPrice = 1 }
//        //    };
//        //    //response.Records.AddRange(from tercero in terceros.FindRange(filterExpression, String.Format("{0} {1}", sortingName, request.SortingOrder), request.PageIndex * request.RecordsCount, (request.PagesCount.HasValue ? request.PagesCount.Value : 1) * request.RecordsCount)
//        //    //                          select new JqGridRecord<TerceroViewModel>(tercero.NIT, new TerceroViewModel(tercero)));

//        //    return new JqGridJsonResult() { Data = response };
//        //}

//        private string GetFilter(string searchingName, JqGridSearchOperators searchingOperator, string searchingValue)
//        {
//            string searchingOperatorString = String.Empty;
//            switch (searchingOperator)
//            {
//                case JqGridSearchOperators.Eq:
//                    searchingOperatorString = "=";
//                    break;
//                case JqGridSearchOperators.Ne:
//                    searchingOperatorString = "!=";
//                    break;
//                case JqGridSearchOperators.Lt:
//                    searchingOperatorString = "<";
//                    break;
//                case JqGridSearchOperators.Le:
//                    searchingOperatorString = "<=";
//                    break;
//                case JqGridSearchOperators.Gt:
//                    searchingOperatorString = ">";
//                    break;
//                case JqGridSearchOperators.Ge:
//                    searchingOperatorString = ">=";
//                    break;
//            }

//            searchingName = searchingName.Replace("Category", "CategoryId");
//            if ((searchingName == "Id") || (searchingName == "SupplierId") || (searchingName == "CategoryId"))
//                return String.Format("{0} {1} {2}", searchingName, searchingOperatorString, searchingValue);

//            if ((searchingName == "Name"))
//            {
//                switch (searchingOperator)
//                {
//                    case JqGridSearchOperators.Bw:
//                        return String.Format("{0}.StartsWith(\"{1}\")", searchingName, searchingValue);
//                    case JqGridSearchOperators.Bn:
//                        return String.Format("!{0}.StartsWith(\"{1}\")", searchingName, searchingValue);
//                    case JqGridSearchOperators.Ew:
//                        return String.Format("{0}.EndsWith(\"{1}\")", searchingName, searchingValue);
//                    case JqGridSearchOperators.En:
//                        return String.Format("!{0}.EndsWith(\"{1}\")", searchingName, searchingValue);
//                    case JqGridSearchOperators.Cn:
//                        return String.Format("{0}.Contains(\"{1}\")", searchingName, searchingValue);
//                    case JqGridSearchOperators.Nc:
//                        return String.Format("!{0}.Contains(\"{1}\")", searchingName, searchingValue);
//                    default:
//                        return String.Format("{0} {1} \"{2}\"", searchingName, searchingOperatorString, searchingValue);
//                }
//            }

//            return String.Empty;
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult UpdateTercero(TerceroViewModel viewModel)
//        {
//            //Product product = _productsRepository.FindByKey(viewModel.ProductId);
//            //switch (viewModel.PropertyName)
//            //{
//            //    case "Name":
//            //        product.Name = (string)viewModel.PropertyValue;
//            //        break;
//            //    case "CategoryId":
//            //        product.CategoryId = (int)viewModel.PropertyValue;
//            //        break;
//            //    case "QuantityPerUnit":
//            //        product.QuantityPerUnit = (string)viewModel.PropertyValue;
//            //        break;
//            //    case "UnitPrice":
//            //        product.UnitPrice = (decimal)viewModel.PropertyValue;
//            //        break;
//            //}

//            try
//            {
//                //   var ctx = new .Terceros.AsociadosContext();
//                // var tercero = ctx.Terceros.Find(viewModel.NIT);     
//                //new .Terceros.AsociadosContext().Terceros.Find(viewModel.NIT); 

//                #region Mapper
//                var config = new MapperConfiguration(cfg =>
//                {
//                    cfg.CreateMap<TerceroViewModel, Tercero2DTO>();
//                });
//                var mapper = config.CreateMapper();
//                #endregion

//                var tercerrUpdated = mapper.Map<TerceroDTO>(viewModel);
//                var result = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().UpdateTercero(tercerrUpdated);
//                return Json(result);
//            }
//            catch (Exception exxx)
//            {
//                return Json(false);
//            }

//            //  return Json(true);
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult CreateTercero(TerceroViewModel viewModel)
//        {
//            try
//            {
//                //   var ctx = new .Terceros.AsociadosContext();
//                // var tercero = ctx.Terceros.Find(viewModel.NIT);     
//                //new .Terceros.AsociadosContext().Terceros.Find(viewModel.NIT); 

//                #region Mapper
//                var config = new MapperConfiguration(cfg =>
//                {
//                    cfg.CreateMap<TerceroViewModel, Tercero2DTO>()
//                          .ForMember(
//                                  src => src.NOMBRE
//                                  , opt => opt.MapFrom(src => (src.NOMBRE1 + src.APELLIDO1)))
//                        ;
//                });
//                var mapper = config.CreateMapper();
//                #endregion

//                var tercerrUpdated = mapper.Map<TerceroDTO>(viewModel);
//                var result = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().CreateTercero(tercerrUpdated);
//                return Json(result);
//            }
//            catch (Exception exxx)
//            {
//                return Json(false);
//            }

//        }

//    }
//}
