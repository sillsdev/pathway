namespace SIL.PublishingSolution
{
    public class ClassAttribute
    {
        /// <summary>
        /// h2[level] { 
        ///  color:red; 
        ///}
        ///h2[level='1'] { 
        ///  color:green; 
        ///}
        /// For storing above class attributes
        /// </summary>
        public string Name;
        public string AttributeValue;
        public string AttributeSeperator;
        /// <summary>
        /// To set the Attribute
        /// </summary>
        /// <param name="attribName">Attribute Name</param>
        public void SetAttribute(string attribName)
        {
            Name = attribName.Replace("\'", "");
            AttributeValue = string.Empty;
            AttributeSeperator = string.Empty;
        }
        /// <summary>
        /// To set the Attribute
        /// </summary>
        /// <param name="attribName">Attribute Name</param>
        /// <param name="attribSeperator">Attribute Seperator</param>
        /// <param name="attribValue">Attribute Value</param>
        public void SetAttribute(string attribName, string attribSeperator, string attribValue)
        {
            Name = attribName.Replace("\'", "");
            AttributeValue = attribValue.Replace("\'", "");
            AttributeSeperator = attribSeperator.Replace("\'", "");
        }
    }
}