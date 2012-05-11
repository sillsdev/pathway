#-----------------------------------------------------------------------------
# Name:        Xhtml2Blog.py
# Purpose:     Import xhtml (possibly processed by xslt) into blog site
#
# Author:      Greg Trihus <greg_trihus@sil.org>
#
# Created:     2010/06/16 (Python 2.5)
# RCS-ID:      $Id: etreeLoad.py $
# Copyright:   (c) 2011 SIL International
# Licence:     LPGL
#-----------------------------------------------------------------------------
#!/usr/bin/env python
#Boa:PyApp:main

import os,sys,datetime,getopt
from lxml import etree

modules ={}

navTpl = '''<table style="border:medium none white;width:100%%;"><tr><td>%s</td><td style="text-align:right;">%s</td></tr></table>'''
lfArrowTpl = '''<a href="?p=%d"><img src="%s/wp-content/icons/ArrowLeft.png" width="32" height="32" border="0" alt="&lt;&#x2014;" /></a>'''
rtArrowTpl = '''<a href="?p=%d"><img src="%s/wp-content/icons/ArrowRight.png" width="32" height="32" border="0" alt="&#x2014;&gt;" /></a>'''

def FixSpace(s):
    """Replace incorrectly converted spaces in string s"""
    return s.replace(u'\ufffd', '')

