using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SIL.PublishingSolution.Properties;
using SIL.Tool;

namespace SIL.PublishingSolution
{
	public partial class UILanguageDialog : Form
	{
		public ConfigurationTool CTool = new ConfigurationTool();
		public ConfigurationToolBL CToolBl = new ConfigurationToolBL();
		private string _uiLanguage = "en";
		private string _fontName = "Microsoft Sans Serif";
		private string _fontSize = "8";
		private bool _isLinux = false;

		public UILanguageDialog()
		{
			InitializeComponent();
		}

		private void UILanguageDialog_Load(object sender, EventArgs e)
		{
			try
			{
				_isLinux = Common.IsUnixOS();
				CreateUserInterfaceLanguagexml();
				if (_isLinux)
				{
					_fontName = "Liberation Serif";
					_fontSize = "8";
				}
				Param.LoadUiLanguageFontInfo();
				UpdateFontOnL10NSharp(_uiLanguage);
				LoadUiLanguages();
				ReadLocalizationSettings(_uiLanguage);
				LoadInstalledFonts();
				LoadFontSize();

				rtbPreview.Text = ddlUILanguage.SelectedItem.ToString();
				if (ddlFontName.SelectedItem != null)
					rtbPreview.Font = new Font(ddlFontName.SelectedItem.ToString(), float.Parse(ddlFontSize.SelectedItem.ToString(), CultureInfo.InvariantCulture.NumberFormat));
			}
			catch{}
		}

        private void LoadUiLanguages()
        {
            ddlUILanguage.Items.Clear();
            foreach (var lang in LocalizationManager.GetUILanguages(true))
            {
                var item = new ComboBoxItem(lang.Name, lang.NativeName);
                ddlUILanguage.Items.Add(item);
            }
            ddlUILanguage.SelectedItem = ddlUILanguage.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == _uiLanguage);
        }

        private void LoadInstalledFonts()
        {
            ddlFontName.Items.Clear();
            foreach (FontFamily font in FontFamily.Families)
            {
                ddlFontName.Items.Add(font.Name);
                ddlFontName.SelectedIndex = 0;
            }
            ddlFontName.SelectedItem = _fontName;
        }

        private void LoadFontSize()
        {
            ddlFontSize.Items.Clear();
            ddlFontSize.Items.Add("7");
            ddlFontSize.Items.Add("8");
            ddlFontSize.Items.Add("9");
            ddlFontSize.Items.Add("10");
            ddlFontSize.Items.Add("11");
            ddlFontSize.Items.Add("12");
            ddlFontSize.Items.Add("14");
			ddlFontSize.Items.Add("16");
			ddlFontSize.Items.Add("18");
			ddlFontSize.Items.Add("20");
			ddlFontSize.Items.Add("22");
			ddlFontSize.Items.Add("24");
            ddlFontSize.SelectedItem = _fontSize;
        }

