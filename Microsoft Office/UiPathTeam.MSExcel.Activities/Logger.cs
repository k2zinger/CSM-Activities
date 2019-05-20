using System.Diagnostics;

namespace UiPathTeam.MSExcel.Activities
{
    internal class Logger
    {
        private static Logger _instance;
        private TraceSource _traceSource;

        private Logger()
        {

            _traceSource = new TraceSource("Workflow");
        }

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }

                return _instance;
            }
        }

        public void Info(string message)
        {
            _traceSource.TraceEvent(TraceEventType.Information, 0, message);
        }
    }
}
