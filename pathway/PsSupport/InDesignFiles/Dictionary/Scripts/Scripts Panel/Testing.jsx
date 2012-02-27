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

$.level = 1; 
debugger;
myDocument = app.documents.item(0);
myDocument.pages.add(1634104421,myDocument.pages.item(2));

//Move();
//Work();
//TOC();
  //TocNew();
//toc();
//Test();

//main();TitleMain_1
//var mystr = GetDocumentType();
//alert(mystr);
//alert(mystr.indexof("scrBook"))
//	var myDocument = app.activeDocument;

function Move()
{
	
	
	myPage = myDocument.pages.item(0);//stories
	myStory = myPage.textFrames.item(myPage.textFrames.length-1)

	//alert(myStory.contents)
     MoveFrame(myStory, 3, 3);
	}

//Move Frame to given position
function MoveFrame(myStory, currentMarginTop, pageNo)
{
	//alert(currentMarginTop);
	myPage = myDocument.pages.item(pageNo);
	marginLeft = 3;//myPage.marginPreferences.left;
	marginRight = 3;// myPage.marginPreferences.right;
		
	myStory.move(myDocument.pages[pageNo]);	
	if(myDocument.documentPreferences.facingPages)
	{
		if((pageNo % 2) == 0 && pageNo > 0)//For Right Page and Not First page
		{
			//alert(pageWidth);
			myStory.move([40, currentMarginTop]);
		}
		else
		{
			//alert(marginTop, pageHeight);
			myStory.move([marginLeft, currentMarginTop]);
		}
	}
	else
	{
		myStory.move([marginLeft, currentMarginTop]);
	}
		
}

function FrontMatter()
{
	//alert("hai");
	var myFrames=new Array();
	var myDocument = app.documents.item(0);
	//var myDocument  = app.documents[app.documents.length-1]
	myPage = myDocument.pages.item(0);
	marginBottom =  myPage.marginPreferences.bottom;
	marginTop = myPage.marginPreferences.top;
	pageHeight = myDocument.documentPreferences.pageHeight - marginBottom;
	var paraStyleName="";
	myPage = myDocument.pages.item(0);//stories
	for(var myStoryCounter=myPage.textFrames.length-1; myStoryCounter >= 0; myStoryCounter--)
	//for(var myStoryCounter=0; myStoryCounter <= myFrames.length-1;myStoryCounter++)
	{
		myStory = myPage.textFrames.item(myStoryCounter);//stories, 
		paraStyleName = myStory.paragraphs[0].appliedParagraphStyle.name.substring(0,5).toLowerCase();
		if(paraStyleName == "cover" || paraStyleName == "title" || paraStyleName == "copyr")
		{
			frameBounds = myStory.geometricBounds;
			myStory.geometricBounds=[marginTop,frameBounds[1] , pageHeight ,frameBounds[3]];
			if(paraStyleName == "copyr")
				return;
		}
	}
}
function TOC()
{
	try
	{	
	  CreateTOCStyle();
	  PlaceTOC();
	}
	catch(myError)
	{
		
	}	  
}

function PlaceTOC()
{
	try
	{	
		  myDocument  = app.documents[app.documents.length-1]
		  myTOCStyle = myDocument.tocStyles.itemByName("TOC");
		  myMasterSpread = myDocument.masterSpreads.item("F-FirstPage");
		  myTocPage = myDocument.pages.add(LocationOptions.AT_BEGINNING, myMasterSpread);
		  myIndexFrame = myDocument.textFrames.lastItem();
		  myBounds = myIndexFrame.geometricBounds;
		  myX1 = myBounds[1];
		  myY1 = myBounds[0];
		  if(isEmptyPage(myDocument.pages[0]))
		  {
			myDocument.pages[0].remove();
		  }
		  myDocument.createTOC(myTOCStyle, true, undefined, [myX1, myY1]);
	}
	catch(myError)
	{
		myDocument.createTOC(myTOCStyle, true, undefined, [myX1, myY1]);
	}		
}

function CreateTOCStyle()
{

	var myDocument = app.documents.item(0);
	//If the TOC Style "myToc" does not already exist, create it.
	// Importent: Try and Catch 
	try{//For Existing
		myTocStyle = myDocument.tocStyles.item("TOC");
		myTocStyle.name;
	}//For New
	catch (myError){
			myTocStyle = myDocument.tocStyles.add();
			myTocStyle.name = "TOC";
			myTocStyleEntry =  myTocStyle.tocStyleEntries.add();
			myTocStyleEntry =  myTocStyle.tocStyleEntries.add();
	}
	//myTocStyleEntry =  myTocStyle.tocStyleEntries.add();
	//Set Properties for TOC Style
	with(myTocStyle){
		createBookmarks = true;
		includeBookDocuments = true;
		//label = "myLabel";
		title = "Contents";
		//Set Properties for TOC Style
		tocStyleEntries[0].name = "TitleMain_1";
		tocStyleEntries[0].separator = "^y";
		tocStyleEntries[0].formatStyle = "[Same Style]";
		tocStyleEntries[0].level = 1;
		
		try{
		tocStyleEntries[1].name = "TitleMain_2";
		tocStyleEntries[1].separator = "^y";
		tocStyleEntries[1].formatStyle = "[Same Style]";
		tocStyleEntries[1].level = 1;
		}
	catch (myError)
	{
		tocStyleEntries[1].name = "TitleMain";
		tocStyleEntries[1].separator = "^y";
		tocStyleEntries[1].formatStyle = "[Same Style]";
		tocStyleEntries[1].level = 1;
		}
// Enter more Properties
		}
	//myTocStyle.tocStyleEntries.name = "TitleMain_1";
	}

