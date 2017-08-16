using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utiles.Logger
{
    public class Tracer
    {
        public static void Generate(String basedir)
        {
            Trace.Listeners.Clear();

            String dn = String.Format(@"{0}\LOG\{1}", basedir, Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location));

            if (!Directory.Exists(dn))
            {
                Directory.CreateDirectory(dn);
            }

            TextWriterTraceListener twtl = new TextWriterTraceListener(String.Format(@"{0}\{1:yyyMMdd_HHmmss}.log", dn, DateTime.Now));
            twtl.Name = "TextLogger";
            twtl.TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime | TraceOptions.Timestamp;

            ConsoleTraceListener ctl = new ConsoleTraceListener(false);
            ctl.TraceOutputOptions = TraceOptions.DateTime;

            Trace.Listeners.Add(twtl);
            Trace.Listeners.Add(ctl);

            Trace.AutoFlush = true;

            PrintDateTime();
        }

        public static void PrintDateTime()
        {
            Trace.WriteLine(string.Format("==]{0}", DateTime.Now));
        }
    }
}
