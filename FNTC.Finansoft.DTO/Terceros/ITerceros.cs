using FNTC.Finansoft.Accounting.DTO.Interfaces;
using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{


    public interface ITercerosRepository : IGenericRepository<Tercero>
    {

        Tercero2DTO GetSingle(string NIT);
        IEnumerable<Tercero2DTO> FindRange(string filterExpression, string sortingExpression, int startIndex, int count);


    }
}
