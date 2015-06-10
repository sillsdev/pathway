using L10NSharp;

namespace SIL.PublishingSolution
{
    public static class LocalizeItems
    {
        public static string LocalizeItem(string id, string controlText)
        {
            switch (id)
            {
                case "Running Head":
                    if (controlText.ToLower() == "every page")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.RunningHead.EveryPage", "Every Page", null);
                    else if (controlText.ToLower() == "mirrored")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.RunningHead.Mirrored", "Mirrored", null);
                    else if (controlText.ToLower() == "none")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.RunningHead.None", "None", null);
                    break;

                case "Page Number":
                    if (controlText.ToLower() == "top inside margin")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageNumber.TopInsideMargin", "Top Inside Margin", null);
                    else if (controlText.ToLower() == "top outside margin")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageNumber.TopOutsideMargin", "Top Outside Margin", null);
                    else if (controlText.ToLower() == "top center")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageNumber.TopCenter", "Top Center", null);
                    else if (controlText.ToLower() == "bottom center")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageNumber.BottomCenter", "Bottom Center", null);
                    else if (controlText.ToLower() == "bottom inside margin")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageNumber.BottomInsideMargin", "Bottom Inside Margin", null);
                    else if (controlText.ToLower() == "bottom outside margin")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageNumber.BottomOutsideMargin", "Bottom Outside Margin", null);
                    else if (controlText.ToLower() == "none")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageNumber.None", "None", null);
                    break;

                case "Rules":
                    if(controlText.ToLower() == "yes")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Rules.Yes", "Yes", null);
                    else if (controlText.ToLower() == "no")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Rules.No", "No", null);
                    break;

                case "Pictures":
                    if (controlText.ToLower() == "frame")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Pictures.Frame", "Frame", null);
                    else if (controlText.ToLower() == "paragraph")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Pictures.Paragraph", "Paragraph", null);
                    else if (controlText.ToLower() == "no")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Pictures.No", "No", null);
                    break;
                    

                case "Sense":
                    if (controlText.ToLower() == "bullet")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Sense.Bullet", "Bullet", null);
                    else if (controlText.ToLower() == "no change")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Sense.Nochange", "No change", null);
                    break;

                case "Justified":
                    if(controlText.ToLower() == "yes")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Justified.Yes", "Yes", null);
                    else if (controlText.ToLower() == "no")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Justified.No", "No", null);
                    break;

                case "VerticalJustify":
                    if (controlText.ToLower() == "top")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.VerticalJustify.Top", "Top", null);
                    else if (controlText.ToLower() == "center")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.VerticalJustify.Center", "Center", null);
                    else if (controlText.ToLower() == "bottom")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.VerticalJustify.Bottom", "Bottom", null);
                    break;

                case "FileProduced":
                    if (controlText.ToLower() == "one")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.FileProduced.One", "One", null);
                    else if (controlText.ToLower() == "one per letter")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.FileProduced.OnePerLetter", "One Per Letter", null);
                    break;

                case "RedLetter":
                    if(controlText.ToLower() == "yes")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.RedLetter.Yes", "Yes", null);
                    else if (controlText.ToLower() == "no")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.RedLetter.No", "No", null);
                    break;

                case "ChapterNumbers":
                    if(controlText.ToLower() == "yes")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.ChapterNumbers.Yes", "Yes", null);
                    else if (controlText.ToLower() == "no")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.ChapterNumbers.No", "No", null);
                    break;

                case "References":
                    if(controlText.ToLower() == "yes")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.References.Yes", "Yes", null);
                    else if (controlText.ToLower() == "no")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.References.No", "No", null);
                    break;

                case "DefaultAlignment":
                    if(controlText.ToLower() == "left")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.DefaultAlignment.Left", "Left", null);
                    else if (controlText.ToLower() == "right")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.DefaultAlignment.Right", "Right", null);
                    else if (controlText.ToLower() == "justify")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.DefaultAlignment.Justify", "Justify", null);
                    break;

                case "MissingFont":
                    if (controlText.ToLower() == "use fallback font")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.MissingFont.UseFallbackFont", "Use Fallback Font", null);
                    else if (controlText.ToLower() == "prompt user")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.MissingFont.PromptUser", "Prompt User", null);
                    else if (controlText.ToLower() == "cancel export")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.MissingFont.CancelExport", "Cancel Export", null);
                    break;

                case "NonSILFont":
                    if (controlText.ToLower() == "embed font anyway")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.NonSILFont.EmbedFontAnyway", "Embed Font Anyway", null);
                    else if (controlText.ToLower() == "use fallback font")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.NonSILFont.UseFallbackFont", "Use Fallback Font", null);
                    else if (controlText.ToLower() == "prompt user")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.NonSILFont.PromptUser", "Prompt User", null);
                    else if (controlText.ToLower() == "cancel export")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.NonSILFont.CancelExport", "Cancel Export", null);
                    break;

                case "TOCLevel":
                    if (controlText.ToLower() == "1 - letter only")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.TOCLevel.LetterOnly", "1 - Letter Only", null);
                    else if (controlText.ToLower() == "2 - letter and entry")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.TOCLevel.LetterandEntry", "2 - Letter and Entry", null);
                    else if (controlText.ToLower() == "3 - letter, entry and sense")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.TOCLevel.LetterEntryandSense", "3 - Letter, Entry and Sense", null);
                    break;

                case "Page Size":
                    if (controlText.ToLower() == "letter")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageSize.Letter", "Letter", null);
                    else if (controlText.ToLower() == "half letter")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.PageSize.HalfLetter", "Half letter", null);
                    break;

                case "Leading":
                    if (controlText.ToLower() == "no change")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.Leading.NoChange", "No Change", null);
                    break;
                case "Font Size":
                    if (controlText.ToLower() == "no change")
                        controlText = LocalizationManager.GetString("ConfigurationToolBL.DropDownControl.FontSize.NoChange", "No Change", null);
                    break;
            }

            return controlText;
        }
    }
}
