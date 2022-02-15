using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistReceiptViewModel : CustomBaseViewModel
    {
        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            Messenger.Default.Register<object>(this, EnumMessageKey.PhysDistReceiptClose, data =>
            {
                Messenger.Default.Send<object>(null, EnumMessageKey.RefreshPhysDistSign);
                this.CloseContentWindow();
            });
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            Messenger.Default.Send<object[]>(this.Args, EnumMessageKey.PhysDistReceiptArgsSendToHuman);
        }
    }
}
