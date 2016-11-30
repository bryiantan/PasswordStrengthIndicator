/*
Password Strength Indicator using jQuery and XML

By: Bryian Tan (bryian.tan@hotmail.com)

11-22-2016 - v02.01.00 //check keyboard sequence
12-01-2015 - v02.00.00
07-23-2012 - v01.03.00
01-17-2011 - v01.02.00
01-12-2011 - v01.01.00

Description:
Password Strength Indicator somewhat similar to ASP.NET AJAX PasswordStrength extender control behavior 
and implemented by using jQuery and XML. The password information is stored in an XML file. 
Sample XML file contents:
<PasswordPolicy>
<Password>
<duration>180</duration> //password age, expired in xxx days
<minLength>14</minLength> //password minimum length
<maxLength>25</maxLength> //password maximum length
<numsLength>2</numsLength> //number of required digits  
<upperLength>1</upperLength> //number of required upper case  
<specialLength>1</specialLength> //number of required special characters 
<barWidth>200</barWidth> //the bar indicator width
<barColor>Green</barColor> //the bar indicator colors
<specialChars>!@#\$%*()_+^&amp;}{:;?.</specialChars> //allowable special characters
 <maxConsecutiveRepeatedChars>1</maxConsecutiveRepeatedChars>
</Password>
</PasswordPolicy>
*/

