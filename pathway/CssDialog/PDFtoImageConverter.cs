using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;

namespace SIL.PublishingSolution
{

    /// <summary> 
    /// Class to convert a pdf to an image using GhostScript DLL for Linux
    /// A big Credit for this code go to:ECGroup 
    /// I mainly create a better interface and refactor it to made it ready to use! 
    /// </summary> 

    public class PDFtoImageConverter
    {
        #region Static
        /// <summary>Use to check for default transformation</summary> 
        private static bool useSimpleAnsiConversion = true;
        /// <summary>Thanks to 	tchu_2000 to remind that u should never hardcode strings! :)</summary> 
        private const string GS_OutputFileFormat = "-sOUTPUTFILE={0}";
        private const string GS_DeviceFormat = "-sDEVICE={0}";
        private const string GS_FirstParameter = "gs";
        private const string GS_ResolutionXFormat = "-r{0}";
        private const string GS_ResolutionXYFormat = "-r{0}x{1}";
        private const string GS_FirstPageFormat = "-dFIRSTPAGE={0}";
        private const string GS_LastPageFormat = "-dLASTPAGE={0}";
        private const string GS_FitPage = "-dPDFFitPage";
        private const string GS_PageSizeFormat = "-g{0}x{1}";
        private const string GS_JpegQualityFormat = "-dJPEGQ={0}";
        private const string GS_Fixed1stParameter = "-dSAFER";
        private const string GS_Fixed2ndParameter = "-dNOPAUSE";
        private const string GS_Fixed3rdParameter = "-dQUIET -dBATCH";
        private const string GS_QuiteOperation = "-dQUIET";
        private const string GS_StandardOutputDevice = "-";
        private const string GS_MultiplePageCharacter = "%";
        #endregion
        #region Windows Import
        /// <summary>Needed to copy memory from one location to another, used to fill the struct</summary> 
        /// <param name="Destination"></param> 
        /// <param name="Source"></param> 
        /// <param name="Length"></param> 
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);
        #endregion
        #region GhostScript Import

        /// <summary>Create a new instance of Ghostscript. This instance is passed to most other gsapi functions. The caller_handle will be provided to callback functions. 
        ///  At this stage, Ghostscript supports only one instance. </summary> 
        /// <param name="pinstance"></param> 
        /// <param name="caller_handle"></param> 
        /// <returns></returns> 
        [DllImport("libgs.so.8.71", EntryPoint = "gsapi_new_instance")]
        private static extern int gsapi_new_instance(out IntPtr pinstance, IntPtr caller_handle);

        /// <summary>This is the important function that will perform the conversion</summary> 
        /// <param name="instance"></param> 
        /// <param name="argc"></param> 
        /// <param name="argv"></param> 
        /// <returns></returns> 
        [DllImport("libgs.so.8.71", EntryPoint = "gsapi_init_with_args")]
        private static extern int gsapi_init_with_args(IntPtr instance, int argc, IntPtr argv);
        /// <summary> 
        /// Exit the interpreter. This must be called on shutdown if gsapi_init_with_args() has been called, and just before gsapi_delete_instance().  
        /// </summary> 
        /// <param name="instance"></param> 
        /// <returns></returns> 
        [DllImport("libgs.so.8.71", EntryPoint = "gsapi_exit")]
        private static extern int gsapi_exit(IntPtr instance);

        /// <summary> 
        /// Destroy an instance of Ghostscript. Before you call this, Ghostscript must have finished. If Ghostscript has been initialised, you must call gsapi_exit before gsapi_delete_instance.  
        /// </summary> 
        /// <param name="instance"></param> 
        [DllImport("libgs.so.8.71", EntryPoint = "gsapi_delete_instance")]
        private static extern void gsapi_delete_instance(IntPtr instance);
        /// <summary>Get info about the version of Ghostscript i'm using</summary> 
        /// <param name="pGSRevisionInfo"></param> 
        /// <param name="intLen"></param> 
        /// <returns></returns> 
        [DllImport("libgs.so.8.71", EntryPoint = "gsapi_revision")]
        private static extern int gsapi_revision(ref GS_Revision pGSRevisionInfo, int intLen);
        /// <summary>Use a different I/O</summary> 
        /// <param name="lngGSInstance"></param> 
        /// <param name="gsdll_stdin">Function that menage the Standard INPUT</param> 
        /// <param name="gsdll_stdout">Function that menage the Standard OUTPUT</param> 
        /// <param name="gsdll_stderr">Function that menage the Standard ERROR output</param> 
        /// <returns></returns> 
        [DllImport("libgs.so.8.71", EntryPoint = "gsapi_set_stdio")]
        private static extern int gsapi_set_stdio(IntPtr lngGSInstance, StdioCallBack gsdll_stdin, StdioCallBack gsdll_stdout, StdioCallBack gsdll_stderr);

