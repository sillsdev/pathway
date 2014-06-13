import sys,re
from os.path import join
versionPattern = re.compile(r"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+")
if len(sys.argv) < 2 or versionPattern.match(sys.argv[1]) == None:
    sys.stderr.write('Usage: updateChangeLog a.b.c.d\n\nwhere a.b.c.d represents a version number with four components')
    sys.exit(1)
changeLogFullPath = join("..", "debian", "changelog")
changeLog = open(changeLogFullPath).read()
versionMatch = versionPattern.search(changeLog)
if versionMatch != None:
    outLog = open(changeLogFullPath, "w")
    outLog.write(changeLog[:versionMatch.start(0)])
    outLog.write(sys.argv[1])
    outLog.write(changeLog[versionMatch.end(0):])
    outLog.close()
