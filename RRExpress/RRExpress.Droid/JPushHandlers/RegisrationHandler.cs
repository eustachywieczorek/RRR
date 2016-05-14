using Android.OS;
using CN.Jpush.Android.Api;

namespace RRExpress.Droid.JPushHandlers {

    /// <summary>
    /// 
    /// </summary>
    public class RegisrationHandler : BaseHandler {
        public override string Action {
            get {
                return JPushInterface.ActionRegistrationId;
                //return "cn.jpush.android.intent.REGISTRATION";
            }
        }

        public override void Handle(Bundle bundle) {
            //SDK �� JPush Server ע�����õ���ע�� ȫ��Ψһ�� ID ������ͨ���� ID ���Ӧ�Ŀͻ��˷�����Ϣ��֪ͨ��
            var id = bundle.GetString(JPushInterface.ExtraRegistrationId);
        }
    }
}