(function ($) {
    getAbsolutePath = function (appFolderXMLPath) {
        if (appFolderXMLPath.length > 0) {
            return window.location.protocol + '//' + window.location.host + appFolderXMLPath;
        } else {
            return window.location.protocol + '//' + window.location.host + '/';
        }
    }
    $.fn.center = function () {
        this.css("position", "absolute");
        this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");
        this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");
        return this;
    }
    var password_Strength = new function () {
        this.countRegExp = function (passwordVal, regx) {
            var match = passwordVal.match(regx);
            return match ? match.length : 0;
        }
        this.getKeyboardSequenceChar = function (passwordVal) {
            var maxQwertySequence = password_settings.maxKeyboardSequence + 1;
            var keyboardSequenceCharacters = password_settings.keyboardSequenceCharacters;
            var num = 0;
            var startIndex = 0;
            if (passwordVal.length >= maxQwertySequence) {
                while (num < passwordVal.length) {
                    num = startIndex + maxQwertySequence;

                    var lastXsubString = passwordVal.substr(startIndex, maxQwertySequence);

                    if (keyboardSequenceCharacters.indexOf(lastXsubString.toLowerCase()) >= 0) {
                        return lastXsubString;
                    }
                    startIndex++;
                }
            }

            return '';
        }
        this.getStrengthInfo = function (passwordVal) {
            var len = passwordVal.length;
            var pStrength = 0;
            var msg = "",
                inValidChars = "",
                repeatedChars = "";
            var allowableSpecilaChars = new RegExp("[" + password_settings.specialChars + "]", "g")
            var nums = this.countRegExp(passwordVal, /\d/g),
                lowers = this.countRegExp(passwordVal, /[a-z]/g),
                uppers = this.countRegExp(passwordVal, /[A-Z]/g),
                specials = this.countRegExp(passwordVal, allowableSpecilaChars),
                spaces = this.countRegExp(passwordVal, /\s/g);
            var keyboardSequenceCharInPassword = this.getKeyboardSequenceChar(passwordVal);
            var repeatedCharsRegex = new RegExp("(.+)\\1{" + password_settings.maxConsecutiveRepeatedChars + ",}", "i");
            var repeats = 0;
            if (password_settings.maxConsecutiveRepeatedChars > 0) {
                repeats = this.countRegExp(passwordVal, repeatedCharsRegex);
            }
            inValidChars = passwordVal.replace(/[a-z]/gi, "") + inValidChars.replace(/\d/g, "");
            inValidChars = inValidChars.replace(/\d/g, "");
            inValidChars = inValidChars.replace(allowableSpecilaChars, "");
            if (spaces > 0) {
                return "No spaces!";
            }
            if (keyboardSequenceCharInPassword !== '') {
                return "Keyboard sequence characters not allow: " + keyboardSequenceCharInPassword;
            }
            if (inValidChars !== '') {
                return "Invalid character: " + inValidChars;
            }
            if (len > password_settings.maxLength) {
                return "Password too long!";
            }
            if (repeats > 0) {
                repeatedChars = passwordVal.match(repeatedCharsRegex);
                return repeatedChars + " repeated more than " + password_settings.maxConsecutiveRepeatedChars + " times!";
            }
            if ((specials + uppers + nums + lowers) < password_settings.minLength) {
                msg += password_settings.minLength - (specials + uppers + nums + lowers) + " more characters, ";
            }
            if (specials == 0 || uppers == 0 || nums == 0 || lowers == 0) {
                msg += "At least ";
            }
            if (nums >= password_settings.numberLength) {
                nums = password_settings.numberLength;
            } else {
                msg += (password_settings.numberLength - nums) + " more numbers, ";
            }
            if (specials >= password_settings.specialLength) {
                specials = password_settings.specialLength
            } else {
                msg += (password_settings.specialLength - specials) + " more symbol, ";
            }
            if (uppers >= password_settings.upperLength) {
                uppers = password_settings.upperLength
            } else {
                msg += (password_settings.upperLength - uppers) + " Upper case characters, ";
            }
            if ((len - (uppers + specials + nums)) >= (password_settings.minLength - password_settings.numberLength - password_settings.specialLength - password_settings.upperLength)) {
                pStrength += (password_settings.minLength - password_settings.numberLength - password_settings.specialLength - password_settings.upperLength);
            } else {
                pStrength += (len - (uppers + specials + nums));
            }
            pStrength += uppers + specials + nums;
            if (lowers === 0) {
                if (pStrength > 1) {
                    pStrength -= 1;
                }
                msg += "1 lower case character, ";
            }
            if (pStrength == password_settings.minLength && lowers > 0) {
                msg = "Strong password!";
            }
            return msg + ';' + pStrength;
        }
    }
    var password_settings = {
        minLength: 12,
        maxLength: 25,
        specialLength: 1,
        upperLength: 1,
        numberLength: 1,
        barWidth: 200,
        barColor: 'Red',
        specialChars: '!@#$',
        metRequirement: false,
        useMultipleColors: 0,
        maxConsecutiveRepeatedChars: 0,
        maxKeyboardSequence: 2,
        keyboardSequenceCharacters: '123456789'
    };
    $.fn.password_strength = function (options) {
        var settings = $.extend({
            appFolderXMLPath: "",
            passwordPolicyLinkId: ""
        }, options);
        this.metReq = function () {
            return password_settings.metRequirement;
        }
        var xmlAbsolutePath = getAbsolutePath(settings.appFolderXMLPath) + "PasswordPolicy.xml";
        this.getXMLAbsolutePath = function () {
            return xmlAbsolutePath;
        }
        if (settings.passwordPolicyLinkId.length > 0) {
            $("[id=" + settings.passwordPolicyLinkId + "]").click(function (event) {
                var width = 350,
                    height = 300,
                    left = (screen.width / 2) - (width / 2),
                    top = (screen.height / 2) - (height / 2);
                if (typeof ($.blockUI) !== 'undefined') {
                    $.blockUI({
                        onOverlayClick: $.unblockUI,
                        message: '<br /> <iframe src="' + xmlAbsolutePath + '" height="370px" width="370px" scrolling="no" frameborder="0" id="displayXMLIframe" /><br/>'
                    });
                    $('.blockUI.blockMsg').center();
                } else {
                    window.open(xmlAbsolutePath, 'Password_poplicy', 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top);
                    event.preventDefault();
                }
                return false;
            });
            $(window).resize(function () {
                if (typeof ($.blockUI) !== 'undefined') {
                    $('.blockUI.blockMsg').center();
                }
            });
        }
        $.ajax({
            type: "GET",
            url: xmlAbsolutePath,
            dataType: "xml",
            success: function (xml) {
                $(xml).find('Password').each(function () {
                    var _minLength = $(this).find('minLength').text(),
                        _maxLength = $(this).find('maxLength').text(),
                        _numsLength = $(this).find('numsLength').text(),
                        _upperLength = $(this).find('upperLength').text(),
                        _specialLength = $(this).find('specialLength').text(),
                        _barWidth = $(this).find('barWidth').text(),
                        _barColor = $(this).find('barColor').text(),
                        _specialChars = $(this).find('specialChars').text(),
                        _maxConsecutiveRepeatedChars = $(this).find('maxConsecutiveRepeatedChars').text(),
                        _maxKeyboardSequence = $(this).find('maxKeyboardSequence').text(),
                        _keyboardSequenceCharacters = $(this).find('keyboardSequenceCharacters').text(),
                        _useMultipleColors = $(this).find('useMultipleColors').text();
                    password_settings.minLength = parseInt(_minLength);
                    password_settings.maxLength = parseInt(_maxLength);
                    password_settings.specialLength = parseInt(_specialLength);
                    password_settings.upperLength = parseInt(_upperLength);
                    password_settings.numberLength = parseInt(_numsLength);
                    password_settings.barWidth = parseInt(_barWidth);
                    password_settings.barColor = _barColor;
                    password_settings.specialChars = _specialChars;
                    password_settings.useMultipleColors = _useMultipleColors;
                    password_settings.maxConsecutiveRepeatedChars = _maxConsecutiveRepeatedChars;
                    password_settings.maxKeyboardSequence = parseInt(_maxKeyboardSequence);
                    password_settings.keyboardSequenceCharacters = _keyboardSequenceCharacters;
                });
            }
        });
        return this.each(function () {
            var barLeftPos = $("[id='" + this.id + "']").position().left + $("[id='" + this.id + "']").width()
                    + parseInt($("[id='" + this.id + "']").css("padding-left").replace("px", ""));
            var barTopPos = $("[id='" + this.id + "']").position().top + $("[id='" + this.id + "']").height();
            var container = $('<span></span>').css({
                position: 'absolute',
                top: barTopPos - 6,
                left: barLeftPos + 15,
                'font-size': '75%',
                display: 'inline-block',
                width: password_settings.barWidth + 40
            });
            $(this).after(container);
            var passIndi = $('<div id="PasswordStrengthBorder"></div><div id="PasswordStrengthBar" class="BarIndicator"></div>').css({
                position: 'absolute',
                display: 'none'
            }).eq(0).css({
                height: 3,
                top: barTopPos - 16,
                left: barLeftPos + 15,
                'border-style': 'solid',
                'border-width': 1,
                padding: 2,
                'box-sizing': 'unset'
            }).end().eq(1).css({
                height: 5,
                top: barTopPos - 14,
                left: barLeftPos + 17
            }).end()
            container.before(passIndi);
            $(this).keyup(function () {
                var passwordVal = $(this).val();
                password_settings.metRequirement = false;
                if (passwordVal.length > 0) {
                    var msgNstrength = password_Strength.getStrengthInfo(passwordVal);
                    var msgNstrength_array = msgNstrength.split(";"),
                        strengthPercent = 0,
                        barWidth = password_settings.barWidth,
                        backColor = password_settings.barColor;
                    if (msgNstrength_array.length > 1) {
                        strengthPercent = (msgNstrength_array[1] / password_settings.minLength) * barWidth;
                    }
                    $("div[id='PasswordStrengthBorder']").css({
                        display: 'inline',
                        width: barWidth
                    });
                    if (password_settings.useMultipleColors === "1") {
                        if (parseInt(strengthPercent) >= 0 && parseInt(strengthPercent) <= (barWidth * .33)) {
                            backColor = "red";
                        } else if (parseInt(strengthPercent) >= (barWidth * .33) && parseInt(strengthPercent) <= (barWidth * .67)) {
                            backColor = "blue";
                        } else {
                            backColor = password_settings.barColor;
                        }
                    }
                    $("div[id='PasswordStrengthBar']").css({
                        display: 'inline',
                        width: strengthPercent,
                        'background-color': backColor
                    });
                    if (msgNstrength_array[0].lastIndexOf(",") !== -1) {
                        container.text(msgNstrength_array[0].substring(0, msgNstrength_array[0].length - 2));
                    } else {
                        container.text(msgNstrength_array[0]);
                    }
                    if (strengthPercent == barWidth) {
                        password_settings.metRequirement = true;
                    }
                } else {
                    container.text('');
                    $("div[id='PasswordStrengthBorder']").css("display", "none");
                    $("div[id='PasswordStrengthBar']").css("display", "none");
                }
            });
        });
    };
})(jQuery);