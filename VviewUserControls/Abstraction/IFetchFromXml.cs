using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VviewUserControls.Abstraction
{
   public interface IFetchFromXml
    {
        List<string> FetchBooknames(string lang);
        List<string> FetchOtherLangBooknames(string lang);
        List<string> FetchChapterNos(string lang,int bookno);
        List<List<string>> FetchVerseNos(string lang, int bookno, string chapterno);
        List<string> FetchVersesList();
    }
}
