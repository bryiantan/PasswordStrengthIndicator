using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace PasswordStrengthIndicator.Core
{
    public class Helper
    {
        public Helper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static PasswordSetting GetPasswordSetting()
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["passwordPolicyXMLLocation"]));

            PasswordSetting passwordSetting = new PasswordSetting();

            foreach (XmlNode node in xmlDoc.ChildNodes)
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    passwordSetting.Duration = int.Parse(node2["duration"].InnerText);
                    passwordSetting.MinLength = int.Parse(node2["minLength"].InnerText);
                    passwordSetting.MaxLength = int.Parse(node2["maxLength"].InnerText);
                    passwordSetting.NumsLength = int.Parse(node2["numsLength"].InnerText);
                    passwordSetting.SpecialLength = int.Parse(node2["specialLength"].InnerText);
                    passwordSetting.UpperLength = int.Parse(node2["upperLength"].InnerText);
                    passwordSetting.SpecialChars = node2["specialChars"].InnerText;
                    passwordSetting.MaxConsecutiveRepeatedChars = int.Parse(node2["maxConsecutiveRepeatedChars"].InnerText);
                    passwordSetting.MaxKeyboardSequence = int.Parse(node2["maxKeyboardSequence"].InnerText);
                    passwordSetting.KeyboardSequenceCharacters = node2["keyboardSequenceCharacters"].InnerText;
                }
            }

            return passwordSetting;

        }

        public static bool IsPasswordMeetPolicy(string strPassword)
        {
            PasswordSetting passwordSetting = Helper.GetPasswordSetting();
            StringBuilder sbPasswordRegx = new StringBuilder(string.Empty);

            //min and max
            sbPasswordRegx.Append(@"(?=^.{" + passwordSetting.MinLength + "," + passwordSetting.MaxLength + "}$)");

            //numbers length
            sbPasswordRegx.Append(@"(?=(?:.*?\d){" + passwordSetting.NumsLength + "})");

            //a-z characters
            sbPasswordRegx.Append(@"(?=.*[a-z])");

            //A-Z length
            sbPasswordRegx.Append(@"(?=(?:.*?[A-Z]){" + passwordSetting.UpperLength + "})");

            //special characters length
            sbPasswordRegx.Append(@"(?=(?:.*?[" + passwordSetting.SpecialChars + "]){" + passwordSetting.SpecialLength + "})");

            //(?!.*\s) - no spaces
            //^(?:(?!([0-9a-zA-Z!@#$%*()_+^&]+)\1{1,}).)*$
            //[0-9a-zA-Z!@#$%*()_+^&] -- valid characters
            if (passwordSetting.MaxConsecutiveRepeatedChars > 0)
            {
                sbPasswordRegx.Append(string.Format(@"(?!.*\s)^(?i:(?!([0-9a-zA-Z{0}]+)\1{{{1},}}).)*$", passwordSetting.SpecialChars, passwordSetting.MaxConsecutiveRepeatedChars));
            }
            else
            {
                sbPasswordRegx.Append(@"(?!.*\s)[0-9a-zA-Z" + passwordSetting.SpecialChars + "]*$");
            }

            //check sequence
            if (Regex.IsMatch(strPassword, sbPasswordRegx.ToString()))
                return !Helper.IsPasswordContainSequence(strPassword, passwordSetting);
            //more custom validation here ....

            return false;// Regex.IsMatch(strPassword, sbPasswordRegx.ToString());

        }

        //check for keybard sequence
        public static bool IsPasswordContainSequence(string strPassword, PasswordSetting passwordSetting)
        {
            int startIndex = 0;
            int num = 0;
            int length = passwordSetting.MaxKeyboardSequence + 1;
            string empty = string.Empty;
            string str1 = passwordSetting.KeyboardSequenceCharacters;
                //"`1234567890-==-0987654321`qwertyuiopasdfghjklzxcvbnm1qazxsw23edcvfr45tgbnhy67ujm,ki89ol./;p0abcdefghijklmnopqrstuvwxyz" + "zyxwvutsrqponmlkjihgfedcba!qazxsw23edcvfr45tgbnhy67ujm,ki89ol./;p0poiuytrewqlkjhgfdsamnbvcxz" + "poiuytrewqlkjhgfdsamnbvcxz~!@#$%^&*()_+" + "zaq12wsxcde34rfvbgt56yhnmju78ik,.lo90p;/";
            while (num < strPassword.Length)
            {
                num = startIndex + length;
                string str2 = strPassword.ToLower().Substring(startIndex, length);
                if (str1.Contains(str2))
                    return true;
                ++startIndex;
            }
            return false;
        }
    }
}
