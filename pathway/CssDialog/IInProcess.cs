using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    public interface IInProcess
    {
        ProgressBar Bar();

        void AddToMaximum(int n);

        void PerformStep();
    }
}