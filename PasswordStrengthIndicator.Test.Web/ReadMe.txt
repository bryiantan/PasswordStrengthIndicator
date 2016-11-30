Password Strength Indicator with jQuery and XML (v01.00.00)
By: Bryian Tan
Email: bryian.tan at ysatech.com

Included in this sample application: 
* Sample code in Classic ASP
* Sample code in ASP.NET c# with and without MasterPage
* Sample code in HTML format
* Password policy xml file
* XSL Transformations file
* If you want to test the Classic ASP sample code, first deploy it to IIS Web Server.

12-01-2015 v02.00.00
- Added maximum number of successive repetitions (maxConsecutiveRepeatedChars)
- Added option to specify the xml file location (xmlFileLocation)
- Added option to specify the password policy link id (passwordPolicyLinkId)
- Tested on jQuery v1.11.3
- Minor changes to jquery.password-strength.js

07-23-2012 v01.03.00
- minor update

01-17-2011 v01.02.00
- Added useMultipleColors attribute to PasswordPolicy.xml file. 1=Yes, 0=No
- Added new logic in the plug-in to display background color in red if the strength is 
  betwen 0 and 33%. The background color will be blue if the strength is between 33% and 67%.

01-12-2011 v01.01.00
-Set the width of the text container to the width of the meter bar + 40 pixel
-Display missing lower case character correctly.
-Include the source code for the plug-in.
  
Resources:
http://fyneworks.blogspot.com/2007/04/dynamic-regular-expressions-in.html
http://projects.sharkmediallc.com/pass/
http://docs.jquery.com/Plugins/Authoring
http://stackoverflow.com/questions/1034306/public-functions-from-within-a-jquery-plugin