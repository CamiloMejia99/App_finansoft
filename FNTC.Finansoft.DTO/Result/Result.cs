using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.DTO.Result
{
    public class Result
    {
        public int ResultCode { get; set; }
        public int RowsAffected { get; set; }

        public Dictionary<int, string> Warnings { get; set; }
        public Dictionary<int, string> Errors { get; set; }
        public Dictionary<string, string> ErrorsWithKey { get; set; }

        public Result()
        {
            Warnings = new Dictionary<int, string>();
            Errors = new Dictionary<int, string>();
            ErrorsWithKey = new Dictionary<string, string>();
        }
    }

    public static class ResultCode
    {
        public static int Added { get { return 1; } }
        public static int Updated { get { return 2; } }
        public static int Deleted { get { return 3; } }

        public static int Success { get { return 4; } }
        public static int Delayed { get { return 5; } }
        public static int Error { get { return 6; } }
        public static int Duplicated { get { return 6; } }
        //otros
    }



}

