using System;
using System.Windows.Forms;
using System.Xml;
using SIL.PublishingSolution.Filter;
using SIL.PublishingSolution.Sort;

namespace SIL.PublishingSolution
{
    public partial class LiftPreparer : Form
    {
        private LiftReader lift;
        private LiftFilter[] filters;
        private LiftWriter intermediateFile;
        private string intermediateFileName;
        private string workingDirectoryPath;

        public LiftPreparer()
        {
            InitializeComponent();
        }

        public LiftPreparer(string workingDirectoryPath)
        {
            InitializeComponent();
            this.workingDirectoryPath = workingDirectoryPath;
        }

        private void liftPrepareLoad(object sender, EventArgs e)
        {

        }

        public void loadLift(string liftURI)
        {
            var strippedLift = stripWhitespaceFromLift(liftURI);
            lift = new LiftReader(strippedLift);
        }

        private string stripWhitespaceFromLift(string liftURI)
        {
            string strippedLiftURI = workingDirectoryPath + @"stripped.lift";
            var xmldoc = new XmlDocument();
            var liftreader = new LiftReader(liftURI);
            var liftwriter = new LiftWriter(strippedLiftURI);
            xmldoc.Load(liftreader);
            xmldoc.Save(liftwriter);
            liftreader.Close();
            liftwriter.Close();
            return strippedLiftURI;
        }

        public void loadFilters(string[] filterURIs)
        {
            filters = new LiftFilter[filterURIs.Length];
            for (int i = 0; i < filterURIs.Length; i++)
            {
                filters[i] = new LiftFilter(filterURIs[i]);
            }
        }

        public void applyFilters()
        {
            for (int i = 0; i < filters.Length; i++)
            {
                openIntermediateFile(i, "Filter");
                applyFilter(filters[i]);
                closeIntermediateFile();
                loadLiftFromMostRecentIntermediateFile();
            }
        }

        private void applyFilter(LiftFilter filter)
        {
            //var xmlTransformer = new XslCompiledTransform();
            //xmlTransformer.Load(applyTo);
            //xmlTransformer.Transform(lift, null, intermediateFile);
        }

        private void openIntermediateFile(int suffix, string type)
        {
            intermediateFileName = workingDirectoryPath + "after" + type + suffix + ".lift";
            intermediateFile = new LiftWriter(intermediateFileName);

        }

        private void closeIntermediateFile()
        {
            intermediateFile.Close();
        }

        private void loadLiftFromMostRecentIntermediateFile()
        {
            lift = new LiftReader(intermediateFileName);
        }

        public XmlTextReader getCurrentLift()
        {
            return lift;
        }

        public void loadSorters(string[] sorters)
        {
            
        }

        public void applySort()
        {
            var sorter = new LiftEntrySorter();
            
            openIntermediateFile(2, "Sorter");
            sorter.sort(lift,intermediateFile);
            closeIntermediateFile();
            loadLiftFromMostRecentIntermediateFile();

        }

        public void sortWritingSystems()
        {
            var liftDoc = new LiftDocument();
            liftDoc.Load(lift);
            var sorter = new LiftLangSorter(liftDoc);
            sorter.sortWritingSystems();
            openIntermediateFile(3,"Sorter");
            liftDoc.Save(intermediateFile);
            closeIntermediateFile();
            loadLiftFromMostRecentIntermediateFile();
        }
    }
}