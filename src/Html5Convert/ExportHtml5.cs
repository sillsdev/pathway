// --------------------------------------------------------------------------------------
// <copyright file="ExportHtml5.cs" from='2017' to='2017' company='SIL International'>
//      Copyright ( c ) 2017, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// Html5 is based on Epub export
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using epubConvert;
using SIL.Tool;

namespace SIL.PublishingSolution
{
	public class ExportHtml5 : Exportepub, IExportProcess
	{
		private XslCompiledTransform _toc2Html5;

		// interface methods
		public string ExportType
		{
			get
			{
				return "Browser (HTML5)";
			}
		}


		/// <summary>
		/// Returns what input data types this export process handles. The epub exporter
		/// currently handles scripture and dictionary data types.
		/// </summary>
		/// <param name="inputDataType">input data type to test</param>
		/// <returns>true if this export process handles the specified data type</returns>
		public bool Handle(string inputDataType)
		{
			var returnValue = inputDataType.ToLower() == "dictionary" || inputDataType.ToLower() == "scripture";
			return returnValue;
		}

		/// <summary>
		/// Entry point for epub converter
		/// </summary>
		/// <param name="projInfo">values passed including xhtml and css names</param>
		/// <returns>true if succeeds</returns>
		public bool Export(PublicationInformation projInfo)
		{
			Common.SetupLocalization();
			if (projInfo == null)
				return false;
			const bool success = true;
			#region Set up progress reporting
#if (TIME_IT)
						DateTime dt1 = DateTime.Now;    // time this thing
#endif
			var myCursor = Common.UseWaitCursor();
			var curdir = Environment.CurrentDirectory;
			var inProcess = Common.SetupProgressReporting(20, "Export " + ExportType);
			#endregion Set up progress reporting
			PreExportProcess preProcessor;
			EpubToc epubToc;
			var bookId = SetupConversion(projInfo, inProcess, out preProcessor, out epubToc);
			_toc2Html5 = LoadToc2Html5Xslt();

			string[] langArray;
			var outputFolder = PreprocessXhtml(projInfo, inProcess, preProcessor, out langArray);

			string mergedCss;
			string defaultCss;
			string tempCssFile;
			var tempFolder = ProcessingCss(projInfo, inProcess, preProcessor, out mergedCss, out defaultCss, out tempCssFile);

			FixIssuesWithFlexXhtml(projInfo, preProcessor, langArray, inProcess);

			AddNavitation(inProcess, preProcessor);

			var frontMatter = AddFrontMatter(inProcess, preProcessor, tempFolder);

			List<string> splitFiles;
			Common.SplitEveryEntry = true;
			var htmlFiles = SplitIntoSections(projInfo, inProcess, frontMatter, preProcessor, defaultCss, langArray, out splitFiles);

			var contentFolder = CreateContentStructure(projInfo, inProcess, tempFolder);

			AddEndNotesIfNecessary(inProcess, contentFolder, preProcessor, splitFiles);
			if (!FontEmbedding(projInfo, inProcess, langArray, mergedCss, contentFolder, curdir, myCursor)) return false;

			CopyContentToEpubFolder(inProcess, mergedCss, defaultCss, htmlFiles, contentFolder);

			InsertChapterLinks(inProcess, contentFolder);

			UpdateHyperlinks(inProcess, contentFolder);

			ProcessImages(inProcess, tempFolder, contentFolder);

			CreateEpubManifest(projInfo, inProcess, contentFolder, bookId, epubToc, tempCssFile);

			var epub3Path = MakeCopyOfEpub2(inProcess, projInfo);

			SemanticDomainIndex(inProcess, contentFolder, epub3Path);

			var html5Folder = CreateHtml5(projInfo, inProcess, epub3Path);

			var outputPathWithFileNameHtml5 = PackageHtml5(projInfo, inProcess, html5Folder);

			CleanUp(inProcess, outputPathWithFileNameHtml5);

			CreateArchiveSubmission(projInfo, inProcess, epub3Path);

			FinalCleanUp(inProcess, outputPathWithFileNameHtml5);

			#region Close Reporting
			inProcess.Close();

			Environment.CurrentDirectory = curdir;
			Cursor.Current = myCursor;
			#endregion Close Reporting
			if (Common.IsUnixOS())
			{
				SubProcess.Run("", "nautilus", Common.HandleSpaceinLinuxPath(projInfo.ProjectPath), false);
			}
			else
			{
				SubProcess.Run(projInfo.ProjectPath, "explorer.exe", projInfo.ProjectPath, false);
			}

			return success;
		}

