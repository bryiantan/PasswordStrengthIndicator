# PasswordStrengthIndicator
Password strength indicator using jQuery and XML

Password Strength Indicator somewhat similar to AJAX PasswordStrength extender control behavior and implemented by using jQuery and XML.

------------------------------------------------------------------------

Available Features:

•	Password setting are stored in xml file

•	Client side and server side validation

•	Password strength in different colors

•	Check if password contains x number of Uppercase characters (A-Z)

•	Check if password contains any Lowercase characters (a-z)

•	Check if password contains x number of Base 10 digits (0 through 9)

•	Check if password contains x number of allowable Nonalphanumeric characters

•	Check if password meet the Minimum and Maximum password length requirement

•	Check if password exceeded the allowable Maximum consecutive repeated character

•	Check if password contains keyboard sequence (i.e., 123456, qwerty, …)

Demo: http://download.ysatech.com/ASP-NET-jQuery-Password-Strength-v2/
Nuget: https://www.nuget.org/packages/PasswordStrengthIndicator/
------------------------------------------------------------------------

This plugin depends on:
jQuery 1.4 and above, jquery.blockUI (optional)

------------------------------------------------------------------------

Password setting is in PasswordPolicy.xml:

duration - password age, expired in xxx days (example, not in use)

minLength - password minimum length

maxLength - password maximum length

numsLength - minimum number of required digits  

upperLength - minimum number of required upper case 

specialLength - minimum number of required special characters 

specialChars - allowable special characters

barWidth - set the bar indicator width

barColor - the bar indicator colors

useMultipleColors - 1=Yes, 0=No

maxConsecutiveRepeatedChars - 0=allow repeat, 1..n = allow 1..n maximum number of successive repetitions of a given character (example: if 1, aa will not be valid because it repeated more than 1)

maxKeyboardSequence - maximun allowable keyboard sequence (example: if 2, 123 will not be valid, but 12 will be valid. Note the sequence defined in keyboardSequenceCharacters)

keyboardSequenceCharacters - defined the list of keyboard sequence
