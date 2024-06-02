using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Calculator_231581L
{
    public partial class MainForm_231581L : Form
    {
        // Function to load private fonts into dictionaries (variable sizes)
        private void LoadFont(byte[] fontData, Dictionary<int, Font> font, int[] sizes)
        {
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length); // Allocation of unmanaged memory to hold font data
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0; // Unused variable required by AddFontMemResourceEx
            fonts.AddMemoryFont(fontPtr, fontData.Length);
            AddFontMemResourceEx(fontPtr, (uint)fontData.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr); // Free unmanaged memory

            // Creates dictionary of fonts with requested font sizes
            foreach (int size in sizes) font[size] = new Font(fonts.Families[fonts.Families.Length - 1], size);
        }

        /** FONT **/
        [DllImport("gdi32.dll")] // Import Graphics Device Interface (GDI) library for AddFontMemResourceEx function
        private static extern IntPtr // Method signature for AddFontMemResourceEx
            AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection(); // Collection used to hold private fonts
        private Dictionary<int, Font> fontFed = new Dictionary<int, Font>(),
            fontDSEG = new Dictionary<int, Font>(),
            fontFxEs = new Dictionary<int, Font>();

        /** COLOR **/
        // Set colors here using hex values
        Color btnForeColor = ColorTranslator.FromHtml("#EFEDE7"), // All button text
                btnPressedForeColor = ColorTranslator.FromHtml("#D6D1C2"), // All button text (pressed)
                stdBtnBackColor = ColorTranslator.FromHtml("#54574C"), // Standard button
                stdBtnHoveredBackColor = ColorTranslator.FromHtml("#4A4C42"), // Standard button (hovered)
                stdBtnPressedBackColor = ColorTranslator.FromHtml("#404238"), // Standard button (pressed)
                sciBtnBackColor = ColorTranslator.FromHtml("#2C2D28"), // Scientific button
                sciBtnHoveredBackColor = ColorTranslator.FromHtml("#20201D"), // Scientific button (hovered)
                sciBtnPressedBackColor = ColorTranslator.FromHtml("#20201D"), // Scientific button (pressed)
                clearBtnBackColor = ColorTranslator.FromHtml("#8D4740"), // Clear button
                clearBtnHoveredBackColor = ColorTranslator.FromHtml("#63312C"), // Clear button (hovered)
                clearBtnPressedBackColor = ColorTranslator.FromHtml("#542A26"), // Clear button (pressed)
                displayBackColor = ColorTranslator.FromHtml("#7E8562"), // LCD background
                displayForeColor = ColorTranslator.FromHtml("#1D1E15"), // LCD text
                formulaForeColor = ColorTranslator.FromHtml("#52543B"), // Formula LCD text
                shiftColor = ColorTranslator.FromHtml("#C2B670"), // Shift button and respective labels
                lblInactiveColor = ColorTranslator.FromHtml("#6F7557"); // DEG/ RAD/ SHIFT inactive

        public MainForm_231581L()  // Constructor shifted here
        {
            InitializeComponent();
            this.KeyPreview = true;

            /** FONT **/
            LoadFont(Properties.Resources.DSEG7, fontDSEG, new int[] { 13, 26 });
            LoadFont(Properties.Resources.FED_BOLD, fontFed, new int[] { 6, 8, 10, 12, 14, 16 });
            LoadFont(Properties.Resources.FX_ES, fontFxEs, new int[] { 14 });
            lblDisplay.Font = fontDSEG[26];
            lblSecDisplay.Font = fontDSEG[13];
            lblFormula.Font = fontFxEs[14];

            // STD buttons
            btn0.Font = fontFed[16];
            btnDP.Font = fontFed[16];
            btn1.Font = fontFed[16];
            btn2.Font = fontFed[16];
            btn3.Font = fontFed[16];
            btn4.Font = fontFed[16];
            btn5.Font = fontFed[16];
            btn6.Font = fontFed[16];
            btn7.Font = fontFed[16];
            btn8.Font = fontFed[16];
            btn9.Font = fontFed[16];
            btnC.Font = fontFed[16];
            btnAC.Font = fontFed[14];
            lblShiftDivide.Font = fontFed[8];

            // SCI buttons
            btnSin.Font = fontFed[12];
            btnCos.Font = fontFed[12];
            btnTan.Font = fontFed[12];
            btnLog.Font = fontFed[12];
            btnLn.Font = fontFed[12];
            btnExp.Font = fontFed[10];

            // Function buttons
            btnShift.Font = fontFed[8];
            btnTrigMode.Font = fontFed[10];
            btnCalcMode.Font = fontFed[10];
            btnSpk.Font = fontFed[6];
            btnCopy.Font = fontFed[6];
            btnDel.Font = fontFed[10];

            /** COLOR **/
            // Background color of form (calculator main color)
            this.BackColor = ColorTranslator.FromHtml("#42453C");

            // Display BackColor
            pbDisplayBg.BackColor = displayBackColor;
            lblFormula.BackColor = displayBackColor;
            lblDisplay.BackColor = displayBackColor;
            lblSecDisplay.BackColor = displayBackColor;
            lblLeftArrow.BackColor = displayBackColor;
            lblRightArrow.BackColor = displayBackColor;
            lblExpActive.BackColor = displayBackColor;
            lblShift.BackColor = displayBackColor;
            lblDeg.BackColor = displayBackColor;
            lblRad.BackColor = displayBackColor;
            lblCalcMode.BackColor = displayBackColor;
            lblRslt.BackColor = displayBackColor;
            lblClk.BackColor = displayBackColor;
            lblDisabled.BackColor = displayBackColor;
            // Display ForeColor
            lblFormula.ForeColor = ColorTranslator.FromHtml("#454833");
            lblDisplay.ForeColor = displayForeColor;
            lblSecDisplay.ForeColor = displayForeColor;
            lblFormula.ForeColor = formulaForeColor;
            lblLeftArrow.ForeColor = lblInactiveColor;
            lblRightArrow.ForeColor = lblInactiveColor;
            lblExpActive.ForeColor = displayForeColor;
            lblShift.ForeColor = lblInactiveColor;
            lblDeg.ForeColor = displayForeColor;
            lblRad.ForeColor = lblInactiveColor;
            lblCalcMode.ForeColor = displayForeColor;
            lblRslt.ForeColor = lblInactiveColor;
            lblClk.ForeColor = displayForeColor;
            lblDisabled.ForeColor = displayBackColor;

            // STD buttons ForeColor
            btn0.ForeColor = btnForeColor;
            btnDP.ForeColor = btnForeColor;
            btnPlusMinus.ForeColor = btnForeColor;
            btn1.ForeColor = btnForeColor;
            btn2.ForeColor = btnForeColor;
            btn3.ForeColor = btnForeColor;
            btn4.ForeColor = btnForeColor;
            btn5.ForeColor = btnForeColor;
            btn6.ForeColor = btnForeColor;
            btn7.ForeColor = btnForeColor;
            btn8.ForeColor = btnForeColor;
            btn9.ForeColor = btnForeColor;
            btnMultiply.ForeColor = btnForeColor;
            btnDivide.ForeColor = btnForeColor;
            btnAdd.ForeColor = btnForeColor;
            btnSubtract.ForeColor = btnForeColor;
            btnEqual.ForeColor = btnForeColor;
            btnC.ForeColor = btnForeColor;
            btnAC.ForeColor = btnForeColor;
            // STD buttons BackColor
            btn0.BackColor = stdBtnBackColor;
            btnDP.BackColor = stdBtnBackColor;
            btnPlusMinus.BackColor = stdBtnBackColor;
            btn1.BackColor = stdBtnBackColor;
            btn2.BackColor = stdBtnBackColor;
            btn3.BackColor = stdBtnBackColor;
            btn4.BackColor = stdBtnBackColor;
            btn5.BackColor = stdBtnBackColor;
            btn6.BackColor = stdBtnBackColor;
            btn7.BackColor = stdBtnBackColor;
            btn8.BackColor = stdBtnBackColor;
            btn9.BackColor = stdBtnBackColor;
            btnMultiply.BackColor = stdBtnBackColor;
            btnDivide.BackColor = stdBtnBackColor;
            btnAdd.BackColor = stdBtnBackColor;
            btnSubtract.BackColor = stdBtnBackColor;
            btnEqual.BackColor = stdBtnBackColor;
            btnC.BackColor = clearBtnBackColor;
            btnAC.BackColor = clearBtnBackColor;
            // STD buttons BackColor MouseHover
            btn0.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnDP.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnPlusMinus.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn1.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn2.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn3.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn4.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn5.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn6.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn7.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn8.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btn9.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnMultiply.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnDivide.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnAdd.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnSubtract.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnEqual.FlatAppearance.MouseOverBackColor = stdBtnHoveredBackColor;
            btnC.FlatAppearance.MouseOverBackColor = clearBtnHoveredBackColor;
            btnAC.FlatAppearance.MouseOverBackColor = clearBtnHoveredBackColor;
            // STD buttons BackColor MouseDown
            btn0.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnDP.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnPlusMinus.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn1.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn2.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn3.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn4.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn5.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn6.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn7.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn8.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btn9.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnMultiply.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnDivide.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnAdd.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnSubtract.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnEqual.FlatAppearance.MouseDownBackColor = stdBtnPressedBackColor;
            btnC.FlatAppearance.MouseDownBackColor = clearBtnPressedBackColor;
            btnAC.FlatAppearance.MouseDownBackColor = clearBtnPressedBackColor;

            // SCI buttons ForeColor
            btnSin.ForeColor = btnForeColor;
            btnCos.ForeColor = btnForeColor;
            btnTan.ForeColor = btnForeColor;
            btnLog.ForeColor = btnForeColor;
            btnLn.ForeColor = btnForeColor;
            btnSqrt.ForeColor = btnForeColor;
            btnSquare.ForeColor = btnForeColor;
            btnExp.ForeColor = btnForeColor;
            btnInverse.ForeColor = btnForeColor;
            // SCI buttons BackColor
            btnSin.BackColor = sciBtnBackColor;
            btnCos.BackColor = sciBtnBackColor;
            btnTan.BackColor = sciBtnBackColor;
            btnLog.BackColor = sciBtnBackColor;
            btnLn.BackColor = sciBtnBackColor;
            btnSqrt.BackColor = sciBtnBackColor;
            btnSquare.BackColor = sciBtnBackColor;
            btnExp.BackColor = sciBtnBackColor;
            btnInverse.BackColor = sciBtnBackColor;
            // SCI buttons BackColor MouseHover
            btnSin.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnCos.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnTan.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnLog.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnLn.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnSqrt.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnSquare.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnExp.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            btnInverse.FlatAppearance.MouseOverBackColor = sciBtnHoveredBackColor;
            // SCI buttons BackColor MouseDown
            btnSin.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnCos.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnTan.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnLog.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnLn.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnSqrt.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnSquare.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnExp.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;
            btnInverse.FlatAppearance.MouseDownBackColor = sciBtnPressedBackColor;

            // Function buttons ForeColor
            btnShift.ForeColor = btnPressedForeColor;
            btnCalcMode.ForeColor = btnPressedForeColor;
            btnSpk.ForeColor = btnPressedForeColor;
            btnCopy.ForeColor = btnPressedForeColor;
            btnTrigMode.ForeColor = btnPressedForeColor;
            btnDel.ForeColor = btnPressedForeColor;
            // Function buttons BackColor
            btnShift.BackColor = ColorTranslator.FromHtml("#BCAE62");
            btnCalcMode.BackColor = ColorTranslator.FromHtml("#296EA3");
            btnSpk.BackColor = ColorTranslator.FromHtml("#32AE4B");
            btnCopy.BackColor = ColorTranslator.FromHtml("#704C5E");
            btnTrigMode.BackColor = ColorTranslator.FromHtml("#04776F");
            btnDel.BackColor = ColorTranslator.FromHtml("#DE6449");
            // Function buttons BackColor MouseOver
            btnShift.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#B6A754");
            btnCalcMode.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#256393");
            btnSpk.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2E9E44");
            btnCopy.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#614252");
            btnTrigMode.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#04625C");
            btnDel.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#CF4526");
            // Function buttons BackColor MouseDown
            btnShift.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#9D9043");
            btnCalcMode.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#1D4D72");
            btnSpk.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#257E37");
            btnCopy.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#3D2933");
            btnTrigMode.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#023B37");
            btnDel.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#BE3F23");

            // Shift labels ForeColor
            lblShiftDivide.ForeColor = shiftColor;
            lblShiftSin.ForeColor = shiftColor;
            lblShiftCos.ForeColor = shiftColor;
            lblShiftTan.ForeColor = shiftColor;
            lblShiftLog.ForeColor = shiftColor;
            lblShiftLn.ForeColor = shiftColor;
            lblShiftSqrt.ForeColor = shiftColor;
            lblShiftSquare.ForeColor = shiftColor;
            lblShiftInverse.ForeColor = shiftColor;
        }
    }
}
