using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticAbstractDemo
{
    public static class Checker
    {

        public static List<string> CalculationLog = new List<string>();


        public static int CalculateSum(int a, int b)
        {
            int result = a + b;
            return result; 
        }

        public static float CalculateProduct(float a, float b)
        {
            float result = a * b;
            Logging(result.ToString());
            return result; 

        }
        

        public static void Logging (string result)
        {

            string logTime = DateTime.Now.ToString();
            CalculationLog.Add("Operation run at: " + logTime + " output recorded: " + result);

        }

        public static void ShowLog()
        {
            foreach (string log in CalculationLog)
            {
                Console.WriteLine(log);
            }
        }

    }
}
