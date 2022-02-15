using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ReportChargeBackViewModel : CustomBaseViewModel
    {
        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand ConfirmCommand { get; set; }


        public ReportChargeBackViewModel()
        {

        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.ConfirmCommand = new RelayCommand<object>(data =>
            {
                RichTextBox rtb = data as RichTextBox;
                TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                string result = textRange.Text.Trim();

                if(string.IsNullOrEmpty(result))
                {
                    this.ShowMessageBox("退单原因不能为空，请重新填写！");
                    return;
                }

                this.Result = result;
                this.CloseContentWindow();
            });
        }
    }
}