function Work1()
{
	var myDocument = app.activeDocument;
alert(myDocument.tocStyles[1].tocStyleEntries[0].formatStyle);
//myDocument.tocStyles[1].tocStyleEntries[0].name	    TitleMain_1
//myDocument.tocStyles[1].tocStyleEntries[0].level			1
//myDocument.tocStyles[1].tocStyleEntries[0].separator	^y
//myDocument.tocStyles[1].tocStyleEntries[0].formatStyle	Same style

/*for( i = 0; i < myDocument.tocStyles.length; i++ ) {
    myToc = myDocument.tocStyles.item(i);
		alert(myToc.tocStyleEntries.length;
    }*/
}

function CreateTOC()
{

	var myDocument = app.documents.item(0);
	//If the TOC Style "myToc" does not already exist, create it.
	// Importent: Try and Catch 
	try{//For Existing
		myTocStyle = myDocument.tocStyles.item("TOC");
		myTocStyle.name;
	}//For New
	catch (myError){
			myTocStyle = myDocument.tocStyles.add();
			myTocStyle.name = "TOC";
			myTocStyleEntry =  myTocStyle.tocStyleEntries.add();
	}
	//myTocStyleEntry =  myTocStyle.tocStyleEntries.add();
	//Set Properties for TOC Style
	with(myTocStyle){
		createBookmarks = true;
		includeBookDocuments = true;
		label = "myLabel";
		title = "TOC";
		//Set Properties for TOC Style
		tocStyleEntries[0].name = "TitleMain_1";
		tocStyleEntries[0].separator = "^y";
		tocStyleEntries[0].formatStyle = "[Same Style]";
		// Enter more Properties
		}
	//myTocStyle.tocStyleEntries.name = "TitleMain_1";
	}

function DeleteFirstFrame()
{
	myDocument  = app.documents[app.documents.length-1];
	myFrame = myDocument.pages.item(0).textFrames;
	var i = myDocument.pages.item(0).textFrames.length-1;
		if(myFrame[i].contents == " ")
		{
			myFrame[i].remove();	
		}
}

function TocNew()
{
	myDocument  = app.documents[app.documents.length-1]
	myTOCStyle = myDocument.tocStyles.itemByName("TOC Style 1");
  myMasterSpread = myDocument.masterSpreads.item("F-FirstPage");
  myTocPage = myDocument.pages.add(LocationOptions.AT_BEGINNING, myMasterSpread);
  myIndexFrame = myDocument.textFrames.lastItem();
  myBounds = myIndexFrame.geometricBounds;
  myX1 = myBounds[1];
  myY1 = myBounds[0];
  myDocument.createTOC(myTOCStyle, true, undefined, [myX1, myY1]);
	}

function toc() {
	//    Set MyTOCStyle = myDoc.TOCStyles.Add 
    //Set myPage = myDocument.Pages.Item(1) 
	myDocument  = app.documents[app.documents.length-1]
	//alert(myDocument.tocStyles.itemByName("Default"));
	//return;
    var myTOCStyle = myDocument.tocStyles.itemByName("[Default]");
	alert(myTOCStyle.name);
    var myTocPage = myDocument.pages.add(LocationOptions.AT_END);
    //var myBounds = myGetBounds(myDocument, myTocPage);
    //var myX1 = myBounds[1];
    //var myY1 = myBounds[0];
    var myStory = myDocument.createTOC(myTOCStyle, true, undefined, [50, 50]);
    var myFrame = myStory.textContainers[0];
	alert("End");
 }


function Test()
{
 //debugger;
	var myPage, myStory, myPageLength
	var result="Dictionary";

			myDocument  = app.documents[app.documents.length-1]
			//myDocument = app.documents[app.documents.length-1];
			myPage = myDocument.pages.item(0);//stories
			//alert(myPage.textFrames.length);
			for(var myStoryCounter=0; myStoryCounter < myPage.textFrames.length; myStoryCounter++)
			{
				myStory = myPage.textFrames.item(myStoryCounter);//stories
				//alert(myStory.contents.indexOf(SpecialCharacters.footnoteSymbol));	
alert(myStory.footnotes.length);				
				//alert(myStory.contents,myStory.geometricBounds);
			}
}
