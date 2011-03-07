using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using str = Microsoft.VisualBasic.Strings;
namespace PrincessConvert
{
    public class CreateCss : Form
    {
	private string sFontName;
	private string sFaceName;
	private string sFontSize;
	private string sSpacingInterline;
	private string sParJustify;
		// default
	private string sPageOrientation = "portrait";
		// units included
	private string sPageWidth;
	private string sPageHeight;
	private string sMarginTop;
	private string sMarginBottom;
	private string sMarginInside;
	private string sMarginOutside;
	private string sNumberOfColumns;
	private string sColumnGutter;

	private string sColumnGutterRule;
	// # Chapter/Verse
	private string sChapterNumber;
	private string sPsalmsChapterNumber;
	private string sPsalmString;
	private string sFontTitleCenteredChapterSize;
	private string sSpacingTitleCenteredChapterInterline;
	private string sSpacingTitleCenteredChapterMarginTop;
	private string sSpacingTitleCenteredChapterMarginBottom;
	private string sVerseFormatBold;
	private string sVerseFormatRaised;
	private string sVerseFirstInChapterDisplay;

	private string sSingleChapterBookHideChapterNum;
	// # Headings
	private string sTitleMainSize;
	private string sFontTitleSectionStyle;
	private string sAlignTitleSection;
	private string sFontTitleSectionSize;
	private string sSpacingTitleSectionInterline;
	private string sSpacingTitleSectionMarginTop;
	private string sSpacingTitleSectionMarginBottom;
	private string sFontTitleSectionReferenceStyle;
	private string sFontTitleSectionReferenceSize;
	private string sFontTitleMajorSectionStyle;
	private string sAlignTitleMajorSection;
	private string sFontTitleMajorSectionSize;
	private string sSpacingTitleMajorSectionInterline;
	private string sSpacingTitleMajorSectionMarginTop;
	private string sSpacingTitleMajorSectionMarginBottom;
	private string sFontTitleMajorSectionReferenceStyle;
	private string sFontTitleMajorSectionReferenceSize;
	private string sFontTitleSubSectionStyle;
	private string sAlignTitleSubSection;
	private string sFontTitleSubSectionSize;
	private string sSpacingTitleSubSectionInterline;
	private string sSpacingTitleSubSectionMarginTop;
	private string sSpacingTitleSubSectionMarginBottom;
	private string sFontTitleSubSectionReferenceStyle;

	private string sFontTitleSubSectionReferenceSize;
	// # Footnotes
	private string sFontSizeNotes;
	private string sSpacingInterlineNotes;
	private string sFootnoteCallerFontStyle;
	private string sCrossReferenceCallerFontStyle;
	private string sFootnoteIncludeReference;
	private string sFootnoteReferenceFontStyle;
	private string sNoteRule;
	private string sNoteOnNewline;
	private string sSpaceBetweenFtAndXt;
	private string sNoteFrameStyle;

	private string sNoteGutterRule;

	// # Text Spacing
	private string sSpacingWordBasic;
	private string sSpacingWordSquash;
	private string sSpacingWordStretch;
	private string sSpacingLetterBasic;
	private string sSpacingLetterSquash;
	private string sSpacingLetterStretch;
	private string sKern;
	private string sVerticalJustify;

	private string sHyphenation;
	// # Other
	private string sHeaderLabelsFontStyle;
	private string sTableSpaceBefore;
	private string sTableSpaceAfter;
	private string sRunningHeaderIncludeVerses;
	private string sRunningHeaderReferenceLocation;
	private string sRunningHeaderPageNumberLocation;
	private string sFontRunningHeaderSize;
	private string sRunningHeaderChapterVerseSeparator;
	private string sRunningHeaderConsecutiveSeparator;
	private string sRunningHeaderNonConsecutiveSeparator;
	private string sRunningHeaderSpaceBelow;
	private string sItalicFontName;
	private string sItalicFaceName;
	private string sBoldFontName;
	private string sBoldFaceName;
	private string sBoldItalicFontName;

	private string sBoldItalicFaceName;
	// # Layout
	private string sNRComposer;
	private string sOMA;
	private string sIDME;
	private string sChineseJapaneseLayout;
	private string sHorizontalScaling;

	private string sStudyBibleLayout;
	// ....
	private string s_callee_cTypeface;
	private string s_callee_cNoBreak;
	private string s_callee_cSize;

	private string s_callee_cBaselineShift;

	private string s_calleeSpace_cSize;

	private string s_cnum_cTypeface;
	private string s_crossReferenceCaller_cSize;

	private string s_crossReferenceCaller_cBaselineShift;

	private string s_fk_cTypeface;
	private string s_fm_cTypeface;

	private string s_fm_cNoBreak;
	private string s_fr_cTypeface;

	private string s_fr_cNoBreak;

	private string s_ft_cTypeface;
	private string s_k_cColor;

	private string s_k_cTypeface;
	private string s_noteCaller_cTypeface;
	private string s_noteCaller_cSize;
	private string s_noteCaller_cBaselineShift;

	private string s_noteCaller_cNoBreak;
	private string s_r_cTypeface;

	private string s_r_cSize;
	private string s_v_cTypeface;
	private string s_v_cSize;
	private string s_v_cBaselineShift;

	private string s_v_cNoBreak;

	private string s_w_cTypeface;
	private string s_defaultHeadings_pLeftIndent;

	private string s_defaultHeadings_pRightIndent;
	private string s_c_pSpaceBefore;

	private string s_c_pSpaceAfter;

	private string s_c1q_pTabRuler;

	private string s_c2q_pTabRuler;

	private string s_c3q_pTabRuler;
	private string s_caption_cTypeface;

	private string s_caption_cSize;

	private string s_f_pSpaceAfter;

	private string s_head_cTypeface;
	private string s_ib_pSpaceBefore;

	private string s_ib_pSpaceAfter;
	private string s_iex_pSpaceBefore;

	private string s_iex_pSpaceAfter;
	private string s_im_cSize;

	private string s_im_cLeading;
	private string s_imi_pLeftIndent;

	private string s_imi_pRightIndent;
	private string s_imq_pLeftIndent;

	private string s_imq_pRightIndent;
	private string s_imt1_cSize;
	private string s_imt1_cLeading;

	private string s_imt1_pSpaceBefore;
	private string s_imt2_cSize;
	private string s_imt2_cLeading;
	private string s_imt2_pSpaceBefore;

	private string s_imt2_pSpaceAfter;
	private string s_imt3_cSize;
	private string s_imt3_cLeading;
	private string s_imt3_pSpaceBefore;

	private string s_imt3_pSpaceAfter;
	private string s_imt_cSize;
	private string s_imt_cLeading;

	private string s_imt_pLeftIndent;
	private string s_imte_cSize;

	private string s_imte_cLeading;
	private string s_introDefault_cSize;

	private string s_introDefault_cLeading;
	private string s_io_cSize;
	private string s_io_cLeading;
	private string s_io_pLeftIndent;

	private string s_io_pFirstLineIndent;
	private string s_io2_pLeftIndent;

	private string s_io2_pFirstLineIndent;
	private string s_io3_pLeftIndent;

	private string s_io3_pFirstLineIndent;
	private string s_iot_cSize;
	private string s_iot_cLeading;
	private string s_iot_pLeftIndent;
	private string s_iot_pRightIndent;

	private string s_iot_pTextAlignment;
	private string s_ip_cSize;
	private string s_ip_cLeading;

	private string s_ip_pFirstLineIndent;
	private string s_ipi_pLeftIndent;

	private string s_ipi_pFirstLineIndent;
	private string s_ipq_pLeftIndent;

	private string s_ipq_pRightIndent;
	private string s_ipr_pLeftIndent;

	private string s_ipr_pRightIndent;
	private string s_iq1_pLeftIndent;

	private string s_iq1_pFirstLineIndent;
	private string s_iq2_pLeftIndent;

	private string s_iq2_pFirstLineIndent;
	private string s_iq3_pLeftIndent;

	private string s_iq3_pFirstLineIndent;
	private string s_is_cSize;
	private string s_is_cLeading;
	private string s_is_pLeftIndent;
	private string s_is_pRightIndent;

	private string s_is_pTextAlignment;
	private string s_is1_cSize;
	private string s_is1_cLeading;
	private string s_is1_pLeftIndent;
	private string s_is1_pRightIndent;

	private string s_is1_pTextAlignment;
	private string s_is2_cSize;
	private string s_is2_cLeading;
	private string s_is2_pLeftIndent;
	private string s_is2_pRightIndent;

	private string s_is2_pTextAlignment;
	private string s_li_pLeftIndent;
	private string s_li_pFirstLineIndent;

	private string s_li_pTextAlignment;
	private string s_liv_pLeftIndent;
	private string s_liv_pFirstLineIndent;
	private string s_liv_pTextAlignment;

	private string s_liv_pTabRuler;
	private string s_li2_pLeftIndent;
	private string s_li2_pFirstLineIndent;

	private string s_li2_pTextAlignment;
	private string s_li2v_pLeftIndent;
	private string s_li2v_pFirstLineIndent;
	private string s_li2v_pTextAlignment;

	private string s_li2v_pTabRuler;
	private string s_li3_pLeftIndent;
	private string s_li3_pFirstLineIndent;

	private string s_li3_pTextAlignment;
	private string s_li3v_pLeftIndent;
	private string s_li3v_pFirstLineIndent;
	private string s_li3v_pTextAlignment;

	private string s_li3v_pTabRuler;
	private string s_mi_pLeftIndent;

	private string s_mi_pRightIndent;
	private string s_mt_cLeading;
	private string s_mt_pSpaceBefore;

