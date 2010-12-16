var columnRule=new Array("");
var borderRule=new Array("letHead", "none", "none", ".5 solid #aa0000", "none");
var cropMarks = false;
var indexTab = false;
var margin=new Array("letHead", "1.5", "4.5", "1.666667", "4.5");//


var myDocument;
var myFrames=new Array();
var marginTop, marginBottom, marginLeft,marginRight, pageHeight, pageWidth, headerMinHeight = 6, dataMinHeight = 3, curPageNo =0,currentMarginTop;
var isBorderBottomExists, borderClass,times="";
var letterParagraphStyle, letterPushMargin=0, constantLetterParagraphStyle="letter_lethead_dicbody";
var activePageNumber=0;
var startEvent =1;


Test();

//main();
//var mystr = GetDocumentType();
//alert(mystr);
//alert(mystr.indexof("scrBook"))


function Test()
{

	var myPage, myStory, myPageLength
	var result="Dictionary";

			myDocument  = app.documents[app.documents.length-1]
			//myDocument = app.documents[app.documents.length-1];
			myPage = myDocument.pages.item(1);//stories
			alert(myPage.textFrames.length);
			for(var myStoryCounter=0; myStoryCounter < myPage.textFrames.length; myStoryCounter++)
			{
				myStory = myPage.textFrames.item(myStoryCounter);//stories
				alert(myStory.contents,myStory.geometricBounds);
			}
}
