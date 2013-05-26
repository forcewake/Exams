using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Task14
{
    /*
     Создать на языке C# статический метод, который:
- вызывает функцию операционной системы:
void WINAPI GetNativeSystemInfo(LPSYSTEM_INFO lpSystemInfo);
где структура SYSTEM_INFO объявлена следующим образом:
struct _SYSTEM_INFO {
  union {
    DWORD  dwOemId;
    struct {
      WORD wProcessorArchitecture;
      WORD wReserved;
    };
  };
  DWORD     dwPageSize;
  LPVOID    lpMinimumApplicationAddress;
  LPVOID    lpMaximumApplicationAddress;
  DWORD_PTR dwActiveProcessorMask;
  DWORD     dwNumberOfProcessors;
  DWORD     dwProcessorType;
  DWORD     dwAllocationGranularity;
  WORD      wProcessorLevel;
  WORD      wProcessorRevision;
} SYSTEM_INFO;
Реализовать простейший пример использования статического метода GetNativeSystemInfo на языке C#.
     */

    internal class Program
    {
        private const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
        private const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
        private const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
        private const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

        private static void Main(string[] args)
        {
            Test.SystemInfo sysInfo = Test.LoadData();

            switch (sysInfo.wProcessorArchitecture)
            {
                case PROCESSOR_ARCHITECTURE_IA64:
                case PROCESSOR_ARCHITECTURE_AMD64:
                    Console.WriteLine(Test.Platform.X64.ToString());
                    break;
                case PROCESSOR_ARCHITECTURE_INTEL:
                    Console.WriteLine(Test.Platform.X86.ToString());
                    break;
                default:
                    Console.WriteLine(Test.Platform.Unknown.ToString());
                    break;
            }

            Console.WriteLine("dwNumberOfProcessors ->" + sysInfo.dwNumberOfProcessors);
            Console.WriteLine("wProcessorLevel ->" + sysInfo.wProcessorLevel);
            Console.WriteLine("dwProcessorType ->" + sysInfo.dwProcessorType);
            Console.ReadLine();
        }

        public static class Test
        {


            [StructLayout(LayoutKind.Sequential)]
            public struct SystemInfo
            {
                public ushort wProcessorArchitecture;
                public ushort wReserved;
                public uint dwPageSize;
                public IntPtr lpMinimumApplicationAddress;
                public IntPtr lpMaximumApplicationAddress;
                public UIntPtr dwActiveProcessorMask;
                public uint dwNumberOfProcessors;
                public uint dwProcessorType;
                public uint dwAllocationGranularity;
                public ushort wProcessorLevel;
                public ushort wProcessorRevision;
            };

            [DllImport("kernel32.dll")]
            private static extern void GetNativeSystemInfo(ref SystemInfo lpSystemInfo);

            [DllImport("kernel32.dll")]
            private static extern void GetSystemInfo(ref SystemInfo lpSystemInfo);

            public static SystemInfo LoadData()
            {
                var sysInfo = new SystemInfo();

                // Если WinXP или старше - используем GetNativeSystemInfo
                if (Environment.OSVersion.Version.Major > 5 ||
                    (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1))
                {
                    GetNativeSystemInfo(ref sysInfo);
                }
                    // Иначе используем GetSystemInfo
                else
                {
                    GetSystemInfo(ref sysInfo);
                }

                return sysInfo;
            }

            public enum Platform
            {
                X86,
                X64,
                Unknown
            }
        }
    }
}
