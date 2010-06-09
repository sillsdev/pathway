using System;
using System.Windows.Forms;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class EntrySenseFilter : Form
    {
        public string FilterKey;
        public string FilterString;
        public bool IsFilterMatchCase;
        private readonly PublicationInformation _projectInfo;
        private readonly string _filter;
        public EntrySenseFilter()
        {
            InitializeComponent();
        }

        public EntrySenseFilter(PublicationInformation pi,string filter)
        {
            InitializeComponent();
            _projectInfo = pi;
            _filter = filter;
        }
        private void BtnOk_Click(object sender, EventArgs e)
        {
            if(!radioNone.Checked && TxtFilter.Text.Length == 0)
            {
                string[] msg = new[]
                                   {
                                       "Please enter the filter keyword"
                                   };
                LocDB.Message("EnterKeyword", "Please enter the filter keyword", msg, LocDB.MessageTypes.Error,
                              LocDB.MessageDefault.First);
                return;
            }
            string selectedItem = string.Empty;
            foreach (Control c in this.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Checked)
                    {
                        selectedItem = rb.Text;
                        break;
                    }
                }
            }
            FilterString = TxtFilter.Text;
            FilterKey = selectedItem;
            IsFilterMatchCase = ChkMatchCase.Checked;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void EntrySenseFilter_Load(object sender, EventArgs e)
        {
            string filterKey;
            string filterString;
            bool matchCase;
            if (_filter.ToLower() == "entry")
            {
                filterKey =_projectInfo.EntryFilterKey ;
                filterString = _projectInfo.EntryFilterString;
                matchCase = _projectInfo.IsEntryFilterMatchCase;
            }
            else if (_filter.ToLower() == "sense")
            {
                filterKey = _projectInfo.SenseFilterKey;
                filterString = _projectInfo.SenseFilterString;
                matchCase = _projectInfo.IsSenseFilterMatchCase;
            }
            else
            {
                filterKey = _projectInfo.LanguageFilterKey;
                filterString =_projectInfo.LanguageFilterString;
                matchCase =_projectInfo.IsLanguageFilterMatchCase;
            }

            foreach (Control c in this.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Text == filterKey)
                    {
                        rb.Checked = true;
                        break;
                    }
                }
            }
            this.Text = _filter.ToUpper() + " Filter for Items Containing...".ToUpper();
            TxtFilter.Text = filterString;
            ChkMatchCase.Checked = matchCase;
        }
     }
}
