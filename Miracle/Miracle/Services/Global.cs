using System;
using System.Collections.Generic;
using System.Text;

namespace Miracle.Services
{
    public static class G
    {
        public static string DBConString = @"SERVER=14.34.80.72; DATABASE=miracle; UID=miracle; PASSWORD=miracle!;Charset=utf8;SslMode=none";
        //public static string DBConString = @"SERVER=14.34.80.72; DATABASE=hs_aquafeed_mes; UID=root; PASSWORD=innobe#123;Charset=utf8;SslMode=none";
        //public static string DBConString = @"SERVER=220.71.215.170; DATABASE=haedong_mes; UID=root; PASSWORD=innobe#123;Charset=utf8;SslMode=none";
        public static string DBQueryCustomer = @"SELECT CustomerID AS `고객번호`, CustomerName AS `고객명` FROM customer ORDER BY CustomerName LIMIT 3;";
        //public static string DBConString = @"server=220.71.215.170; port=3306; database=haedong_mes; user=root; password=innobe#123;";
        //public static MariaDB mariaDB = new MariaDB(); 

        public static string CustomerName = "";
    }
}
