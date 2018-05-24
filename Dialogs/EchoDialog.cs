using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;
        bool FirstTimeChat = false;


        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            

            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                switch (message.Text.ToString())
                {
                    case "สวัสดี":
                        await context.PostAsync("สวัสดีขอรับอ้อเจ้า");
                        FirstTimeChat = true;
                        break;
                    case "สวัสดีครับ":
                        await context.PostAsync("สวัสดีสุดหล่อ");
                        FirstTimeChat = true;
                        break;
                    case "สวัสดีค่ะ":
                        await context.PostAsync("สวัสดีน้องสาว");
                        FirstTimeChat = true;
                        break;
                    case "สวัสดีคะ":
                        await context.PostAsync("คำว่า คะ ใช้ต่อหลังประโยคคำถาม 'เท่าไรคะ' 'อะไรคะ' อย่าใช้สลับกันนะคะ");
                        FirstTimeChat = true;
                        break;
                    default:
                        if (FirstTimeChat == true)
                        {
                            await context.PostAsync($"{this.count++}: คุณกำลังพูดว่า: {message.Text}");
                            FirstTimeChat = true;
                        }
                        else
                        {
                            await context.PostAsync("มารยาทแบบไทย ๆ เริ่มด้วยคำว่า สวัสดี ครับ/ค่ะ");
                            FirstTimeChat = true;
                        }
                        break;
                }
                
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}