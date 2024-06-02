using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Calculator_231581L
{
    public partial class MainForm_231581L : Form
    {
        Control ctrlPressed = new Control(), shiftPressed = new Control();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!msg.HWnd.Equals(this.Handle) &&
                (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Tab))
                return true;
            if ((keyData & Keys.Control) == Keys.Control)
            {
                if (downedControlE.ContainsKey(ctrlPressed)) downedControlE.Remove(ctrlPressed);
                else downedControlE[ctrlPressed] = null;
            }
            if ((keyData & Keys.Shift) == Keys.Shift)
            {
                if (downedControlE.ContainsKey(shiftPressed)) downedControlE.Remove(shiftPressed);
                else downedControlE[shiftPressed] = null;
            } 
            switch (keyData)
            {
                case Keys.Left:
                    if (downedControlE.ContainsKey(ctrlPressed))
                        lblArrow_Down(lblLeftArrow, new KeyEventArgs((Keys)Enum.Parse(typeof(Keys), "Left")), maxDirection: true);
                    else lblArrow_Down(lblLeftArrow, new KeyEventArgs((Keys)Enum.Parse(typeof(Keys), "Left")));
                    return true;
                case Keys.Right:
                    if (downedControlE.ContainsKey(ctrlPressed))
                        lblArrow_Down(lblRightArrow, new KeyEventArgs((Keys)Enum.Parse(typeof(Keys), "Right")), maxDirection: true);
                    else lblArrow_Down(lblRightArrow, new KeyEventArgs((Keys)Enum.Parse(typeof(Keys), "Right")));
                    return true;
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_231581L_Load(object sender, EventArgs e)
        {
            ClearBtn_Action("AC");
        }

        private void MainForm_231581L_Shown(object sender, EventArgs e)
        {
            btnEqual.Focus();
        }

        // Pairs button (key) with its KeyEventArgs (value). KeyEventArgs e is null if button is pressed by mouse.
        // Prevents unique inputs that trigger the same button to prevent UI inconsistency.
        Dictionary<Control, KeyEventArgs> downedControlE = new Dictionary<Control, KeyEventArgs>();
        // Variable to track which button has been pressed by a mouse
        object mouseDownedControl = null;

        /* KEYBOARD PRESSES */
        private void MainForm_231581L_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if ((e.Modifiers != Keys.Shift) && (e.Modifiers != Keys.Control))
            {
                switch (key) // Switch statement used for consistent access time
                {
                    case Keys.CapsLock:
                        ShiftBtn_DownAction();
                        break;
                    case Keys.Add:
                        StdBtn_Down(btnAdd, e);
                        break;
                    case Keys.OemMinus:
                    case Keys.Subtract:
                        StdBtn_Down(btnSubtract, e);
                        break;
                    case Keys.Multiply:
                        StdBtn_Down(btnMultiply, e);
                        break;
                    case Keys.OemQuestion:
                    case Keys.Divide:
                        StdBtn_Down(btnDivide, e);
                        break;
                    case Keys.D0:
                    case Keys.NumPad0:
                        StdBtn_Down(btn0, e);
                        break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        StdBtn_Down(btn1, e);
                        break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        StdBtn_Down(btn2, e);
                        break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        StdBtn_Down(btn3, e);
                        break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        StdBtn_Down(btn4, e);
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        StdBtn_Down(btn5, e);
                        break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        StdBtn_Down(btn6, e);
                        break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        StdBtn_Down(btn7, e);
                        break;
                    case Keys.D8:
                    case Keys.NumPad8:
                        StdBtn_Down(btn8, e);
                        break;
                    case Keys.D9:
                    case Keys.NumPad9:
                        StdBtn_Down(btn9, e);
                        break;
                    case Keys.OemPeriod:
                    case Keys.Decimal:
                        StdBtn_Down(btnDP, e);
                        break;
                    case Keys.E:
                        SciBtn_Down(btnExp);
                        break;
                    case Keys.Back:
                        FncBtn_Down(btnDel, e); // BACKSPACE to delete
                        break;
                    case Keys.Enter:
                        EqualBtn_DownAction();
                        break;
                    default:
                        return;
                }
            }
            else if (e.Modifiers == Keys.Shift)
            {
                switch (key)
                {
                    case Keys.Oemplus:
                        StdBtn_Down(btnAdd, e);
                        break;
                    case Keys.D8:
                        StdBtn_Down(btnMultiply, e);
                        break;
                    case Keys.Back:
                        ClearBtn_Down(btnAC, e); // SHIFT + BACKSPACE to all clear
                        break;
                    default:
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (key)
                {
                    case Keys.Back:
                        ClearBtn_Down(btnC, e); // CTRL + BACKSPACE to clear
                        break;
                    case Keys.C:
                        FncBtn_Down(btnCopy, e);
                        break;
                    default:
                        break;
                }
            }
        }
        private void MainForm_231581L_KeyUp(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            Control keyToModify = null;
            switch (key) // Switch statement used for consistent access time
            {
                case Keys.ShiftKey:
                    foreach (KeyValuePair<Control, KeyEventArgs> kvp in downedControlE)
                    {
                        if (kvp.Value == null) continue;
                        if (kvp.Value.Modifiers == Keys.Shift)
                        {
                            keyToModify = kvp.Key;
                            break;
                        }
                    }
                    if (keyToModify != null)
                    {
                        switch (keyToModify.Tag)
                        {
                            case "AC":
                                ClearBtn_Up(btnAC);
                                break;
                            case "Multiply":
                                StdBtn_Up(btnMultiply);
                                break;
                            case "Add":
                                StdBtn_Up(btnAdd);
                                break;
                        }
                    }
                    break;
                case Keys.ControlKey:
                    foreach (KeyValuePair<Control, KeyEventArgs> kvp in downedControlE)
                    {
                        if (kvp.Value == null) continue;
                        if (kvp.Value.Modifiers == Keys.Control)
                        {
                            keyToModify = kvp.Key;
                            break;
                        }
                    }
                    if (keyToModify != null)
                    {
                        switch (keyToModify.Tag)
                        {
                            case "C":
                                ClearBtn_Up(btnC);
                                break;
                            case "Copy":
                                FncBtn_Up(btnCopy);
                                break;
                        }
                    }
                    break;
                case Keys.CapsLock:
                    ShiftBtn_UpAction();
                    break;
                case Keys.Oemplus:
                    if (downedControlE.ContainsKey(btnAdd) && downedControlE[btnAdd] != null)
                        StdBtn_Up(btnAdd);
                    break;
                case Keys.Add:
                    StdBtn_Up(btnAdd);
                    break;
                case Keys.OemMinus:
                case Keys.Subtract:
                    StdBtn_Up(btnSubtract);
                    break;
                case Keys.D8:
                    if (downedControlE.ContainsKey(btn8) && downedControlE[btn8] != null)
                    {
                        if (downedControlE[btn8].Modifiers == Keys.None)
                            StdBtn_Up(btn8);
                    }
                    else StdBtn_Up(btnMultiply);
                    break;
                case Keys.Multiply:
                    StdBtn_Up(btnMultiply);
                    break;
                case Keys.OemQuestion:
                case Keys.Divide:
                    StdBtn_Up(btnDivide);
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                    StdBtn_Up(btn0);
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    StdBtn_Up(btn1);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    StdBtn_Up(btn2);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    StdBtn_Up(btn3);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    StdBtn_Up(btn4);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    StdBtn_Up(btn5);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    StdBtn_Up(btn6);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    StdBtn_Up(btn7);
                    break;
                case Keys.NumPad8:
                    StdBtn_Up(btn8);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    StdBtn_Up(btn9);
                    break;
                case Keys.OemPeriod:
                case Keys.Decimal:
                    StdBtn_Up(btnDP);
                    break;
                case Keys.E:
                    SciBtn_Up(btnExp);
                    break;
                case Keys.Back:
                    if (downedControlE.ContainsKey(btnDel) && downedControlE[btnDel] != null)
                    {
                        if (downedControlE[btnDel].Modifiers == Keys.None) // Backspace key was pressed with no modifiers
                            FncBtn_Up(btnDel);
                    }
                    else if (downedControlE.ContainsKey(btnC) && downedControlE[btnC] != null)
                    {
                        if (downedControlE[btnC].Modifiers == Keys.Control) // Backspace key was pressed with CTRL modifier
                            ClearBtn_Up(btnC);
                    }
                    else ClearBtn_Up(btnAC); // Backspace key was pressed with SHIFT modifier
                    break;
                case Keys.Enter:
                    EqualBtn_UpAction();
                    break;
                case Keys.C:
                    if (downedControlE.ContainsKey(btnCopy) && downedControlE[btnCopy] != null) // C key was pressed with CTRL modifier
                        FncBtn_Up(btnCopy);
                    break;
                case Keys.Left:
                    lblArrow_Up(lblLeftArrow);
                    break;
                case Keys.Right:
                    lblArrow_Up(lblRightArrow);
                    break;
                default:
                    break;
            }
        }

        /* EQUAL BUTTON */
        private void Enter_Click(object sender, EventArgs e)
        {
            EqualBtn_DownAction();
        }
        private void EqualBtn_UpAction(object sender = null, MouseEventArgs e = null)
        {
            if (mouseDownedControl != btnEqual && !downedControlE.ContainsKey(btnEqual)) return;
            else mouseDownedControl = null;
            audioPlayer.PlayResource(Properties.Resources.SPACE_R);
            btnEqual.BackColor = stdBtnBackColor;
            btnEqual.ForeColor = btnForeColor;
            downedControlE.Remove(btnEqual);
        }
        private void EqualBtn_MouseLeave(object sender, EventArgs e)
        {
            if (btnEqual == mouseDownedControl) EqualBtn_UpAction();
        }

        /* SHIFT BUTTON */
        private void ShiftBtn_DownAction(object sender = null, MouseEventArgs e = null)
        {
            if (downedControlE.ContainsKey(btnShift)) return;
            else downedControlE.Add(btnShift, null);
            if (e != null) mouseDownedControl = btnShift;
            audioPlayer.PlayResource(Properties.Resources.ENTER_P);
            btnEqual.Focus();
            shiftToggled = !shiftToggled;
            if (shiftToggled)
            {
                btnShift.BackColor = ColorTranslator.FromHtml("#9D9043"); // Shift Toggled (Pressed) BackColor
                btnShift.ForeColor = btnPressedForeColor;
                lblShift.ForeColor = displayForeColor;
                btnSpk.Text = "SPK\nCLK";
                btnCopy.Text = "COPY\nEQN";
            }
            else
            {
                btnShift.BackColor = ColorTranslator.FromHtml("#BCAE62");
                btnShift.ForeColor = btnForeColor;
                lblShift.ForeColor = lblInactiveColor;
                btnSpk.Text = "SPK\nRSLT";
                btnCopy.Text = "COPY\nRSLT";
            }
        }
        private void ShiftBtn_Released()
        {
            audioPlayer.PlayResource(Properties.Resources.ENTER_R);
            btnShift.BackColor = shiftColor;
            btnShift.ForeColor = btnForeColor;
            lblShift.ForeColor = lblInactiveColor;
            btnSpk.Text = "SPK\nRSLT";
            btnCopy.Text = "COPY\nRSLT";
            shiftToggled = false;
        }
        private void ShiftBtn_UpAction(object sender = null, MouseEventArgs e = null)
        {
            if (mouseDownedControl != btnShift && !downedControlE.ContainsKey(btnShift)) return;
            else mouseDownedControl = null;
            audioPlayer.PlayResource(Properties.Resources.ENTER_R);
            downedControlE.Remove(btnShift);
        }
        private void ShiftBtn_MouseLeave(object sender, EventArgs e)
        {
            if (btnShift == mouseDownedControl && !downedControlE.ContainsKey(btnShift)) ShiftBtn_UpAction();
        }

        /* FUNCTION BUTTONS */
        private void FncBtn_Down(Button btn, KeyEventArgs e = null)
        {
            if (downedControlE.ContainsKey(btn)) return;
            else downedControlE[btn] = e;
            audioPlayer.PlayResource(Properties.Resources.BACKSPACE_P);
            switch ((string)btn.Tag)
            {
                case "CalcMode":
                    btnCalcMode.BackColor = ColorTranslator.FromHtml("#1D4D72");
                    break;
                case "Speaker":
                    btnSpk.BackColor = ColorTranslator.FromHtml("#257E37");
                    break;
                case "Copy":
                    btnCopy.BackColor = ColorTranslator.FromHtml("#3D2933");
                    break;
                case "TrigMode":
                    btnTrigMode.BackColor = ColorTranslator.FromHtml("#023B37");
                    break;
                case "Delete":
                    btnDel.BackColor = ColorTranslator.FromHtml("#BE3F23");
                    break;
            }
            if (!shiftToggled)
            {
                switch ((string)btn.Tag)
                {
                    case "CalcMode":
                        FncBtn_Action("CalcMode");
                        break;
                    case "Speaker":
                        FncBtn_Action("Speaker");
                        break;
                    case "Copy":
                        FncBtn_Action("Copy");
                        break;
                    case "TrigMode":
                        FncBtn_Action("TrigMode");
                        break;
                    case "Delete":
                        FncBtn_Action("Delete");
                        break;
                }
            }
            else
            {
                switch ((string)btn.Tag)
                {
                    case "Speaker":
                        FncBtn_Action("SpeakerShift");
                        break;
                    case "Copy":
                        FncBtn_Action("CopyShift");
                        break;
                    default:
                        break;
                }
            }
            if (shiftToggled) ShiftBtn_Released();
        }
        private void FncBtn_Up(Button btn)
        {
            audioPlayer.PlayResource(Properties.Resources.BACKSPACE_R);
            switch ((string)btn.Tag)
            {
                case "CalcMode":
                    btnCalcMode.BackColor = ColorTranslator.FromHtml("#296EA3");
                    break;
                case "Speaker":
                    btnSpk.BackColor = ColorTranslator.FromHtml("#32AE4B");
                    break;
                case "Copy":
                    btnCopy.BackColor = ColorTranslator.FromHtml("#704C5E");
                    break;
                case "TrigMode":
                    btnTrigMode.BackColor = ColorTranslator.FromHtml("#04776F");
                    break;
                case "Delete":
                    btnDel.BackColor = ColorTranslator.FromHtml("#DE6449");
                    break;
            }           
            downedControlE.Remove(btn);
        }
        private void FncBtn_MouseDown(object sender, MouseEventArgs e)
        {
            btnEqual.Focus();
            if (!downedControlE.ContainsKey((Button)sender)) mouseDownedControl = (Button)sender;
            else return;
            FncBtn_Down((Button)sender);
        }
        private void FncBtn_MouseUpOrLeave(object sender, MouseEventArgs e)
        {
            if ((Button)sender == mouseDownedControl) FncBtn_Up((Button)sender);
            mouseDownedControl = null;
        }
        private void FncBtn_MouseUpOrLeave(object sender, EventArgs e)
        {
            if ((Button)sender == mouseDownedControl) FncBtn_Up((Button)sender);
            mouseDownedControl = null;
        }
        private void lblArrow_Down(Label lbl, KeyEventArgs e = null, bool maxDirection = false)
        {
            downedControlE.Remove(ctrlPressed);
            if (formulaText.Length <= 24) return;
            if (!downedControlE.ContainsKey(lbl))
            {
                switch (random.Next(0, 5))
                {
                    case 0:
                        audioPlayer.PlayResource(Properties.Resources.ALT_GENERIC_P0);
                        break;
                    case 1:
                        audioPlayer.PlayResource(Properties.Resources.ALT_GENERIC_P1);
                        break;
                    case 2:
                        audioPlayer.PlayResource(Properties.Resources.ALT_GENERIC_P2);
                        break;
                    case 3:
                        audioPlayer.PlayResource(Properties.Resources.ALT_GENERIC_P3);
                        break;
                    case 4:
                        audioPlayer.PlayResource(Properties.Resources.ALT_GENERIC_P4);
                        break;
                }
                downedControlE[lbl] = e;
            }
            switch((string)lbl.Tag)
            {
                case "Left":
                    break;
                case "Right":
                    break;
            }
            lblArrow_Action((string)lbl.Tag, maxDirection);
        }
        private void lblArrow_Up(Label lbl)
        {   
            if (downedControlE.ContainsKey(lbl)) audioPlayer.PlayResource(Properties.Resources.ALT_GENERIC_R);
            downedControlE.Remove(lbl);
        }
        private void lblArrow_MouseDown(object sender, MouseEventArgs e)
        {
            btnEqual.Focus();
            if (!downedControlE.ContainsKey((Label)sender)) mouseDownedControl = (Label)sender;
            else return;
            lblArrow_Down((Label)sender);
        }
        private void lblArrow_MouseUpOrLeave(object sender, MouseEventArgs e)
        {
            if ((Label)sender == mouseDownedControl) lblArrow_Up((Label)sender);
            mouseDownedControl = null;
        }
        private void lblArrow_MouseUpOrLeave(object sender, EventArgs e)
        {
            if ((Label)sender == mouseDownedControl) lblArrow_Up((Label)sender);
            mouseDownedControl = null;
        }

        /* CLEAR BUUTTONS (C AND AC) */
        private void ClearBtn_Down(Button btn, KeyEventArgs e = null)
        {
            if (downedControlE.ContainsKey(btn)) return;
            else downedControlE[btn] = e;
            audioPlayer.PlayResource(Properties.Resources.ENTER_P);
            btn.BackColor = clearBtnPressedBackColor;
            btn.ForeColor = btnPressedForeColor;
            switch (btn.Text)
            {
                case "AC":
                    ClearBtn_Action("AC");
                    break;
                case "C":
                    ClearBtn_Action("C");
                    break;
            }
        }
        private void ClearBtn_Up(Button btn)
        {
            audioPlayer.PlayResource(Properties.Resources.ENTER_R);
            btn.BackColor = clearBtnBackColor;
            btn.ForeColor = btnForeColor;
            downedControlE.Remove(btn);
        }
        private void ClearBtn_MouseDown(object sender, MouseEventArgs e)
        {
            btnEqual.Focus();
            if (!downedControlE.ContainsKey((Button)sender)) mouseDownedControl = (Button)sender;
            else return;
            ClearBtn_Down((Button)sender);
        }
        private void ClearBtn_MouseUpOrLeave(object sender, MouseEventArgs e)
        {
            if ((Button)sender == mouseDownedControl) ClearBtn_Up((Button)sender);
            mouseDownedControl = null;
        }
        private void ClearBtn_MouseUpOrLeave(object sender, EventArgs e)
        {
            if ((Button)sender == mouseDownedControl) ClearBtn_Up((Button)sender);
            mouseDownedControl = null;
        }

        /* NUMPAD AND STANDARD OPERATOR BUTTONS */
        private void StdBtn_Down(Button btn, KeyEventArgs e = null)
        {
            if (downedControlE.ContainsKey(btn)) return;
            else downedControlE[btn] = e;
            switch (random.Next(0, 5))
            {
                case 0:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P0);
                    break;
                case 1:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P1);
                    break;
                case 2:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P2);
                    break;
                case 3:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P3);
                    break;
                case 4:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P4);
                    break;
            }
            btn.BackColor = stdBtnPressedBackColor;
            btn.ForeColor = btnPressedForeColor;
            if (!shiftToggled)
            {
                switch ((string)btn.Tag)
                {
                    case "Add":
                        BinaryOp_Action("Add");
                        break;
                    case "Subtract":
                        BinaryOp_Action("Subtract");
                        break;
                    case "Multiply":
                        BinaryOp_Action("Multiply");
                        break;
                    case "Divide":
                        BinaryOp_Action("Divide");
                        break;
                    case "PlusMinus":
                        UnaryOp_Action("PlusMinus");
                        break;
                    default:
                        NumPad_Action(btn.Text);
                        break;
                }
            }
            else if ((string)btn.Tag == "Divide") BinaryOp_Action("Modulus");
            if (shiftToggled) ShiftBtn_Released();
        }
        private void StdBtn_Up(Button btn)
        {
            audioPlayer.PlayResource(Properties.Resources.GENERIC_R);
            btn.BackColor = stdBtnBackColor;
            btn.ForeColor = btnForeColor;
            downedControlE.Remove(btn);
        }
        private void StdBtn_MouseDown(object sender, MouseEventArgs e)
        {
            btnEqual.Focus();
            if (!downedControlE.ContainsKey((Button)sender)) mouseDownedControl = (Button)sender;
            else return;
            StdBtn_Down((Button)sender);            
        }
        private void StdBtn_MouseUpOrLeave(object sender, MouseEventArgs e)
        {
            if ((Button)sender == mouseDownedControl) StdBtn_Up((Button)sender);
            mouseDownedControl = null;
        }
        private void StdBtn_MouseUpOrLeave(object sender, EventArgs e)
        {
            if ((Button)sender == mouseDownedControl) StdBtn_Up((Button)sender);
            mouseDownedControl = null;
        }

        /* SCIENTIFIC BUTTONS */
        private void SciBtn_Down(Button btn, KeyEventArgs e = null)
        {
            if (downedControlE.ContainsKey(btn)) return;
            else downedControlE[btn] = e;
            switch (random.Next(0, 5))
            {
                case 0:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P0);
                    break;
                case 1:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P1);
                    break;
                case 2:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P2);
                    break;
                case 3:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P3);
                    break;
                case 4:
                    audioPlayer.PlayResource(Properties.Resources.GENERIC_P4);
                    break;
            }
            btn.BackColor = sciBtnPressedBackColor;
            btn.ForeColor = btnPressedForeColor;
            if (!shiftToggled)
            {
                if ((string)btn.Tag == "Exp") NumPad_Action("Exp");
                else UnaryOp_Action((string)btn.Tag);
            }
            else
            {
                switch ((string)btn.Tag)
                {
                    case "Sin":
                        UnaryOp_Action("ArcSin");
                        break;
                    case "Cos":
                        UnaryOp_Action("ArcCos");
                        break;
                    case "Tan":
                        UnaryOp_Action("ArcTan");
                        break;
                    case "Log":
                        UnaryOp_Action("LogInv");
                        break;
                    case "Ln":
                        UnaryOp_Action("LnInv");
                        break;
                    case "Sqrt":
                        BinaryOp_Action("Root");
                        break;
                    case "Square":
                        BinaryOp_Action("Power");
                        break;
                    case "Inverse":
                        UnaryOp_Action("Factorial");
                        break;
                    case "PlusMinus":
                        break;
                }
            }
            if (shiftToggled) ShiftBtn_Released();
        }
        private void SciBtn_Up(Button btn)
        {
            audioPlayer.PlayResource(Properties.Resources.GENERIC_R);
            btn.BackColor = sciBtnBackColor;
            btn.ForeColor = btnForeColor;
            downedControlE.Remove(btn);
        }
        private void SciBtn_MouseDown(object sender, MouseEventArgs e)
        {
            btnEqual.Focus();
            if (!downedControlE.ContainsKey((Button)sender)) mouseDownedControl = (Button)sender;
            else return;
            SciBtn_Down((Button)sender);
        }
        private void SciBtn_MouseUpOrLeave(object sender, MouseEventArgs e)
        {
            if ((Button)sender == mouseDownedControl) SciBtn_Up((Button)sender);
            mouseDownedControl = null;
        }
        private void SciBtn_MouseUpOrLeave(object sender, EventArgs e)
        {
            if ((Button)sender == mouseDownedControl) SciBtn_Up((Button)sender);
            mouseDownedControl = null;
        }
    }
}
