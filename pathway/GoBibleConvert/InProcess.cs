using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    public partial class InProcess : Form, IInProcess
    {
        public InProcess()
        {
            InitializeComponent();
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
        }

        public InProcess(int min, int max)
        {
            InitializeComponent();
            progressBar1.Visible = true;
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;
            progressBar1.Value = min;
            progressBar1.Step = 1;
        }

        public void PerformStep()
        {
            progressBar1.PerformStep();
        }

        public ProgressBar Bar()
        {
            return progressBar1;
        }

        public void AddToMaximum(int n)
        {
            progressBar1.Maximum += n;
        }
    }
}
