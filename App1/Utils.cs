using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Display;

namespace App1
{
    static class Utils
    {
        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> source, T element)
        {
            bool first = true;
            foreach (T value in source)
            {
                if (!first) yield return element;
                yield return value;
                first = false;
            }
        }

        public static Task NullTask = Task.FromResult(0);
        private static DisplayRequest dr = new DisplayRequest();

        private static bool displayRequested = false;

        public static void RequestDisplay()
        {
            if (!displayRequested)
            {
                displayRequested = true;
                dr.RequestActive();
            }
        }

        public static void ReleaseDisplay()
        {
            if (displayRequested)
            {
                displayRequested = false;
                dr.RequestRelease();
            }
        }
    }
}