        #endregion
        #region Const
        const int e_Quit = -101;
        const int e_NeedInput = -106;
        #endregion
        #region Variables
        private string _sDeviceFormat;
        private string _sParametersUsed;

        private int _iWidth;
        private int _iHeight;
        private int _iResolutionX;
        private int _iResolutionY;
        private int _iJPEGQuality;
        /// <summary>The first page to convert in image</summary> 
        private int _iFirstPageToConvert = -1;
        /// <summary>The last page to conver in an image</summary> 
        private int _iLastPageToConvert = -1;
        private bool _bFitPage;
        private bool _bThrowOnlyException = false;
        private bool _bRedirectIO = false;
        private IntPtr _objHandle;
        /// <summary>If true i will try to output everypage to a different file!</summary> 
        private bool _didOutputToMultipleFile = false;

        private System.Diagnostics.Process myProcess;
        public StringBuilder output;
        //public string output; 
        //private List<byte> outputBytes; 
        //public string error; 
        #endregion
        #region Proprieties
        /// <summary> 
        /// What format to use to convert 
        /// is suggested to use png256 instead of jpeg for document! 
        /// they are smaller and better suited! 
        /// </summary> 
        public string OutputFormat
        {
            get { return _sDeviceFormat; }
            set { _sDeviceFormat = value; }
        }

        public string ParametersUsed
        {
            get { return _sParametersUsed; }
            set { _sParametersUsed = value; }
        }

        public int Width
        {
            get { return _iWidth; }
            set { _iWidth = value; }
        }

        public int Height
        {
            get { return _iHeight; }
            set { _iHeight = value; }
        }

        public int ResolutionX
        {
            get { return _iResolutionX; }
            set { _iResolutionX = value; }
        }

        public int ResolutionY
        {
            get { return _iResolutionY; }
            set { _iResolutionY = value; }
        }

        public Boolean FitPage
        {
            get { return _bFitPage; }
            set { _bFitPage = value; }
        }
        /// <summary>Quality of compression of JPG</summary> 
        public int JPEGQuality
        {
            get { return _iJPEGQuality; }
            set { _iJPEGQuality = value; }
        }
        /// <summary>The first page to convert in image</summary> 
        public int FirstPageToConvert
        {
            get { return _iFirstPageToConvert; }
            set { _iFirstPageToConvert = value; }
        }
        /// <summary>The last page to conver in an image</summary> 
        public int LastPageToConvert
        {
            get { return _iLastPageToConvert; }
            set { _iLastPageToConvert = value; }
        }
        /// <summary>Set to True if u want the program to never display Messagebox 
        /// but otherwise throw exception</summary> 
        public Boolean ThrowOnlyException
        {
            get { return _bThrowOnlyException; }
            set { _bThrowOnlyException = value; }
        }
        /// <summary>If i should redirect the Output of Ghostscript library somewhere</summary> 
        public bool RedirectIO
        {
            get { return _bRedirectIO; }
            set { _bRedirectIO = value; }
        }
        /// <summary>If true i will try to output everypage to a different file!</summary> 
        public bool OutputToMultipleFile
        {
            get { return _didOutputToMultipleFile; }
            set { _didOutputToMultipleFile = value; }
        }
        #endregion
        #region Init
        public PDFtoImageConverter(IntPtr objHandle)
        {
            _objHandle = objHandle;
        }

        public PDFtoImageConverter()
        {
            _objHandle = IntPtr.Zero;
        }
        #endregion

        #region Convert
        /// <summary>Convert a single file!</summary> 
        /// <param name="inputFile">The file PDf to convert</param> 
        /// <param name="outputFile">The image file that will be created</param> 
        /// <remarks>You must pass all the parameter for the conversion 
        /// as Proprieties of this class</remarks> 
        /// <returns>True if the conversion succed!</returns> 
        public bool Convert(string inputFile, string outputFile)
        {
            return Convert(inputFile, outputFile, _bThrowOnlyException, null);
        }

