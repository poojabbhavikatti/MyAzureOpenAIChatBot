using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;
using MyChatBot.Services;

namespace MyChatBot
{
    public class OpenAIBot : ActivityHandler
    {
        private readonly OpenAIService _openAIService;

        public OpenAIBot(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var userMessage = turnContext.Activity.Text;
            var response = await _openAIService.GetOpenAIResponse(userMessage);
            await turnContext.SendActivityAsync(MessageFactory.Text(response), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Welcome! How can I assist you today?";

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText), cancellationToken);
                }
            }
        }

    }
}