		private void ddlFontName_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (ddlFontName.SelectedItem != null)
				{
					rtbPreview.Font = new Font(ddlFontName.SelectedItem.ToString(),
						float.Parse(ddlFontSize.SelectedItem.ToString(), CultureInfo.InvariantCulture.NumberFormat));
					_fontName = ddlFontName.SelectedItem.ToString();
				}
			}
			catch
			{
				rtbPreview.Font = new Font(_fontName, 12, FontStyle.Regular);
			}
		}

		private void ddlFontSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (ddlFontSize.SelectedItem != null)
				{
					rtbPreview.Font = new Font(ddlFontName.SelectedItem.ToString(), float.Parse(ddlFontSize.SelectedItem.ToString(), CultureInfo.InvariantCulture.NumberFormat));
					_fontName = ddlFontName.SelectedItem.ToString();
					_fontSize = ddlFontSize.SelectedItem.ToString();
				}
			}
			catch
			{
				rtbPreview.Font = new Font(_fontName, 12, FontStyle.Regular);
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ShowL10NsharpDlg();
		}

		public static void ShowL10NsharpDlg()
		{
			Common.L10NMngr.ShowLocalizationDialogBox(false);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			var lang = ((ComboBoxItem)ddlUILanguage.SelectedItem).Value;
			LocalizationManager.SetUILanguage(lang, true);
			Settings.Default.UserInterfaceLanguage = lang;
			LocalizationManager.ReapplyLocalizationsToAllObjects(Common.L10NMngr.Id);
			Common.SaveLocalizationSettings(Settings.Default.UserInterfaceLanguage, _fontName.ToString(), ddlFontSize.SelectedItem.ToString());
			Param.LoadUiLanguageFontInfo();
			this.Close();
		}

		private void CreateUserInterfaceLanguagexml()
		{
			string fileName = Common.PathCombine(Common.GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
			if (!File.Exists(fileName))
			{
				using (XmlWriter writer = XmlWriter.Create(fileName))
				{
					writer.WriteStartElement("UILanguage");
					writer.WriteElementString("string", "en");
					writer.WriteStartElement("fontstyle");
					writer.WriteStartElement("font");
					writer.WriteAttributeString("lang", "en");
					writer.WriteAttributeString("name", _fontName);
					writer.WriteAttributeString("size", _fontSize);
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.Flush();
					writer.Close();
				}
			}
		}

		private void ReadLocalizationSettings(string setting)
		{
			var xmlDoc = new XmlDocument();
			string fileName = Common.PathCombine(Common.GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
			if (File.Exists(fileName))
			{
				var content = File.ReadAllText(fileName);
				xmlDoc.LoadXml(content);
				var uiValueNode = xmlDoc.SelectSingleNode("//UILanguage/string");
				if (uiValueNode != null) 
					uiValueNode.InnerText = setting;

				XmlNode fontNode = xmlDoc.SelectSingleNode("//UILanguage/fontstyle/font[@lang='" + setting + "']");
				if (fontNode != null && fontNode.Attributes["name"].InnerText != null && fontNode.Attributes["size"].InnerText != null)
				{
					
					if (fontNode != null && fontNode.Attributes != null)
					{
						_fontName = fontNode.Attributes["name"].InnerText;
						if (_fontName == "Microsoft Sans Serif" && _isLinux)
						{
							_fontName = "Liberation Serif";
						}
						_fontSize = fontNode.Attributes["size"].InnerText;
					}
				}
			}
		}
	
		/// <summary>
		/// To set the font-name and font-size to the controls in the form.
		/// </summary>
		/// <param name="langId"></param>
		public void UpdateFontOnL10NSharp(string langId)
		{
            _uiLanguage = Common.GetLocalizationSettings();
            Param.GetFontValues(_uiLanguage, ref _fontName, ref _fontSize);
      		//For all labels and textboxes
			List<Control> allControls = GetAllControls(this);
			allControls.ForEach(k => k.Font = new Font(_fontName, float.Parse(_fontSize)));
		}

	    /// <summary>
		/// Get the list of controls in the form
		/// </summary>
		/// <param name="container"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		private List<Control> GetAllControls(Control container, List<Control> list)
		{
			foreach (Control c in container.Controls)
			{
				if (c.Controls.Count > 0)
					list = GetAllControls(c, list);
				else
				{
					if (c.Name != "ddlFontName" && c.Name != "ddlFontSize" && c.Name != "ddlUILanguage" && c.Name != "linkLabel1")
						list.Add(c);
				}
			}
			return list;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		private List<Control> GetAllControls(Control container)
		{
			return GetAllControls(container, new List<Control>());
		}

		private void ddlUILanguage_SelectedIndexChanged(object sender, EventArgs e)
		{
			var lang = ((ComboBoxItem)ddlUILanguage.SelectedItem).Value;
			rtbPreview.Text = ddlUILanguage.SelectedItem.ToString();
			Param.GetFontValues(lang, ref _fontName, ref _fontSize);
			if (_fontName == "Microsoft Sans Serif" && _isLinux)
			{
				_fontName = "Liberation Serif";
			}
			ddlFontName.SelectedItem = _fontName;
			ddlFontSize.SelectedItem = _fontSize;
		}
	}
}
