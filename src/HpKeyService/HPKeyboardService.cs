using System.ServiceProcess;
using System.Threading;

namespace HpKeyService
{
    public partial class HPKeyboardService : ServiceBase
    {
        private Thread _thread;

        public HPKeyboardService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _thread = new Thread(HpKeyboard.Run)
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest,
            };
            _thread.Start();
        }

        protected override void OnStop()
        {
            _thread.Abort();
            _thread = null;
        }
    }
}
