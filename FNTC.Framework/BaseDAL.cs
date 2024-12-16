using AutoMapper;
using System.Data.Entity;


namespace FNTC.Framework
{
    public class BaseDAL
    {
        //  public  Mapper Mapper { get; set; }
        public IMapper mapper;
        public DbContext ctx { get; set; }

        public BaseDAL()
        {

        }
    }
}
