using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Xml;

namespace HW3Part2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private const string initVector = "armgay4kuzpgzl51";
        private const int keysize = 256;
        public string top10words(string url)
        {
            string response = "Top 10 most used words are: ";
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] wordArr = helper(url).Split(delimiterChars);
            List<string> words = new List<string>(wordArr);
            foreach (string word in words)
            {
                //word.Replace(" ","");
                if (word.Length >= 3)
                {
                    if (dictionary.ContainsKey(word.ToLower()))
                        dictionary[word.ToLower()] = dictionary[word.ToLower()] + 1;
                    else
                        dictionary[word.ToLower()] = 1;
                }
            }
            dictionary = dictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            for (int i = 0; i < 10; i++)
            {
                response += dictionary.ElementAt(i);
                response += " ";
            }

            return response;
        }
        public string top10wordscontent(string url)
        {
            string response = "Top 10 most used content words are: ";
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] wordArr = helper(url).Split(delimiterChars);
            List<string> words = new List<string>(wordArr);
            words.RemoveAll(item=> item == "");
            words = removeStop(words);
            foreach (string word in words)
            {
                //word.Replace(" ","");
                if (word.Length >= 3)
                {
                    if (dictionary.ContainsKey(word.ToLower()))
                        dictionary[word.ToLower()] = dictionary[word.ToLower()] + 1;
                    else
                        dictionary[word.ToLower()] = 1;
                }
            }
            dictionary = dictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            for (int i = 0; i < 10; i++)
            {
                response += dictionary.ElementAt(i);
                response += " ";
            }
            return response;
        }

        public string[] replaceWithSynonymsAntonyms(string url, string wordsToReplace)
        {
            string content = helper(url);
            content = Regex.Replace(content, " {2,}", " ");
            string[] toReturn = new String[2];
            toReturn[0] = content;
            toReturn[1] = content;
            string synReplaced = String.Empty;
            string antReplaced = String.Empty;
            wordsToReplace = Regex.Replace(wordsToReplace, " {1,}", "");
            string[] wordsArr = wordsToReplace.Split(',');
            List<string> words = new List<string>(wordsArr);
            words.RemoveAll(item => item == "");
            words.RemoveAll(item => item == " ");
            List<string> synonyms = new List<string>();
            try
            {
                foreach (string w in words)
                {
                    w.Trim();
                    string webRequestUrl = "http://words.bighugelabs.com/api/2/"/*Insert API key*/ + w + "/xml";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webRequestUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        String j = reader.ReadToEnd();
                        //dynamic parsedObject = JsonConvert.DeserializeObject(j);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(j);

                        string xpath = "words/w";
                        XmlNodeList nodes = xmlDoc.SelectNodes(xpath);
                        if (nodes.Count > 0)
                        {
                            int index = 0;
                            XmlNode node = nodes.Item(index);
                            while (node.Attributes["r"].Value != "syn" && nodes.Count - 1 > index)
                            {
                                index++;
                                node = nodes.Item(index);
                            }
                            if (node.Attributes["r"].Value == "syn")
                            {
                                synReplaced += w + " replaced with synonym: " + node.InnerText + " ";
                                toReturn[0] = Regex.Replace(toReturn[0], w, node.InnerText, RegexOptions.IgnoreCase);
                            }
                            else
                            {
                                synReplaced += w + " was not replaced by a synonym ";
                            }
                            index = 0;
                            node = nodes.Item(index);
                            while (node.Attributes["r"].Value != "ant" && nodes.Count - 1 > index)
                            {
                                index++;
                                node = nodes.Item(index);
                            }
                            if (node.Attributes["r"].Value == "ant")
                            {
                                antReplaced += w + " replaced with antonym: " + node.InnerText + " ";
                                toReturn[1] = Regex.Replace(toReturn[1], w, node.InnerText, RegexOptions.IgnoreCase);
                            }
                            else
                            {
                                antReplaced += w + " was not replaced by a antonym ";
                            }
                        }
                    }
                    else
                    {
                        synReplaced += w + " was not found ";
                        antReplaced += w + " was not found ";
                        continue;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            toReturn[0] = synReplaced + toReturn[0];
            toReturn[1] = antReplaced + toReturn[1];
            return toReturn;
        }
        public string customStop(string url, string stopWords)
        {
            stopWords = Regex.Replace(stopWords, " {1,}", "");
            string [] stopWordsArr = stopWords.Split(',');
            List<string> stopWordsList = new List<string>(stopWordsArr);
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] wordArr = helper(url).Split(delimiterChars);
            List<string> wordList = new List<string>(wordArr);
            wordList.RemoveAll(item => item == "");
            wordList.RemoveAll(item => stopWordsList.Contains(item.ToLower()));
            return String.Join(" ", wordList.ToArray());
        }
        public string encryptDecrypt(string passPhrase, string encryptIn)
        {
            String url = Encoding.UTF8.GetString(OperationContext.Current.RequestContext.RequestMessage.GetBody<byte[]>());
            string response = String.Empty;
            if(encryptIn == "encrypt")
            {
                response = encrypt(url, passPhrase);
            }
            else
            {
                response = decrypt(url, passPhrase);
            }
            return response;
        }
        public string encrypt(string url, string passPhrase)
        {

            string content = helper(url);
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(content);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        public string decrypt(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        public decimal averageWordsPerSentence(string url)
        {
            string content = helper(url);
            char[] delimiterChars = {'.', '?', '!'};
            char[] delimiterCharsWords = { ' ', ',', '.', ':', '\t' };
            List<string> sentences = new List<string>(content.Split(delimiterChars));
            List<string> words = new List<string>(content.Split(delimiterCharsWords));
            decimal totalWords = words.Count();
            decimal noOfSentences = sentences.Count();
            return decimal.Round((totalWords/noOfSentences), 2, MidpointRounding.AwayFromZero);
        }

        public List<String> removeStop(List<String> words)
        {
            string[] stopWordsArr = {"a", "about", "above", "after", "again", "against", "all", "am", "an", "and", "any", "are", "aren\'t", "as", "at", "be", "because", "been", "before", "being", "below", "between", "both", "but", "by", "can\'t", "cannot", "could", "couldn\'t", "did", "didn\'t", "do", "does", "doesn\'t", "doing", "don\'t", "down", "during", "each", "few", "for", "from", "further", "had", "hadn\'t", "has", "hasn\'t", "have", "haven\'t", "having", "he", "he\'d", "he\'ll", "he\'s", "her", "here", "here\'s", "hers", "herself", "him", "himself", "his", "how", "how\'s", "i", "i\'d", "i\'ll", "i\'m", "i\'ve", "if", "in", "into", "is", "isn\'t", "it", "it\'s", "its", "itself", "let\'s", "me", "more", "most", "mustn\'t", "my", "myself", "no", "nor", "not", "of", "off", "on", "once", "only", "or", "other", "ought", "our", "ours", "ourselves", "out", "over", "own", "same", "shan\'t", "she", "she\'d", "she\'ll", "she\'s", "should", "shouldn\'t", "so", "some", "such", "than", "that", "that\'s", "the", "their", "theirs", "them", "themselves", "then", "there", "there\'s", "these", "they", "they\'d", "they\'ll", "they\'re", "they\'ve", "this", "those", "through", "to", "too", "under", "until", "up", "very", "was", "wasn\'t", "we", "we\'d", "we\'ll", "we\'re", "we\'ve", "were", "weren\'t", "what", "what\'s", "when", "when\'s", "where", "where\'s", "which", "while", "who", "who\'s", "whom", "why", "why\'s", "with", "won\'t", "would", "wouldn\'t", "you", "you\'d", "you\'ll", "you\'re", "you\'ve", "your", "yours", "yourself","yourselves", "|"};
            List<string> stopWords = new List<string>(stopWordsArr);
            words.RemoveAll(item => stopWords.Contains(item));
            return words;
        }

        public string helper(string url)
        {
            if (url.Substring(0,7) != "http://" || url.Substring(0,8) != "https://")
            {
                url = "http://" + url;
            }
            string content = String.Empty;
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead(url))
                using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    content = textReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            HtmlDocument pageDoc = new HtmlDocument();
            pageDoc.LoadHtml(content);
            StringBuilder TempString = new StringBuilder();
            foreach (HtmlNode style in pageDoc.DocumentNode.Descendants("style").ToArray())
            {
                style.Remove();
            }
            foreach (HtmlNode script in pageDoc.DocumentNode.Descendants("script").ToArray())
            {
                script.Remove();
            }
            foreach (HtmlTextNode node in pageDoc.DocumentNode.SelectNodes("//text()"))
            {
                TempString.AppendLine(node.InnerText + " ");
            }
            string toReturn = TempString.ToString();
            toReturn = Regex.Replace(toReturn, " {2,}", " ");
            toReturn = HttpUtility.HtmlDecode(toReturn);
            toReturn = new string(toReturn.Where(c => !char.IsControl(c)).ToArray());
            return toReturn.Replace("\r", "").Replace("\n", "");
        }
    }
}