	private string s_mt_pSpaceAfter;
	private string s_mt2_cSize;
	private string s_mt2_cLeading;
	private string s_mt2_pSpaceBefore;

	private string s_mt2_pSpaceAfter;
	private string s_mt3_cSize;
	private string s_mt3_cTypeface;
	private string s_mt3_cLeading;
	private string s_mt3_pSpaceBefore;

	private string s_mt3_pSpaceAfter;

	private string s_p_pFirstLineIndent;

	private string s_Page_Num_cTypeface;
	private string s_pb_cColor;
	private string s_pb_cSize;
	private string s_pb_cLeading;

	private string s_pb_pBreakBefore;

	private string s_pc_pTextAlignment;
	private string s_ph_BasedOn;
	private string s_ph_pLeftIndent;

	private string s_ph_pFirstLineIndent;
	private string s_pi_pLeftIndent;
	private string s_pi_pRightIndent;

	private string s_pi_pFirstLineIndent;
	private string s_pm_pLeftIndent;

	private string s_pm_pRightIndent;
	private string s_pmc_pLeftIndent;

	private string s_pmc_pRightIndent;
	private string s_pmo_pLeftIndent;

	private string s_pmo_pRightIndent;
	private string s_q_pLeftIndent;
	private string s_q_pFirstLineIndent;

	private string s_q_pTextAlignment;
	private string s_qv_pLeftIndent;
	private string s_qv_pFirstLineIndent;
	private string s_qv_pTabRuler;

	private string s_qv_pTextAlignment;
	private string s_q2_pLeftIndent;

	private string s_q2_pFirstLineIndent;
	private string s_q2v_pLeftIndent;
	private string s_q2v_pFirstLineIndent;

	private string s_q2v_pTabRuler;
	private string s_q3_pLeftIndent;

	private string s_q3_pFirstLineIndent;
	private string s_q3v_pLeftIndent;
	private string s_q3v_pFirstLineIndent;

	private string s_q3v_pTabRuler;
	private string s_s_after_c_pSpaceBefore;

	private string s_s_after_c_pSpaceAfter;
	private string s_s2_cSize;
	private string s_s2_cTypeface;
	private string s_s2_pSpaceBefore;

	private string s_s2_pSpaceAfter;
	private string s_s2_after_c_pSpaceBefore;

	private string s_s2_after_c_pSpaceAfter;
	private string s_sp_pLeftIndent;
	private string s_sp_pRightIndent;
	private int filenum = 2;
    Main main = new Main();

        public CreateCss()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initializationAfterthe InitializeComponent() call.
            //   sw.Ne(Main.sGeneratedStylesheetFileName)



            //     Dim generated As String = Main.sGeneratedStylesheetFileName
            Main.getGeneratedStylesheetName();
            lblGPSStylesheetName.Text = Main.sGPSStylesheetFileName;
            lblGeneratedStylesheetName.Text = Main.sGeneratedStylesheetFileName;
            //readGPSStylesheet()

        }

        private void InitializeComponent()
        {
this.lblGeneratedStylesheetName = new System.Windows.Forms.Label();
this.lblGPSStylesheetName = new System.Windows.Forms.Label();
this.btnConvert = new System.Windows.Forms.Button();
this.SuspendLayout();
// 
// lblGeneratedStylesheetName
// 
this.lblGeneratedStylesheetName.AutoSize = true;
this.lblGeneratedStylesheetName.Location = new System.Drawing.Point(45, 81);
this.lblGeneratedStylesheetName.Name = "lblGeneratedStylesheetName";
this.lblGeneratedStylesheetName.Size = new System.Drawing.Size(168, 13);
this.lblGeneratedStylesheetName.TabIndex = 5;
this.lblGeneratedStylesheetName.Text = "xxx generated stylesheet file name";
// 
// lblGPSStylesheetName
// 
this.lblGPSStylesheetName.AutoSize = true;
this.lblGPSStylesheetName.Location = new System.Drawing.Point(45, 68);
this.lblGPSStylesheetName.Name = "lblGPSStylesheetName";
this.lblGPSStylesheetName.Size = new System.Drawing.Size(142, 13);
this.lblGPSStylesheetName.TabIndex = 4;
this.lblGPSStylesheetName.Text = "xxx GPS stylesheet file name";
// 
// btnConvert
// 
this.btnConvert.Location = new System.Drawing.Point(190, 239);
this.btnConvert.Name = "btnConvert";
this.btnConvert.Size = new System.Drawing.Size(75, 23);
this.btnConvert.TabIndex = 3;
this.btnConvert.Text = "Button1";
this.btnConvert.UseVisualStyleBackColor = true;
// 
// CreateCss
// 
this.ClientSize = new System.Drawing.Size(510, 373);
this.Controls.Add(this.lblGeneratedStylesheetName);
this.Controls.Add(this.lblGPSStylesheetName);
this.Controls.Add(this.btnConvert);
this.Name = "CreateCss";
this.ResumeLayout(false);
this.PerformLayout();

        }

	private void btnConvert_Click(System.Object sender, System.EventArgs e)
	{

	}
	private void readGPSStylesheet()
	{
		string InputLine = null;
		string[] InputStrings = new string[3];
		string sDefine = "";
		StreamReader sr = new StreamReader(Main.sGPSStylesheetFileName);
		try {
			do {
				InputLine = sr.ReadLine();
				if (InputLine == null) {
				// skip
				} else {
					if (InputLine.StartsWith("#")) {
					// comment so skip
					} else if (InputLine.StartsWith("[")) {
						sDefine = InputLine.Replace("[DefineCharStyle:", "");
						sDefine = sDefine.Replace( "[DefineParaStyle:", "");
						sDefine = sDefine.Replace(" ", "_");
						sDefine = sDefine.Replace("]", "_");
					} else {
						if (InputLine.Contains("=")) {
							if (InputLine.StartsWith("*")) {
								InputLine = str.Mid(InputLine, 2);
							} else if (InputLine.StartsWith(Constants.vbTab)) {
								InputLine = InputLine.Replace(Constants.vbTab, "");
								InputLine = InputLine.Replace(":current", "");
							//   InputLine = str.Replace(InputLine, " after ", "After")
							//  InputLine = str.Replace(InputLine, "Italic", "italic")
							//  InputLine = str.Replace(InputLine, "Bold", "bold")

							} else {
								//skip line is fine
							}
							InputStrings = InputLine.Split('=');
						}
						assignValues(ref sDefine, ref InputStrings);
					}
				}
			} while (!(sr.EndOfStream));
			sr.Close();
		//     FileClose(filenum)
		} catch (Exception ex) {
			// corrupt file - write blank
			//TODO if (Main.DebuggingModeToolStripMenuItem.Checked) {
				//MessageBox.Show(ex.Message + Constants.vbCrLf + "Problem reading GPS stylesheet", "GPS stylesheet read error", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//}
			sr.Close();
		}
		sr.Close();

	}

