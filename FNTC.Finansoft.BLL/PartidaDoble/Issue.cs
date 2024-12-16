
namespace FNTC.Finansoft.Accounting.BLL
{
    public class Issue
    {


        public Issue(int index, string key, string message)
        {
            // TODO: Complete member initialization
            this.Index = index;
            this.Key = key;
            this.Message = message;
        }
        public int Index { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