		private void SemanticDomainIndex(InProcess inProcess, string contentFolder, string html5Folder)
		{
			var xDoc = Common.DeclareXMLDocument(false);
			var domain = new SortedDictionary<string, string>();
			var entries = new Dictionary<string, List<string>>();
			var files = new DirectoryInfo(contentFolder).GetFiles("PartFile*.xhtml");
			inProcess.AddToMaximum(files.Length);
			foreach (FileInfo fileInfo in files)
			{
				var sr = new StreamReader(fileInfo.FullName);
				xDoc.Load(sr);
				sr.Close();
				// ReSharper disable once PossibleNullReferenceException
				foreach (XmlElement node in xDoc.SelectNodes("//*[starts-with(@class,'semanticdomain')]"))
				{
					inProcess.PerformStep();
					var firstSibling = node.NextSibling as XmlElement;
					if (firstSibling == null || !firstSibling.GetAttribute("class").StartsWith("abbr")) continue;
					var domainKey = firstSibling.InnerText;
					var domainMenuItem = new StringBuilder();
					var haveAbbr = false;
					var haveName = false;
					while (firstSibling != null)
					{
						var firstClass = firstSibling.GetAttribute("class");
						if (firstClass.StartsWith("abbr"))
						{
							if (haveAbbr) break;
							haveAbbr = true;
						}
						else if (firstClass.StartsWith("name"))
						{
							if (haveName) break;
							haveName = true;
						}
						else if (!firstClass.StartsWith("semanticdomain") || haveName) break;
						domainMenuItem.Append(firstSibling.InnerText);
						firstSibling = firstSibling.NextSibling as XmlElement;
					}
					domain[domainKey] = domainMenuItem.ToString();
					AddParts(domainKey, domain);
					var entryNode = node.SelectSingleNode("ancestor::*[starts-with(@class,'entry') or starts-with(@class,'minorentry') or starts-with(@class,'reversalindexentry')]");
					var anchor = xDoc.CreateElement("a");
					anchor.SetAttribute("href", (entryNode.FirstChild as XmlElement).GetAttribute("href").Replace(".xhtml", ".html"));
					anchor.SetAttribute("lang", (entryNode.FirstChild.FirstChild as XmlElement).GetAttribute("lang"));
					anchor.InnerText = (entryNode.FirstChild.FirstChild as XmlElement).InnerText;
					if (entries.ContainsKey(domainKey))
					{
						entries[domainKey].Add(anchor.OuterXml);
					}
					else
					{
						entries[domainKey] = new List<string> {anchor.OuterXml};
					}
				}
				Debug.Assert(xDoc.DocumentElement != null);
				xDoc.DocumentElement.RemoveAll();
			}
			xDoc.LoadXml(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml""><head><title>sindex</title></head><body></body></html>");
			var curNode = xDoc.SelectSingleNode("//*[local-name()='body']");
			var level = 1;
			XmlElement saveLevel = null;
			foreach (var domainKey in domain.Keys)
			{
				var parts = domainKey.Split('.');
				while (level > parts.Length)
				{
					curNode = curNode.ParentNode.ParentNode;
					level -= 1;
				}
				if (parts.Length > level)
				{
					level += 1;
					if (saveLevel == null)
					{
						curNode = curNode.LastChild;
						var nextLevel = CreateElement(xDoc, "ul");
						nextLevel.SetAttribute("class", "nav nav-second-level");
						curNode.AppendChild(nextLevel);
						curNode = curNode.LastChild;
					}
					else
					{
						curNode = saveLevel;
					}
				}
				saveLevel = null;
				var header = CreateElement(xDoc, "li");
				var headAnchor = CreateElement(xDoc, "a");
				headAnchor.SetAttribute("class", "s");
				headAnchor.InnerXml = domain[domainKey];
				var arrowSpan = CreateElement(xDoc, "span");
				arrowSpan.SetAttribute("class", "fa arrow");
				headAnchor.AppendChild(arrowSpan);
				header.AppendChild(headAnchor);
				curNode.AppendChild(header);
				if (entries.ContainsKey(domainKey))
				{
					var group = CreateElement(xDoc, "ul");
					group.SetAttribute("class", "nav nav-third-level");
					foreach (var entry in entries[domainKey])
					{
						var itemNode = CreateElement(xDoc, "li");
						itemNode.InnerXml = entry;
						group.AppendChild(itemNode);
					}
					header.AppendChild(group);
					saveLevel = group;
				}
			}
			var sw = new StreamWriter(Path.Combine(html5Folder, "bootstrapSem.html"));
			xDoc.Save(sw);
			sw.Close();
			Debug.Assert(xDoc.DocumentElement != null);
			xDoc.DocumentElement.RemoveAll();
		}

		private static XmlElement CreateElement(XmlDocument xDoc, string name)
		{
			return xDoc.CreateElement(name, "http://www.w3.org/1999/xhtml");
		}

		private void AddParts(string domainKey, SortedDictionary<string, string> domain)
		{
			var parts = domainKey.Split('.');
			var partial = new StringBuilder();
			for (var i = 0; i < parts.Length - 1; i += 1)
			{
				partial.Append(parts[i] + ".");
				var partialString = partial.ToString();
				partialString = partialString.Substring(0, partialString.Length - 1);
				if (domain.ContainsKey(partialString)) continue;
				domain[partialString] = string.Format(@"<span class=""abbreviation"" lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">{0}</span>", partialString);
			}
		}

		protected string CreateHtml5(PublicationInformation projInfo, InProcess inProcess, string epub3Path)
		{
			inProcess.SetStatus("Copy html files");
			string htmlFolderPath = Common.PathCombine(Path.GetDirectoryName(epub3Path), "HTML5");
			string oebpsFolderPath = Common.PathCombine(epub3Path, "OEBPS");
			var bootstrapToc = Common.PathCombine(epub3Path, "bootstrapToc.html");
			File.Copy(Path.Combine(oebpsFolderPath, "toc.ncx"), bootstrapToc);
			Common.ApplyXslt(bootstrapToc, _toc2Html5);

			Common.CustomizedFileCopy(inProcess, oebpsFolderPath, htmlFolderPath, "content.opf,toc.xhtml,toc.ncx,File3TOC00000_.html,File2TOC00000_.html,File1TOC00000_.html,File0TOC00000_.html");
			var avFolder = Path.Combine(projInfo.ProjectPath, "AudioVisual");
			if (Directory.Exists(avFolder))
			{
				FolderTree.Copy(avFolder, Path.Combine(htmlFolderPath, "pages", "AudioVisual"));
			}
			File.Delete(bootstrapToc);
			return htmlFolderPath;
		}

		protected string PackageHtml5(PublicationInformation projInfo, InProcess inProcess, string path)
		{
			inProcess.SetStatus("Packaging for Html5");

			string fileNameV3 = CreateFileNameFromTitle(projInfo);
			var resultFileName = Path.Combine(projInfo.DictionaryPath, fileNameV3 + ".zip");
			var zf = new ZipFolder();
			zf.CreateZip(path, resultFileName, 0);
#if (TIME_IT)
            TimeSpan tsTotal = DateTime.Now - dt1;
            Debug.WriteLine("Exportepub: time spent in .epub conversion: " + tsTotal);
#endif
			inProcess.PerformStep();
			return resultFileName;
		}

		protected static void CleanUp(InProcess inProcess,string outputPathWithFileName)
		{
			inProcess.SetStatus("Clean up");
			Common.CleanupExportFolder(outputPathWithFileName, ".tmp,.de", "_1.x", string.Empty);
			inProcess.PerformStep();
		}

		protected void CreateArchiveSubmission(PublicationInformation projInfo, InProcess inProcess, string epub3Path)
		{
			inProcess.SetStatus("Archive");
			var ramp = new Ramp { ProjInputType = projInfo.ProjectInputType };
			ramp.Create(projInfo.DefaultXhtmlFileWithPath, ".zip", projInfo.ProjectInputType);
			inProcess.PerformStep();
		}

		protected static void FinalCleanUp(InProcess inProcess, string outputPathWithFileName)
		{
			inProcess.SetStatus("Final Clean up");
			CopyToParentFolder(outputPathWithFileName);
			var extn = Path.GetExtension(outputPathWithFileName);
			var rampfileName = outputPathWithFileName.Substring(0, outputPathWithFileName.Length - extn.Length) + ".ramp";
			CopyToParentFolder(rampfileName);
			Common.CleanupExportFolder(Path.GetDirectoryName(outputPathWithFileName), string.Empty, string.Empty, "Epub2,Epub3,AudioVisual,pictures");
			inProcess.PerformStep();
		}

		private static void CopyToParentFolder(string outputPathWithFileName)
		{
			File.Copy(outputPathWithFileName,
				Path.Combine(GetParentDirectoryName(outputPathWithFileName),
					Path.GetFileName(outputPathWithFileName)));
		}

		private static string GetParentDirectoryName(string outputPathWithFileName)
		{
			return Path.GetDirectoryName(Path.GetDirectoryName(outputPathWithFileName));
		}

		private static XslCompiledTransform LoadToc2Html5Xslt()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Html5Convert.toc2html5.xsl");
			Debug.Assert(stream != null);
			var toc2Html5 = new XslCompiledTransform();
			toc2Html5.Load(XmlReader.Create(stream));
			return toc2Html5;
		}

	}
}
