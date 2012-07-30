using System;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportXhtmlToSqlData
    {
        #region public class variables

        public PublicationInformation _projInfo;
        public string MysqlDataFileName = string.Empty;

        #endregion public class variables

        #region Private class variables

        private int _nodeCount;
        private TextWriter _textWriter;

        int _intPost = 20;
        int _intTerm = 20;
        int _relationId = 20;
        int _intSearch = 20;

        int _nCounter;
        int _vCounter;
        int _advCounter;
        int _adjCounter;
        int _conjCounter;
        int _nsCounter;
        int _vsCounter;
        int _numCounter;
        int _proCounter;

        private string _postValue = string.Empty;
        private string _termValue = string.Empty;
        private string _taxonomyValue = string.Empty;
        private string _relationshipValue = string.Empty;
        private string _searchValue = string.Empty;

        #endregion Private class variables

        #region Methods

        public void XhtmlToBlog()
        {
            ExportWordpress();
        }

        private void ExportWordpress()
        {
            InsertMetaTags();
            InsertLetHead();
            InsertTexonomy();
            InsertTable();
        }

        private void InsertTexonomy()
        {
            InsertPartOfSpeech("n", _nCounter);
            InsertPartOfSpeech("v", _vCounter);
            InsertPartOfSpeech("adv", _advCounter);
            InsertPartOfSpeech("adj", _adjCounter);
            InsertPartOfSpeech("conj", _conjCounter);
            InsertPartOfSpeech("ns", _nsCounter);
            InsertPartOfSpeech("vs", _vsCounter);
            InsertPartOfSpeech("num", _numCounter);
            InsertPartOfSpeech("pro", _proCounter);
        }

        private void InsertPartOfSpeech(string PartOfSpeech, int Counter)
        {
            if (Counter > 0)
            {
                _termValue += "(" + _intTerm + ",'" + PartOfSpeech + "','" + PartOfSpeech + "',0)," + "\n";
                _taxonomyValue += "(" + _intTerm + "," + _intTerm + ",'sil_parts_of_speech','',0," + Counter + ")," + "\n";
                _intTerm += 1;
            }
        }

        private string SetSemiColon(string Query)
        {
            if(Query.Trim() == string.Empty)
            {
                return ";\n";
            }

            Query = Query.Substring(0, Query.Length - 2);
            Query += ";\n";
            return Query;
        }

        private void InsertTable()
        {
            string postTable = @"DELETE FROM `wordpress`.`wp_posts` WHERE `wp_posts`.`post_type` = ""post"";" + "\n";
            postTable += @"INSERT INTO `wp_posts` (`ID`, `post_author`, `post_date`, `post_date_gmt`, `post_content`, `post_title`, `post_excerpt`, `post_status`, `comment_status`, `ping_status`, `post_password`, `post_name`, `to_ping`, `pinged`, `post_modified`, `post_modified_gmt`, `post_content_filtered`, `post_parent`, `guid`, `menu_order`, `post_type`, `post_mime_type`, `comment_count`) VALUES " + "\n";
            postTable += SetSemiColon(_postValue);

            string termTable = @"DELETE FROM `wordpress`.`wp_terms` WHERE `wp_terms`.`term_id` > 13;" + "\n";
            termTable += @"INSERT INTO `wp_terms` (`term_id`, `name`, `slug`, `term_group`) VALUES " + "\n";
            termTable += "(14,'Main','main',0)," + "\n";
            termTable += "(15,'Language','Language',0)," + "\n";
            termTable += SetSemiColon(_termValue);

            string texonomyTable = @"DELETE FROM `wordpress`.`wp_term_taxonomy` WHERE `wp_term_taxonomy`.`term_id` > 15;" + "\n";
            texonomyTable += @"INSERT INTO `wp_term_taxonomy` (`term_taxonomy_id`, `term_id`, `taxonomy`, `description`, `parent`, `count`) VALUES " + "\n";
            texonomyTable += SetSemiColon(_taxonomyValue);

            string relationshipTable = @"DELETE FROM `wordpress`.`wp_term_relationships` WHERE `wp_term_relationships`.`term_taxonomy_id` > 15;" + "\n";
            relationshipTable += @"INSERT INTO `wp_term_relationships` (`object_id`, `term_taxonomy_id`, `term_order`) VALUES " + "\n";
            relationshipTable += SetSemiColon(_relationshipValue);

            string searchTable = @"DELETE FROM `wordpress`.`sil_multilingual_search` where `post_id` > 0;" + "\n";
            searchTable += @"INSERT INTO `sil_multilingual_search` (`post_id`, `language_code`, `relevance`, `search_strings`) VALUES " + "\n";
            searchTable += SetSemiColon(_searchValue);

            //_textWriter = new StreamWriter(_projInfo.ProjectPath + @"\" + MysqlDataFileName);
            _textWriter = new StreamWriter(Common.PathCombine(_projInfo.ProjectPath, MysqlDataFileName));
            _textWriter.WriteLine(postTable);
            _textWriter.WriteLine(termTable);
            _textWriter.WriteLine(texonomyTable);
            _textWriter.WriteLine(relationshipTable);
            _textWriter.WriteLine(searchTable);
            _textWriter.Close();
        }

        private void InsertLetHead()
        {
            string xhtmlFile = _projInfo.DefaultXhtmlFileWithPath;
            string headword = string.Empty;
            string letHead = string.Empty;
            string strPost = string.Empty;
            int letDataCount = 0;
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            xmlDocument.Load(xhtmlFile);
            XmlNodeList xmlNL = xmlDocument.GetElementsByTagName("div");

            foreach (XmlNode node in xmlNL)
            {
                if (node == null || node.Attributes["class"] == null)
                    continue;
                string className = node.Attributes["class"].Value;
                if (className.ToLower() == "entry")
                {
                    _nodeCount++;
                }
            }

            foreach (XmlNode node in xmlNL)
            {
                if (node == null || node.Attributes["class"] == null)
                    continue;
                string className = node.Attributes["class"].Value;

                if (className.ToLower() == "letter")
                {
                    if (headword.Trim().Length > 0)
                    {
                        InsertLetter(letHead, headword, letDataCount);
                        letDataCount = 0;
                    }
                    letHead = node.FirstChild.InnerText.Trim();
                }
                else if (className.ToLower() == "entry" || className.ToLower() == "minorentry")
                {
                    XmlDocument xdocEntry = new XmlDocument { XmlResolver = null };
                    xdocEntry.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                        + node.OuterXml);
                    XmlNodeList spanNodeList = xdocEntry.GetElementsByTagName("span");
                    foreach (XmlNode spanNode in spanNodeList)
                    {
                        if (spanNode.Attributes["class"] != null)
                        {
                            if (spanNode.Attributes["class"].Value.ToLower() == "headword" || spanNode.Attributes["class"].Value.ToLower() == "headword-minor")
                            {
                                headword = spanNode.InnerText.Trim();
                                InsertPosts(node.OuterXml.Trim().Replace("'", "''"), headword);
                                _intPost += 1;
                                letDataCount += 1;
                            }

                            else if (spanNode.Attributes["class"].Value.ToLower() == "partofspeech")
                            {
                                SetPartOfSpeechCount(spanNode.InnerText.Trim().ToLower());
                            }
                            else if (spanNode.Attributes["class"].Value.ToLower() == "definition")
                            {
                                _intSearch += 1;
                                _searchValue += "(" + (_intSearch) + ", 'en', 50,'" + spanNode.InnerText.Trim().Replace("'", "''") + "')," + "\n";
                                _searchValue += "(" + (_intSearch) + ", 'ggo-Telu-IN', 70,'" + headword + "')," + "\n";
                            }
                        }
                    }
                }
            }
            InsertLetter(letHead, headword, letDataCount);
            xmlDocument.Save(xhtmlFile);
        }

        private void InsertLetter(string letHead, string headword, int letDataCount)
        {
            _termValue += "(" + _intTerm + ",'" + letHead + " - " + letHead + "','" + letHead + " - " + letHead + "',0)," + "\n";
            _taxonomyValue += "(" + _intTerm + "," + _intTerm + ",'category','',0,999999)," + "\n";

            _intTerm += 1;

            _termValue += "(" + _intTerm + ",'" + headword + " - " + headword + "','" + headword + " - " + headword + "',0)," + "\n";
            _taxonomyValue += "(" + _intTerm + "," + _intTerm + ",'category',''," + (_intTerm - 1) + "," + letDataCount + ")," + "\n";

            for (int counter = 0; counter < letDataCount; counter++)
            {
                _relationshipValue += "(" + (_relationId + counter) + "," + (_intTerm - 1) + ",0)," + "\n";
                _relationshipValue += "(" + (_relationId + counter) + "," + (_intTerm) + ",0)," + "\n";

            }
            _relationId += letDataCount;
            _intTerm += 1;
        }

        private void SetPartOfSpeechCount(string partOfSpeech)
        {
            if (partOfSpeech == "n")
                _nCounter++;
            else if (partOfSpeech == "v")
                _vCounter++;
            else if (partOfSpeech == "adv")
                _advCounter++;
            else if (partOfSpeech == "adj")
                _adjCounter++;
            else if (partOfSpeech == "conj")
                _conjCounter++;
            else if (partOfSpeech == "ns")
                _nsCounter++;
            else if (partOfSpeech == "vs")
                _vsCounter++;
            else if (partOfSpeech == "num")
                _numCounter++;
            else if (partOfSpeech == "pro")
                _proCounter++;
        }

        private void InsertPosts(string insertValue, string headword)
        {
            int ID = _intPost;
            int post_author = 0;
            string todayDate = DateTime.Now.ToString("u");
            string post_date = todayDate;
            string post_date_gmt = todayDate;

            string post_content = SetArrow() + insertValue;

            string post_title = headword;
            string post_excerpt = headword;
            string post_status = "publish";
            string comment_status = "open";
            string ping_status = "open";
            string post_password = "";
            string post_name = headword + ID + ".htm";
            string to_ping = "";
            string pinged = "";
            string post_modified = todayDate;
            string post_modified_gmt = todayDate;
            string post_content_filtered = "";
            int post_parent = 0;
            string guid = "http://localhost/wordpress/?p=" + ID;
            int menu_order = 0;
            string post_type = "post";
            string post_mime_type = "";
            int comment_count = 0;
            _postValue += "(" + ID + "," + post_author + ",'" + post_date + "','" + post_date_gmt + "','" + post_content.Replace(" "," ") + "','" +
                post_title + "','" + post_excerpt + "','" + post_status + "','" + comment_status + "','" + ping_status + "','" + post_password +
                "','" + post_name + "','" + to_ping + "','" + pinged + "','" + post_modified + "','" + post_modified_gmt + "','" +
                post_content_filtered + "'," + post_parent + ",'" + guid + "'," + menu_order + ",'" + post_type + "','" +
                post_mime_type + "'," + comment_count + ")," + "\n";
        }

        private void InsertMetaTags()
        {
            string metaName = string.Empty;
            string metaContent = string.Empty;
            string lastAttribute = string.Empty;

            string xhtmlFile = _projInfo.DefaultXhtmlFileWithPath;
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            xmlDocument.Load(xhtmlFile);
            XmlNodeList xmlNL = xmlDocument.GetElementsByTagName("meta");

            foreach (XmlNode node in xmlNL)
            {
                if (node == null || node.Attributes["name"] == null || node.Attributes["content"] == null)
                    continue;

                metaName = node.Attributes["name"].Value;
                metaContent = node.Attributes["content"].Value;

                if (lastAttribute != metaName && metaName.ToLower() != "linkedfilesrootdir" && metaName.ToLower() != "filename")
                {
                    _termValue += "(" + _intTerm + ",'" + metaContent + "','" + metaName + "',0)," + "\n";
                    _taxonomyValue += "(" + _intTerm + "," + _intTerm + ",'sil_writing_systems','',0,999999)," + "\n";
                    _intTerm += 1;
                }

                lastAttribute = metaName;
            }
        }

        private string SetArrow()
        {
            string strArrow = string.Empty;
            if (_nodeCount == 1) //If only one entry, Arrow is not needed
                return strArrow;
            else if (_intPost == _nodeCount + 19)//Left Only for Last Entry
                strArrow = "<table style=`border:medium none white;width:100%;`><tr><td><a href=?p=" + (_intPost - 1) + "><img src=http://localhost/wordpress/wp-content/icons/ArrowLeft.png width=`32` height=`32` border=`0` alt=`&lt;&#x2014;` /></a></td><td style=`text-align:right;`></td></tr></table>";
            else if (_intPost == 20)//Right Only for First Entry
                strArrow = "<table style=`border:medium none white;width:100%;`><tr><td><a href=?p=" + (_intPost + 1) + "><img src=http://localhost/wordpress/wp-content/icons/ArrowRight.png width=`32` height=`32` border=`0` alt=`&lt;&#x2014;` /></a></td></tr></table>";
            else//Left & Right for All other than First and Last Entries
                strArrow = "<table style=`border:medium none white;width:100%;`><tr><td><a href=?p=" + (_intPost - 1) + "><img src=http://localhost/wordpress/wp-content/icons/ArrowLeft.png width=`32` height=`32` border=`0` alt=`&lt;&#x2014;` /></a></td><td style=`text-align:right;`><a href=?p=" + (_intPost + 1) + "><img src=http://localhost/wordpress/wp-content/icons/ArrowRight.png width=`32` height=`32` border=`0` alt=`&lt;&#x2014;` /></a></td></tr></table>";
            return strArrow;
        }

        #endregion

    }
}
