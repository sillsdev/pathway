#-----------------------------------------------------------------------------
# Name:        blogExport.py
# Purpose:     build root based dictionary web site
#
# Author:      Greg Trihus <greg_trihus@sil.org>
#
# Created:     2010/06/16 (Python 2.5)
# RCS-ID:      $Id: etreeLoad.py $
# Copyright:   (c) 2010 SIL International
# Licence:     MIT
#-----------------------------------------------------------------------------
#!/usr/bin/env python
#Boa:PyApp:main

import os,sys,datetime,getopt
from lxml import etree

modules ={}

class IndexPage:
    """Generate the index page"""
    pages = {}
    def Add(self, map, outname):
        """Add(map, outname) - add one item to index
        
        map = fields from XHTML file (used to generate reference text)
        outname = name of file referred to by link
        """
        link = "%s (%s)" % (map["Citation"], map["Stem"])
        n = 1
        plink = link
        while self.pages.has_key(plink):
            n += 1
            plink = link + ('<sub>%d</sub>' % n)
        self.pages[plink] = outname
    def Make(self, outpath):
        """Make(outpath) - generate index file
        
        outpath = path name where index file should be placed
        """
        map = {}
        links = self.pages.keys()
        links.sort()
        anchors = []
        for link in links:
            anchors.append('<a href="%s">%s</a>' % (self.pages[link], link))
        map["Pages"] = ', '.join(anchors)
        map["Date"] = datetime.date.today().strftime('%B %d, %Y')  # like June 16, 2010
        indextpl = open('index-tpl.htt').read()
        indexdata = indextpl % map
        ix = open(os.path.join(outpath,"index.htm"),'w')
        ix.write(indexdata.encode('utf-8'))
        ix.close()
        
class PostFile:
    """
    obj = PostFile(addr, n, outpath, prefix, database)
    
    addr = url base for site
    n = first post number to use
    outpath = where to put file with generated sql query
    prefix = table prefix (defaults to wp_)
    database = database name (defaults to wordpress)
    """
    def __init__(self, addr, n, outpath, prefix='wp_', database='wordpress'):
        self.database=database
        self.prefix = prefix
        self.file = open(os.path.join(outpath,'data.sql'),'w')
        self.DelQuery()
        self.NewQuery()
        self.initn = n
        self.n = self.initn
        self.cont = ''
        self.site = addr
    def DelQuery(self):
        """Write query to delete previous wordpress posts"""
        self.file.write('DELETE FROM `%s`.`%sposts` WHERE `%sposts`.`post_type` = "post";\n' % (self.database, self.prefix, self.prefix))
    def NewQuery(self):
        """Write beginning of query to insert new posts"""
        insql = "INSERT INTO `%sposts` (`ID`, `post_author`, `post_date`, " % self.prefix
        insql += "`post_date_gmt`, `post_content`, `post_title`, `post_excerpt`, "
        insql += "`post_status`, `comment_status`, `ping_status`, `post_password`, "
        insql += "`post_name`, `to_ping`, `pinged`, `post_modified`, `post_modified_gmt`, "
        insql += "`post_content_filtered`, `post_parent`, `guid`, `menu_order`, `post_type`, "
        insql += "`post_mime_type`, `comment_count`) VALUES\n"
        self.file.write(insql)
    def EndQuery(self):
        """Write end of query"""
        self.file.write(';\n')
    def Write(self, outname, outdata, map):
        """Write(outname, outdata, map) - Write the data for one post
        
        outname = unique post identifier (w/o special characters or spaces)
        outdata = data for post
        map = fields collected (used to generate header for post)
        """
        page = outdata.replace('\r\n', '\\r\\n')
        page = page.replace('\n', '\\r\\n')
        page = page.replace("'", "''")
        date = datetime.datetime.now().isoformat()
        wpdate = "%s %s" % (date[:10], date[11:19])
        isodate = datetime.datetime.utcnow().isoformat()
        wpudate = "%s %s" % (isodate[:10], isodate[11:19])
        title = "%s (%s)" % (map["Citation"], map["Stem"])
        title = title.replace("'", "''")
        rec = u"(%d, 1, '%s', '%s', '%s', '%s', " % (self.n, wpdate, wpudate, page, title)
        rec += "'', 'publish', 'open', 'open', '', '%s', '', '', '%s', '%s', " % (outname, wpdate, wpudate)
        rec += "'', 0, '%s/?p=%d', 0, 'post', '', 0)" % (self.site, self.n)
        self.n += 1
        if (self.n - self.initn) % 10 == 0:
            self.EndQuery()
            self.NewQuery()
        else:
            self.file.write(self.cont)
        self.file.write(rec.encode('utf-8'))
        self.cont = ',\n'
    def Close(self):
        """Close file with query to add posts"""
        self.EndQuery()
        self.file.close()
    def N(self):
        """Return key field for current post"""
        return self.n