        /// <summary>Convert a single file!</summary> 
        /// <param name="inputFile">The file PDf to convert</param> 
        /// <param name="outputFile">The image file that will be created</param> 
        /// <param name="parameters">You must pass all the parameter for the conversion here</param> 
        /// <remarks>Thanks to 	tchu_2000 for the help!</remarks> 
        /// <returns>True if the conversion succed!</returns> 
        public bool Convert(string inputFile, string outputFile, string parameters)
        {
            return Convert(inputFile, outputFile, _bThrowOnlyException, parameters);
        }

        /// <summary>Convert a single file!</summary> 
        /// <param name="inputFile">The file PDf to convert</param> 
        /// <param name="outputFile">The image file that will be created</param> 
        /// <param name="throwException">if the function should throw an exception 
        /// or display a message box</param> 
        /// <remarks>You must pass all the parameter for the conversion 
        /// as Proprieties of this class</remarks> 
        /// <returns>True if the conversion succed!</returns> 
        private bool Convert(string inputFile, string outputFile, bool throwException, string options)
        {
            #region Check Input
            //Avoid to work when the file doesn't exist 
            if (string.IsNullOrEmpty(inputFile))
            {
                if (throwException)
                    throw new ArgumentNullException("inputFile");
                else
                {
                    System.Windows.Forms.MessageBox.Show("The inputfile is missing");
                    return false;
                }
            }
            if (!System.IO.File.Exists(inputFile))
            {
                if (throwException)
                    throw new ArgumentException(string.Format("The file :'{0}' doesn't exist", inputFile), "inputFile");
                else
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("The file :'{0}' doesn't exist", inputFile));
                    return false;
                }
            }
            if (string.IsNullOrEmpty(_sDeviceFormat))
            {
                if (throwException)
                    throw new ArgumentNullException("Device");
                else
                {
                    System.Windows.Forms.MessageBox.Show("You didn't provide a device for the conversion");
                    return false;
                }
            }
            //be sure that if i specify multiple page outpage i added the % to the filename! 
            #endregion

            //Generate the list of the parameters i need to pass to the dll 
            string[] sArgs = GetGeneratedArgs(inputFile, outputFile, options);


            return true;
        }

        /// <summary>Remove the memory allocated</summary> 
        /// <param name="aGCHandle"></param> 
        /// <param name="gchandleArgs"></param> 
        private void ClearParameters(ref GCHandle[] aGCHandle, ref GCHandle gchandleArgs)
        {
            for (int intCounter = 0; (intCounter < aGCHandle.Length); intCounter++)
                aGCHandle[intCounter].Free();
            gchandleArgs.Free();
        }
        #region Test (code not used)
        void SaveOutputToImage(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            output.Append(e.Data);
        }
        #endregion
        #endregion

        #region Accessory Functions
        /// <summary>This function create the list of parameters to pass to the dll with parameters given directly from the program</summary> 
        /// <param name="inputFile"></param> 
        /// <param name="outputFile"></param> 
        /// <param name="otherParameters">The other parameters i could be interested</param> 
        /// <remarks>Be very Cautious using this! code provided and modified from tchu_2000</remarks> 
        /// <returns></returns> 
        private string[] GetGeneratedArgs(string inputFile, string outputFile, string otherParameters)
        {
            if (!string.IsNullOrEmpty(otherParameters))
                return GetGeneratedArgs(inputFile, outputFile, otherParameters.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            else
                return GetGeneratedArgs(inputFile, outputFile, (string[])null);
        }

        /// <summary>This function create the list of parameters to pass to the dll</summary> 
        /// <param name="inputFile">the file to convert</param> 
        /// <param name="outputFile">where to write the image</param> 
        /// <returns>the list of the arguments</returns> 
        private string[] GetGeneratedArgs(string inputFile, string outputFile, string[] presetParameters)
        {
            string[] args;
            ArrayList lstExtraArgs = new ArrayList();
            //ok if i haven't been passed a list of parameters create my own 
            if ((presetParameters == null) || (presetParameters.Length == 0))
            {
                if (_sDeviceFormat == "jpeg" && _iJPEGQuality > 0 && _iJPEGQuality < 101)
                    lstExtraArgs.Add(string.Format(GS_JpegQualityFormat, _iJPEGQuality));

                if (_iWidth > 0 && _iHeight > 0)
                    lstExtraArgs.Add(string.Format(GS_PageSizeFormat, _iWidth, _iHeight));

                if (_bFitPage) lstExtraArgs.Add(GS_FitPage);

                if (_iResolutionX > 0)
                {
                    if (_iResolutionY > 0)
                        lstExtraArgs.Add(String.Format(GS_ResolutionXYFormat, _iResolutionX, _iResolutionY));
                    else
                        lstExtraArgs.Add(String.Format(GS_ResolutionXFormat, _iResolutionX));
                }
                if (_iFirstPageToConvert > 0)
                    lstExtraArgs.Add(String.Format(GS_FirstPageFormat, _iFirstPageToConvert));
                if (_iLastPageToConvert > 0)
                    lstExtraArgs.Add(String.Format(GS_LastPageFormat, _iLastPageToConvert));
                //If i want to redirect write it to the standard output! 
                if (_bRedirectIO)
                {
                    //In this case you must also use the -q switch to prevent Ghostscript 
                    //from writing messages to standard output which become 
                    //mixed with the intended output stream.  
                    outputFile = GS_StandardOutputDevice;
                    lstExtraArgs.Add(GS_QuiteOperation);
                }
                int iFixedCount = 7;
                int iExtraArgsCount = lstExtraArgs.Count;
                args = new string[iFixedCount + lstExtraArgs.Count];
                args[1] = GS_Fixed1stParameter;//"-dNOPAUSE";//I don't want interruptions 
                args[2] = GS_Fixed2ndParameter;//"-dBATCH";//stop after 
                args[3] = GS_Fixed3rdParameter;//"-dSAFER"; 
                args[4] = string.Format(GS_DeviceFormat, _sDeviceFormat);//what kind of export format i should provide 
                //For a complete list watch here: 
                
                //Fill the remaining parameters 
                for (int i = 0; i < iExtraArgsCount; i++)
                    args[5 + i] = (string)lstExtraArgs[i];
            }
            else
            {//3 arguments MUST be added 0 (meaningless) and at the end the output and the inputfile 
                args = new string[presetParameters.Length + 3];
                //now use the parameters i receive (thanks CrucialBT to point this out!) 
                for (int i = 1; i <= presetParameters.Length; i++)
                    args[i] = presetParameters[i - 1];
            }
            args[0] = GS_FirstParameter;//this parameter have little real use 
            //Now check if i want to update to 1 file per page i have to be sure do add % to the output filename 
            if ((_didOutputToMultipleFile) && (!outputFile.Contains(GS_MultiplePageCharacter)))
            {// Thanks to Spillie to show me the error! 
                int lastDotIndex = outputFile.LastIndexOf('.');
                if (lastDotIndex > 0)
                    outputFile = outputFile.Insert(lastDotIndex, "%d");
            }
            //Ok now save them to be shown 4 debug use 
            _sParametersUsed = string.Empty;
            //Copy all the args except the 1st that is useless and the last 2 
            for (int i = 1; i <= args.Length - 2; i++)
                _sParametersUsed += " " + args[i];
            //Fill outputfile and inputfile as last 2 arguments! 
            args[args.Length - 2] = string.Format(GS_OutputFileFormat, outputFile);
            args[args.Length - 1] = string.Format("{0}", inputFile);

            _sParametersUsed += " " + string.Format(GS_OutputFileFormat, string.Format("\"{0}\"", outputFile))
            + " " + string.Format("\"{0}\"", inputFile);

            bool runComm = false;

            runComm = RunCommand("gs", _sParametersUsed, 5);

            return args;
        }

        protected virtual bool RunCommand(string szCmd, string szArgs, int wait)
        {
            if (szCmd == null) return false;
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            myproc.EnableRaisingEvents = false;
            myproc.StartInfo.FileName = szCmd;
            myproc.StartInfo.Arguments = szArgs;
            if (myproc.Start())
            {
                //Using WaitForExit( ) allows for the host program
                //to wait for the command its executing before it continues
                if (wait == 1) myproc.WaitForExit();
                else myproc.Close();
                return true;
            }
            else return false;
        }

        /// <summary> 
        /// Convert a Unicode string to a null terminated Ansi string for Ghostscript. 
        /// The result is stored in a byte array 
        /// </summary> 
        /// <param name="str">The parameter i want to convert</param> 
        /// <returns>the byte array that contain the string</returns> 
        private static byte[] StringToAnsiZ(string str)
        {
            // Later you will need to convert 
            // this byte array to a pointer with 
            // GCHandle.Alloc(XXXX, GCHandleType.Pinned) 
            // and GSHandle.AddrOfPinnedObject() 
            //int intElementCount,intCounter; 

            //This with Encoding.Default should work also with Chineese Japaneese 
            //Thanks to tchu_2000 I18N related patch 
            if (str == null) str = String.Empty;
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>Convert a Pointer to a string to a real string</summary> 
        /// <param name="strz">the pointer to the string in memory</param> 
        /// <returns>The string</returns> 
        public static string AnsiZtoString(IntPtr strz)
        {
            if (strz != IntPtr.Zero)
                return Marshal.PtrToStringAnsi(strz);
            else
                return string.Empty;
        }
        #endregion
        #region Menage Standard Input & Standard Output
        public int gsdll_stdin(IntPtr intGSInstanceHandle, IntPtr strz, int intBytes)
        {
            // This is dumb code that reads one byte at a time 
            // Ghostscript doesn't mind this, it is just very slow 
            if (intBytes == 0)
                return 0;
            else
            {
                int ich = Console.Read();
                if (ich == -1)
                    return 0; // EOF 
                else
                {
                    byte bch = (byte)ich;
                    GCHandle gcByte = GCHandle.Alloc(bch, GCHandleType.Pinned);
                    IntPtr ptrByte = gcByte.AddrOfPinnedObject();
                    CopyMemory(strz, ptrByte, 1);
                    ptrByte = IntPtr.Zero;
                    gcByte.Free();
                    return 1;
                }
            }
        }

        public int gsdll_stdout(IntPtr intGSInstanceHandle, IntPtr strz, int intBytes)
        {
            if (intBytes > 0)
            {
                Console.Write(Marshal.PtrToStringAnsi(strz));
            }
            return 0;
        }

        public int gsdll_stderr(IntPtr intGSInstanceHandle, IntPtr strz, int intBytes)
        {
            //return gsdll_stdout(intGSInstanceHandle, strz, intBytes); 
            Console.Write(Marshal.PtrToStringAnsi(strz));
            return intBytes;
        }
        #endregion
        #region Menage Revision
        public GhostScriptRevision GetRevision()
        {
            // Check revision number of Ghostscript 
            int intReturn;
            GS_Revision udtGSRevInfo = new GS_Revision();
            GhostScriptRevision output;
            GCHandle gcRevision;
            gcRevision = GCHandle.Alloc(udtGSRevInfo, GCHandleType.Pinned);
            intReturn = gsapi_revision(ref udtGSRevInfo, 16);
            output.intRevision = udtGSRevInfo.intRevision;
            output.intRevisionDate = udtGSRevInfo.intRevisionDate;
            output.ProductInformation = AnsiZtoString(udtGSRevInfo.strProduct);
            output.CopyrightInformations = AnsiZtoString(udtGSRevInfo.strCopyright);
            gcRevision.Free();
            return output;
        }
        #endregion
    }

    /// <summary>Delegate used by Ghostscript to perform I/O operations</summary> 
    /// <param name="handle"></param> 
    /// <param name="strptr"></param> 
    /// <param name="count"></param> 
    /// <returns></returns> 
    public delegate int StdioCallBack(IntPtr handle, IntPtr strptr, int count);
    /// <summary>This struct is filled with the information of the version of this ghostscript</summary> 
    /// <remarks>Have the layout defined cuz i will fill it with a kernel copy memory</remarks> 
    [StructLayout(LayoutKind.Sequential)]
    struct GS_Revision
    {
        public IntPtr strProduct;
        public IntPtr strCopyright;
        public int intRevision;
        public int intRevisionDate;
    }

    public struct GhostScriptRevision
    {
        public string ProductInformation;
        public string CopyrightInformations;
        public int intRevision;
        public int intRevisionDate;
    }
}
