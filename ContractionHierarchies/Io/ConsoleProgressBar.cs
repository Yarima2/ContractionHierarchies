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
        const int progressBarSize = 0;
        const float minProgressIncrement = 0.005f;

        private float lastProgress = 0f;
        private int lastLineLength;

        public ConsoleProgressBar()
        {
            WriteProgress(0f);
        }

        public void ReportProgress(float progress)
        {
            if (Math.Abs(progress - lastProgress) > minProgressIncrement)
            {
                DeleteProgress();
                WriteProgress(progress);
            }
        }

        private void WriteProgress(float progress)
        {

            Console.Write("[");
            lastProgress = progress;
            string percentString = (progress * 100).ToString("0.00");
            Console.Write("] " + percentString + "%");
            lastLineLength = progressBarSize + percentString.Length + 4;
        }

        private void DeleteProgress()
        {
            for (int i = 0; i < lastLineLength; i++)
            {
                Console.WriteLine("\b");
            }
        }
    }
}
