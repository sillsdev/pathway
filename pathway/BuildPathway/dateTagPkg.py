import os,sys,glob,datetime,shutil
debPat = os.path.join(os.getenv('HOME'), 'git/pathway/*.deb')
pkgs = glob.glob(debPat)
if len(pkgs) == 1:
    if len(pkgs[0].split('_')) == 3:
        fl = os.path.splitext(pkgs[0])
        maj, min, mic, lev, ser = sys.version_info
        linVer = ['', '', '', '', '', 'h', 'l', 'p', 't', 'x']
        daTag = datetime.date.today().strftime('%y%m%d')
        outname = fl[0] + '_' + linVer[min] + daTag + fl[1]
        shutil.move(pkgs[0], outname)
