using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Task08
{
    public class FileMover
    {
        private readonly object _lockObject = new object();
        private int _filesCounter = 0;

        public int FilesCounter
        {
            get
            {
                lock (_lockObject)
                {
                    return _filesCounter;
                }
            }
        }

        public FileMover(string source, string target, int numberOfThreads)
        {
            ThreadPool.SetMaxThreads(numberOfThreads, numberOfThreads);
            SourceFiles = new ConcurrentQueue<string>();
            Target = new ConcurrentQueue<string>();
            FillFilesName(source, target);
        }

        private ConcurrentQueue<string> SourceFiles { get; set; }
        private ConcurrentQueue<string> Target { get; set; }


        private void FillFilesName(string source, string target)
        {
            string[] files = Directory.GetFiles(source);

            foreach (string file in files)
            {
                string name = file;
                string dest = Path.Combine(target, Path.GetFileName(file));
                SourceFiles.Enqueue(name);
                Target.Enqueue(dest);
            }
            string[] folders = Directory.GetDirectories(source);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(target, name);
                FillFilesName(folder, dest);
            }
        }

        public void Copy()
        {
            var events = new List<ManualResetEvent>();
            for (int index = 0; index < SourceFiles.Count; index++)
            {
                var resetEvent = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(state =>
                    {

                        string source;
                        string target;
                        SourceFiles.TryDequeue(out source);
                        Target.TryDequeue(out target);
                        if (source != null && target != null)
                        {
                            lock (_lockObject)
                            {
                                if (Target != null && !Directory.Exists(Path.GetDirectoryName(target)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(target));
                                }
                            }
                            File.Copy(source, target, true);
                            Interlocked.Increment(ref _filesCounter);
                        }
                        resetEvent.Set();
                    });
                events.Add(resetEvent);
            }
            WaitHandle.WaitAll(events.ToArray()); 
        }
    }
}