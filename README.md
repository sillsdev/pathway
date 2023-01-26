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
Pathway is currently running under .net 4.8

*Before building Pathway*

Before building Pathway, the developer should install the latest dependencies. All dependencies have been moved to nuGet packages except for the libpalaso packages: `icu.net.dll`, `Spart.dll`, `SIL.Core.dll`, `SIL.Core.Desktop.dll` and `SIL.WritingSystems.dll`. These were installed in the `lib` folder from `libpalaso_palaso-win32-master_Continuous_8.0.0_artifacts` in the net461 folder. The latest icu files: `icudt56.dll`, `icuin56.dll`, and `icuuc56.dll` were also added to that folder.

*Building on Windows*

Wix 3.11.x should be installed to build installer packages. A Developer will also need the Visual Studio Wix extension for the version of Visual Studio being used.

Development is normally done using the ReSharper Visual Studio Extension. This is used to run the unit tests.

On Windows Pathway is built with Visual Studio. The Debug Configuration is used for building while doing development. The Platform should be set to x86. Currently the most important Configureation is `Corporate7SE`. This is used for Fieldworks. Other Configurations that were built were `Corporate7BTE`, `Release7SE`, and `Release7BTE`. The two Release Configurations included some export paths that were less well tested.

Unit tests and some scenario tests are contained in the Test project. Some tests are marked with SkipOnTeamCity so they won't run on the build server. Also some tests are marked with LongTest since they take more than 3 seconds to complete. The folder structure of the Test project mirrors the folder structure of pathway.

The Installer project depends on all the others so that it will build last. In the BeforeBuild target in the .csproj file for the Installer project, copies the outputs of other Visual Studio projects to a single folder so they can be packaged by Wix from there. (The Test project also has a BeforeBuild copy process to set up a folder with everything needed for testing.)

Pathway is structured so that additional Back ends can be added for other output formats. The interface for each back end is somewhat simple but allows the Pathway program to get the name of the back end for the drop down and then pass control to the Export method in the back end if the user has selected it.

The user interface elements are kept in the CssDialog project. The BuildTasks project contains some C# methods called during the windows packaging process.

*Building on Linux*

[Building on Linux](https://github.com/sillsdev/pathway/blob/develop/pathway/Documentation/Linux%20build%20instructions.txt). The command:

`./buildTest.sh`

will download the NuGet packages and build Pathway. Once the NuGet packages are in place, the Makefile in the pathway folder is used for development on Linux so:

`make debug`

will build the Debug build and

`./nunit-mono4-sil.sh`

will run the unit tests. As you can see Pathway requires mono4-sil to be installed to have access to the latest patched version of mono.

NB: Pathway can also be built with mono-sil and one of the build processes does this for Paratext and FieldWorks versions that are not using the latest mono4-sil yet.
