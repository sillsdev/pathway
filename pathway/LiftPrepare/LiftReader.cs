// --------------------------------------------------------------------------------------------
// <copyright file="LiftReader.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Xml;

namespace SIL.PublishingSolution
{
    public class LiftReader : XmlTextReader
    {
        public LiftReader(String liftFile) : base(liftFile)
        {
            if (!isValidLift(liftFile))
            {
                throw new Environ.InvalidLiftException();
            }
            
        }

        private bool isValidLift(String liftFile)
        {
            var validatingReaderSettings = new XmlReaderSettings();
            validatingReaderSettings.Schemas.Add(Environ.liftXMLSchema);
            validatingReaderSettings.Schemas.Add(null,Environ.PathToLiftSchema);
            validatingReaderSettings.ValidationType = ValidationType.Schema;
            validatingReaderSettings.ValidationEventHandler += Environ.liftValidationEventHandler;
            var validatingReader = XmlReader.Create(liftFile,validatingReaderSettings);
            try
            {
                while (validatingReader.Read())
                {
                    //Simply need to read through the entire file.
                }
                return true;
            }
            catch (Environ.InvalidLiftException)
            {
                return false;
            }
        }
    }
}