#region // Copyright (C) 2014, SIL International. All Rights Reserved.
// --------------------------------------------------------------------------------------------
// <copyright file="MySwordSqlite.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
#endregion

using System;
using System.Data;
using System.Collections.Generic;
using Devart.Data.SQLite;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class MySwordSqlite
    {
        private readonly XmlDocument _vrs = new XmlDocument();
        private readonly Regex _vrsPat = new Regex(@"[0-9]+\:([0-9]+)");
        protected readonly Dictionary<string, string> DbParams = new Dictionary<string, string>();
        private StreamReader _sr;

        public MySwordSqlite()
        {
            var executingAssembly = Assembly.GetExecutingAssembly().Location;
            var executingPath = Path.GetDirectoryName(executingAssembly);
            Debug.Assert(executingPath != null);
            var vrsFullName = Path.Combine(executingPath, "vrs.xml");
            if (!File.Exists(vrsFullName))
            {
                return;
            }
            _vrs.Load(vrsFullName);
        }

	    public MySwordSqlite(string vrsFullName)
	    {
		    Debug.Assert(File.Exists(vrsFullName));
		    var sr = XmlReader.Create(vrsFullName);
			_vrs.Load(sr);
			sr.Close();
	    }

        public void Execute(string inName)
        {
            if (_vrs.DocumentElement == null || string.IsNullOrEmpty(inName) || !File.Exists(inName))
            {
                return;
            }
            var filename = GetFilename(inName);
            var updConn = NewMyBible(filename);
            updConn.Open();
            _sr = new StreamReader(inName);
            var ot = inName.EndsWith(".ont");
            UpdateScripture(updConn, ot);
            LoadDbParams();
            PostDbParams(inName, updConn);
            _sr.Close();
            updConn.Dispose();
        }

        string GetFilename(string inName)
        {
            if (inName.EndsWith("x"))
            {
                throw new ArgumentException("Unable to process encrypted files");
            }
            var nameOnly = Path.GetFileNameWithoutExtension(inName);
            var filename = nameOnly + ".bbl.mybible";
            var inPath = Path.GetDirectoryName(inName);
            if (!string.IsNullOrEmpty(inPath))
            {
                filename = Path.Combine(inPath, filename);
            }
            return filename;
        }

        private SQLiteConnection NewMyBible(string filename)
        {
            var basePath = Common.FromRegistry("TheWord");
            var templateFilename = Path.Combine(basePath, "biblebase.sqlite");
            File.Copy(templateFilename, filename, true);
            return new SQLiteConnection("Data Source=" + filename + ";Version=3;");
        }

        private void UpdateScripture(SQLiteConnection updConn, bool ot)
        {
            var books = _vrs.SelectNodes("//bk");
            Debug.Assert(books != null);
            for (var bkn = ot ? 0 : 39; bkn < 66; )
            {
                var bk = books[bkn];
                bkn += 1;
                var matches = _vrsPat.Matches(bk.InnerText);
                for (var chn = 1; chn <= matches.Count; chn++)
                {
                    var trx = updConn.BeginTransaction();
                    var vrsCount = int.Parse(matches[chn - 1].Groups[1].Value);
                    for (var vrsn = 1; vrsn <= vrsCount; vrsn++)
                    {
                        var line = _sr.ReadLine();
                        Debug.Assert(line != null);
                        var cmd = updConn.CreateCommand();
                        line = line.Replace("\"", "\"\"");
                        cmd.CommandText =
	                        string.Format(
		                        @"update Bible SET Scripture = ""{0}"" WHERE Book = {1} AND Chapter = {2} AND Verse = {3};",
		                        line, bkn, chn, vrsn);
                        int result = cmd.ExecuteNonQuery();
                        Debug.Assert(result == 1);
                    }
                    trx.Commit();
                }
            }
        }

        private void LoadDbParams()
        {
            while (true)
            {
                var pLine = _sr.ReadLine();
                if (string.IsNullOrEmpty(pLine))
                {
                    break;
                }
                var eqPos = pLine.IndexOf('=');
                if (eqPos == -1)
                {
                    throw new SyntaxErrorException("= expected in Parameter setting");
                }
                var vName = pLine.Substring(0, eqPos).Trim();
                var vValue = pLine.Substring(eqPos + 1).TrimStart();
                while (vValue.EndsWith("\\"))
                {
                    pLine = _sr.ReadLine();
                    vValue = vValue.Substring(0, vValue.Length - 1) + "\n" + pLine;
                }
                DbParams[vName] = vValue;
            }
        }

        private void PostDbParams(string inName, SQLiteConnection updConn)
        {
            DbParams["version"] = DbParams["version.major"] + "." + DbParams["version.minor"];
            DbParams["ot"] = inName.EndsWith(".ont") ? "1" : "0";
            DbParams["nt"] = "1";
            DbParams["strong"] = "0";
            if (!DbParams.ContainsKey("rtl"))
            {
                DbParams["rtl"] = "0";
            }
            var detailTrx = updConn.BeginTransaction();
            const string map = "Description=description,Abbreviation=short.title,Comments=about,Version=version,VersionDate=version.date,PublishDate=publish.date,RightToLeft=rtl,OT=ot,NT=nt,Strong=strong";
            var mapPat = new Regex(@"([A-Za-z]+)=([\.a-z]+)");
            foreach (Match match in mapPat.Matches(map))
            {
                var key = match.Groups[2].Value;
                if (DbParams.ContainsKey(key))
                {
                    var detailCmd = updConn.CreateCommand();
                    var newValue = string.Format(@"""{0}""", DbParams[key]);
                    newValue = ParseDateIfNecessary(key, newValue);
                    detailCmd.CommandText = string.Format("update Details SET {0} = {1}", match.Groups[1].Value,
	                    newValue);
                    int result = detailCmd.ExecuteNonQuery();
                    Debug.Assert(result == 1, match.Groups[0].Value);
                }
            }
            detailTrx.Commit();
        }

        private readonly Regex _datePat = new Regex(@"([0-9]+)[\./-]([0-9]+)[\./-]([0-9]+)");
        private readonly Regex _yearPat = new Regex(@"([0-9]+)");
        protected string ParseDateIfNecessary(string key, string newValue)
        {
            try
            {
                if (key.EndsWith(".date"))
                {
                    var monthValue = 1;
                    var dayValue = 1;
                    var dateMatch = _datePat.Match(DbParams[key]);
                    if (dateMatch.Success)
                    {
                        monthValue = int.Parse(dateMatch.Groups[2].Value);
                        dayValue = int.Parse(dateMatch.Groups[3].Value);
                    }
                    else
                    {
                        dateMatch = _yearPat.Match(DbParams[key]);
                    }
                    newValue = string.Format(@"date('{0}-{1:00}-{2:00}')", dateMatch.Groups[1].Value, monthValue,
	                    dayValue);
                }
            }
            catch (Exception)
            {
                throw new FormatException(string.Format("error parsing date {0} with value {1}", key, DbParams[key]));
            }
            return newValue;
        }
    }
}
