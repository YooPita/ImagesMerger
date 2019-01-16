using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace img_merger
{
    class Program
    {
        static void Main(string[] args)
        {
            bool del = true;
            bool hor = true;
            int col = 100;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Y/N - да/нет, пустое или отличное - стандартное значение.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Удалять файлы(Y/N)? Стандартное: Y");
            if (Console.ReadLine() == "N") del = false;
            Console.WriteLine("Складывать по горизонтали(Y/N)? Стандартное: Y");
            if (Console.ReadLine() == "N") hor = false;
            Console.WriteLine("Сколько изображений в колонке()? Стандартное: 100");
            int.TryParse(Console.ReadLine(), out col);
            if (col == 0) col = 100;
            ImgMerger.MergeFolder(del, hor, col);
            Console.ReadKey();
        }
    }
}
