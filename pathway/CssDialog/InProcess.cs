using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    public partial class InProcess : Form, IInProcess
    {
        /// <summary>
        /// Controls whether the status label is displayed. Use the SetStatus call to display
        /// detailed information on what your process is doing.
        /// </summary>
        public bool ShowStatus 
        { 
            get { return lblDetails.Visible; }
            set 
            {
                // EDB - 11/15/2011: Argh. This needs to be reworked. Currently all processing is done on the 
                // main UI thread, so any status updates get blocked by longer-running processes.
                // This was a first attempt to update the progress bar with detailed information on what was
                // happening, but it doesn't work if the work gets too long.
                // Refer to LT-12139 and TD-2706 for more information.

                //lblDetails.Visible = value;
                //Height = SystemInformation.CaptionHeight + ((value == true) ? lblDetails.Bottom : progressBar1.Bottom) + 15;
                lblDetails.Visible = false;
                Height = SystemInformation.CaptionHeight + lblDetails.Top + 20;
            }
        }

        public InProcess()
        {
            InitializeComponent();
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            ShowStatus = false; // initially don't display the details (not all exports use it)
        }

        public InProcess(int min, int max)
        {
            InitializeComponent();
            progressBar1.Visible = true;
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;
            progressBar1.Value = min;
            progressBar1.Step = 1;
            ShowStatus = false; // initially don't display the details (not all exports use it)
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

        public void SetStatus(string newStatus)
        {
            // EDB 11/15/2011:
            // TODO: Processing really needs to go to a worker thread, with status updates getting passed to the UI
            // thread. The combination of ShowStatus and SetStatus code here (including the refresh() call)
            // works for shorter processing, but gets blocked on longer processing calls (ironically when it's needed most).

            // update the status text
            //lblDetails.Text = newStatus;
            //Refresh();
        }
    }
}
