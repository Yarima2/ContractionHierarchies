using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies.Io
{
    class ConsoleProgressBar
    {

        const char done = '■';
        const char empty = '_';
        const int progressBarSize = 50;
        const float minProgressIncrement = 0.005f;

        private DateTime lastProgress = DateTime.MinValue;
        private int lastLineLength;

        public ConsoleProgressBar()
        {
            WriteProgress(0f);
        }

        public void ReportProgress(float progress)
        {
            if (DateTime.Now - lastProgress > TimeSpan.FromMilliseconds(100) || progress == 1)
            {
                DeleteProgress();
                WriteProgress(progress);
            }
        }

        private void WriteProgress(float progress)
        {
            lastProgress = DateTime.Now;
            Console.Write("[");
            for (int i = 0; i < progressBarSize; i++)
            {
                if ((float) i / progressBarSize < progress)
                {
                    Console.Write(done);
                }
                else
                {
                    Console.Write(empty);
                }
            }
            string percentString = (progress * 100).ToString("0.00");
            Console.Write("] " + percentString + "%");
            lastLineLength = progressBarSize + percentString.Length + 4;
        }

        private void DeleteProgress()
        {
            for (int i = 0; i < lastLineLength; i++)
            {
                Console.Write("\b");
            }
        }
    }
}
