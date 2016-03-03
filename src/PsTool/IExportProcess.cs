// --------------------------------------------------------------------------------------------
// <copyright file="IExportProcess.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// IExportProcess allows output conversion assemblies to be dynamically loaded.
// </remarks>
// --------------------------------------------------------------------------------------------


namespace SIL.Tool
{
    public interface IExportProcess
    {
        /// <summary>Gets Output format (Open Office, PDF, INX, TeX, HTM, PDB, etc.)</summary>
        string ExportType { get; }

        /// <summary>
        /// Returns True if the converter can handle the input type given
        /// </summary>
        /// <param name="inputDataType">scripture or dictionary</param>
        /// <returns>true if the converter knows how to handle the type</returns>
        bool Handle(string inputDataType);

        /// <summary>
        /// convert the project data to the format of the converter
        /// </summary>
        /// <param name="projInfo">all necessary info about project</param>
        /// <returns>true if conversion is successful</returns>
        bool Export(PublicationInformation projInfo);
    }
}
