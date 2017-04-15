# C#-webpage-analyzer

A collection of restful web service written in C# that perform webpage based processing and analyzing.  

Solution WebappClient provides an interface to test the web services.

The different functions performed are:

1. Top 10 words: Takes a url and returns the top 10 most used words on the webpage.
2. Top 10 content words (After removing the stop words): Takes a url and returns the top 10 most used words after removing stop words
    on the webpage.
3. Custom stop words: Takes a url and a list of comma seperated stop words to remove from the content. Returns processed text.
4. Replace with synonyms/antonyms: Takes a url and a list of words to replace and returns two strings. 
    One replaced with synonyms and one replaced with antonyms
5. Average words per sentence: Takes a url and returns the average words per sentence.
6. Encrypt/Decrypt: Takes a url/cipher text and a passphrase and returns the encrypted content on the url, or decrypted text.


*NOTE: For function 4. Replace with synonyms/antonyms, the api key was removed for privacy. To use the funciton, get an API key from https://words.bighugelabs.com/getkey.php and replace the comment in the function within Service1.svc.cs file. 
