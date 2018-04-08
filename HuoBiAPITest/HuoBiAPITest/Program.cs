using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuobiAPI;

namespace HuoBiAPITest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("火币网API测试：");

            HuobiService huoBiService = new HuobiService();

            // 这里添加相应的交易操作等
            string accountInfoJson = huoBiService.getAccountInfo();
            Console.WriteLine(accountInfoJson);
            Console.ReadKey();
        }
    }
}
