# headless-chrome

Test using chrome in .net core unit test project using both headless chrome and interactive chrome.

Idea came from reading this [article](https://tutorialzine.com/2017/08/automating-google-chrome-with-node-js).

Which with a little research, showed we can do this straight from existing C# Selenium like so:

```
// headless option
var chromeOptions = new ChromeOptions();
chromeOptions.AddArgument("--headless");
var chrome = new ChromeDriver(chromeOptions);
```

Still need to test this from a headless service like Azure Service Fabric, serverless function or a container (docker, etc.) that has Chrome installed.


Some additional references:

 * https://developers.google.com/web/updates/2017/04/headless-chrome
