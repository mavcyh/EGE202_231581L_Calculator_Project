using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Calculator_231581L
{
    public partial class MainForm_231581L : Form
    {
        private void lblID_Click(object sender, EventArgs e) // Copies GUID attribute to clipboard
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            Clipboard.SetText(attribute.Value.ToString());
        }

        /* AUDIO */
        private AudioPlayer audioPlayer = new AudioPlayer(); // Set up private field audioPlayer to hold instance of audioPlayer
        Random random = new Random(); // For random audio playback
  
        /* VARIABLE INITIALISATION */
        private string currentOp = "", currentOpIcon = "", displayString = "", displayFormula = "", formulaSaved = "", formulaText = "";
        private double operand = 0.0, displayValue = 0.0;
        private bool firstOpPressed = false, unaryOpPressed = false, shiftToggled = false,
            calcSciMode = true, calcDegMode = true, calcChangeAllowed = true, disabledMode = false, mathError = false;
        private int? formulaTextStart = null;

        /*** LOGIC PERFORMED (MAIN CODE) ***/
        private void ClearBtn_Action(string value)
        {
            if (disabledMode && value == "C") return;
            displayString = "";
            displayFormula = "";
            displayValue = 0.0;
            unaryOpPressed = false;
            if (shiftToggled) ShiftBtn_Released();
            if (value == "AC" || currentOp == "Equal")
            {
                currentOp = "";
                currentOpIcon = "";
                formulaSaved = "";
                formulaText = "";
                operand = 0.0;
                firstOpPressed = false;
                disabledMode = false;
                mathError = false;
                calcChangeAllowed = true;
            }
            UpdateScreen();
        }

        private void FormatDisplayString()
        {
            // Trim 0s from end of decimals
            if (displayString.Contains('.') && !displayString.Contains('e')) displayString = displayString.TrimEnd('0');
            // Remove '.' or 'e' if ending with it
            if (displayString[displayString.Length - 1] == '.' || displayString[displayString.Length - 1] == 'e')
                displayString = displayString.Substring(0, displayString.Length - 1);
        }

        private void DisplayStringToValue()
        {
            // Trim 0s from end of decimals
            if (displayString.Contains('.') && !displayString.Contains('e')) displayString = displayString.TrimEnd('0');
            // Remove '.' or 'e' if display ends with it
            else if (displayString[displayString.Length - 1] == '.' || displayString[displayString.Length - 1] == 'e')
                displayString = displayString.Substring(0, displayString.Length - 1);
            displayValue = double.Parse(displayString);
        }

        private void DisplayValueToString(double? doubleNullableValue = null)
        {
            /* Defaults to formatting displayValue to displayString.
             * It will instead format the optional double passed into the function if done so */
            double value;
            if (doubleNullableValue == null) value = displayValue;
            else value = (double)doubleNullableValue;
            double valueAbs = Math.Abs(value);
            // |value| >= 1.0e100
            if (valueAbs >= 1.0e100 || Double.IsNaN(value)) mathError = true;
            else if (valueAbs == 0.0) displayString = "0";
            // |value| >= 1.0e10 or |value| <= 1.0e-10
            else if (valueAbs >= 1.0e10 || valueAbs <= 1.0e-10)
            {
                string[] displayStringSplit = value.ToString("e9").Split('e');
                displayString = displayStringSplit[0].TrimEnd('0') + "e" + int.Parse(displayStringSplit[1]).ToString("D2");
            }
            // Result fits in normal display
            else displayString = value.ToString("F9").TrimEnd('0');
        }

        private void UpdateScreen()
        {
            /* UPDATE FORMULA */
            if (mathError)
            {
                lblFormula.Text = "Math Error [AC]";
                lblSecDisplay.ForeColor = displayBackColor;
                lblExpActive.ForeColor = displayBackColor;
                lblDisplay.Text = "";
                return;
            }
            else if (currentOp == "Equal") formulaText = formulaSaved + "=";
            else if (currentOp == "PlusMinus" && !firstOpPressed) formulaText = formulaSaved;
            // Normal operation
            else formulaText = formulaSaved + currentOpIcon + displayFormula;

            if (formulaText.Length > 24)
            {
                lblFormula.Text = formulaText.Substring(formulaText.Length - 24);
                lblLeftArrow.ForeColor = formulaForeColor;
            }
            else
            {
                lblFormula.Text = formulaText;
                lblLeftArrow.ForeColor = lblInactiveColor;
                lblRightArrow.ForeColor = lblInactiveColor;
            }
            /* */

            if (displayString == "")
            {
                lblDisplay.Text = "0.";
                lblSecDisplay.ForeColor = displayBackColor;
                lblExpActive.ForeColor = displayBackColor;
            }
            else if (displayString == "-") lblDisplay.Text = "-0.";
            // Normal operation
            else
            {   // Exponential form number
                if (displayString.Contains('e'))
                {
                    string[] displayStringSplit = displayString.Split('e');
                    lblDisplay.Text = displayStringSplit[0];
                    if (displayStringSplit[1] != "") lblSecDisplay.Text = int.Parse(displayStringSplit[1]).ToString("D2");
                    else lblSecDisplay.Text = "00";
                    lblSecDisplay.ForeColor = displayForeColor;
                    lblExpActive.ForeColor = displayForeColor;
                }
                // Normal number
                else
                {
                    if (!displayString.Contains('.')) lblDisplay.Text = displayString + ".";
                    else lblDisplay.Text = displayString;
                    lblSecDisplay.ForeColor = displayBackColor;
                    lblExpActive.ForeColor = displayBackColor;
                }
            }

            if (disabledMode) lblDisabled.ForeColor = displayForeColor;
            else lblDisabled.ForeColor = displayBackColor;
        }

        private void lblArrow_Action(string direction, bool maxDirection = false)
        {   
            if (formulaTextStart == null) formulaTextStart = formulaText.Length - 24;
            switch (direction)
            {
                case "Left":
                    if (formulaTextStart > 0)
                    {
                        if (maxDirection) formulaTextStart = 0;
                        else formulaTextStart--;
                    }
                    break;
                case "Right":
                    if (formulaTextStart < formulaText.Length - 24)
                    {
                        if (maxDirection) formulaTextStart = formulaText.Length - 24;
                        else formulaTextStart++;
                    }  
                    break;
            }
            lblFormula.Text = formulaText.Substring((int)formulaTextStart, 24);
            if (formulaTextStart > 0) lblLeftArrow.ForeColor = formulaForeColor;
            else lblLeftArrow.ForeColor = lblInactiveColor;
            if (formulaTextStart < formulaText.Length - 24) lblRightArrow.ForeColor = formulaForeColor;
            else lblRightArrow.ForeColor= lblInactiveColor;
            btnEqual.Focus();
        }

        private void EqualBtn_DownAction(object sender = null, MouseEventArgs e = null)
        {   
            if (shiftToggled)
            {
                ShiftBtn_Released();
                return;
            }
            if (downedControlE.ContainsKey(btnEqual) || disabledMode || mathError) return;
            else downedControlE.Add(btnEqual, null);
            if (e != null) mouseDownedControl = btnEqual;
            audioPlayer.PlayResource(Properties.Resources.SPACE_P);
            btnEqual.BackColor = stdBtnPressedBackColor;
            btnEqual.ForeColor = btnPressedForeColor;
            
            /* LOGIC */
            if (currentOp == "Equal" || displayString == "") return;
            if (!unaryOpPressed)
            {
                FormatDisplayString();
                displayFormula = displayString;
            }
            SaveFormula(prevOpIcon: currentOpIcon);
            DisplayStringToValue();
            CalculateNewOperand();
            currentOp = "Equal";
            firstOpPressed = false;
            unaryOpPressed = false;
            displayValue = operand;
            DisplayValueToString();
            audioPlayer.PlayResult(displayString);
            UpdateScreen();
        }

        private void NumPad_Action(string value)
        {
            if (disabledMode || mathError || unaryOpPressed) return;
            if (currentOp == "Equal") ClearBtn_Action("AC");
            // Check if display is filled
            int maxLength = 10;
            string displayStringOrExp = displayString;
            if (!displayString.Contains('e')) // Check length of number without '-' or '.' < 10
            {
                if (displayString.Contains('-')) maxLength++;
                if (displayString.Contains('.')) maxLength++;
                if (value == "Exp") maxLength++; // 'e' is allowed even if the main display is full
            }
            else // Check length of exponent without '-' < 2
            {
                displayStringOrExp = displayString.Split('e')[1].TrimStart('-');
                maxLength = 2;
            }
            if (displayStringOrExp.Length >= maxLength) return;

            if (unaryOpPressed)
            {
                switch (value)
                {
                    case ".":
                    case "Exp":
                        return;
                    default:
                        ClearBtn_Action("C");
                        displayString = value;
                        UpdateScreen();
                        break;
                }
            }
            // Normal operation
            else
            {
                switch (value)
                {
                    case ".":
                        // Display already contains '.' or 'e'
                        if (displayString.Contains('.') || displayString.Contains('e')) return;
                        // Pressed '.' without first entering '0'
                        if (displayString == "") displayString = "0.";
                        // Normal operation
                        else displayString += ".";
                        break;
                    case "Exp":
                        int displayStringLength = displayString.Length;
                        // Display is empty or already contains 'e'
                        if (displayString == "" || displayString.Contains('e')) return;
                        // Display ends with '.', remove '.'
                        if (displayString[displayString.Length - 1] == '.')
                            displayString = displayString.Substring(0, displayStringLength - 1) + "e";
                        // Normal operation
                        else displayString += "e";
                        break;
                    case "0":
                        if (displayString == "0") return;
                        else displayString += value;
                        break;
                    default:
                        // Add numbers (1 - 9)
                        if (displayString == "0") displayString = value;
                        else displayString += value;
                        break;
                }
                displayFormula = displayString;
                UpdateScreen();
            }
        }

        private void BinaryOp_Action(string opr)
        {
            if (disabledMode || mathError) return;
            if (displayString == "" && !firstOpPressed) return;
            if (displayString != "")
            {
                if (!unaryOpPressed)
                {
                    FormatDisplayString();
                    displayFormula = displayString;
                }
                if (currentOp != "PlusMinus") SaveFormula(prevOpIcon: currentOpIcon);
                DisplayStringToValue();
                CalculateNewOperand();
                displayString = "";
                displayFormula = "";
            }
            switch (opr)
            {
                case "Add":
                    currentOpIcon = "+";
                    break;
                case "Subtract":
                    currentOpIcon = "-";
                    break;
                case "Multiply":
                    currentOpIcon = "×";
                    break;
                case "Divide":
                    currentOpIcon = "÷";
                    break;
                case "Modulus":
                    currentOpIcon = "%";
                    break;
                case "Root":
                    currentOpIcon = "^(1/";
                    break;
                case "Power":
                    currentOpIcon = "^";
                    break;
            }
            currentOp = opr;
            unaryOpPressed = false;
            calcChangeAllowed = false;
            UpdateScreen();
        }

        private void CalculateNewOperand()
        {
            if (firstOpPressed)
            {
                switch (currentOp)
                {
                    case "Add":
                        operand += displayValue;
                        break;
                    case "Subtract":
                        operand -= displayValue;
                        break;
                    case "Multiply":
                        operand *= displayValue;
                        break;
                    case "Divide":
                        operand /= displayValue;
                        break;
                    case "Modulus":
                        operand %= displayValue;
                        break;
                    case "Root":
                        operand = Math.Pow(operand, 1 / displayValue);
                        break;
                    case "Power":
                        operand = Math.Pow(operand, displayValue);
                        break;
                    case "Equal":
                        break;
                }
            }
            else
            {
                if (currentOp != "Equal") operand = displayValue;
                firstOpPressed = true;
            }
        }

        private void UnaryOp_Action(string opr)
        {   
            if (disabledMode || mathError) return;
            if (displayString == "") return;
            if (opr == "PlusMinus")
            {
                if (displayString == "") return;
                // PlusMinus after pressing equal button
                else if (currentOp == "Equal" && !firstOpPressed)
                {
                    currentOp = "PlusMinus";
                    formulaSaved = "-Ans";
                    currentOpIcon = "";
                    displayFormula = "";
                    if (displayString[0] == '-') displayString = displayString.TrimStart('-');
                    else displayString = '-' + displayString;
                }
                else if (currentOp == "PlusMinus")
                {
                    if (formulaSaved[0] == '-') formulaSaved = formulaSaved.TrimStart('-');
                    else formulaSaved = '-' + formulaSaved;
                    if (displayString[0] == '-') displayString = displayString.TrimStart('-');
                    else displayString = '-' + displayString;
                }
                // PlusMinus on exponent
                else if (displayString.Contains('e'))
                {
                    string displayStringSignificand = displayString.Split('e')[0],
                        displayStringExp = displayString.Split('e')[1];
                    if (displayStringExp == "") return;
                    else if (displayStringExp[0] == '-')
                        displayString = displayStringSignificand + "e" + displayStringExp.TrimStart('-');
                    else displayString = displayStringSignificand + "e-" + displayStringExp;
                    displayFormula = displayString;
                }
                // Normal operation
                else
                {
                    if (displayFormula[0] == '-') displayFormula = displayFormula.TrimStart('-');
                    else displayFormula = '-' + displayFormula;
                    if (displayString[0] == '-') displayString = displayString.TrimStart('-');
                    else displayString = '-' + displayString;
                }
                DisplayStringToValue();
            }
            else
            {
                if (currentOp == "Equal")
                {
                    formulaSaved = "";
                    currentOpIcon = "";
                    displayFormula = "Ans";
                    currentOp = "";
                }
                else if (!unaryOpPressed)
                {
                    FormatDisplayString();
                    displayFormula = displayString;
                    DisplayStringToValue();
                }
                else DisplayStringToValue();
                switch (opr)
                {
                    case "Sin":
                        if (calcDegMode) displayValue = displayValue * (Math.PI / 180.0);
                        displayValue = Math.Sin(displayValue);
                        displayFormula = $"sin({displayFormula})";
                        break;
                    case "Cos":
                        if (calcDegMode) displayValue = displayValue * (Math.PI / 180.0);
                        displayValue = Math.Cos(displayValue);
                        displayFormula = $"cos({displayFormula})";
                        break;
                    case "Tan":
                        if (calcDegMode) displayValue = displayValue * (Math.PI / 180.0);
                        displayValue = Math.Tan(displayValue);
                        displayFormula = $"tan({displayFormula})";
                        break;
                    case "Log":
                        displayValue = Math.Log10(displayValue);
                        displayFormula = $"log({displayFormula})";
                        break;
                    case "LogInv":
                        displayValue = Math.Pow(10.0, displayValue);
                        displayFormula = $"10^({displayFormula})";
                        break;
                    case "Ln":
                        displayValue = Math.Log(displayValue);
                        displayFormula = $"ln({displayFormula})";
                        break;
                    case "LnInv":
                        displayValue = Math.Pow(Math.E, displayValue);
                        displayFormula = $"e^({displayFormula})";
                        break;
                    case "Sqrt":
                        displayValue = Math.Sqrt(displayValue);
                        displayFormula = $"√({displayFormula})";
                        break;
                    case "Square":
                        displayValue = Math.Pow(displayValue, 2);
                        displayFormula = $"({displayFormula})^2";
                        break;
                    case "Inverse":
                        displayValue = 1.0 / displayValue;
                        displayFormula = $"1/({displayFormula})";
                        break;
                    case "Factorial":
                        displayValue = Factorial(displayValue);
                        displayFormula = $"({displayFormula})!";
                        break;
                    case "ArcSin":
                        displayValue = Math.Asin(displayValue);
                        displayFormula = $"asin({displayFormula})";
                        if (calcDegMode) displayValue = displayValue * (180.0 / Math.PI);
                        break;
                    case "ArcCos":
                        displayValue = Math.Acos(displayValue);
                        displayFormula = $"acos({displayFormula})";
                        if (calcDegMode) displayValue = displayValue * (180.0 / Math.PI);
                        break;
                    case "ArcTan":
                        displayValue = Math.Atan(displayValue);
                        displayFormula = $"atan({displayFormula})";
                        if (calcDegMode) displayValue = displayValue * (180.0 / Math.PI);
                        break;
                }
                unaryOpPressed = true;
                calcChangeAllowed = false;
                DisplayValueToString();
            }
            UpdateScreen();
        }

        private double Factorial(double value)
        {
            if (value % 1 != 0 || value < 0) return Double.NaN;
            int valueInt = (int)value;
            double result = 1;
            for (int n = 1; n <= valueInt; n++) result *= n;
            return result;
        }

        private void SaveFormula(string prevOpIcon = null)
        {
            switch (currentOp)
            {
                case "Root":
                    formulaSaved = formulaSaved + prevOpIcon + displayFormula + ")";
                    break;
                case "Equal":
                    formulaSaved = "Ans";
                    break;
                default:
                    formulaSaved = formulaSaved + prevOpIcon + displayFormula;
                    break;
            }
        }

        private void FncBtn_Action(string function)
        {
            if (disabledMode && function != "Delete") return;
            string copyDisplayString = "";
            if (function == "Copy" || function == "CopyShift")
            {   
                string[] displayStringSplit = displayString.Split('e');
                if (displayStringSplit.Length > 1)
                    copyDisplayString = displayStringSplit[0].TrimEnd('.') + "e" + displayStringSplit[1];
                else copyDisplayString = displayStringSplit[0].TrimEnd('.');
            }
            switch (function)
            {
                case "CalcMode":
                    if (!calcChangeAllowed) return;
                    calcSciMode = !calcSciMode;
                    if (calcSciMode)
                    {
                        btnCalcMode.Text = "STD";
                        lblCalcMode.Text = "SCI";
                        btnTrigMode.Visible = true;
                        btnDel.Location = new Point(344, 197);
                        btnSin.Visible = true;
                        btnCos.Visible = true;
                        btnTan.Visible = true;
                        btnLog.Visible = true;
                        btnLn.Visible = true;
                        btnSqrt.Visible = true;
                        btnSquare.Visible = true;
                        btnExp.Visible = true;
                        btnInverse.Visible = true;
                        lblShiftSin.Visible = true;
                        lblShiftCos.Visible = true;
                        lblShiftTan.Visible = true;
                        lblShiftLog.Visible = true;
                        lblShiftLn.Visible = true;
                        lblShiftSqrt.Visible = true;
                        lblShiftSquare.Visible = true;
                        lblShiftInverse.Visible = true;
                        lblDeg.Visible = true;
                        lblRad.Visible = true;

                        btn7.Top = 302;
                        btn8.Top = btn7.Top;
                        btn9.Top = btn7.Top;
                        btnC.Top = btn7.Top;
                        btnAC.Top = btn7.Top;
                        lblShiftDivide.Top = btn7.Top + 45;
                        btn4.Top = btn7.Top + 58;
                        btn5.Top = btn4.Top;
                        btn6.Top = btn4.Top;
                        btnMultiply.Top = btn4.Top;
                        btnDivide.Top = btn4.Top;
                        btn1.Top = btn4.Top + 58;
                        btn2.Top = btn1.Top;
                        btn3.Top = btn1.Top;
                        btnAdd.Top = btn1.Top;
                        btnSubtract.Top = btn1.Top;
                        btn0.Top = btn1.Top + 58;
                        btnDP.Top = btn0.Top;
                        btnPlusMinus.Top = btn0.Top;
                        btnEqual.Top = btn0.Top;
                        lblID.Top = btn0.Top + 51;
                        this.Height = 594;
                    }
                    else
                    {
                        btnCalcMode.Text = "SCI";
                        lblCalcMode.Text = "STD";
                        btnTrigMode.Visible = false;
                        btnDel.Location = new Point(344, 142);
                        btnSin.Visible = false;
                        btnCos.Visible = false;
                        btnTan.Visible = false;
                        btnLog.Visible = false;
                        btnLn.Visible = false;
                        btnSqrt.Visible = false;
                        btnSquare.Visible = false;
                        btnExp.Visible = false;
                        btnInverse.Visible = false;
                        lblShiftSin.Visible = false;
                        lblShiftCos.Visible = false;
                        lblShiftTan.Visible = false;
                        lblShiftLog.Visible = false;
                        lblShiftLn.Visible = false;
                        lblShiftSqrt.Visible = false;
                        lblShiftSquare.Visible = false;
                        lblShiftInverse.Visible = false;
                        lblDeg.Visible = false;
                        lblRad.Visible = false;

                        btn7.Top = 194;
                        btn8.Top = btn7.Top;
                        btn9.Top = btn7.Top;
                        btnC.Top = btn7.Top;
                        btnAC.Top = btn7.Top;
                        btn4.Top = btn7.Top + 58;
                        lblShiftDivide.Top = btn7.Top + 45;
                        btn5.Top = btn4.Top;
                        btn6.Top = btn4.Top;
                        btnMultiply.Top = btn4.Top;
                        btnDivide.Top = btn4.Top;
                        btn1.Top = btn4.Top + 58;
                        btn2.Top = btn1.Top;
                        btn3.Top = btn1.Top;
                        btnAdd.Top = btn1.Top;
                        btnSubtract.Top = btn1.Top;
                        btn0.Top = btn1.Top + 58;
                        btnDP.Top = btn0.Top;
                        btnPlusMinus.Top = btn0.Top;
                        btnEqual.Top = btn0.Top;
                        lblID.Top = btn0.Top + 51;
                        this.Height = 486;
                    }
                    break;
                case "Speaker":
                    audioPlayer.ToggleResultsVoice();
                    if (audioPlayer.resultsVoiceEnabled) lblRslt.ForeColor = displayForeColor;
                    else lblRslt.ForeColor = lblInactiveColor;
                    break;
                case "SpeakerShift":
                    audioPlayer.ToggleClickSounds();
                    if (audioPlayer.clickSoundsEnabled) lblClk.ForeColor = displayForeColor;
                    else lblClk.ForeColor = lblInactiveColor;
                    break;
                case "Copy":
                    if (currentOp != "Equal") Clipboard.SetText("0");
                    else Clipboard.SetText(copyDisplayString);
                    break;
                case "CopyShift":
                    if (currentOp != "Equal") return;
                    else Clipboard.SetText(formulaSaved + "=" + copyDisplayString);
                    break;
                case "TrigMode":
                    if (!calcChangeAllowed) return;
                    calcDegMode = !calcDegMode;
                    if (calcDegMode)
                    {
                        lblDeg.ForeColor = displayForeColor;
                        lblRad.ForeColor = lblInactiveColor;
                        btnTrigMode.Text = "RAD";
                    }
                    else
                    {
                        lblDeg.ForeColor = lblInactiveColor;
                        lblRad.ForeColor = displayForeColor;
                        btnTrigMode.Text = "DEG";
                    }
                    break;
                case "Delete":
                    if (!disabledMode)
                    {
                        if (unaryOpPressed)
                        {
                            displayFormula = displayFormula.Substring(0, displayFormula.Length - 1);
                            formulaSaved = formulaSaved + currentOpIcon + displayFormula;
                            currentOpIcon = "";
                            displayFormula = "";
                            disabledMode = true;
                        }
                        else if (currentOp == "Equal")
                        {
                            currentOp = "";
                            currentOpIcon = "";
                            displayFormula = "";
                            disabledMode = true;
                        }
                        else if (displayFormula != "")
                        {
                            displayString = displayString.Substring(0, displayString.Length - 1);
                            displayFormula = displayString;
                        }
                        else
                        {
                            if (currentOp != "")
                            {
                                currentOpIcon = "";
                                DisplayValueToString(operand);
                            }
                            else
                            {
                                if (formulaSaved == "") return;
                                formulaSaved = formulaSaved.Substring(0, formulaSaved.Length - 1);
                            }
                            disabledMode = true;
                        }
                    }
                    else
                    {
                        formulaSaved = formulaSaved.Substring(0, formulaSaved.Length - 1);
                        if (formulaSaved == "") ClearBtn_Action("AC");
                    }
                    UpdateScreen();
                    break;
            }
        }
    }
}