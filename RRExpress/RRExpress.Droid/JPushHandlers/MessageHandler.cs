using Android.OS;
using CN.Jpush.Android.Api;

namespace RRExpress.Droid.JPushHandlers {

    /// <summary>
    /// SDK ���Զ�����Ϣ��ֻ�Ǵ��ݣ��������κν����ϵ�չʾ��
    /// </summary>
    public class MessageHandler : BaseHandler {
        public override string Action {
            get {
                return "cn.jpush.android.intent.MESSAGE_RECEIVED";
            }
        }

        public override void Handle(Bundle bundle) {
           
            //���������������������Ϣ�ı���
            var title = bundle.GetString(JPushInterface.ExtraTitle);
            
            //���������������������Ϣ����
            var msg = bundle.GetString(JPushInterface.ExtraMessage);
            
            //������������������ĸ����ֶΡ����Ǹ� JSON �ַ�����
            //��Ӧ API ��Ϣ���ݵ� extras �ֶΡ�
            //��Ӧ Portal ������Ϣ�����ϵġ���ѡ���á���ĸ����ֶΡ�
            var extra = bundle.GetString(JPushInterface.ExtraExtra);
            
            //��������������������������͡�
            //��Ӧ API ��Ϣ���ݵ� content_type �ֶΡ�
            var contentType = bundle.GetString(JPushInterface.ExtraContentType);
            
            //SDK 1.4.0 ���ϰ汾֧�֡�
            //��ý��ͨ��Ϣ�������غ���ļ�·�����ļ�����
            var richFilePath = bundle.GetString(JPushInterface.ExtraRichpushFilePath);

            //SDK 1.6.1 ���ϰ汾֧�֡�
            //Ψһ��ʶ��Ϣ�� ID, �������ϱ�ͳ�Ƶȡ�
            var msgID = bundle.GetString(JPushInterface.ExtraMsgId);
        }
    }
}