class TermTable:
    """
    tt = TermTable(taxn, outpath, prefix, database)
    taxn = starting index for taxonomy table
    outpath = where to create the sql query file
    prefix = table prefix (defaults to wp_)
    database = database name (defaults to wordpress)
    """
    def __init__(self, taxn, outpath, prefix='wp_', database='wordpress'):
        self.database = database
        self.prefix = prefix
        self.outpath = outpath
        self.initn = 4
        self.n = self.initn
        self.term = {'Uncategorized':1, 'Domain':2, 'Part of speech':3}
        self.count = {1:0, 2:0, 3:0}
        self.parent = {1:0, 2:0, 3:0}
        self.rel = {}
        self.taxn = taxn
    def AddSem(self, r, s):
        """
        AddSem(r, s)
        r = post #
        s = semantic domain items (space separated)
        """
        semCats = s.split()
        for t in semCats:
            self.AddTerm(r, t, 2)
    def AddTerm(self, r, s, p):
        """
        AddTerm(r, s, p)
        r = post #
        s = term to add
        p = index to parent of term (2 for Semantic Domain, 3 for Part of speech)
        """
        k = s.lower()
        if s == '':
            i = 1
            self.count[1] += 1
        elif self.term.has_key(k):
            i = self.term[k]
            self.count[i] += 1
        else:
            i = self.n
            self.term[k] = i
            self.count[i] = 1
            self.parent[i] = p
            self.n += 1
        if self.rel.has_key(r):
            try:
                self.rel[r].index(i)
            except:
                self.rel[r].append(i)
        else:
            self.rel[r] = [i]
    def Write(self):
        """Write() - write file containing queries to add terms to wordpress blog site"""
        self.file = open(os.path.join(self.outpath,'data.sql'),'a')
        idx = {}
        for k in self.term.keys():
            idx[self.term[k]] = k
        self.DelTermQuery()
        self.NewTermQuery()
        n = 0
        keys = idx.keys()
        keys.sort()
        for tn in keys:
            rec = u"(%d, '%s', '%s', 0)" % (tn, idx[tn], idx[tn])
            n += 1
            if n % 10 == 0:
                self.EndQuery()
                self.NewTermQuery()
            else:
                self.file.write(self.cont)
            self.file.write(rec.encode('utf-8'))
            self.cont = ",\n"
        self.EndQuery()
        tax = {1:1}
        self.DelTaxQuery()
        self.NewTaxQuery()
        n = 0
        keys = self.parent.keys()
        keys.sort()
        for cat in keys:
            if cat == 1:
                continue
            rec = u"(%d, %d, 'category', '', %d, %d)" % (self.taxn, cat, self.parent[cat], self.count[cat])
            tax[cat] = self.taxn
            self.taxn += 1
            n += 1
            if n % 10 == 0:
                self.EndQuery()
                self.NewTaxQuery()
            else:
                self.file.write(self.cont)
            self.file.write(rec.encode('utf-8'))
            self.cont = ",\n"
        self.EndQuery()
        self.DelRelQuery()
        self.NewRelQuery()
        n = 0
        keys = self.rel.keys()
        keys.sort()
        for obj in keys:
            for cat in self.rel[obj]:
                rec = u"(%d, %d, 0)" % (obj, tax[cat])
                n += 1
                if n % 10 == 0:
                    self.EndQuery()
                    self.NewRelQuery()
                else:
                    self.file.write(self.cont)
                self.file.write(rec.encode('utf-8'))
                self.cont = ",\n"
        self.EndQuery()
    def DelTermQuery(self):
        """Write query to delete previous wordpress terms"""
        self.file.write('DELETE FROM `%s`.`%sterms` ;\n' % (self.database, self.prefix))
    def NewTermQuery(self):
        """Write beginning of query to insert new terms"""
        insql = "INSERT INTO `%sterms` (`term_id`, `name`, `slug`, `term_group`) VALUES\n" % self.prefix
        self.file.write(insql)
        self.cont = ''
    def DelRelQuery(self):
        """Write query to delete relationships of posts to term taxonomies"""
        self.file.write('DELETE FROM `%s`.`%sterm_relationships` WHERE `%sterm_relationships`.`object_id` > 1;\n' % (self.database, self.prefix, self.prefix))
    def NewRelQuery(self):
        """Write beginning of query to insert new relationships of posts to term taxonomies"""
        insql = "INSERT INTO `%sterm_relationships` (`object_id`, `term_taxonomy_id`, `term_order`) VALUES\n" % self.prefix
        self.file.write(insql)
        self.cont = ''
    def DelTaxQuery(self):
        """Write query to delete term taxonomies"""
        self.file.write('DELETE FROM `%s`.`%sterm_taxonomy` WHERE `%sterm_taxonomy`.`taxonomy` = "category";\n' % (self.database, self.prefix, self.prefix))
    def NewTaxQuery(self):
        """Write beginning of query to insert new term taxonomies"""
        insql = "INSERT INTO `%sterm_taxonomy` (`term_taxonomy_id`, `term_id`, `taxonomy`, `description`, `parent`, `count`) VALUES\n" % self.prefix
        self.file.write(insql)
        self.cont = ''
    def EndQuery(self):
        """Write end of a query"""
        self.file.write(';\n')
    def Close(self):
        """Close query file"""
        self.file.close()

