using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VviewUserControls.Abstraction;

namespace VviewUserControls.Concrete
{
    public class XmlHandler : IFetchFromXml
    {
        List<string> BookNames;
        List<string> ChapterNos;
        List<string> VerseNos;
        List<string> VerseList;
        string recfile = "";
        XmlDocument xml_base = new XmlDocument();

        public List<string> FetchBooknames(string lang)
        {
            BookNames = new List<string>();
            //recfile = Environment.CurrentDirectory + "/Bible/" + lang + ".xml";
            recfile = Environment.CurrentDirectory + "/Bible/English.xml";
            FileStream fsk = new FileStream(recfile, FileMode.Open, FileAccess.Read);
            xml_base.Load(fsk);
            XmlNode xmlgendata = xml_base.SelectSingleNode("bible/booknames");
            //XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible");
            XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible/b");
            foreach(XmlNode node in xmlgendlistdata)
            {
                BookNames.Add(node.Attributes["nm"].Value);
            }
            //XmlElement xelegendata = (XmlElement)xmlgendata;
            //if (!String.IsNullOrEmpty(xelegendata.InnerText.ToString())){
            //    BookNames = xelegendata.InnerText.ToString().Split(',').ToList();

            //}
            fsk.Close();
            return BookNames;
            
            //throw new NotImplementedException();
        }

        public List<string> FetchOtherLangBooknames(string lang)
        {
            BookNames = new List<string>();
            recfile = Environment.CurrentDirectory + "/Bible/" + lang + ".xml";
            //recfile = Environment.CurrentDirectory + "/Bible/English.xml";
            FileStream fsk = new FileStream(recfile, FileMode.Open, FileAccess.Read);
            xml_base.Load(fsk);
            XmlNode xmlgendata = xml_base.SelectSingleNode("bible/booknames");

            //XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible");
            //XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible/b");
           

                BookNames = xmlgendata.InnerText.Split(',').ToList();
          
            //XmlElement xelegendata = (XmlElement)xmlgendata;
            //if (!String.IsNullOrEmpty(xelegendata.InnerText.ToString())){
            //    BookNames = xelegendata.InnerText.ToString().Split(',').ToList();

            //}
            fsk.Close();
            return BookNames;

            //throw new NotImplementedException();
        }

        public List<string> FetchChapterNos(string lang,int bookno)
        {
            ChapterNos = new List<string>();
            recfile = Environment.CurrentDirectory + "/Bible/" + lang + ".xml";

            FileStream fsk = new FileStream(recfile, FileMode.Open, FileAccess.Read);
            xml_base.Load(fsk);
            XmlNode xmlgendata = xml_base.SelectSingleNode("bible/booknames");
            //XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible");
            XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible/b");
            foreach (XmlNode node in xmlgendlistdata)
            {
                if ((node.Attributes["n"].Value.Equals(bookno.ToString().Trim())))
                {                    
                    foreach (XmlNode chapternode in node.ChildNodes)
                    {
                        ChapterNos.Add(chapternode.Attributes["n"].Value);
                    }
                }
            }
            //XmlElement xelegendata = (XmlElement)xmlgendata;
            //if (!String.IsNullOrEmpty(xelegendata.InnerText.ToString())){
            //    BookNames = xelegendata.InnerText.ToString().Split(',').ToList();

            //}
            fsk.Close();
            return ChapterNos;
        }

        public List<List<string>> FetchVerseNos(string lang, int bookno, string chapterno)
        {
            List<List<string>> mylist = new List<List<string>>();
            VerseNos = new List<string>();
            VerseList = new List<string>();
            recfile = Environment.CurrentDirectory + "/Bible/" + lang + ".xml";

            FileStream fsk = new FileStream(recfile, FileMode.Open, FileAccess.Read);
            xml_base.Load(fsk);
            XmlNode xmlgendata = xml_base.SelectSingleNode("bible/booknames");
            //XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible");
            XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible/b");
            foreach (XmlNode node in xmlgendlistdata)
            {
                if ((node.Attributes["n"].Value.Equals(bookno.ToString().Trim())))
                {
                    foreach (XmlNode chapternode in node.ChildNodes)
                    {
                        if ((chapternode.Attributes["n"].Value.Equals(chapterno)))
                        {
                            foreach (XmlNode versenode in chapternode.ChildNodes)
                            {
                                VerseNos.Add(versenode.Attributes["n"].Value);
                                VerseList.Add(versenode.Attributes["n"].Value +". "+ versenode.InnerText);
                            }                               
                        }                           
                    }
                }
            }
            //XmlElement xelegendata = (XmlElement)xmlgendata;
            //if (!String.IsNullOrEmpty(xelegendata.InnerText.ToString())){
            //    BookNames = xelegendata.InnerText.ToString().Split(',').ToList();

            //}
            fsk.Close();
            mylist.Add(VerseNos);
            mylist.Add(VerseList);
            return mylist;
        }

        public List<string> FetchVersesList()
        {
            //VerseNos = new List<string>();
            VerseList = new List<string>();
            //recfile = Environment.CurrentDirectory + "/Bible/" + lang + ".xml";

            //FileStream fsk = new FileStream(recfile, FileMode.Open, FileAccess.Read);
            //xml_base.Load(fsk);
            //XmlNode xmlgendata = xml_base.SelectSingleNode("bible/booknames");
            ////XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible");
            //XmlNodeList xmlgendlistdata = xml_base.SelectNodes("bible/b");
            //foreach (XmlNode node in xmlgendlistdata)
            //{
            //    if ((node.Attributes["n"].Value.Equals(bookname)))
            //    {
            //        foreach (XmlNode chapternode in node.ChildNodes)
            //        {
            //            if ((chapternode.Attributes["n"].Value.Equals(chapterno)))
            //            {
            //                foreach (XmlNode versenode in chapternode.ChildNodes)
            //                {
            //                    //VerseNos.Add(versenode.Attributes["n"].Value);
            //                    VerseList.Add(versenode.Attributes["n"].Value + ". " + versenode.InnerText);
            //                }
            //            }
            //        }
            //    }
            //}
            ////XmlElement xelegendata = (XmlElement)xmlgendata;
            ////if (!String.IsNullOrEmpty(xelegendata.InnerText.ToString())){
            ////    BookNames = xelegendata.InnerText.ToString().Split(',').ToList();

            ////}
            //fsk.Close();
            return VerseList;
        }
    }
}