class IndexPage:
    """Generate the index page"""
    pages = {}
    def Add(self, stem, citation, outname):
        """Add(map, outname) - add one item to index
        
        map = fields from XHTML file (used to generate reference text)
        outname = name of file referred to by link
        """
        link = "%s (%s)" % (citation, stem)
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
    def __init__(self, addr, outpath, prefix='wp_', database='wordpress'):
        self.database=database
        self.prefix = prefix
        self.file = open(os.path.join(outpath,'data.sql'),'w')
        self.DelQuery()
        self.NewQuery()
        self.cont = ''
        self.site = addr
        self.n = 0
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
    def Write(self, idx, outname, outdata, stem, citation):
        """Write(idx, outname, outdata, map) - Write the data for one post
        
        idx = post id
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
        title = "%s (%s)" % (citation, stem)
        title = title.replace("'", "''")
        rec = u"(%d, 1, '%s', '%s', '%s', '%s', '%s', " % (idx, wpdate, wpudate, page, title, title)
        rec += "'publish', 'open', 'open', '', '%s', '', '', '%s', '%s', " % (outname, wpdate, wpudate)
        rec += "'', 0, '%s/?p=%d', 0, 'post', '', 0)" % (self.site, idx)
        self.n += 1
        if self.n == 10:
            self.EndQuery()
            self.NewQuery()
            self.n = 0
        else:
            self.file.write(self.cont)
        self.file.write(rec.encode('utf-8'))
        self.cont = ',\n'
    def Close(self):
        """Close file with query to add posts"""
        self.EndQuery()
        self.file.close()

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
        self.initn = 20
        self.n = self.initn
        self.term = {'Main':14, 'Language':15}
        self.slug = {14:'main', 15:'language'}
        self.firsttoc = ''
        self.toc = []
        self.tocPageCount = []
        self.tocPageTerm = []
        self.tocPageMax = 10
        self.count = {1:0}
        self.parent = {1:0}
        self.rel = {}
        self.taxn = taxn
        self.cont = ''
    def _FirstLetter(self, value):
        value = FixSpace(value)
        firstletter = value[:1]
        if firstletter == '-':
            firstletter = value[1:2]
        return firstletter.upper() + " - " + firstletter.lower()
    def AddToc(self, list):
        for value in list:
            if FixSpace(value) == '': continue
            firstletter = self._FirstLetter(value)
            if not firstletter in self.toc:
                if self.firsttoc == '':
                    self.firsttoc = self.n
                self.toc.append(firstletter)
                self.tocPageCount.append(0)
                self.tocPageTerm.append(0)
                self.AddTerm(0, firstletter, firstletter, 0)
    def AddTocItem(self, post, value):
        firstletter = self._FirstLetter(value)
        idx = self.toc.index(firstletter)
        if self.tocPageCount[idx] % self.tocPageMax == 0:
            self.AddTerm(post, value, value, idx + self.firsttoc)
            self.tocPageTerm[idx] = value
        else:
            term = self.tocPageTerm[idx]
            self.AddTerm(post, term, term, idx + self.firsttoc)
        self.tocPageCount[idx] += 1
    def AddLanguage(self, slug, name):
        """
        AddLanguage(slug, name)
        slug = language token
        name = language full name
        """
        self.AddTerm(0, name, slug, -1)
    def AddSem(self, post, domain):
        """
        AddSem(r, s)
        post = post #
        domain = semantic domain items (space separated)
        """
        semCats = domain.split()
        for term in semCats:
            self.AddTerm(post, term, term, -3)
    def AddTerm(self, post, term, slug, parent):
        """
        AddTerm(post, term, slug, parent)
        post = post #
        term = term to add
        parent = index to parent of term (2 for Semantic Domain, 3 for Part of speech)
        """
        if parent < -1:
            key = term.lower()
        else:
            key = term
        if term == '':
            i = 1
            self.count[1] += 1
        elif self.term.has_key(key):
            i = self.term[key]
            self.count[i] += 1
        else:
            i = self.n
            self.term[key] = i
            self.slug[i] = slug
            self.count[i] = 1
            self.parent[i] = parent
            self.n += 1
        if post == 0:
            self.count[i] = 999999
            return
        if self.rel.has_key(post):
            try:
                self.rel[post].index(i)
            except:
                self.rel[post].append(i)
        else:
            self.rel[post] = [i]
    def Write(self):
        """Write() - write file containing queries to add terms to wordpress blog site"""
        self.file = open(os.path.join(self.outpath,'data.sql'),'a')
        idx = {}
        for key in self.term.keys():
            idx[self.term[key]] = key
        self.DelTermQuery()
        self.NewTermQuery()
        n = 0
        keys = idx.keys()
        keys.sort()
        for tn in keys:
            rec = u"(%d, '%s', '%s', 0)" % (tn, idx[tn].replace("'", "''"), self.slug[tn].replace("'", "''"))
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
            parent = self.parent[cat]
            if parent == -1:
                parent = 0
                description = 'sil_writing_systems'
            elif parent == -2:
                parent = 0
                description = 'sil_parts_of_speech'
            elif parent == -3:
                parent = 0
                description = 'post_tag'
            else:
                description = 'category'
            rec = u"(%d, %d, '%s', '', %d, %d)" % (self.taxn, cat, description, parent, self.count[cat])
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
        self.file.write('DELETE FROM `%s`.`%sterms` WHERE `%sterms`.`term_id` > 13;\n' % (self.database, self.prefix, self.prefix))
    def NewTermQuery(self):
        """Write beginning of query to insert new terms"""
        insql = "INSERT INTO `%sterms` (`term_id`, `name`, `slug`, `term_group`) VALUES\n" % self.prefix
        self.file.write(insql)
        self.cont = ''
    def DelRelQuery(self):
        """Write query to delete relationships of posts to term taxonomies"""
        self.file.write('DELETE FROM `%s`.`%sterm_relationships` WHERE `%sterm_relationships`.`term_taxonomy_id` > 15;\n' % (self.database, self.prefix, self.prefix))
    def NewRelQuery(self):
        """Write beginning of query to insert new relationships of posts to term taxonomies"""
        insql = "INSERT INTO `%sterm_relationships` (`object_id`, `term_taxonomy_id`, `term_order`) VALUES\n" % self.prefix
        self.file.write(insql)
        self.cont = ''
    def DelTaxQuery(self):
        """Write query to delete term taxonomies"""
        self.file.write('DELETE FROM `%s`.`%sterm_taxonomy` WHERE `%sterm_taxonomy`.`term_id` > 15;\n' % (self.database, self.prefix, self.prefix))
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

class MultiLingualSearch:
    """
    obj = MultiLingualSearch(outputpath, database)
    outputpath = where to put the query file
    database = name of the MySql database to contain the table
    """
    HeadwordRelevance = 100
    LexemeRelevance = 70
    DefinitionRelevance = 50
    ExampleRelevance = 10
    def __init__(self, outpath, database='wordpress'):
        self.database=database
        self.file = open(os.path.join(outpath,'data.sql'),'a')
        self.DelQuery()
        self.NewQuery()
        self.content = []
        self.n = 0
        self.cont = ''
    def Add(self, post, language, relevance, text):
        """
        Add(post, language, relevance, text)
        post = post id
        language = language slug
        relevance = numeric weight (see below)
        text = text to search for
        """
        self.content.append((post, language, relevance, text.replace("'", "''")))
    def DelQuery(self):
        """Write query to delete previous wordpress posts"""
        self.file.write('DELETE FROM `%s`.`sil_multilingual_search` ;\n' % self.database)
    def NewQuery(self):
        """Write beginning of query to insert new posts"""
        self.file.write("INSERT INTO `sil_multilingual_search` (`post_id`, `language_code`, `relevance`, `search_strings`) VALUES\n")
    def EndQuery(self):
        """Write end of query"""
        self.file.write(';\n')
    def Write(self):
        """Write() - Write the data for the multilingual search table
        """
        for line in self.content:
            rec = u"(%d, '%s', %d, '%s')" % line
            self.n += 1
            if self.n == 10:
                self.EndQuery()
                self.NewQuery()
                self.n = 0
            else:
                self.file.write(self.cont)
            self.file.write(rec.encode('utf-8'))
            self.cont = ',\n'
    def Close(self):
        """Close file with query to add posts"""
        self.EndQuery()
        self.file.close()

class Xhtml2Blog:
    """
    Xhtml2Blog [opts] xhtml
    
    Convert dictionary xhtml to a blog site
    
    \t-a, --arrows\t\tadd toc and arrows to entries
    \t-e, --email=\t\temail address for response
    \t-m, --media-root=\turl to root of site
    \t-w, --wordpress\t\ttoggle whether wordpress sql input generated
    \t-s, --site\t\ttoggle whether html web site files generated
    \t-f, --folders\t\ttoggle whether html files are put in folders
    \t-n, --database=\tdatabase name
    \t-p, --post-index=\tstarting index key for posts
    \t-t, --taxonomy-index=\tstarting index key for taxonomies
    \t-d, --table-prefix=\ttable prefix (normally wp_)
    \t-i, --include-example=\tText which must be contained in example
    """
    def __init__(self, xhtml, url, wordpress=True, site=False, folders=False, arrows=False, postIndex=85, taxonomyIndex=10, database='wordpress', prefix='wp_', includeExample=''):
        self.namesinuse = {}
        root = etree.parse(xhtml)
        outpath = os.path.dirname(xhtml)
        if outpath == '':
            outpath = '.'
        entries = self.FindAll(root, '//x:div[@class="entry"]')
        ids = self.FindAll(root, '//x:div[@class="entry"]/@id')
        print len(entries), "entries"
        actualCount = 0
        if site:
            indexPage = IndexPage()
        if wordpress:
            postFile = PostFile(url, outpath, prefix, database)
            multiLingualSearch = MultiLingualSearch(outpath, database)
            termTable = TermTable(taxonomyIndex, outpath, prefix, database)
            languages = self.FindAll(root, '//x:meta[@scheme="Language Name"]')
            for language in languages:
                thislanguage = self.JustNode(language)
                slug = self.FindItem(thislanguage, './@name')
                fullName = self.FindItem(thislanguage, './@content')
                termTable.AddLanguage(slug, fullName)
            stemLanguage = ''
            if arrows:
                termTable.AddToc(self.FindAll(root, '//*[@class="def"]//text()'))
                termTable.AddToc(self.FindAll(root, '//*[@class="headword"]/text()'))
        for e in range(len(entries),0,-1):
            nav = ''
            entry = entries[e-1]
            if includeExample != "" and not self.IsTextInNode(entry, includeExample):
                continue
            thisentry = self.JustNode(entry)
            id = self.FindItem(thisentry, './@id')
            idx = ids.index(id) + postIndex
            stem = self.FindItem(thisentry, '*[@class="headword"]//text()')
            citation = FixSpace(self.FindItem(thisentry, '//*[@class="def"]//text()'))
            partofspeech = self.FindItem(thisentry, '//*[@class="grammatical-info"]//text()')
            if self.SanitizeName(stem) == "" and citation == "":
                continue
            if wordpress:
                if stemLanguage == '':
                    stemLanguage = self.FindItem(thisentry, '*[@class="headword"]/@lang')
                    citationLanguage = self.FindItem(thisentry, '//*[@class="def"]/*/@lang')
                    definitionLanguage = self.FindItem(thisentry, '//*[@class="definition"]/@lang')
                multiLingualSearch.Add(idx, stemLanguage, multiLingualSearch.LexemeRelevance, stem)
                if citation != '':
                    multiLingualSearch.Add(idx, citationLanguage, multiLingualSearch.HeadwordRelevance, citation)
                definition = self.FindItem(thisentry, '//*[@class="definition"]//text()')
                if definition != '':
                    multiLingualSearch.Add(idx, definitionLanguage, multiLingualSearch.DefinitionRelevance, definition)
                cherokeeExample = self.FindItem(thisentry, '//*[@class="syll"]//text()')
                if cherokeeExample != '':
                    multiLingualSearch.Add(idx, citationLanguage, multiLingualSearch.ExampleRelevance, cherokeeExample)
                example = self.FindItem(thisentry, '//*[@class="exam"]//text()')
                if example != '':
                    multiLingualSearch.Add(idx, stemLanguage, multiLingualSearch.ExampleRelevance, example)
                translation = self.FindItem(thisentry, '//*[@class="tran"]//text()')
                if translation != '':
                    multiLingualSearch.Add(idx, definitionLanguage, multiLingualSearch.ExampleRelevance, translation)

                if arrows:
                    termTable.AddTocItem(idx, stem)
                    if e > 0:
                        navlf = lfArrowTpl % (idx-1, url)
                    else:
                        navlf = ''
                    if e < len(entries) -1:
                        navrt = rtArrowTpl % (idx+1, url)
                    else:
                        navrt = ''
                    nav = navTpl % (navlf, navrt)
                    if citation != '':
                        termTable.AddTocItem(idx, citation)
                termTable.AddTerm(idx, partofspeech, partofspeech, -2)
                cats = self.FindAll(thisentry, '//*[@class="semantic-domains"]/x:span/text()')
                for cat in cats:
                    if cat != '':
                        termTable.AddSem(idx, cat)
                anchors = self.FindAll(thisentry, '//x:a')
                for anchor in anchors:
                    linkId = anchor.attrib['href'][1:]
                    try:
                        linkIdx = ids.index(linkId)
                        anchor.attrib['href'] = '?p=%d' % (linkIdx + postIndex)
                    except:
                        anchor.attrib['href'] = '?p=%d' % idx
            actualCount += 1
            outdata = nav + etree.tostring(thisentry).encode('utf-8').replace('&#65533;','')
            outname = self.UniqueName(self.SanitizeName(stem)) + ".htm"
            if wordpress:
                postFile.Write(idx, outname, outdata, stem, citation)
            if folders:
                outname = self.UseFolders(outname)
            if site:
                self.MakeFile(os.path.join(outpath, outname), outdata)
                indexPage.Add(stem, citation, outname)
        print actualCount, "filtered entrie(s)"
        if wordpress:
            postFile.Close()
            termTable.Write()
            termTable.Close()
            multiLingualSearch.Write()
            multiLingualSearch.Close()
        if site:
            indexPage.Make(outpath)

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
            return r[0]
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
    sys.stderr.write(Xhtml2Blog.__doc__)
    sys.exit(2)

if __name__ == '__main__':
    longopt = ['--wordpress', '--site', '--folders', '--arrows', '--url',
    '--post-index=', '--taxonomy-index=', '--database=', '--table-prefix=', '--include-example=']
    try:
        optlist, args = getopt.getopt(sys.argv[1:], 'wsfau:t:p:n:d:i:', longopt)
    except getopt.GetoptError:
        Usage()
    xhtml = ''
    #xhtml = etree.parse('excerpt.xhtml')
    #xhtml = etree.parse('C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee Subentries/Dictionary/FieldworksStyles_2010-06-23_0317/main.xhtml')
    #xhtml = etree.parse('C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee Sample Subentries/Dictionary/FieldworksStyles_2010-06-18_0339/main.xhtml')
    #xhtml = 'C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee/Dictionary/FieldWorksStyles_2010-08-19_0251/main.xhtml'
    #xhtml = 'C:/Users/Trihus.DALLAS/Documents/Publications/Cherokee/Dictionary/FieldWorksStyles_2010-08-26_1117/main.xhtml'
    #url = 'http://www.cherokeenationfoundation.org/dictionary'
    url = 'http://localhost/wordpress'
    wordpress = True
    site = False
    folders = False
    arrows = False
    postIndex = 20
    taxonomyIndex = 20
    #database = 'cnec-wp'
    #database = 'cherokee_wordpress'
    database = 'wordpress'
    prefix = 'wp_'
    user = False
    includeExample = ''
    for o, a in optlist:
        if o in ("-w", "--wordpress"):
            wordpress = not wordpress
        elif o in ("-s", "--site"):
            site = not site
        elif o in ("-f", "--folders"):
            folders = not folders
        elif o in ("-a", "--arrows"):
            arrows = not arrows
        elif o in ("-p", "--post-index"):
            postIndex = int(a)
        elif o in ("-t", "--taxonomy-index"):
            taxonomyIndex = int(a)
        elif o in ("-u", "--url"):
            url = a
        elif o in ("-n", "--database"):
            database = a
        elif o in ("-d", "--table-prefix"):
            prefix = a
        elif o in ("-i", "--include-example"):
            includeExample = a
    if not len(args):
        Usage()
    xhtml = args[0]
    if xhtml == '':
        Usage()
    x = Xhtml2Blog(xhtml, url=url, wordpress=wordpress, site=site, folders=folders, arrows=arrows,
    postIndex=postIndex, taxonomyIndex=taxonomyIndex,
    database=database, prefix=prefix, includeExample=includeExample)