class BlogExport:
    """
    blogExport [opts] xhtml
    
    Convert SIL FieldWorks Flex xhtml export to a blog site
    
    \t-e, --email=\t\temail address for response
    \t-m, --media-root=\turl to root of site
    \t-w, --wordpress\t\ttoggle whether wordpress sql input generated
    \t-s, --site\t\ttoggle whether html web site files generated
    \t-f, --folders\t\ttoggle whether html files are put in folders
    \t-n, --database=\tdatabase name
    \t-p, --post-index=\tstarting index key for posts
    \t-t, --taxonomy-index=\tstarting index key for taxonomies
    \t-d, --table-prefix=\ttable prefix (normally wp_)
    \t-u, --user-styles\tuse user styles from FieldWorks
    \t-i, --include-example=\tText which must be contained in example
    """
    typedisplay = {
        'PRC':'Present Continuous', 
        'INC':'Incompletive',
        'IMM':'Immediate',
        'CMP':'Completive',
        'DVN':'Deverbal Noun',
        'pl':'Plural',
        }
    subentryorder = ['PRC','INC','IMM','CMP','DVN']
    def __init__(self, xhtml, email, mediaRoot, wordpress=True, site=False, folders=False, postIndex=85, taxonomyIndex=10, database='wordpress', prefix='wp_', user=False, includeExample=''):
        self.namesinuse = {}
        root = etree.parse(xhtml)
        outpath = os.path.dirname(xhtml)
        if outpath == '':
            outpath = '.'
        if site:
            if user:
                entrytpl = open('FwEntry-tpl.htt').read()
            else:
                entrytpl = open('Entry-tpl.htt').read()
        mediatpl = open('Media-tpl.htt').read()
        if not user:
            tabletpl = open('Table-tpl.htt').read()
            subentrytpl = open('SubEntry-tpl.htt').read()
            self.exampletpl = open('Example-tpl.htt').read()
        entries = self.FindAll(root, '//x:div[@class="entry"]')
        print len(entries), "entries"
        actualCount = 0
        if site:
            indexPage = IndexPage()
        if wordpress:
            postFile = PostFile(mediaRoot, postIndex, outpath, prefix, database)
            termTable = TermTable(taxonomyIndex, outpath, prefix, database)
        for e in range(len(entries),0,-1):
            entry = entries[e-1]
            if includeExample != "" and not self.IsTextInNode(entry, includeExample):
                continue
            map = {}
            thisentry = self.JustNode(entry)
            map["Email"] = email
            map["Root"] = mediaRoot
            map["lastSearch"] = map["Root"] + 'index.htm'
            map["Stem"] = self.FindItem(thisentry, '//x:span[@class="LexEntry-publishRootPara-MyLex"]//text()')
            if map["Stem"] == '':
                map["Stem"] = self.FindItem(thisentry, '//x:span[@class="headword"]//text()')
            
            map["Background"] = ''
            citations = self.FindAll(thisentry, '//x:span[@class="LexEntry-publishRootPara-CitationFormPub_L3"]/x:span[@class="xitem"]/x:span/text()')
            if not len(citations):
                citations = self.FindAll(thisentry, '//x:span[@class="LexEntry-publishRootPara-CitationFormPub_L3"]/x:span/text()')
            citation = ''
            for candidate in citations:
                normalizedCandidate = self.FixSpace(candidate).strip()
                if normalizedCandidate != "":
                    citation +=  normalizedCandidate + " "
            map["Citation"] = citation
            if self.SanitizeName(map["Stem"]) == "" and map["Citation"] == "":
                continue
            map["Definition"] = self.FindItem(thisentry, '//x:span[@class="definition"]/x:span/text()')
            map["PartOfSpeech"] = self.FindItem(thisentry, '//x:span[@class="partofspeech"]/x:span/text()')
            map["SoundFile"] = self.FindItem(thisentry, '//x:span[@class="LexEntry-publishRootPara-Sound--File"]/text()')
            if wordpress:
                termTable.AddTerm(postFile.N(), map["PartOfSpeech"], 3)
                cat = self.FindItem(thisentry, '//x:span[@class="LexSense-publishRoot-Semantic--Field"]/x:span/text()')
                if cat != '':
                    termTable.AddSem(postFile.N(), cat)
            map["ImageFile"] = self.FindItem(thisentry, '//x:img/@src')
            map["ImageCaption"] = map["Citation"]
            if map["ImageCaption"] == '':
                map["ImageCaption"] = map["Stem"]
            if map["ImageFile"] != "":
                map["Media"] = mediatpl % map;
            else:
                map["Media"] = ""
            map["Date"] = datetime.date.today().strftime('%B %d, %Y')  # like June 16, 2010
            actualCount += 1
            if user:
                outdata = etree.tostring(thisentry).encode('utf-8')
                map["Entry"] = outdata
            else:
                subentries = self.FindAll(thisentry, '//x:div[@class="subentry"]')
                subentrytext = u''
                #Put recognized subentry types out in order
                for curType in self.subentryorder:
                    for subentry in subentries:
                        if self.ProcessSubentry(subentry, curType, includeExample, map):
                            subentrytext += subentrytpl % map
                for subentry in subentries:
                    if self.ProcessSubentry(subentry, "", includeExample, map):
                        subentrytext += subentrytpl % map
                map["SubEntries"] = subentrytext
                self.MoreExamples(thisentry, '', includeExample, map)
                outdata = tabletpl % map
                map["Table"] = outdata
            outname = self.UniqueName(self.SanitizeName(map["Stem"])) + ".htm"
            if wordpress:
                postFile.Write(outname, outdata, map)
            if folders:
                outname = self.UseFolders(outname)
            if site:
                outdata = entrytpl % map
                self.MakeFile(os.path.join(outpath, outname), outdata)
                indexPage.Add(map, outname)
        print actualCount, "filtered entrie(s)"
        if wordpress:
            postFile.Close()
            termTable.Write()
            termTable.Close()
        if site:
            indexPage.Make(outpath)

    def ProcessSubentry(self, subentry, curType, inc, map):
        """ProcessSubentry(subentry, curType, inc, map) - create output for each subentry
        
        subentry = etree of xhtml for the subentry
        curType = if not empty, the type must agree with this one
        inc = if not empty, the subentry must include this text
        map = fields found so far in entry (results are added to this dictionary)
        """
        if not self.IsTextInNode(subentry, inc):
            return False
        thissub = self.JustNode(subentry)
        subtype = self.FindItem(thissub, '//x:span[@class="entry-type-abbr-sub"]/x:span/text()')
        if curType != "" and subtype != curType:
            return False
        if curType == "" and subtype in self.subentryorder:
            return False
        map["Background"] = subtype
        if self.typedisplay.has_key(subtype):
            subtype = self.typedisplay[subtype]
        map["Example"] = u''
        map["Translation"] = u''
        self.MoreExamples(thissub, '-sub', inc, map)
        Form = self.FindItem(thissub, '//x:span[@class="headword-sub"]/text()')
        if Form != "":
            map["Type"] = subtype + ": " + Form
        else:
            map["Type"] = subtype
        return True
    
    def MoreExamples(self, thissub, tag, inc, map):
        """MoreExamples(thissub, tag, inc, map) - generate text for additional examples
        
        thissub = etree of xhtml for the subentry
        tag = '-sub' if part of a subentry otherwise empty string for main entry
        inc = if not empty, this text must be included in the example
        map = fields found so far in entry (results are added to this dictionary)
        """
        map["MoreExamples"] = u''
        examples = self.FindAll(thissub, '//x:span[@class="examples%s"]/x:span[@class="xitem"]' % tag)
        if not len(examples):
            examples = self.FindAll(thissub, '//x:span[@class="examples%s"]' % tag)
        if inc:
            examples = [x for x in examples if self.IsTextInNode(x, inc)]
        if len(examples):
            start = 0
            if tag == '-sub':
                example1 = self.JustNode(examples[0])
                map["Form"] = self.FixSpace(self.FindItem(example1, '//x:span/x:span/x:span[@lang="chr"]/text()'))
                map["Example"] = self.FindItem(example1, '//x:span/x:span/x:span[@lang="chr-x-SP"]/text()')
                map["Translation"] = self.FindItem(example1, '//x:span[@class="translation%s"]/x:span[@lang="en"]/text()' % tag)
                start = 1
            exampletext = u''
            combine = False
            for example in examples[start:]:
                thisexample = self.JustNode(example)
                if combine:
                    combine = False
                else:
                    map["AddForm"] = self.FixSpace(self.FindItem(thisexample, '//x:span/x:span/x:span[@lang="chr"]/text()'))
                map["AddExample"] = self.FindItem(thisexample, '//x:span/x:span/x:span[@lang="chr-x-SP"]/text()')
                map["AddTranslation"] = self.FindItem(thisexample, '//x:span[@class="translation%s"]/x:span[@lang="en"]/text()' % tag)
                if map.has_key("Example") and map["Example"] == '' and map["Translation"] == '' and map["AddForm"] == '':
                    map["Example"] = map["AddExample"]
                    map["Translation"] = map["AddTranslation"]
                    continue
                if map["AddForm"] != '' and map["AddExample"] == '' and map["AddTranslation"] == '':
                    combine = True
                    continue
                exampletext += self.exampletpl % map
            map["MoreExamples"] = exampletext

    def UseFolders(self, outname):
        """UseFolders(outname) - modify name to place in a folder according to first letter"""
        return outname[:1] + '/' + outname

    def MakeFile(self, outname, outdata):
        """MakeFile(outname, outdata) - create file for one entry"""
        dirname = os.path.dirname(outname)
        if not os.path.isdir(dirname):
            os.mkdir(dirname)
        f = open(outname,'w')
        f.write(outdata.encode('utf-8'))
        f.close()

    def FixSpace(self, s):
        """Replace incorrectly converted spaces in string s"""
        return s.replace(u'\ufffd', ' ')

    def JustNode(self, node):
        """return tree with just the node"""
        xml = etree.tostring(node, encoding='utf-8', xml_declaration=True)
        return etree.XML(xml)

    def IsTextInNode(self, node, text):
        """True if node contains text"""
        xml = etree.tostring(node, encoding='utf-8', xml_declaration=True)
        return xml.find(text) >= 0

    def FindAll(self, root, xpath):
        """return a list of results of applying xpath to root"""
        XHTML_NAMESPACE = "http://www.w3.org/1999/xhtml"
        return root.xpath(xpath, namespaces = {'x': XHTML_NAMESPACE})

    def FindItem(self, node, xpath):
        """return a string with the first occurance of xpath in node"""
        r = self.FindAll(node, xpath)
        if len(r):
            return ''.join(r)
        return ""

    def SanitizeName(self, proposal):
        """return a string only containing alphanumeric characters of proposal"""
        name = u''
        for c in proposal:
            if c.isalpha() or c.isdigit():
                name += c
        return name

    def UniqueName(self, name):
        """Return a unique name by adding digits as necessary."""
        if self.namesinuse.has_key(name):
            self.namesinuse[name] += 1
        else:
            self.namesinuse[name] = 1
        return name + str(self.namesinuse[name])

