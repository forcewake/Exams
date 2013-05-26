using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Task01
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string sourceFile;
            //sourceFile = args[0];
            string destFile;
            //destFile = args[1];

            sourceFile = @"E:\test-out.zip";
            destFile = @"E:\destFile-test-out.zip";

            Copy(sourceFile, destFile);

            Console.ReadLine();
        }

        private static async void Copy(string sourceFile, string destFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException("check sourceFile");
            }

            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }

            var cancellationToken = new CancellationToken();

            try
            {
                var progress = new Progress<long>();

                using (var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var destination = new FileStream(destFile, FileMode.CreateNew, FileAccess.Write))
                {
                    Console.WriteLine("Source length: {0}", source.Length.ToString());
                    double percentage = 0;
                    double current = 0;
                    progress.ProgressChanged += (sender, length) =>
                        {
                            percentage = double.Parse(length.ToString())/double.Parse(source.Length.ToString())*
                                         100;
                            if (percentage - current > 1)
                            {
                                Console.Clear();
                                current = percentage;
                                Console.WriteLine("{0} percents read.", int.Parse(Math.Truncate(percentage).ToString()));
                            }
                        };
                    await CopyToAsync(source, destination, cancellationToken, progress);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Copy canceled.");
            }
        }

        private static async Task CopyToAsync(Stream source, Stream destination,
                                              CancellationToken cancellationToken,
                                              IProgress<long> progress)
        {
            var buffer = new byte[0x1000];
            int bytesRead;
            long totalRead = 0;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead);
                cancellationToken.ThrowIfCancellationRequested(); // cancellation support
                totalRead += bytesRead;
                progress.Report(totalRead); // progress support
            }
        }
    }
}