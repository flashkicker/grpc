using App2.Services;
using COS.SDK;
using Google.Protobuf;
using Grpc.Net.Client;
using GRPC2;
using GrpcDotNetNamedPipes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App2
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
            //var result = (MessageReply)await COSSDK.CallAsync("GRPC1.GRPC1Service.SendMessage", new object[] { new MessageRequest { Message = "Hello App1, this is a message from App2" }, typeof(MessageReply) });

            //var localResult = (MessageReply)await COSSDK.CallAsync("App2Service.SendMessage", new object[] { new MessageRequest { Message = "Hello App1, this is a message from App2" }, null } );

            var messageRequest = new MessageRequest { Message = "Hello App1, this is a remote message from App2" };

            var res1 = await COSSDK.CallAsync<MessageRequest, MessageReply>("GRPC1.GRPC1Service", "SendMessage", messageRequest);


            var localMessageReq = new MessageRequest { Message = "Hello App1, this is an internal message from App2" } ;

            var localRes = await COSSDK.CallAsync<MessageRequest, MessageReply>(ServiceHelper.GetServiceName(typeof(App2Service)), "SendMessage", localMessageReq);
        }
    }
}