	private void assignValues(ref string sDefine, ref string[] inputstrings)
	{
		string sParameterName = null;
		string sParameterValue = null;

		sParameterName = inputstrings[0];
		sParameterName = sParameterName.Replace(" ", "_");

		sParameterValue = inputstrings[1];
		sParameterValue = sParameterValue.Replace("Italic", "italic");
		sParameterValue = sParameterValue.Replace("Bold", "bold");

		if (sDefine == null) {
		// skip
		} else {
			sParameterName = sDefine + sParameterName;
		}
		switch (sParameterName) {
			// Basic
			case "ppFontName":
				sFontName = sParameterValue;
				break;
			case "ppFaceName":
				if (sParameterValue == "Regular") {
					sFaceName = "normal";
				} else {
					sFaceName = sParameterValue;
				}
				break;
			case "ppFontSize":
				sFontSize = sParameterValue;
				break;
			case "ppSpacingInterline":
				sSpacingInterline = sParameterValue;
				break;
			case "ppParJustify":
				sParJustify = sParameterValue;
				break;
			//       If sParameterValue = "No" Then
			//sParJustify = "left"
			//ElseIf sParameterValue = "Yes" Then
			//sParJustify = "justify"
			//End If
			case "ppPageWidth":
				sPageWidth = sParameterValue;
				break;
			case "ppPageHeight":
				sPageHeight = sParameterValue;
				break;
			case "ppMarginTop":
				sMarginTop = sParameterValue;
				break;
			case "ppMarginBottom":
				sMarginBottom = sParameterValue;
				break;
			case "ppMarginInside":
				sMarginInside = sParameterValue;
				break;
			case "ppMarginOutside":
				sMarginOutside = sParameterValue;
				break;
			case "ppNumberOfColumns":
				sNumberOfColumns = sParameterValue;
				break;
			case "ppColumnGutter":
				sColumnGutter = sParameterValue;
				break;
			case "ppColumnGutterRule":
				sColumnGutterRule = sParameterValue;
				break;
			case "ppPageOrientation":
				sPageOrientation = sParameterValue;
				break;
			// Basic end
			// Chapter verse
			case "ppChapterNumber":
				sChapterNumber = sParameterValue;
				break;
			case "ppPsalmsChapterNumber":
				sPsalmsChapterNumber = sParameterValue;
				break;
			case "ppPsalmString":
				sPsalmString = sParameterValue;
				break;
			case "ppFontTitleCenteredChapterSize":
				sFontTitleCenteredChapterSize = sParameterValue;
				break;
			case "ppSpacingTitleCenteredChapterInterline":
				sSpacingTitleCenteredChapterInterline = sParameterValue;
				break;
			case "ppSpacingTitleCenteredChapterMarginTop":
				sSpacingTitleCenteredChapterMarginTop = sParameterValue;
				break;
			case "ppSpacingTitleCenteredChapterMarginBottom":
				sSpacingTitleCenteredChapterMarginBottom = sParameterValue;
				break;
			case "ppVerseFormatBold":
				sVerseFormatBold = sParameterValue;
				break;
			case "ppVerseFormatRaised":
				sVerseFormatRaised = sParameterValue;
				break;
			case "ppVerseFirstInChapterDisplay":
				sVerseFirstInChapterDisplay = sParameterValue;
				break;
			case "ppSingleChapterBookHideChapterNum":
				sSingleChapterBookHideChapterNum = sParameterValue;
				break;
			// Chapter verse end
			// # Headings
			case "ppTitleMainSize":
				sTitleMainSize = sParameterValue;
				break;
			case "ppFontTitleSectionStyle":
				sFontTitleSectionStyle = sParameterValue;
				break;
			case "ppAlignTitleSection":
				sAlignTitleSection = sParameterValue;
				break;
			case "ppFontTitleSectionSize":
				sFontTitleSectionSize = sParameterValue;
				break;
			case "ppSpacingTitleSectionInterline":
				sSpacingTitleSectionInterline = sParameterValue;
				break;
			case "ppSpacingTitleSectionMarginTop":
				sSpacingTitleSectionMarginTop = sParameterValue;
				break;
			case "ppSpacingTitleSectionMarginBottom":
				sSpacingTitleSectionMarginBottom = sParameterValue;
				break;
			case "ppFontTitleSectionReferenceStyle":
				sFontTitleSectionReferenceStyle = sParameterValue;
				break;
			case "ppFontTitleSectionReferenceSize":
				sFontTitleSectionReferenceSize = sParameterValue;
				break;
			case "ppFontTitleMajorSectionStyle":
				sFontTitleMajorSectionStyle = sParameterValue;
				break;
			case "ppAlignTitleMajorSection":
				sAlignTitleMajorSection = sParameterValue;
				break;
			case "ppFontTitleMajorSectionSize":
				sFontTitleMajorSectionSize = sParameterValue;
				break;
			case "ppSpacingTitleMajorSectionInterline":
				sSpacingTitleMajorSectionInterline = sParameterValue;
				break;
			case "ppSpacingTitleMajorSectionMarginTop":
				sSpacingTitleMajorSectionMarginTop = sParameterValue;
				break;
			case "ppSpacingTitleMajorSectionMarginBottom":
				sSpacingTitleMajorSectionMarginBottom = sParameterValue;
				break;
			case "ppFontTitleMajorSectionReferenceStyle":
				sFontTitleMajorSectionReferenceStyle = sParameterValue;
				break;
			case "ppFontTitleMajorSectionReferenceSize":
				sFontTitleMajorSectionReferenceSize = sParameterValue;
				break;
			case "ppFontTitleSubSectionStyle":
				sFontTitleSubSectionStyle = sParameterValue;
				break;
			case "ppAlignTitleSubSection":
				sAlignTitleSubSection = sParameterValue;
				break;
			case "ppFontTitleSubSectionSize":
				sFontTitleSubSectionSize = sParameterValue;
				break;
			case "ppSpacingTitleSubSectionInterline":
				sSpacingTitleSubSectionInterline = sParameterValue;
				break;
			case "ppSpacingTitleSubSectionMarginTop":
				sSpacingTitleSubSectionMarginTop = sParameterValue;
				break;
			case "ppSpacingTitleSubSectionMarginBottom":
				sSpacingTitleSubSectionMarginBottom = sParameterValue;
				break;
			case "ppFontTitleSubSectionReferenceStyle":
				sFontTitleSubSectionReferenceStyle = sParameterValue;
				break;
			case "ppFontTitleSubSectionReferenceSize":
				sFontTitleSubSectionReferenceSize = sParameterValue;
				break;
			// Headings end
			// # Footnotes
			case "ppFontSizeNotes":
				sFontSizeNotes = sParameterValue;
				break;
			case "ppSpacingInterlineNotes":
				sSpacingInterlineNotes = sParameterValue;
				break;
			case "ppFootnoteCallerFontStyle":
				sFootnoteCallerFontStyle = sParameterValue;
				break;
			case "ppCrossReferenceCallerFontStyle":
				sCrossReferenceCallerFontStyle = sParameterValue;
				break;
			case "ppFootnoteIncludeReference":
				sFootnoteIncludeReference = sParameterValue;
				break;
			case "ppFootnoteReferenceFontStyle":
				sFootnoteReferenceFontStyle = sParameterValue;
				break;
			case "ppNoteRule":
				sNoteRule = sParameterValue;
				break;
			case "ppNoteOnNewline":
				sNoteOnNewline = sParameterValue;
				break;
			case "ppSpaceBetweenFtAndXt":
				sSpaceBetweenFtAndXt = sParameterValue;
				break;
			case "ppNoteFrameStyle":
				sNoteFrameStyle = sParameterValue;
				break;
			case "ppNoteGutterRule":
				sNoteGutterRule = sParameterValue;
				break;
			// # Footnotes end
			// # Text Spacing
			case "ppSpacingWordBasic":
				sSpacingWordBasic = sParameterValue;
				break;
			case "ppSpacingWordSquash":
				sSpacingWordSquash = sParameterValue;
				break;
			case "ppSpacingWordStretch":
				sSpacingWordStretch = sParameterValue;
				break;
			case "ppSpacingLetterBasic":
				sSpacingLetterBasic = sParameterValue;
				break;
			case "ppSpacingLetterSquash":
				sSpacingLetterSquash = sParameterValue;
				break;
			case "ppSpacingLetterStretch":
				sSpacingLetterStretch = sParameterValue;
				break;
			case "ppKern":
				sKern = sParameterValue;
				break;
			case "ppVerticalJustify":
				sVerticalJustify = sParameterValue;
				break;
			case "ppHyphenation":
				sHyphenation = sParameterValue;
				break;
			// # Text Spacing end
			// # Other
			case "ppHeaderLabelsFontStyle":
				sHeaderLabelsFontStyle = sParameterValue;
				break;
			case "ppTableSpaceBefore":
				sTableSpaceBefore = sParameterValue;
				break;
			case "ppTableSpaceAfter":
				sTableSpaceAfter = sParameterValue;
				break;
			case "ppRunningHeaderIncludeVerses":
				sRunningHeaderIncludeVerses = sParameterValue;
				break;
			case "ppRunningHeaderReferenceLocation":
				sRunningHeaderReferenceLocation = sParameterValue;
				break;
			case "ppRunningHeaderPageNumberLocation":
				sRunningHeaderPageNumberLocation = sParameterValue;
				break;
			case "ppFontRunningHeaderSize":
				sFontRunningHeaderSize = sParameterValue;
				break;
			case "ppRunningHeaderChapterVerseSeparator":
				sRunningHeaderChapterVerseSeparator = sParameterValue;
				break;
			case "ppRunningHeaderConsecutiveSeparator":
				sRunningHeaderConsecutiveSeparator = sParameterValue;
				break;
			case "ppRunningHeaderNonConsecutiveSeparator":
				sRunningHeaderNonConsecutiveSeparator = sParameterValue;
				break;
			case "ppRunningHeaderSpaceBelow":
				sRunningHeaderSpaceBelow = sParameterValue;
				break;
			case "ppItalicFontName":
				sItalicFontName = sParameterValue;
				break;
			case "ppItalicFaceName":
				sItalicFaceName = sParameterValue;
				break;
			case "ppBoldFontName":
				sBoldFontName = sParameterValue;
				break;
			case "ppBoldFaceName":
				sBoldFaceName = sParameterValue;
				break;
			case "ppBoldItalicFontName":
				sBoldItalicFontName = sParameterValue;
				break;
			case "ppBoldItalicFaceName":
				sBoldItalicFaceName = sParameterValue;
				break;
			// # Other end
			// # Layout
			case "ppNRComposer":
				sNRComposer = sParameterValue;
				break;
			case "ppOMA":
				sOMA = sParameterValue;
				break;
			case "ppIDME":
				sIDME = sParameterValue;
				break;
			case "ppChineseJapaneseLayout":
				sChineseJapaneseLayout = sParameterValue;
				break;
			case "ppHorizontalScaling":
				sHorizontalScaling = sParameterValue;
				break;
			case "ppStudyBibleLayout":
				sStudyBibleLayout = sParameterValue;
				break;
			// # Layout end
			// ## Other Stylesheet Adjustments
			case "callee_cTypeface":
				s_callee_cTypeface = sParameterValue;
				break;
			case "callee_cNoBreak":
				s_callee_cNoBreak = sParameterValue;
				break;
			case "callee_cSize":
				s_callee_cSize = sParameterValue;
				break;
			case "callee_cBaselineShift":
				s_callee_cBaselineShift = sParameterValue;
				break;
			case "calleeSpace_cSize":
				s_calleeSpace_cSize = sParameterValue;
				break;
			case "cnum_cTypeface":
				s_cnum_cTypeface = sParameterValue;
				break;
			case "crossReferenceCaller_cSize":
				s_crossReferenceCaller_cSize = sParameterValue;
				break;
			case "crossReferenceCaller_cBaselineShift":
				s_crossReferenceCaller_cBaselineShift = sParameterValue;
				break;
			case "fk_cTypeface":
				s_fk_cTypeface = sParameterValue;
				break;
			case "fm_cTypeface":
				s_fm_cTypeface = sParameterValue;
				break;
			case "fm_cNoBreak":
				s_fm_cNoBreak = sParameterValue;
				break;
			case "fr_cTypeface":
				s_fr_cTypeface = sParameterValue;
				break;
			case "fr_cNoBreak":
				s_fr_cNoBreak = sParameterValue;
				break;
			case "ft_cTypeface":
				s_ft_cTypeface = sParameterValue;
				break;
			case "k_cColor":
				s_k_cColor = sParameterValue;
				break;
			case "k_cTypeface":
				s_k_cTypeface = sParameterValue;
				break;
			case "noteCaller_cTypeface":
				s_noteCaller_cTypeface = sParameterValue;
				break;
			case "noteCaller_cSize":
				s_noteCaller_cSize = sParameterValue;
				break;
			case "noteCaller_cBaselineShift":
				s_noteCaller_cBaselineShift = sParameterValue;
				break;
			case "noteCaller_cNoBreak":
				s_noteCaller_cNoBreak = sParameterValue;
				break;
			case "r_cTypeface":
				s_r_cTypeface = sParameterValue;
				break;
			case "r_cSize":
				s_r_cSize = sParameterValue;
				break;
			case "v_cTypeface":
				s_v_cTypeface = sParameterValue;
				break;
			case "v_cSize":
				s_v_cSize = sParameterValue;
				break;
			case "v_cBaselineShift":
				s_v_cBaselineShift = sParameterValue;
				break;
			case "v_cNoBreak":
				s_v_cNoBreak = sParameterValue;
				break;
			case "w_cTypeface":
				s_w_cTypeface = sParameterValue;
				break;
			case "defaultHeadings_pLeftIndent":
				s_defaultHeadings_pLeftIndent = sParameterValue;
				break;
			case "defaultHeadings_pRightIndent":
				s_defaultHeadings_pRightIndent = sParameterValue;
				break;
			case "c_pSpaceBefore":
				s_c_pSpaceBefore = sParameterValue;
				break;
			case "c_pSpaceAfter":
				s_c_pSpaceAfter = sParameterValue;
				break;
			case "c1q_pTabRuler":
				s_c1q_pTabRuler = sParameterValue;
				break;
			case "c2q_pTabRuler":
				s_c2q_pTabRuler = sParameterValue;
				break;
			case "c3q_pTabRuler":
				s_c3q_pTabRuler = sParameterValue;
				break;
			case "caption_cTypeface":
				s_caption_cTypeface = sParameterValue;
				break;
			case "caption_cSize":
				s_caption_cSize = sParameterValue;
				break;
			case "f_pSpaceAfter":
				s_f_pSpaceAfter = sParameterValue;
				break;
			case "head_cTypeface":
				s_head_cTypeface = sParameterValue;
				break;
			case "ib_pSpaceBefore":
				s_ib_pSpaceBefore = sParameterValue;
				break;
			case "ib_pSpaceAfter":
				s_ib_pSpaceAfter = sParameterValue;
				break;
			case "iex_pSpaceBefore":
				s_iex_pSpaceBefore = sParameterValue;
				break;
			case "iex_pSpaceAfter":
				s_iex_pSpaceAfter = sParameterValue;
				break;
			case "im_cSize":
				s_im_cSize = sParameterValue;
				break;
			case "im_cLeading":
				s_im_cLeading = sParameterValue;
				break;
			case "imi_pLeftIndent":
				s_imi_pLeftIndent = sParameterValue;
				break;
			case "imi_pRightIndent":
				s_imi_pRightIndent = sParameterValue;
				break;
			case "imq_pLeftIndent":
				s_imq_pLeftIndent = sParameterValue;
				break;
			case "imq_pRightIndent":
				s_imq_pRightIndent = sParameterValue;
				break;
			case "imt1_cSize":
				s_imt1_cSize = sParameterValue;
				break;
			case "imt1_cLeading":
				s_imt1_cLeading = sParameterValue;
				break;
			case "imt1_pSpaceBefore":
				s_imt1_pSpaceBefore = sParameterValue;
				break;
			case "imt2_cSize":
				s_imt2_cSize = sParameterValue;
				break;
			case "imt2_cLeading":
				s_imt2_cLeading = sParameterValue;
				break;
			case "imt2_pSpaceBefore":
				s_imt2_pSpaceBefore = sParameterValue;
				break;
			case "imt2_pSpaceAfter":
				s_imt2_pSpaceAfter = sParameterValue;
				break;
			case "imt3_cSize":
				s_imt3_cSize = sParameterValue;
				break;
			case "imt3_cLeading":
				s_imt3_cLeading = sParameterValue;
				break;
			case "imt3_pSpaceBefore":
				s_imt3_pSpaceBefore = sParameterValue;
				break;
			case "imt3_pSpaceAfter":
				s_imt3_pSpaceAfter = sParameterValue;
				break;
			case "imt_cSize":
				s_imt_cSize = sParameterValue;
				break;
			case "imt_cLeading":
				s_imt_cLeading = sParameterValue;
				break;
			case "imt_pLeftIndent":
				s_imt_pLeftIndent = sParameterValue;
				break;
			case "imte_cSize":
				s_imte_cSize = sParameterValue;
				break;
			case "imte_cLeading":
				s_imte_cLeading = sParameterValue;
				break;
			case "introDefault_cSize":
				s_introDefault_cSize = sParameterValue;
				break;
			case "introDefault_cLeading":
				s_introDefault_cLeading = sParameterValue;
				break;
			case "io_cSize":
				s_io_cSize = sParameterValue;
				break;
			case "io_cLeading":
				s_io_cLeading = sParameterValue;
				break;
			case "io_pLeftIndent":
				s_io_pLeftIndent = sParameterValue;
				break;
			case "io_pFirstLineIndent":
				s_io_pFirstLineIndent = sParameterValue;
				break;
			case "io2_pLeftIndent":
				s_io2_pLeftIndent = sParameterValue;
				break;
			case "io2_pFirstLineIndent":
				s_io2_pFirstLineIndent = sParameterValue;
				break;
			case "io3_pLeftIndent":
				s_io3_pLeftIndent = sParameterValue;
				break;
			case "io3_pFirstLineIndent":
				s_io3_pFirstLineIndent = sParameterValue;
				break;
			case "iot_cSize":
				s_iot_cSize = sParameterValue;
				break;
			case "iot_cLeading":
				s_iot_cLeading = sParameterValue;
				break;
			case "iot_pLeftIndent":
				s_iot_pLeftIndent = sParameterValue;
				break;
			case "iot_pRightIndent":
				s_iot_pRightIndent = sParameterValue;
				break;
			case "iot_pTextAlignment":
				s_iot_pTextAlignment = sParameterValue;
				break;
			case "ip_cSize":
				s_ip_cSize = sParameterValue;
				break;
			case "ip_cLeading":
				s_ip_cLeading = sParameterValue;
				break;
			case "ip_pFirstLineIndent":
				s_ip_pFirstLineIndent = sParameterValue;
				break;
			case "ipi_pLeftIndent":
				s_ipi_pLeftIndent = sParameterValue;
				break;
			case "ipi_pFirstLineIndent":
				s_ipi_pFirstLineIndent = sParameterValue;
				break;
			case "ipq_pLeftIndent":
				s_ipq_pLeftIndent = sParameterValue;
				break;
			case "ipq_pRightIndent":
				s_ipq_pRightIndent = sParameterValue;
				break;
			case "ipr_pLeftIndent":
				s_ipr_pLeftIndent = sParameterValue;
				break;
			case "ipr_pRightIndent":
				s_ipr_pRightIndent = sParameterValue;
				break;
			case "iq1_pLeftIndent":
				s_iq1_pLeftIndent = sParameterValue;
				break;
			case "iq1_pFirstLineIndent":
				s_iq1_pFirstLineIndent = sParameterValue;
				break;
			case "iq2_pLeftIndent":
				s_iq2_pLeftIndent = sParameterValue;
				break;
			case "iq2_pFirstLineIndent":
				s_iq2_pFirstLineIndent = sParameterValue;
				break;
			case "iq3_pLeftIndent":
				s_iq3_pLeftIndent = sParameterValue;
				break;
			case "iq3_pFirstLineIndent":
				s_iq3_pFirstLineIndent = sParameterValue;
				break;
			case "is_cSize":
				s_is_cSize = sParameterValue;
				break;
			case "is_cLeading":
				s_is_cLeading = sParameterValue;
				break;
			case "is_pLeftIndent":
				s_is_pLeftIndent = sParameterValue;
				break;
			case "is_pRightIndent":
				s_is_pRightIndent = sParameterValue;
				break;
			case "is_pTextAlignment":
				s_is_pTextAlignment = sParameterValue;
				break;
			case "is1_cSize":
				s_is1_cSize = sParameterValue;
				break;
			case "is1_cLeading":
				s_is1_cLeading = sParameterValue;
				break;
			case "is1_pLeftIndent":
				s_is1_pLeftIndent = sParameterValue;
				break;
			case "is1_pRightIndent":
				s_is1_pRightIndent = sParameterValue;
				break;
			case "is1_pTextAlignment":
				s_is1_pTextAlignment = sParameterValue;
				break;
			case "is2_cLeading":
				s_is2_cLeading = sParameterValue;
				break;
			case "is2_cSize":
				s_is2_cSize = sParameterValue;
				break;
			case "is2_pLeftIndent":
				s_is2_pLeftIndent = sParameterValue;
				break;
			case "is2_pRightIndent":
				s_is2_pRightIndent = sParameterValue;
				break;
			case "is2_pTextAlignment":
				s_is2_pTextAlignment = sParameterValue;
				break;
			case "li_pLeftIndent":
				s_li_pLeftIndent = sParameterValue;
				break;
			case "li_pFirstLineIndent":
				s_li_pFirstLineIndent = sParameterValue;
				break;
			case "li_pTextAlignment":
				s_li_pTextAlignment = sParameterValue;
				break;
			case "liv_pLeftIndent":
				s_liv_pLeftIndent = sParameterValue;
				break;
			case "liv_pFirstLineIndent":
				s_liv_pFirstLineIndent = sParameterValue;
				break;
			case "liv_pTextAlignment":
				s_liv_pTextAlignment = sParameterValue;
				break;
			case "liv_pTabRuler":
				s_liv_pTabRuler = sParameterValue;
				break;
			case "li2_pLeftIndent":
				s_li2_pLeftIndent = sParameterValue;
				break;
			case "li2_pFirstLineIndent":
				s_li2_pFirstLineIndent = sParameterValue;
				break;
			case "li2_pTextAlignment":
				s_li2_pTextAlignment = sParameterValue;
				break;
			case "li2v_pLeftIndent":
				s_li2v_pLeftIndent = sParameterValue;
				break;
			case "li2v_pFirstLineIndent":
				s_li2v_pFirstLineIndent = sParameterValue;
				break;
			case "li2v_pTextAlignment":
				s_li2v_pTextAlignment = sParameterValue;
				break;
			case "li2v_pTabRuler":
				s_li2v_pTabRuler = sParameterValue;
				break;
			case "li3_pLeftIndent":
				s_li3_pLeftIndent = sParameterValue;
				break;
			case "li3_pFirstLineIndent":
				s_li3_pFirstLineIndent = sParameterValue;
				break;
			case "li3_pTextAlignment":
				s_li3_pTextAlignment = sParameterValue;
				break;
			case "li3v_pLeftIndent":
				s_li3v_pLeftIndent = sParameterValue;
				break;
			case "li3v_pFirstLineIndent":
				s_li3v_pFirstLineIndent = sParameterValue;
				break;
			case "li3v_pTextAlignment":
				s_li2v_pTextAlignment = sParameterValue;
				break;
			case "li3v_pTabRuler":
				s_li3v_pTabRuler = sParameterValue;
				break;
			case "mi_pLeftIndent":
				s_mi_pLeftIndent = sParameterValue;
				break;
			case "mi_pRightIndent":
				s_mi_pRightIndent = sParameterValue;
				break;
			case "mt_cLeading":
				s_mt_cLeading = sParameterValue;
				break;
			case "mt_pSpaceBefore":
				s_mt_pSpaceBefore = sParameterValue;
				break;
			case "mt_pSpaceAfter":
				s_mt_pSpaceAfter = sParameterValue;
				break;
			case "mt2_cSize":
				s_mt2_cSize = sParameterValue;
				break;
			case "mt2_cLeading":
				s_mt2_cLeading = sParameterValue;
				break;
			case "mt2_pSpaceBefore":
				s_mt2_pSpaceBefore = sParameterValue;
				break;
			case "mt2_pSpaceAfter":
				s_mt2_pSpaceAfter = sParameterValue;
				break;
			case "mt3_cSize":
				s_mt3_cSize = sParameterValue;
				break;
			case "mt3_cTypeface":
				s_mt3_cTypeface = sParameterValue;
				break;
			case "mt3_cLeading":
				s_mt3_cLeading = sParameterValue;
				break;
			case "mt3_pSpaceBefore":
				s_mt3_pSpaceBefore = sParameterValue;
				break;
			case "mt3_pSpaceAfter":
				s_mt3_pSpaceAfter = sParameterValue;
				break;
			case "p_pFirstLineIndent":
				s_p_pFirstLineIndent = sParameterValue;
				break;
			case "Page_Num_cTypeface":
				s_Page_Num_cTypeface = sParameterValue;
				break;
			case "pb_cColor":
				s_pb_cColor = sParameterValue;
				break;
			case "pb_cSize":
				s_pb_cSize = sParameterValue;
				break;
			case "pb_cLeading":
				s_pb_cLeading = sParameterValue;
				break;
			case "pb_pBreakBefore":
				s_pb_pBreakBefore = sParameterValue;
				break;
			case "pc_pTextAlignment":
				s_pc_pTextAlignment = sParameterValue;
				break;
			case "ph_BasedOn":
				s_ph_BasedOn = sParameterValue;
				break;
			case "ph_pLeftIndent":
				s_ph_pLeftIndent = sParameterValue;
				break;
			case "ph_pFirstLineIndent":
				s_ph_pFirstLineIndent = sParameterValue;
				break;
			case "pi_pLeftIndent":
				s_pi_pLeftIndent = sParameterValue;
				break;
			case "pi_pRightIndent":
				s_pi_pRightIndent = sParameterValue;
				break;
			case "pi_pFirstLineIndent":
				s_pi_pFirstLineIndent = sParameterValue;
				break;
			case "pm_pLeftIndent":
				s_pm_pLeftIndent = sParameterValue;
				break;
			case "pm_pRightIndent":
				s_pm_pRightIndent = sParameterValue;
				break;
			case "pmc_pLeftIndent":
				s_pmc_pLeftIndent = sParameterValue;
				break;
			case "pmc_pRightIndent":
				s_pmc_pRightIndent = sParameterValue;
				break;
			case "pmo_pLeftIndent":
				s_pmo_pLeftIndent = sParameterValue;
				break;
			case "pmo_pRightIndent":
				s_pmo_pRightIndent = sParameterValue;
				break;
			case "q_pLeftIndent":
				s_q_pLeftIndent = sParameterValue;
				break;
			case "q_pFirstLineIndent":
				s_q_pFirstLineIndent = sParameterValue;
				break;
			case "q_pTextAlignment":
				s_q_pTextAlignment = sParameterValue;
				break;
			case "qv_pLeftIndent":
				s_qv_pLeftIndent = sParameterValue;
				break;
			case "qv_pFirstLineIndent":
				s_qv_pFirstLineIndent = sParameterValue;
				break;
			case "qv_pTabRuler":
				s_qv_pTabRuler = sParameterValue;
				break;
			case "qv_pTextAlignment":
				s_qv_pTextAlignment = sParameterValue;
				break;
			case "q2_pLeftIndent":
				s_q2_pLeftIndent = sParameterValue;
				break;
			case "q2_pFirstLineIndent":
				s_q2_pFirstLineIndent = sParameterValue;
				break;
			case "q2v_pLeftIndent":
				s_q2v_pLeftIndent = sParameterValue;
				break;
			case "q2v_pFirstLineIndent":
				s_q2v_pFirstLineIndent = sParameterValue;
				break;
			case "q2v_pTabRuler":
				s_q2v_pTabRuler = sParameterValue;
				break;
			case "q3_pLeftIndent":
				s_q3_pLeftIndent = sParameterValue;
				break;
			case "q3_pFirstLineIndent":
				s_q3_pFirstLineIndent = sParameterValue;
				break;
			case "q3v_pLeftIndent":
				s_q3v_pLeftIndent = sParameterValue;
				break;
			case "q3v_pFirstLineIndent":
				s_q3v_pFirstLineIndent = sParameterValue;
				break;
			case "q3v_pTabRuler":
				s_q3v_pTabRuler = sParameterValue;
				break;
			case "s_after_c_pSpaceBefore":
				s_s_after_c_pSpaceBefore = sParameterValue;
				break;
			case "s_after_c_pSpaceAfter":
				s_s_after_c_pSpaceAfter = sParameterValue;
				break;
			case "s2_cSize":
				s_s2_cSize = sParameterValue;
				break;
			case "s2_cTypeface":
				s_s2_cTypeface = sParameterValue;
				break;
			case "s2_pSpaceBefore":
				s_s2_pSpaceBefore = sParameterValue;
				break;
			case "s2_pSpaceAfter":
				s_s2_pSpaceAfter = sParameterValue;
				break;
			case "s2_after_c_pSpaceBefore":
				s_s2_after_c_pSpaceBefore = sParameterValue;
				break;
			case "s2_after_c_pSpaceAfter":
				s_s2_after_c_pSpaceAfter = sParameterValue;
				break;
			case "sp_pLeftIndent":
				s_sp_pLeftIndent = sParameterValue;
				break;
			case "sp_pRightIndent":
				s_sp_pRightIndent = sParameterValue;
				break;
			// ## Other Stylesheet Adjustments end





			default:

				MessageBox.Show(sParameterName + Constants.vbCrLf + "=" + Constants.vbCrLf + sParameterValue);

				break;
		}

	}
	private void writeCSS()
	{
		try {
			StreamWriter sw = new StreamWriter(Main.sGeneratedStylesheetFileName);

			// dim at beginning
			sw.WriteLine("/***********************************************************************************/");
			sw.WriteLine("/******************************** start of CSS file ********************************/");
			sw.WriteLine("/***********************************************************************************/");
			//
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine("/***     The box model starts with the text                  ***/");
			sw.WriteLine("/***     Surrounding the text is padding                     ***/");
			sw.WriteLine("/***     Surrounding the padding is border                   ***/");
			sw.WriteLine("/***     Surrounding the border is margin                    ***/");
			sw.WriteLine();
			sw.WriteLine("/***     Shortcut for identifying the 4 sides of box model   ***/");
			sw.WriteLine("/***     attribute: top right bottom left                    ***/");
			sw.WriteLine();
			sw.WriteLine("/***     All units must be declared except for 0             ***/");
			sw.WriteLine("/***     If unit is not declared then attribute is ignored   ***/");
			sw.WriteLine();



			//        writeSettings()
			// # Basic
			sw.WriteLine();
			sw.WriteLine("/***********************************/");
			sw.WriteLine("/********** Page settings **********/");
			sw.WriteLine("/***********************************/");
			sw.WriteLine();
			sw.WriteLine("@page {");
			sw.WriteLine();

			sw.WriteLine(Constants.vbTab + "marks: crop cross" + " ;");
			sw.WriteLine(Constants.vbTab + "font-family: " + sFontName + " ;");
			sw.WriteLine(Constants.vbTab + "font-style: " + sFaceName + " ;");
			sw.WriteLine(Constants.vbTab + "font-size: " + sFontSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + sSpacingInterline + "pt;");
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentJustify(sParJustify) + " ;");
			sw.WriteLine(Constants.vbTab + "size: " + sPageWidth + " " + sPageHeight + " " + sPageOrientation + " ;");
			sw.WriteLine(Constants.vbTab + "margin-top: " + sMarginTop + " ;");
			// units come with sMarginTop
			sw.WriteLine(Constants.vbTab + "margin-bottom: " + sMarginBottom + " ;");
			sw.WriteLine(Constants.vbTab + "margin-inside: " + sMarginInside + " ;");
			sw.WriteLine(Constants.vbTab + "margin-outside: " + sMarginOutside + " ;");
			sw.WriteLine();
			sw.WriteLine("  @top-center {");
			sw.WriteLine(Constants.vbTab + "content: string(bookx, start)  ' '  string(chapterx, start) ':' string(versex, start)  ' — '  string(chapterx, last) ':' string(versex, last) ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for top-center
			sw.WriteLine();
			sw.WriteLine("  @footnotes {");
			sw.WriteLine(Constants.vbTab + "border-top: thin solid black ;");
			sw.WriteLine(Constants.vbTab + "padding: 0.3em 0 ;");
			sw.WriteLine(Constants.vbTab + "margin-top: 0.6em ;");
			sw.WriteLine(Constants.vbTab + "margin-left: 2pi ;");
			sw.WriteLine(Constants.vbTab + "border-top: thin solid black ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for footnotes
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for page
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine("@page :first {");
			sw.WriteLine();
			sw.WriteLine("  @top-center {");
			sw.WriteLine(Constants.vbTab + "content: normal ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for top-center
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine("  @bottom-center {");
			sw.WriteLine(Constants.vbTab + "content: counter(page);");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for bottom-center
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine("  @bottom-right {");
			sw.WriteLine(Constants.vbTab + "content: normal ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for bottom-right
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for page: first
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine("@page :left {");
			sw.WriteLine();
			if (sRunningHeaderPageNumberLocation == "Outside") {
				sw.WriteLine("  @top-left {");
			} else if (sRunningHeaderPageNumberLocation == "Center") {
				sw.WriteLine("  @top-center {");
			} else if (sRunningHeaderPageNumberLocation == "Inside") {
				sw.WriteLine("  @top-right {");
			} else {
				// default position
				sw.WriteLine("  @bottom-left {");
			}
			sw.WriteLine(Constants.vbTab + "content: counter(page);");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for bottom-left
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for page left
			sw.WriteLine();

			sw.WriteLine();
			sw.WriteLine("@page :right {");
			sw.WriteLine();
			if (sRunningHeaderPageNumberLocation == "Outside") {
				sw.WriteLine("  @top-right {");
			} else if (sRunningHeaderPageNumberLocation == "Center") {
				sw.WriteLine("  @top-center {");
			} else if (sRunningHeaderPageNumberLocation == "Inside") {
				sw.WriteLine("  @top-left {");
			} else {
				// default position
				sw.WriteLine("  @bottom-right {");
			}
			sw.WriteLine(Constants.vbTab + "content: counter(page);");

			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for bottom-right
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for page right
			sw.WriteLine();

			sw.WriteLine("/***********************************/");
			sw.WriteLine("/********** Titles *****************/");
			sw.WriteLine("/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".mainTitle {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "string-set: bookx content() ;");
			sw.WriteLine(Constants.vbTab + "font-size: " + sTitleMainSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "font-style: " + sFontTitleSectionStyle + " ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + s_mt_cLeading + "pt ;");
			sw.WriteLine(Constants.vbTab + "margin-before: " + s_mt_pSpaceBefore + "pt ;");
			sw.WriteLine(Constants.vbTab + "margin-after: " + s_mt_pSpaceAfter + "pt ;");
			// xxx this may be default to always set centered
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentCenter(sAlignTitleSection) + " ;");

			//sw.WriteLine(vbtab + "string-set: h1 ;")
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for h1.book
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine(".secondaryTitle {");
			sw.WriteLine();
			// sw.WriteLine(vbTab + "string-set: bookx content() ;")
			sw.WriteLine(Constants.vbTab + "font-size: " + s_mt2_cSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + s_mt2_cLeading + "pt ;");
			sw.WriteLine(Constants.vbTab + "margin-before: " + s_mt2_pSpaceBefore + "pt ;");
			sw.WriteLine(Constants.vbTab + "margin-after: " + s_mt2_pSpaceAfter + "pt ;");
			// sw.WriteLine(vbtab + "string-set: h1 ;")
			// xxx this may be default to always set centered
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentCenter(sAlignTitleSection) + " ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for h2.book
			sw.WriteLine();





			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/********** introduction ***********/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".introduction {");
			sw.WriteLine(Constants.vbTab + "columns: 1 ;");
			sw.WriteLine(Constants.vbTab + "font-style: italic ;");
			sw.WriteLine(Constants.vbTab + "border-bottom-width: 1pt ;");
			sw.WriteLine(Constants.vbTab + "border-bottom-style: solid ;");
			sw.WriteLine(Constants.vbTab + "padding: 0 0 6pt 0 ;");
			sw.WriteLine(Constants.vbTab + "margin: 0 0 6pt 0 ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for introduction
			sw.WriteLine();

			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/********** scripture text *********/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".scriptureText {");
			sw.WriteLine(Constants.vbTab + "columns: " + sNumberOfColumns + " ;");
			sw.WriteLine(Constants.vbTab + "column-gap: " + sColumnGutter + " ;");
			if (sColumnGutterRule == "Yes") {
				sw.WriteLine("/* Value:  <column-rule-width> || <border-style> || [ <color> | transparent ] */");
				sw.WriteLine(Constants.vbTab + "column-rule: thin solid black ;");
			} else {
				// skip
			}
			sw.WriteLine(Constants.vbTab + "column-fill: balance ;");
			// hyphenation file is found with the document file
			// its name is fixed as hyphenation.txt
			sw.WriteLine(Constants.vbTab + "prince-hyphenate-patterns: url(" + Main.sDocumentPath + "\\hyphenation.txt ;");
			sw.WriteLine(Constants.vbTab + "column-fill: balance ;");
			sw.WriteLine(Constants.vbTab + "hyphens: auto ;");
			sw.WriteLine(Constants.vbTab + "hyphenate-before: 2 ;");
			sw.WriteLine(Constants.vbTab + "hyphenate-after: 3 ;");
			sw.WriteLine(Constants.vbTab + "hyphenate-lines: 1 ;");
			sw.WriteLine(Constants.vbTab + "widows: 1 ;");
			sw.WriteLine(Constants.vbTab + "ophans: 1 ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for scripture text
			sw.WriteLine();

			// # Chapter
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/********** chapter number *********/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".chapterNumber {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "string-set: chapterx content() ;");
			// InDesign Middle East
			if (sIDME == "yes") {
				// rtl
				sw.WriteLine(Constants.vbTab + "float: right ; ");
				sw.WriteLine(Constants.vbTab + "padding: 1% 0 0 5% ; ");
			} else {
				// l2r
				sw.WriteLine(Constants.vbTab + "float: left ;");
				sw.WriteLine(Constants.vbTab + "padding: 1% 5% 0 0 ; ");
			}
			// floats and vertical align don't mix -- without float it bumps second line -- sw.WriteLine(vbTab + "vertical-align: top;")
			// two line drop cap
			sw.WriteLine(Constants.vbTab + "font-size: 270% ;");
			// arrived at number by trial and error
			sw.WriteLine(Constants.vbTab + "font-weight: bold ;");
			sw.WriteLine(Constants.vbTab + "line-height: 86% ;");
			// arrived at number by trial and error
			sw.WriteLine(Constants.vbTab + "margin: 0 ;");
			sw.WriteLine(Constants.vbTab + "prince-bookmark-level: 1 ;");
			// sw.WriteLine(vbtab + "font-family: sans-serif ;")
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for chapter number
			sw.WriteLine();


			// # Verse
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/********** verse number ***********/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".verseNumber {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "string-set: versex content() ;");
			// InDesign Middle East
			if (sIDME == "yes") {
				// rtl
				sw.WriteLine(Constants.vbTab + "padding: 0 0 0 0.5pt ; ");
			} else {
				// l2r
				sw.WriteLine(Constants.vbTab + "padding: 0 0.5pt 0 0 ; ");
			}
			sw.WriteLine(Constants.vbTab + "margin: 0 ;");
			if (sVerseFormatBold == "Yes") {
				sw.WriteLine(Constants.vbTab + "font-weight: bold ;");
			} else {
				sw.WriteLine(Constants.vbTab + "font-weight: normal ;");
			}
			if (sVerseFormatRaised == "Yes") {
				sw.WriteLine(Constants.vbTab + "vertical-align: 0.3em ;");
				sw.WriteLine(Constants.vbTab + "font-size: 70% ;");
			} else {
				// leave normal
				sw.WriteLine(Constants.vbTab + "vertical-align: none;");
				sw.WriteLine(Constants.vbTab + "font-size: 100% ;");
			}
			// sw.WriteLine(vbtab + "font-family: sans-serif ;")
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for verse number
			sw.WriteLine();

			sw.WriteLine();
			sw.WriteLine(".verseNumber1 {");
			sw.WriteLine();

			if (sVerseFirstInChapterDisplay == "Yes") {
				sw.WriteLine(Constants.vbTab + "string-set: versex content() ;");
				// InDesign Middle East
				if (sIDME == "yes") {
					// rtl
					sw.WriteLine(Constants.vbTab + "padding: 0 0 0 0.5pt ; ");
				} else {
					// l2r
					sw.WriteLine(Constants.vbTab + "padding: 0 0.5pt 0 0 ; ");
				}
				sw.WriteLine(Constants.vbTab + "margin: 0 ;");
				if (sVerseFormatBold == "Yes") {
					sw.WriteLine(Constants.vbTab + "font-weight: bold ;");
				} else {
					sw.WriteLine(Constants.vbTab + "font-weight: normal ;");
				}
				if (sVerseFormatRaised == "Yes") {
					sw.WriteLine(Constants.vbTab + "vertical-align: 0.3em ;");
					sw.WriteLine(Constants.vbTab + "font-size: 70% ;");
				} else {
					// leave normal
					sw.WriteLine(Constants.vbTab + "vertical-align: none;");
					sw.WriteLine(Constants.vbTab + "font-size: 100% ;");
				}
			// sw.WriteLine(vbtab + "font-family: sans-serif ;")
			} else {
				// skip display of verse 1
			}

			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for verse number
			sw.WriteLine();

			sw.WriteLine("/***********************************/");
			sw.WriteLine("/********** Footnotes***************/");
			sw.WriteLine("/***********************************/");

			sw.WriteLine();

			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/********** footnote text **********/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".footnote {");
			sw.WriteLine();
			// InDesign Middle East
			if (sIDME == "Yes") {
			// rtl
			//   sw.WriteLine(vbtab + "float: right ; ")
			//  sw.WriteLine(vbtab + "padding: 0 0 0 0.5pt ; ")
			} else {
				// l2r
				// sw.WriteLine(vbtab + "float: left ;")
				//sw.WriteLine(vbtab + "padding: 0 0.5pt 0 0 ; ")
			}
			//     sw.WriteLine(vbtab + "display: none ;")
			//    sw.WriteLine(vbtab + "display: footnote ;")
			sw.WriteLine(Constants.vbTab + "display: prince-footnote ;");
			//   sw.WriteLine(vbtab + "position: footnote ;")
			sw.WriteLine(Constants.vbTab + "position: footnote ;");
			//  sw.WriteLine(vbtab + "list-style-position: inside ;")
			sw.WriteLine(Constants.vbTab + "font-size: " + sFontSizeNotes + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + sSpacingInterlineNotes + "pt ;");
			sw.WriteLine(Constants.vbTab + "margin: 0 ;");
			// sw.WriteLine(vbtab + "font-family: sans-serif ;")
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for footnote
			sw.WriteLine();



			//            /* cross-references and footnotes */
			//.crossReference{
			//  display: none;
			//  display: footnote;
			//  display: prince-footnote;
			//  position: footnote;
			//  list-style-position:inside;
			//  font-size: 9pt;
			//  line-height: none;
			//  font-family:arial, helvetica, sans-serif;
			//  color: black;
			//  text-align: left;
			//  text-indent: 0;
			//  margin-left: 4em;
			//  font-weight: normal;
			//}
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/********** cross-reference text ***/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".crossReference {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentLeftOrRight() + " ; ");
			//     sw.WriteLine(vbtab + "display: none ;")
			//    sw.WriteLine(vbtab + "display: footnote ;")
			sw.WriteLine(Constants.vbTab + "display: prince-footnote ;");
			//   sw.WriteLine(vbtab + "position: footnote ;")
			sw.WriteLine(Constants.vbTab + "position: footnote ;");
			//  sw.WriteLine(vbtab + "list-style-position: inside ;")
			sw.WriteLine(Constants.vbTab + "font-size: " + sFontSizeNotes + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + sSpacingInterlineNotes + "pt ;");
			sw.WriteLine(Constants.vbTab + "margin: 0 ;");
			sw.WriteLine(Constants.vbTab + "text-indent: 0 ;");
			sw.WriteLine(Constants.vbTab + "margin: 0 ;");
			// InDesign Middle East
			if (sIDME == "yes") {
				sw.WriteLine(Constants.vbTab + "margin-right: 2em ;");
			} else {
				sw.WriteLine(Constants.vbTab + "margin-left: 2em ;");
			}
			// sw.WriteLine(vbtab + "font-family: sans-serif ;")
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for cross-reference
			sw.WriteLine();





			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/**** footnote caller in text ******/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".footnote::footnote-call  {");
			sw.WriteLine();
			// InDesign Middle East
			if (sIDME == "Yes") {
			// rtl
			//      sw.WriteLine(vbtab + "float: right ; ")
			} else {
				// l2r
				//    sw.WriteLine(vbtab + "float: left ;")
			}
			sw.WriteLine(Constants.vbTab + "content:    '\\2021' ' '; ");
			// font style???  sw.WriteLine(vbtab + "font-style: " + sFootnoteCallerFontStyle + " ; ")
			sw.WriteLine(Constants.vbTab + "font-size: " + s_noteCaller_cSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "font-style: " + s_noteCaller_cTypeface + " ;");
			// ??? sw.WriteLine(vbtab + "font-:" + s_noteCaller_cNoBreak  + " ;")
			//   sw.WriteLine(vbtab + "vertical-align: super ;")
			sw.WriteLine(Constants.vbTab + "vertical-align: " + s_noteCaller_cBaselineShift + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: none ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for footnote-caller in text
			sw.WriteLine();



			///* marker in the body text indicating a cross-reference*/
			//.crossReference::footnote-call {
			//  color:purple;
			//content:    '\2020' ' ';
			//  font-size: 6pt;
			//  vertical-align: super;
			//  line-height: none;
			//}

			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/*** footnote marker in footnote ***/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".footnote::footnote-marker  {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentLeftOrRight() + " ; ");
			//   sw.WriteLine(vbtab + "content:    '\2021' ' '; ")
			// font style???  sw.WriteLine(vbtab + "font-style: " + sFootnoteCallerFontStyle + " ; ")
			sw.WriteLine(Constants.vbTab + "font-size: " + s_noteCaller_cSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "font-style: " + s_noteCaller_cTypeface + " ;");
			if (sFootnoteIncludeReference == "Yes") {
				sw.WriteLine(Constants.vbTab + "content: string(chapterx) ':' string(versex) ' ' ;");
				sw.WriteLine(Constants.vbTab + "font-style: " + sFootnoteReferenceFontStyle + " ;");
			} else {
				// no reference included
			}

			// ??? sw.WriteLine(vbtab + "font-:" + s_noteCaller_cNoBreak  + " ;")
			sw.WriteLine(Constants.vbTab + "vertical-align: super ;");
			sw.WriteLine(Constants.vbTab + "line-height: none ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for footnote-marker in footnote
			sw.WriteLine();


			///* marker in the body text indicating a footnote*/
			//.footnote::footnote-call {
			//  color:purple;
			//content:    '\2021' ' ';
			//  font-size: 6pt;
			//  vertical-align: super;
			//  line-height: none;
			//}
			///* marker in footnote in front of the text */
			//.footnote::footnote-marker {
			//  /*  font-size: 10pt; */
			//    font-weight: bold;
			///*    content: '\2020' ' ' string(chapterx) ":" string(versex) ' =?  ' ;*/
			//  /* content: string(chapterx) ":" string(versex) ' =  ' ;*/
			///* non breaking space below*/
			//   content: string(chapterx) ":" string(versex) ' ' ;
			//   text-align: left;
			//}

			///* marker in footnote for the cross-reference */
			//.crossReference::footnote-marker {
			// /*  font-size: 10pt;*/
			//   font-weight: bold;
			///*  content: string(chapterx) ":" string(versex) ' =  ' ;*/
			///* multiple spaces are removed*/
			///* non breaking space below*/
			//   content: string(chapterx) ":" string(versex)  ' ';
			//   text-align:left;
			//}
			///* end of: cross-references and footnotes */


			// 88888888888888888888888888888888888888888888888

			//  /* page break inside:avoid added to li */
			//}
			///* verse number immediately following p*/
			//p > .verseNumber:first-child  {
			//  string-set: versex content();
			//  font-family: san-serif;
			//
			//  font-size: 8pt;
			//  vertical-align: top;
			//  color: green;
			//  margin: 0;
			//  padding-top: 0pt;  
			//  padding-right: 0.5pt;  /* add a fixed space amount before the attached text*/
			//  padding-bottom: 0;
			//  padding-left:0;

			///* page break inside:avoid added to li */
			//}

			///* verse numbers in poetry */
			//p.line1 > .verseNumber{
			//  /* margin-left: -1.5em;  don't know why this offset works*/
			//  margin-left: -1.5em;
			//  color: red;
			//  float: right;
			//}
			//p.line2 > .verseNumber{
			//  /* margin-left: -1.5em;  don't know why this offset works*/
			//  margin-left: -2.5em;
			//  color: red;
			//  float: right;
			//}
			//p.line3 > .verseNumber{
			//  /* margin-left: -1.5em;  don't know why this offset works*/
			//  margin-left: -3.5em;
			//  color: red;
			//  float: right;
			//}

			//            Private sChapterNumber As String
			//            Private sPsalmsChapterNumber As String
			//'            Private sPsalmString As String
			//           Private sFontTitleCenteredChapterSize As String
			//           Private sSpacingTitleCenteredChapterInterline As String
			//            Private sSpacingTitleCenteredChapterMarginTop As String
			//            Private sSpacingTitleCenteredChapterMarginBottom As String
			//            Private sVerseFormatBold As String
			//            Private sVerseFormatRaised As String
			//            Private sVerseFirstInChapterDisplay As String
			//            Private sSingleChapterBookHideChapterNum As String

			sw.WriteLine("/***********************************/");
			sw.WriteLine("/********** Section heads **********/");
			sw.WriteLine("/***********************************/");
			sw.WriteLine();

			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/*********** section head normal ***/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".section  {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentCenter(sAlignTitleSection) + " ; ");
			if (sFontTitleMajorSectionStyle.Contains("bold")) {
				sw.WriteLine(Constants.vbTab + "font-weight: bold ;");
			} else {
				sw.WriteLine(Constants.vbTab + "font-weight: normal ;");

			}
			if (sFontTitleMajorSectionStyle.Contains("italic")) {
				sw.WriteLine(Constants.vbTab + "font-style: italic ;");
			} else {
				sw.WriteLine(Constants.vbTab + "font-style: normal ;");
			}
			sw.WriteLine(Constants.vbTab + "font-size: " + sFontTitleSectionSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + sSpacingTitleSectionInterline + "pt ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "margin-top: " + sSpacingTitleSectionMarginTop + "pt ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "margin-bottom: " + sSpacingTitleSectionMarginBottom + "pt ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for section head
			sw.WriteLine();

			sw.WriteLine();
			sw.WriteLine(".sectionWithParallelPassageFollowing   {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentCenter(sAlignTitleSection) + " ; ");
			if (sFontTitleMajorSectionStyle.Contains("bold")) {
				sw.WriteLine(Constants.vbTab + "font-weight: bold ;");
			} else {
				sw.WriteLine(Constants.vbTab + "font-weight: normal ;");

			}
			if (sFontTitleMajorSectionStyle.Contains("italic")) {
				sw.WriteLine(Constants.vbTab + "font-style: italic ;");
			} else {
				sw.WriteLine(Constants.vbTab + "font-style: normal ;");
			}
			sw.WriteLine(Constants.vbTab + "font-size: " + sFontTitleSectionSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + sSpacingTitleSectionInterline + "pt ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "margin-top: " + sSpacingTitleSectionMarginTop + "pt ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "margin-bottom: 0 ;");
			// important to maintain register
			//sw.WriteLine(vbTab + "prince-bookmark-level: 3 ;")
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for .sectionWithParallelPassageFollowing
			sw.WriteLine();


			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/*********** parallel passage ******/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".parallelPassage  {");
			sw.WriteLine();
			// looks like parallel passage and section are locked together
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentCenter(sAlignTitleSection) + " ; ");
			sw.WriteLine(Constants.vbTab + "font-style: " + s_r_cTypeface + " ;");
			sw.WriteLine(Constants.vbTab + "font-size: " + s_r_cSize + "pt ;");
			//     sw.WriteLine(vbtab + "line-height: " + sSpacingInterline + "pt ;") ' important to maintain register
			sw.WriteLine(Constants.vbTab + "margin-top: " + sSpacingInterline + "pt ;");
			sw.WriteLine(Constants.vbTab + "margin-bottom: 0 ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for parallel passage
			sw.WriteLine();

			sw.WriteLine();
			sw.WriteLine(".parallelPassageWithSectionHeadPreceding  {");
			sw.WriteLine();
			// looks like parallel passage and section are locked together
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentCenter(sAlignTitleSection) + " ; ");
			sw.WriteLine(Constants.vbTab + "font-style: " + s_r_cTypeface + " ;");
			sw.WriteLine(Constants.vbTab + "font-size: " + s_r_cSize + "pt ;");
			//     sw.WriteLine(vbtab + "line-height: " + sSpacingInterline + "pt ;") ' important to maintain register
			sw.WriteLine(Constants.vbTab + "margin-top: 0 ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "margin-bottom: " + sSpacingTitleSectionMarginBottom + "pt ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for parallel passage
			sw.WriteLine();



			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/*********** paragraph *************/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine("p  {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-align: " + getAlignmentJustify(sParJustify) + " ;");
			sw.WriteLine(Constants.vbTab + "font-style: " + sFaceName + " ;");
			sw.WriteLine(Constants.vbTab + "font-size: " + sFontSize + "pt ;");
			sw.WriteLine(Constants.vbTab + "line-height: " + sSpacingInterline + "pt ;");
			// important to maintain register
			sw.WriteLine(Constants.vbTab + "margin: 0 ;");
			sw.WriteLine(Constants.vbTab + "page-break-inside: auto ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for paragraph
			sw.WriteLine();


			sw.WriteLine();
			sw.WriteLine(".inscription {");
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "font-variant: small-caps ;");
			sw.WriteLine(Constants.vbTab + "text-align: center ;");
			sw.WriteLine(Constants.vbTab + "text-indent: 0 ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for inscription
			sw.WriteLine();

			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine(Constants.vbTab + "/*********** poetry *************/");
			sw.WriteLine(Constants.vbTab + "/***********************************/");
			sw.WriteLine();
			sw.WriteLine(".line1  {");
			// use all of the p styles above plus
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-indent: -3em ;");
			sw.WriteLine(Constants.vbTab + "margin-left: 4em ;");
			sw.WriteLine(Constants.vbTab + "page-break-inside: avoid ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for line1
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine(".line2  {");
			// use all of the p styles above plus
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-indent: -2em ;");
			sw.WriteLine(Constants.vbTab + "margin-left: 4em ;");
			sw.WriteLine(Constants.vbTab + "page-break-inside: avoid ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for line1
			sw.WriteLine();
			sw.WriteLine();
			sw.WriteLine(".line3  {");
			// use all of the p styles above plus
			sw.WriteLine();
			sw.WriteLine(Constants.vbTab + "text-indent: -1em ;");
			sw.WriteLine(Constants.vbTab + "margin-left: 4em ;");
			sw.WriteLine(Constants.vbTab + "page-break-inside: avoid ;");
			sw.WriteLine(Constants.vbTab + "}");
			// closing marker for line1
			sw.WriteLine();











			sw.WriteLine();
			sw.WriteLine("/***********************************************************************************/");
			sw.WriteLine("/******************************** end of CSS file **********************************/");
			sw.WriteLine("/***********************************************************************************/");
			sw.Close();


			//         FileOpen(filenum, Main.sGeneratedStylesheetFileName, OpenMode.Output, OpenAccess.Write)
			//         '  FileOpen(filenum, test, OpenMode.Output, OpenAccess.Write)
			//         WriteLine(filenum, "******************************** start of CSS file ********************************")
			//         writeSettings()
			//         FileClose(filenum)
			//         '    If File.Exists(Main.sGeneratedStylesheetFileName) Then
            Main main = new Main();
            main.enableGPSstylesheet();
            main.enableGeneratedStylesheet();

		} catch (Exception ex) {
			// 
			MessageBox.Show("Error trying to write the CSS file." + Constants.vbCrLf + ex.Message, "Can not create file", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}



	}
	private void writeSettings()
	{
		writePage();

	}
	private void writePage()
	{
		//        @page {
		// marks: crop cross ;
		//size: 5in 8in portrait;
		//font: 13pt Gentium, Georgia, serif;
		//line-height: 13;

		// sw.WriteLine(vbtab + "//Page")
		// sw.WriteLine(vbtab + "@page {")
		// sw.WriteLine(vbtab + "size: ", sPageWidth, sPageHeight, sPageOrientation)

	}




	private object getAlignmentCenter(string alignValue)
	{
		//string alignment = "";
		if (alignValue == "Yes") {
			return "center";
		} else {
			return getAlignmentLeftOrRight();
		}
	}
	private object getAlignmentJustify(string alignValue)
	{
		//string alignment = "";
		if (alignValue == "Yes") {
			return "justify";
		} 
        //else {
			return getAlignmentLeftOrRight();
		//}
	}
	private object getAlignmentLeftOrRight()
	{
		// InDesign Middle East
		if (sIDME == "Yes") {
			// rtl
			return "right";
		} else {
			// l2r
			return "left";
		}

	}

	private void calculatePageWidthInPoints()
	{
		Main.dPageWidthInPoints = int.Parse(sPageWidth.Replace("in", "")) * 72;
		// convert inches to points
        Main.dPageHeightInPoints = int.Parse(sPageHeight.Replace("in", "")) * 72; //str.Replace(sPageHeight, "in", "") * 72;
		// convert inches to points



		Main.iNumberOfColumns = int.Parse(sNumberOfColumns);
		Main.dColumnGapInPoints = decimal.Parse(sColumnGutter.Replace("pt", ""));
		// in pt

		if (int.Parse(sNumberOfColumns) < 2) {
			// if for some reason it thinks it is zero reset it to 1
			Main.iNumberOfColumns = 1;
			Main.dColumnGapInPoints = 0;
		} else {
			if (Main.dColumnGapInPoints < 3) {
				Main.dColumnGapInPoints = 3;
				// must have some gap
			} else {
				//skip
			}
		}

		Main.dMarginLeftInPoints = decimal.Parse(sMarginInside.Replace( "pt", ""));
		Main.dMarginRightInPoints = decimal.Parse(sMarginOutside.Replace( "pt", ""));

		int dMarginsAndGap = (int)(Main.dColumnGapInPoints + Main.dMarginLeftInPoints + Main.dMarginRightInPoints);
		Main.dColumnWidthInPoints = (Main.dPageWidthInPoints - dMarginsAndGap) / Main.iNumberOfColumns;
		main.writeIniFile();
	}

        internal Label lblGeneratedStylesheetName;
        internal Label lblGPSStylesheetName;
        internal Button btnConvert;
    }
}
