using System.Globalization;
using System.Windows.Forms;

namespace LemballEditor.View
{
    /// <summary>
    /// A text box that only accepts numerical input.
    /// From http://msdn.microsoft.com/en-us/library/ms229644.aspx
    /// </summary>
    public class NumericTextBox : TextBox
    {

        /// <summary>
        /// Restricts the entry of characters to digits (including hex), the negative sign,
        /// the decimal point, and editing keystrokes (backspace).
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // Inform base class
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            _ = numberFormatInfo.NumberDecimalSeparator;
            _ = numberFormatInfo.NumberGroupSeparator;
            _ = numberFormatInfo.NegativeSign;
            _ = e.KeyChar.ToString();

            if (char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            /*
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
             keyInput.Equals(negativeSign))
            {
                // Decimal separator is OK
            }
            */
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            //    {
            //     // Let the edit control handle control and alt key combinations
            //    }
            else if (AllowSpace && e.KeyChar == ' ')
            {

            }
            else
            {
                // Consume this invalid key and beep
                e.Handled = true;

                //    MessageBeep();
            }
        }

        public int IntValue => int.Parse(Text);

        public decimal DecimalValue => decimal.Parse(Text);

        public bool AllowSpace { set; get; } = false;
    }

}
