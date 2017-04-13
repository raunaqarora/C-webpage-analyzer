using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HW3Part2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/top10words?v={url}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string top10words(string url);
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/top10content?v={url}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string top10wordscontent(string url);
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/customStop?v={url}&u={stopwords}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string customStop(string url,  string stopWords);
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/replaceWithSynonymsAntonyms?v={url}&u={wordsToReplace}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string[] replaceWithSynonymsAntonyms(string url, string wordsToReplace);
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/averageWordsPerSentence?v={url}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        decimal averageWordsPerSentence(string url);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/encryptDecrypt?v={passPhrase}&u={encrypt}", BodyStyle = WebMessageBodyStyle.Bare)]
        string encryptDecrypt(string passPhrase, string encrypt);
        string helper(string url);
        string encrypt(string url, string passPhrase);
        string decrypt(string cipherText, string passPhrase);
        List<String> removeStop(List<String> words);
    }
}
