<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <html>
      <head>
        <title>Password policy</title>
        <style type="text/css">
          .odd {background-color:#ccc}
          .table { font-family:'Segoe UI',Verdana,Arial; font-size:10pt; border-color:#ccc; }
        </style>
      </head>
      <body>
        <h2>Password policy</h2>
        <table border="1" cellpadding="1" cellspacing="0" class="table">       
            <tr class="odd">
              <td>Password duration</td>
              <td align="center">
                <xsl:value-of select="PasswordPolicy/Password/duration"/> days.
              </td>
            </tr>
            <tr>
              <td>Password minimum length:</td>
              <td align="center">
                <xsl:value-of select="PasswordPolicy/Password/minLength"/>
              </td>
            </tr>
            <tr class="odd">
              <td>Password Maximum length:</td>
              <td align="center">
                <xsl:value-of select="PasswordPolicy/Password/maxLength"/>
              </td>
            </tr>
          <tr>
            <td>Required digits:</td>
            <td align="center">
              <xsl:value-of select="PasswordPolicy/Password/numsLength"/>
            </td>
          </tr>
          <tr class="odd">
            <td>Required upper-case letters:</td>
            <td align="center">
              <xsl:value-of select="PasswordPolicy/Password/upperLength"/>
            </td>
          </tr>
          <tr>
            <td>Required special characters:</td>
            <td align="center">
              <xsl:value-of select="PasswordPolicy/Password/specialLength"/>
            </td>
          </tr>
          <tr class="odd">
            <td>Allowable special characters:</td>
            <td align="center">
              <xsl:value-of select="PasswordPolicy/Password/specialChars"/>
            </td>
          </tr>
          <tr>
            <td>Maximun consecutive repeated characters:</td>
            <td align="center">
              <xsl:value-of select="PasswordPolicy/Password/maxConsecutiveRepeatedChars"/>
            </td>
          </tr>
          <tr>
            <td>Maximun keyboard sequence characters: <br/>
              (Example sequence character: qwerty, 123456789, abcdefg, qazxsw)
          </td>
            <td align="center">
              <xsl:value-of select="PasswordPolicy/Password/maxKeyboardSequence"/>
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
