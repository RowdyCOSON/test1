using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    public static class FruitExtension
    {
        public static string ReverseSumAll(this List<string> FruitList)
        {
            string Result = "";
            for(int i = 0; i < FruitList.Count; i++)
            {
                string ReverseString = new string(FruitList[i].ToCharArray().Reverse().ToArray());
                Result += ReverseString;
            }
            return Result;
        }

        public static int CountAll(this List<string> FruitList, string str)
        {
            int Result = 0;
            char[] CharArray = str.ToCharArray();
            for(int i = 0; i < FruitList.Count; i++)
            {
                char[] StringToArray = FruitList[i].ToCharArray();
                for(int j = 0; j < StringToArray.Length; j++)
                {
                    for (int h = 0; h < CharArray.Length; h++)
                    {
                       if(StringToArray[j] == CharArray[h])
                       {
                           Result++;
                       }
                    }
                }
            }
            return Result;
        }
    }

   
    class Program
    {      
        static void Main(string[] args)
        {
            List<string> fruits = new List<string>();
            
            
            fruits.Add("apple");
            fruits.Add("banana");
            fruits.Add("cranberry");


            // sum1 expected: elppaananabyrrebnarc
            string sum1 = fruits.ReverseSumAll();
            Console.WriteLine(sum1);
            
            // count1 expected: 5
            int count1 = fruits.CountAll("a");
            Console.WriteLine(count1);

        }
        
     
    }
    
}
