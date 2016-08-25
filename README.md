Pathway
=======
Preparing language data for publication

http://pathway.sil.org

Pathway prepares dictionary and Scripture data in open document (odf), e-book (epub 2.0), portable document (pdf), TeX and J2ME (jar) formats. Print and electronic publications can be distributed on web sites, Android cell phones, and Java feature phones. Indirectly Pathway can be used for doc, docx, Kindle®, and Nook®. The epub format will also work on tablets using software for reading e-books.

Pathway installs as an add-on of SIL FieldWorks and Paratext, or it can be launched in batch process.

[Old stable release](http://www.sil.org/resources/software_fonts/pathway)


License
-------
Pathway is licensed under GNU GENERAL PUBLIC LICENSE. See [LICENSE.md](https://github.com/sillsdev/pathway/blob/develop/LICENSE.md)

Documentation
-------------
[Major Features](http://pathway.sil.org/features/)

[Learning Pathway](http://pathway.sil.org/demo/)

[Wiki](https://github.com/sillsdev/pathway/wiki)

[User submitted](http://lingtransoft.info/apps/pathway)


Binaries
--------
[Binary builds](http://build.palaso.org/project.html?projectId=Pathway&tab=projectOverview&guest=1).


Source Code
-----------
[Source Code Repository](https://github.com/sillsdev/pathway)


Issue Tracking
--------------
[Issue Tracking](https://jira.sil.org/browse/TD)

[Submit a request](https://github.com/sillsdev/pathway/wiki/Request-Form)


Development
-----------
Pathway is currently running under .net 4.0.

*Before building Pathway*

Before building Pathway, the developer should install the latest L10NSharp library. This is done with
`Installer/getDependencies.sh`
This command runs a bash script. (On Windows, make sure you have git installed and run from the bash window.) This will go to Team City and download the latest successful build of L10NSharp and put it in the lib folder.
It is very important when building the Windows version to have the Configuration Manager Active solution platform set to x86. If this is not done, a dependency error will be reported when trying to run the localization code even though the build will succeed without error.

*Building on Windows*

On Windows Pathway is built with Visual Studio. The Debug Configuration is used for building while doing development. The Platform should be set to x86. Unit tests and some scenario tests are contained in the Test project. Some tests are marked with SkipOnTeamCity so they won't run on the build server. Also some tests are marked with LongTest since they take more than 3 seconds to complete. The folder structure of the Test project mirrors the folder structure of pathway.

Other configurations are used to prepare package for the various releases. The BTE packages are for FieldWorks BTE and can be used with Paratext as well. The SE packages work with FieldWorks SE versions. The Corporate release packages are intended for stable releases whereas the Release packages also include back ends that are still in development and testing. The Installer project depends on all the others so that it will build last. In the BeforeBuild target in the .csproj file for the Installer project, copies the outputs of other Visual Studio projects to a single folder so they can be packaged by Wix from there. (The Test project also has a BeforeBuild copy process to set up a folder with everything needed for testing.)

Pathway is structured so that additional Back ends can be added for other output formats. The interface for each back end is somewhat simple but allows the Pathway program to get the name of the back end for the drop down and then pass control to the Export method in the back end if the user has selected it.

The user interface elements are kept in the CssDialog project. The BuildTasks project contains some C# methods called during the windows packaging process.

*Building on Linux*

[Building on Linux](https://github.com/sillsdev/pathway/blob/develop/pathway/Documentation/Linux%20build%20instructions.txt). The Makefile in the pathway folder is used for development on Linux so:

`make compile`

will build the Debug build and 

`make test`

will run the unit tests.


