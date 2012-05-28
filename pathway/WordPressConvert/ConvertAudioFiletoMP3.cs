// --------------------------------------------------------------------------------------------
// <copyright file="WordPressConvert.cs" from='2010' to='2010' company='SIL International'>
//      Copyright © 2010, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Create Wordpress blog 
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ConvertAudioFiletoMP3
    {
        #region public class variables

        public PublicationInformation projInfo;

        #endregion public class variables

        #region Methods

        public void ConvertWavtoMP3Format(string audioFilePath)
        {
            string[] directoryLocalfiles;
            directoryLocalfiles = Directory.GetFiles(audioFilePath);

            string getApplicationPath = Common.GetApplicationPath();
            getApplicationPath = Path.Combine(getApplicationPath, "Wordpress");
            string lameEXE = Path.Combine(getApplicationPath, "lame");
            string lameArgs = "-b 128";
            foreach (string fileName in directoryLocalfiles)
            {
                if (File.Exists(fileName))
                if (fileName.IndexOf(".wav") > 0)
                {
                    string wavFile = "\"" + fileName + "\"";
                    string mp3File = "\"" + fileName.Replace(".wav", ".mp3") + "\"";
                    Common.RunCommand(lameEXE, string.Format("{0} {1} {2}", lameArgs, wavFile, mp3File), 1);
                    File.Delete(fileName);
                }
            }
        }

        #endregion

    }
}
