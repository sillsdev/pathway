from distutils.core import setup
import py2exe
import sys
import os

# If run without args, build executables, in quiet mode.
if len(sys.argv) == 1:
    sys.argv.append("py2exe")
    sys.argv.append("-q")

class Target:
    def __init__(self, **kw):
        self.__dict__.update(kw)
        # for the versioninfo resources
        self.version = "0.1.0"
        self.company_name = "SIL International"
        self.copyright = "2011 Greg Trihus"
        self.name = "Flex Text Export"

################################################################
# A program using wxPython

# The manifest will be inserted as resource into test_wx.exe.  This
# gives the controls the Windows XP appearance (if run on XP ;-)
#
# Another option would be to store it in a file named
# test_wx.exe.manifest, and copy it with the data_files option into
# the dist-dir.
#
manifest_template = '''
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<assembly xmlns="urn:schemas-microsoft-com:asm.v1" manifestVersion="1.0">
<assemblyIdentity
    version="5.0.0.0"
    processorArchitecture="x86"
    name="%(prog)s"
    type="win32"
/>
<description>%(prog)s Program</description>
<dependency>
    <dependentAssembly>
        <assemblyIdentity
            type="win32"
            name="Microsoft.Windows.Common-Controls"
            version="6.0.0.0"
            processorArchitecture="X86"
            publicKeyToken="6595b64144ccf1df"
            language="*"
        />
    </dependentAssembly>
</dependency>
</assembly>
'''

RT_MANIFEST = 24

TextExport = Target(
    # used for the versioninfo resource
    description = "Export Texts from Flex",

    # what to build
    script = "TextExport.py",
    other_resources = [(RT_MANIFEST, 1, manifest_template % dict(prog="TextExport"))],
    icon_resources = [(1, "graphics/checkfre.ico")],
    dest_base = "TextExport")

################################################################
MyDataFiles = []
for file in os.listdir('graphics/'):
    f1 = 'graphics/' + file
    if os.path.isfile(f1):
        f2 = 'graphics', [f1]
        MyDataFiles.append(f2)

setup(
      windows=[TextExport],
	  data_files = MyDataFiles,
	  options = {"py2exe": {"compressed":1, 
            "optimize":2, 
            "ascii": 1, 
            "bundle_files": 1, 
            "packages":["lxml", "gzip", "encodings"]}},
	  zipfile = None,
      scripts = ['TextExport.py','TextExportF.py','Flex7Text.py'],
	)

