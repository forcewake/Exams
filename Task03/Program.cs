using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task03
{
    class Program
    {
        static void Main(string[] args)
        {
            OSHandle handle = new OSHandle(new IntPtr(1000));
            var isValid = handle.IsValid;
            var isInvalid = handle.IsInvalid;
            var intPtr = handle.ToHandle();
            Console.WriteLine("isValid -> " + isValid);
            Console.WriteLine("isInvalid -> " + isInvalid);
            Console.WriteLine("intPtr -> " + intPtr.ToString());
            Console.ReadLine();
        }
    }
}
