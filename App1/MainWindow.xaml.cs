using COS.SDK;
using Microsoft.UI.Xaml;
using GRPC1;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            //var result = (MessageReply)await COSSDK.CallAsync("GRPC1.GRPC1Service.SendMessage", new object[] { new MessageRequest { Message = "Hello App2, this is a message from App1" }, typeof(MessageReply) });

            //var localResult = (MessageReply)await COSSDK.CallAsync("App2Service.SendMessage", new object[] { new MessageRequest { Message = "Hello App2, this is a message from App1" }, null });

            var messageRequest = new MessageRequest { Message = "HelloWorld.SayHello" };

            var res1 = await COSSDK.CallAsync<string>("App1", "GRPCExecute", new object[] { "str1", "str2" });

            //Debug.WriteLine(res1.Message);
            //var localMessageReq = new MessageRequest { Message = "Hello App2, this is an internal message from App1" };

            //var localRes = await COSSDK.CallAsync<MessageRequest, MessageReply>(ServiceHelper.GetServiceName(typeof(App1Service)), "SendMessage", localMessageReq);
        }
    }
}