def Usage():
    """display documentation of program for user"""
    sys.stderr.write(BlogExport.__doc__)
    sys.exit(2)

if __name__ == '__main__':
    longopt = ['--email=', '--media-root=', '--wordpress', '--site', '--folders', 
    '--post-index=', '--taxonomy-index=', '--database=', '--table-prefix=', '--user-styles', '--include-example=']
    try:
        optlist, args = getopt.getopt(sys.argv[1:], 'e:m:wsft:p:n:d:ui:', longopt)
    except getopt.GetoptError:
        Usage()
    xhtml = ''
    #xhtml = etree.parse('excerpt.xhtml')
    #xhtml = etree.parse('C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee Subentries/Dictionary/FieldworksStyles_2010-06-23_0317/main.xhtml')
    #xhtml = etree.parse('C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee Sample Subentries/Dictionary/FieldworksStyles_2010-06-18_0339/main.xhtml')
    #xhtml = 'C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee/Dictionary/FieldWorksStyles_2010-08-19_0251/main.xhtml'
    #xhtml = 'C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee/Dictionary/FieldWorksStyles_2010-08-26_1117/main.xhtml'
    email = 'ka.gilliland@cnec-edu.org'
    #mediaRoot = 'http://localhost/wordpress/wp-content/uploads'
    mediaRoot = 'http://www.cnec-edu.org/wp-cherokee/wp-content/uploads'
    #mediaRoot = datetime.date.today().strftime('http://www.cnec-edu.org/wordpress/wp-content/uploads/%Y/%m/')
    #mediaRoot = datetime.date.today().strftime('http://localhost/wordpress/wp-content/uploads/%Y/%m/')
    #mediaRoot = datetime.date.today().strftime('image/') # use ../ if UseFolders is used below
    wordpress = True
    site = False
    folders = False
    postIndex = 5
    taxonomyIndex = 10
    #database = 'cnec-wp'
    database = 'cherokee_wordpress'
    prefix = 'wp_'
    user = False
    includeExample = ''
    for o, a in optlist:
        if o in ("-e", "--email"):
            email = a
        elif o in ("-m", "--media-root"):
            mediaRoot = a
        elif o in ("-w", "--wordpress"):
            wordpress = not wordpress
        elif o in ("-s", "--site"):
            site = not site
        elif o in ("-f", "--folders"):
            folders = not folders
        elif o in ("-p", "--post-index"):
            postIndex = int(a)
        elif o in ("-t", "--taxonomy-index"):
            taxonomyIndex = int(a)
        elif o in ("-n", "--database"):
            database = a
        elif o in ("-d", "--table-prefix"):
            prefix = a
        elif o in ("-u", "--user-styles"):
            user = True
        elif o in ("-i", "--include-example"):
            includeExample = a
    if len(args):
        xhtml = args[0]
    if xhtml == '':
        Usage()
    x = BlogExport(xhtml, email=email, mediaRoot=mediaRoot, wordpress=wordpress, 
    site=site, folders=folders, postIndex=postIndex, taxonomyIndex=taxonomyIndex,
    database=database, prefix=prefix, user=user, includeExample=includeExample)
