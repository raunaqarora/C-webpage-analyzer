<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebappClient._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" id="decrypt">
        <h1>Webpage text analyzer</h1>
        <br />
        <a href="http://localhost:2197/Service1.svc/">http://10.1.22.59:7005/Service1.svc/</a><br />
        Method 1: string top10words(string url) : Gets top 10 most frequently used words in a webpage<br />
        Invoke using: /top10words?v={url}<br />
        <asp:TextBox ID="top10wordsBox" runat="server" Height="39px">Enter a URL</asp:TextBox>
        <asp:Button ID="calcTop10Words" runat="server" Text="Get top 10 words" Height="49px" OnClick="calcTop10Words_Click" Width="246px" />
        <br />
        <asp:TextBox ID="resultTop10" runat="server"></asp:TextBox>
        <br />
        <br />
        Method 2: string top10wordscontent(string url) : Gets top 10 most frequently used words after removing stop words in a webpage<br />
        Invoke using: /top10content?v={url}<br />
        <asp:TextBox ID="top10ContentBox" runat="server" Height="36px">Enter a URL</asp:TextBox>
        <asp:Button ID="calcTop10Content" runat="server" Text="Get top 10 Content words" Height="43px" OnClick="calcTop10Content_Click" />
        <br />
        <asp:TextBox ID="resultTop10Content" runat="server" Height="99px"></asp:TextBox>
        <br />
        <br />
        Method 3: string customStop(string url, string stopWords): Specify a list of &quot;,&quot; seperated stop words to be removed from a webpage<br />
        Invoke using: /customStop?v={url}&amp;u={stopwords}<br />
        <asp:TextBox ID="customStopUrl" runat="server" Height="36px" Width="325px">Enter a URL</asp:TextBox>
        <asp:Button ID="calcCustomStop" runat="server" Text="Remove custom stop words" Height="43px" OnClick="calcCustomStop_Click" />
        <br />
        <asp:TextBox ID="customStopWords" runat="server" Height="36px" Width="324px">Enter &quot;,&quot; seperated stop words</asp:TextBox>
        <br />
        <asp:TextBox ID="resultCustomStop" runat="server" Height="84px" Width="321px"></asp:TextBox>
        <br />
        <br />
        Method 4: decimal averageWordsPerSentence(string url): Get average words per sentence on a webpage<br />
        Invoke using: /averageWordsPerSentence?v={url}<br />
        <asp:TextBox ID="avgWordsUrl" runat="server" Height="36px">Enter a URL</asp:TextBox>
        <asp:Button ID="calcAverageWords" runat="server" Text="Calculate average sentence length" Height="43px" OnClick="calcAverageWords_Click" />
        <br />
        <asp:TextBox ID="resultAvgWords" runat="server" Height="99px"></asp:TextBox>
        <br />
        <br />
        Method 5: string[] replaceWithSynonymsAntonyms(string url, string wordsToReplace): Enter a url and a list of &quot;,&quot; seperated words to be replaced.<br />
        Invoke using: /replaceWithSynonymsAntonyms?v={url}&amp;u={wordsToReplace}<br />
&nbsp;<asp:TextBox ID="synAntUrl" runat="server" Height="36px" Width="244px">Enter a URL</asp:TextBox>
        <asp:Button ID="replaceWords" runat="server" Text="Replace Words" Height="43px" OnClick="replaceWords_Click" />
        <br />
        <asp:TextBox ID="wordsToReplace" runat="server" Height="36px" Width="252px">Enter &quot;,&quot; seperated words</asp:TextBox>
        <br />
        Replaced by synonyms:<br />
        <asp:TextBox ID="synResult" runat="server" Height="99px"></asp:TextBox>
        <br />
        Replaced by antonyms:<br />
        <asp:TextBox ID="antResult" runat="server" Height="99px"></asp:TextBox>
        <br />
        <br />
        Method 6: string encryptDecrypt(string passPhrase, string encrypt): Send url to encrypt or text to decrypt using POST method and passphrase in URL. Set encrypt to &quot;encrypt&quot; to encrypt, anything else to decrypt<br />
        Invoke using: (POST url / cipher text) /encryptDecrypt?v={passPhrase}&amp;u={encrypt}<br />
        <asp:TextBox ID="encryptUrl" runat="server" Height="36px" Width="249px">Enter a URL / Cipher Text</asp:TextBox>
        <asp:Button ID="encrypt" runat="server" Text="Encrypt website at url" Height="43px" OnClick="encrypt_Click" />
        <br />
        <asp:TextBox ID="encryptPassPhrase" runat="server" Height="36px" Width="252px">Enter a passphrase</asp:TextBox>
        <asp:Button ID="replaceWords0" runat="server" Text="Decrypt Cipher Text" Height="43px" OnClick="decrypt_Click" />
        <br />
        <asp:TextBox ID="encryptResult" runat="server" Height="99px"></asp:TextBox>
        <br />
    </div>

    </asp:Content>
