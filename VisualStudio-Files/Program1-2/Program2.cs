using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenCentre
{
    public class Square
    {
        //lenght x width = area. 
        public double Lenght;
        public double Width;

        public void Calculated() //calculate method 
        {

            double total; //stores total size 
            double totalCost; //stores final cost 
            total = Lenght* Width; //calaculate size
            totalCost = total*2.10; //calculates size 
            Console.WriteLine("total cosr is: £{0}", totalCost);
        }
    }   

    


    
   
    class Program
    {
        static void Main(string[] args)
        {

        var square = new Square();
        Console.WriteLine("Input lenght");
        square.Lenght = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Input width");
        square.Width = Convert.ToDouble(Console.ReadLine());

        square.Calculated(); 

        }
    }
}
