using AutoMapper;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Interfaces;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Framework.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.Terceros
{
    public class TercerosRepository :
     GenericRepository<AccountingContext, Tercero>, ITercerosRepository
    {

        public Tercero2DTO GetSingle(string NIT)
        {
            var query = Context.Terceros.FirstOrDefault(x => x.NIT.Equals(NIT));
            return Mapper.Map<Tercero2DTO>(query);
        }

        public Tercero2DTO FindBy(string NIT)
        {
            var query = base.FindBy(x => x.NIT.Equals(NIT));

            var query2 = Context.Terceros.Where(x => x.NIT.Equals(NIT));
            return Mapper.Map<Tercero2DTO>(query);
        }





        public int GetCount(string filterExpression)
        {
            if (!String.IsNullOrWhiteSpace(filterExpression))
            {
                var en = Context;
                var r = en.Terceros.Where(filterExpression).Count();
                return r;
                //return Context.Terceros.Where(filterExpression).Count();


            }

            else
                return Context.Terceros.Count();
        }


        public new IEnumerable<Tercero2DTO> FindRange(string filterExpression, string sortingExpression, int startIndex, int count)
        {
            throw new NotImplementedException();
        }
    }
}
