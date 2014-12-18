using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlauLib.WinForms.Controls
{
    public class HintTextBox : TextBox
    {
        public string HintText { get; set; }
        public bool TrimText { get; set; }
        public bool HasCustomText { get; private set; }

        private bool _isSelfSettingText;

        public HintTextBox()
        {
            HasCustomText = false;
            ShowHintText();
        }

        public void SetHintText(string newHint)
        {
            HintText = newHint;
            if (!HasCustomText && !Focused)
            {
                ShowHintText();
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (!HasCustomText && !Focused)
            {
                ShowHintText();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (!_isSelfSettingText)
            {
                string text = Text;
                if (TrimText)
                {
                    text = text.Trim();
                }
                HasCustomText = !String.IsNullOrWhiteSpace(text);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (!HasCustomText)
            {
                // Remove the Hint Text
                Text = String.Empty;
                ForeColor = Color.Black;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (!HasCustomText)
            {
                ShowHintText();
            }
        }

        private void ShowHintText()
        {
            _isSelfSettingText = true;
            Text = HintText;
            ForeColor = Color.Gray;
            _isSelfSettingText = false;
        }
    }
}
