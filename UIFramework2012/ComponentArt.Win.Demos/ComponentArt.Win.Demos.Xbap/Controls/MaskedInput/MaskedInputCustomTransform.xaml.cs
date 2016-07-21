using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using ComponentArt.Win.UI.Input;

namespace ComponentArt.Win.Demos
{
    public partial class MaskedInputCustomTransform : UserControl, IDisposable
    {
        //ISBN myISBN;

        public MaskedInputCustomTransform()
        {
            InitializeComponent();
            //myISBN = new ISBN();
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MyMaskedInput.Dispose();
            GC.Collect();
        }
        #endregion

        private void MyMaskedInput_ValidityChanged(object sender, MaskedInputEventArgs evargs)
        {
            System.Diagnostics.Debug.WriteLine("MyMaskedInput Validity: " + evargs.IsValid);
        }
    }

    /// <summary>
    /// ISBN Number (International Standard Book Number)
    /// OK: 
    /// ISBN 1 12345-012-X 
    /// ISBN 2 34565 234 9
    /// 
    /// FAIL: 
    /// ISBN-1-12345-0120-X
    /// ISBN A 12345 3432 X
    /// </summary>
    public class ISBN : MaskedInputTransform
    {
        #region IMaskedInputTransform Members

        private static new Regex validationRegex =
            new Regex(@"^ISBN\s(?=[-0-9xX ]{13}$)(?:[0-9]+[- ]){3}[0-9]*[xX0-9]$", RegexOptions.Singleline);

        MatchCollection matches;
        public ISBN()
        {
            maxLength = 18;
            Restrict = InputTextBoxRestrictions.All;
        }
        public override string Mask(string input)
        {
            return input;
        }

        public override string Unmask(string input)
        {
            return input;
        }

        public override bool Validate(string input)
        {
            matches = validationRegex.Matches(input);
            if (matches.Count > 0)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
 }
