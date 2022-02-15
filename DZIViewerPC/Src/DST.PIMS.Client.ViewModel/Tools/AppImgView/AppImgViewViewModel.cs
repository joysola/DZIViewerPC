using DST.ApiClient.Service;
using DST.Database.Model;
using DST.PIMS.Framework.Extensions;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;

namespace DST.PIMS.Client.ViewModel
{
    public class AppImgViewViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 申请单ViewModel
        /// </summary>
        public AppFrmViewModel AppViewModel { get; set; } = new AppFrmViewModel();

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args[0] is string code)
            {
                var result = ApplyFormService.Instance.GetPathInfobyCode(code) ?? new ApplyFrmModel();
                AppViewModel.AppModel = result;
                //AppViewModel.IsAdd = false; // 编辑模式
                AppViewModel.Permissions.SetPermissionIsEdit(false); // 编辑模式
                Messenger.Default.Send((true, AppViewModel.AppModel?.PathID), EnumMessageKey.GeneralImg); // 更新申请单图像
            }
        }
    }
}
