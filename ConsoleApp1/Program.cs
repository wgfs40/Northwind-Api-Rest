using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int date = 22020;
            var formate = string.Format("{0:######000000}", date);


            //var input = "JoanAlcidesAcostaSanchezb9y";
            //if(Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$"))
            //{
            //    Console.WriteLine("es correcto");
            //    Console.ReadLine();
            //}
            //else
            //{   
            //    Console.WriteLine($"es incorrecto");
            //    Console.ReadLine();
            //}
        }
    }
}
