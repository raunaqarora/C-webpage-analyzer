using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebappClient
{
    public partial class _Default : Page
    {
        protected void calcTop10Words_Click(object sender, EventArgs e)
        {
            string url = @"http://10.1.22.59:7005/Service1.svc/top10words?v=" + top10wordsBox.Text;
            resultTop10.Text = makeRequest(url);
        }

        protected void calcTop10Content_Click(object sender, EventArgs e)
        {
            string url = @"http://10.1.22.59:7005/Service1.svc/top10content?v=" + top10ContentBox.Text;
            resultTop10Content.Text = makeRequest(url);
        }
        protected void calcCustomStop_Click(object sender, EventArgs e)
        {
            string url = @"http://10.1.22.59:7005/Service1.svc/customStop?v=" + customStopUrl.Text + "&u=" + customStopWords.Text;
            resultCustomStop.Text = processCustomStopObject(makeRequest(url));
        }
        protected void calcAverageWords_Click(object sender, EventArgs e)
        {
            string url = @"http://localhost:2197/Service1.svc/averageWordsPerSentence?v=" + avgWordsUrl.Text;
            resultAvgWords.Text = makeRequest(url);
        }
        protected void replaceWords_Click(object sender, EventArgs e)
        {
            string url = @"http://10.1.22.59:7005/Service1.svc/replaceWithSynonymsAntonyms?v=" + synAntUrl.Text + "&u=" + wordsToReplace.Text;
            synResult.Text = processSynAntObject(makeRequest(url)).ElementAt(0);
            antResult.Text = processSynAntObject(makeRequest(url)).ElementAt(1);
        }
        
        protected void encrypt_Click(object sender, EventArgs e)
        {
            string url = @"http://10.1.22.59:7005/Service1.svc/encryptDecrypt?v=" + encryptPassPhrase.Text + "&u=encrypt";
            HttpClient client = new HttpClient();
            var response = client.PostAsync(url, new ByteArrayContent(Encoding.UTF8.GetBytes(encryptUrl.Text))).GetAwaiter().GetResult();
            String json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            encryptResult.Text = json;
        }
        protected void decrypt_Click(object sender, EventArgs e)
        {
            string url = @"http://10.1.22.59:7005/Service1.svc/encryptDecrypt?v=" + encryptPassPhrase.Text + "&u=decrypt";
            HttpClient client = new HttpClient();
            var response = client.PostAsync(url, new ByteArrayContent(Encoding.UTF8.GetBytes(encryptUrl.Text))).GetAwaiter().GetResult();
            String json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            encryptResult.Text = json;
        }
        private string makeRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                String j = reader.ReadToEnd();
                return j;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                //Environment.Exit(1);
                return ex.Message.ToString();
            }
        }
        static List<string> processSynAntObject(String data)
        {
            synAntObject js = JsonConvert.DeserializeObject<synAntObject>(data);
            return js.replaceWithSynonymsAntonymsResult;
        }
        static string processCustomStopObject(String data)
        {
            customStopObject js = JsonConvert.DeserializeObject<customStopObject>(data);
            return js.customStopResult;
        }
        [Serializable]
        public class synAntObject
        {
            public List<string> replaceWithSynonymsAntonymsResult { get; set; }
        } 
        [Serializable]
        public class customStopObject
        {
            public string customStopResult { get; set; }
        }
    }
}