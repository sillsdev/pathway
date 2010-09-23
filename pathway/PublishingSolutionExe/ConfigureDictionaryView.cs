using System;
using System.Xml;
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class ConfigureDictionaryView : Form
    {
        private static readonly string WriterFullName = Common.GetAllUserPath() + @"\UserDictionaryView.xml";
        private readonly XmlTextWriter Writer = new XmlTextWriter(WriterFullName, null);
        
        public ConfigureDictionaryView()
        {
            InitializeComponent();
        }

        private void ConfigureDictionaryView_Load(object sender, EventArgs e)
        {
            LoadConfigure();
        }

        private void LoadConfigure()
        {

            TreeNode mainEntry = tvConfigure.Nodes.Add("Main Entry");
            TreeNode minorEntry = tvConfigure.Nodes.Add("Minor Entry");
            mainEntry.Nodes.Add("HeadWord");
            TreeNode Pronunciations = mainEntry.Nodes.Add("Pronunciations");
            mainEntry.Nodes.Add("Lexeme Form");
            mainEntry.Nodes.Add("Citation Form");
            mainEntry.Nodes.Add("Homograph Number");

            Pronunciations.Nodes.Add("Tone");
            Pronunciations.Nodes.Add("Pronunciation");
            Pronunciations.Nodes.Add("CV Pattern");
            TreeNode tnLocation = Pronunciations.Nodes.Add("Location");
            tnLocation.Nodes.Add("Abbreviation");
            tnLocation.Nodes.Add("Name");




            TreeNode VariantForms = mainEntry.Nodes.Add("Variant Forms");
            TreeNode VariantType = VariantForms.Nodes.Add("Variant Type");
            VariantType.Nodes.Add("Reverse Abbreviation");
            VariantType.Nodes.Add("Name");
            VariantForms.Nodes.Add("Variant Form");


            TreeNode tvVariantPronunciation = VariantForms.Nodes.Add("Variant Pronunciation");
            tvVariantPronunciation.Nodes.Add("Pronunciation");
            tvVariantPronunciation.Nodes.Add("CV Pattern");
            tvVariantPronunciation.Nodes.Add("Tone");
            tvVariantPronunciation.Nodes.Add("Location");

            VariantForms.Nodes.Add("Comment");
            VariantForms.Nodes.Add("Summary Definition");

            TreeNode Etymology = mainEntry.Nodes.Add("Etymology");
            Etymology.Nodes.Add("Etymological Form");
            Etymology.Nodes.Add("Gloss");
            Etymology.Nodes.Add("Comment");
            Etymology.Nodes.Add("Source");

            TreeNode CrossReferences = mainEntry.Nodes.Add("Cross References");
            CrossReferences.Nodes.Add("Relation Abbreviation");
            CrossReferences.Nodes.Add("Relation Name");
            TreeNode tvTargets = CrossReferences.Nodes.Add("Targets");
            tvTargets.Nodes.Add("Referenced Headword");
            tvTargets.Nodes.Add("Summary Definition");


            TreeNode ComponentReferences = mainEntry.Nodes.Add("Component References");
            TreeNode ComplexFormType = ComponentReferences.Nodes.Add("Complex Form Type");
            ComplexFormType.Nodes.Add("Abbreviation");
            ComplexFormType.Nodes.Add("Name");

            TreeNode Components = ComponentReferences.Nodes.Add("Components");
            Components.Nodes.Add("Referenced Headword");
            Components.Nodes.Add("Summary Definition");
            ComponentReferences.Nodes.Add("Comment");


            TreeNode Senses = mainEntry.Nodes.Add("Senses");
            TreeNode GrammaticalInfo = Senses.Nodes.Add("GrammaticalInfo");
            GrammaticalInfo.Nodes.Add("Category Info.");
            TreeNode Slots = GrammaticalInfo.Nodes.Add("Slots(for Infl. Affixes)");
            Slots.Nodes.Add("Slot Names");
            GrammaticalInfo.Nodes.Add("Inflection Class");
            GrammaticalInfo.Nodes.Add("Inflection Features");
            GrammaticalInfo.Nodes.Add("Exception Features");

            TreeNode SenseType = Senses.Nodes.Add("SenseType");
            SenseType.Nodes.Add("Abbreviation");
            SenseType.Nodes.Add("Name");

            Senses.Nodes.Add("Definition(or Gloss)");
            Senses.Nodes.Add("Definition");
            Senses.Nodes.Add("Gloss");
            TreeNode Examples = Senses.Nodes.Add("Examples");
            Examples.Nodes.Add("Example");
            TreeNode Translations = Examples.Nodes.Add("Translations");
            TreeNode Type = Translations.Nodes.Add("Type");
            Type.Nodes.Add("Abbreviation");
            Type.Nodes.Add("Name");
            Translations.Nodes.Add("Translation");

            TreeNode EncyclopedicInfo = Senses.Nodes.Add("Encyclopedic Info.");
            Senses.Nodes.Add("Restrictions");
            TreeNode LexicalRelations = Senses.Nodes.Add("Lexical Relations");
            LexicalRelations.Nodes.Add("Relation Abbreviation");
            LexicalRelations.Nodes.Add("Relation Name");
            TreeNode Targets = LexicalRelations.Nodes.Add("Targets");
            Targets.Nodes.Add("Referenced Headword");
            Targets.Nodes.Add("Gloss");

            TreeNode VariantsOfSense = Senses.Nodes.Add("Variants of Sense");
            TreeNode variantType = VariantsOfSense.Nodes.Add("variant Type");
            variantType.Nodes.Add("Reverse Abbreviation");
            variantType.Nodes.Add("GloNamess");

            VariantsOfSense.Nodes.Add("Variant Form");
            TreeNode VariantPronunciation = VariantsOfSense.Nodes.Add("Variant Pronunciation");
            VariantPronunciation.Nodes.Add("Pronunciation");
            VariantPronunciation.Nodes.Add("CV Patternoss");
            VariantPronunciation.Nodes.Add("Tone");
            TreeNode VPLocation = VariantPronunciation.Nodes.Add("Location");
            VPLocation.Nodes.Add("Abbreviation");
            VPLocation.Nodes.Add("Name");

            VariantPronunciation.Nodes.Add("Comment");
            VariantPronunciation.Nodes.Add("Summary Definition");


            VariantsOfSense.Nodes.Add("Anthropology Note");
            VariantsOfSense.Nodes.Add("Bibilography");
            VariantsOfSense.Nodes.Add("Discourse Note");
            VariantsOfSense.Nodes.Add("Phonology Note");
            VariantsOfSense.Nodes.Add("Grammer Note");
            VariantsOfSense.Nodes.Add("Semantics Note");
            VariantsOfSense.Nodes.Add("Sociolinguistics Note");
            VariantsOfSense.Nodes.Add("General Note");
            VariantsOfSense.Nodes.Add("Scientific Name");
            VariantsOfSense.Nodes.Add("Source");
            TreeNode SemanticDomains = VariantsOfSense.Nodes.Add("Semantic Domains");
            SemanticDomains.Nodes.Add("Abbreviation");
            SemanticDomains.Nodes.Add("Name");

            TreeNode AnthropologyCategories = VariantsOfSense.Nodes.Add("Anthropology categories");
            AnthropologyCategories.Nodes.Add("Abbreviation");
            AnthropologyCategories.Nodes.Add("Name");

            TreeNode AcadamicDomains = VariantsOfSense.Nodes.Add("Acadamic Domains");
            AcadamicDomains.Nodes.Add("Abbreviation");
            AcadamicDomains.Nodes.Add("Name");

            TreeNode Usages = VariantsOfSense.Nodes.Add("Usages");
            Usages.Nodes.Add("Abbreviation");
            Usages.Nodes.Add("Name");

            TreeNode Status = VariantsOfSense.Nodes.Add("Status");
            Status.Nodes.Add("Abbreviation");
            Status.Nodes.Add("Name");

            TreeNode ComplexForms = VariantsOfSense.Nodes.Add("Complex Forms");
            TreeNode tvComplexFormType = ComplexForms.Nodes.Add("Complex Form Type");
            tvComplexFormType.Nodes.Add("Abbreviation");
            tvComplexFormType.Nodes.Add("Name");
            ComplexForms.Nodes.Add("Complex Form");
            ComplexForms.Nodes.Add("Comment");
            ComplexForms.Nodes.Add("Summary Definition");
            ComplexForms.Nodes.Add("Example Sentences");
            ComplexForms.Nodes.Add("Example");
            TreeNode tvTranslations = ComplexForms.Nodes.Add("Translations");
            tvTranslations.Nodes.Add("Type");
            tvTranslations.Nodes.Add("Abbreviation");
            tvTranslations.Nodes.Add("Name");
            tvTranslations.Nodes.Add("Translation");

            VariantsOfSense.Nodes.Add("Subsenses");

            Senses.Nodes.Add("Bibliography");
            Senses.Nodes.Add("Note");
            Senses.Nodes.Add("Literal Meaning");
            TreeNode Allomorphs = Senses.Nodes.Add("Allomorphs");
            TreeNode MorphType = Allomorphs.Nodes.Add("Morph Type");
            MorphType.Nodes.Add("Abbreviation");
            MorphType.Nodes.Add("Name");

            Allomorphs.Nodes.Add("Allomorph");
            Allomorphs.Nodes.Add("Environments");

            Senses.Nodes.Add("Date Created");
            Senses.Nodes.Add("Date Modified");
            TreeNode tvComplexForms = Senses.Nodes.Add("Complex Forms");


            TreeNode tv1ComplexFormType = tvComplexForms.Nodes.Add("Complex Form Type");
            tv1ComplexFormType.Nodes.Add("Abbreviation");
            tv1ComplexFormType.Nodes.Add("Name");

            ComplexForms.Nodes.Add("Complex Form");
            TreeNode tvGrammaticalInfo = tvComplexForms.Nodes.Add("Grammatical Info.");
            tvGrammaticalInfo.Nodes.Add("Category Info.");
            TreeNode tvSlots = tvGrammaticalInfo.Nodes.Add("Slots(for Infl. Affixes)");
            tvSlots.Nodes.Add("Slot Names");

            tvGrammaticalInfo.Nodes.Add("Inflection Class");
            tvGrammaticalInfo.Nodes.Add("Inflection Features");
            tvGrammaticalInfo.Nodes.Add("Exception Features");


            ComplexForms.Nodes.Add("Comment");
            ComplexForms.Nodes.Add("Summary Definition");
            TreeNode ExampleSentences = tvComplexForms.Nodes.Add("Example Sentences");
            TreeNode Example = ExampleSentences.Nodes.Add("Example");
            TreeNode tv1Translations = Example.Nodes.Add("Translations");
            TreeNode tvType = tv1Translations.Nodes.Add("Type");
            tvType.Nodes.Add("Abbreviation");
            tvType.Nodes.Add("Name");

            tv1Translations.Nodes.Add("Translation");

            TreeNode Pictures = Senses.Nodes.Add("Pictures");
            Pictures.Nodes.Add("Thumbnail");
            Pictures.Nodes.Add("Sense Number");
            Pictures.Nodes.Add("Caption");

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvConfigure.SelectedNode.PrevNode != null)
                {
                    TreeNode index = tvConfigure.SelectedNode;
                    tvConfigure.SelectedNode.Parent.Nodes.Insert(tvConfigure.SelectedNode.Index - 1, (TreeNode)tvConfigure.SelectedNode.Clone());
                    tvConfigure.SelectedNode.Remove();
                    tvConfigure.SelectedNode = tvConfigure.SelectedNode.Parent.Nodes[tvConfigure.SelectedNode.Index - 2];
                    tvConfigure.Focus();
                }
            }
            catch
            {
                //throw;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvConfigure.SelectedNode.NextNode != null)
                {
                    tvConfigure.SelectedNode.Parent.Nodes.Insert(tvConfigure.SelectedNode.Index + 2, (TreeNode)tvConfigure.SelectedNode.Clone());
                    tvConfigure.SelectedNode.Remove();
                    tvConfigure.SelectedNode = tvConfigure.SelectedNode.Parent.Nodes[tvConfigure.SelectedNode.Index + 1];
                    tvConfigure.Focus();
                }
            }
            catch (Exception)
            {
                
                //throw;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Writer.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            Writer.WriteStartElement("Main_Entry");
            TreeNode mainNode = tvConfigure.Nodes[0];
            GetNodeList(mainNode);
            Writer.WriteEndElement();
            Writer.Flush();
            Writer.Close();

        }
     
        public void GetNodeList(TreeNode mainNode)
        {
            foreach (TreeNode firstLevel in mainNode.Nodes)
            {
                Writer.WriteStartElement(ApplyUnderscoreforSymbols(firstLevel.Text));
                GetNodeNames(firstLevel);
                Writer.WriteEndElement();
            }
        }

        public void GetNodeNames(TreeNode node)
        {
            foreach (TreeNode secondLevel in node.Nodes)
            {
                Writer.WriteStartElement(ApplyUnderscoreforSymbols(secondLevel.Text));
                if (secondLevel.FirstNode != null)
                {
                    GetNodeNames(secondLevel);
                }
                Writer.WriteEndElement();
            }
        }

        public string ApplyUnderscoreforSymbols(string text)
        {
            return text.Replace(" ", "_").Replace("(", "_").Replace(")", "_");
        }
    }
}
