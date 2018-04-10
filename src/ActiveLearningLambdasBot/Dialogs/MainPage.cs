using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace ActiveLearningBot.Dialogs
{
    [Serializable]
    public class MainDialog : BaseLuisDialog<object>
    {
        [NonSerialized]
        private LuisLearnService luisLearnService;

        public MainDialog()
        {
          
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            context.UserData.SetValue("MessageId", result.Query);
            var card = new HeroCard
            {
                Title = "Desculpe...",
                Text = "Ainda estou aprendendo, qual dessas opções representa o que você deseja?",
                Buttons = GetCardActions()
            };

            var activity = context.MakeMessage();
            activity.Id = new Random().Next().ToString();
            activity.Attachments.Add(card.ToAttachment());

            await context.PostAsync(activity);
        }

        private static List<CardAction> GetCardActions()
        {
            return new List<CardAction>
            {
                new CardAction
                {
                    Title = "Desejo apontar as horas",
                    Type = ActionTypes.ImBack,
                    Value = "Desejo apontar as horas"
                },
                new CardAction
                {
                    Title = "Calma Fera, Só estou te cumprimentando",
                    Type = ActionTypes.ImBack,
                    Value = "Só estou te cumprimentando"
                },
                new CardAction
                {
                    Title = "Nenhuma",
                    Type = ActionTypes.ImBack,
                    Value = "Nenhuma"
                }
            };
        }

        [LuisIntent("Saudacao")]
        public async Task Saudacao(IDialogContext context, LuisResult result)
        {
            LearnLatestMessageSended(result.TopScoringIntent.Intent, context);

            await context.PostAsync($"Bem-vindo(a) a Lambda3! Sou o seu assistente virtual, ja apontou suas horas hoje? 🕵");
            context.Wait(MessageReceived);
        }


        [LuisIntent("Requisitar.Ajuda.Bot")]
        public async Task ApontamentoHora(IDialogContext context, LuisResult result)
        {
            LearnLatestMessageSended(result.TopScoringIntent.Intent, context);

            await context.PostAsync($"https://spine.lambda3.com.br/");
            context.Wait(MessageReceived);
        }

        private void LearnLatestMessageSended(string intent, IDialogContext dialogContext)
        {
            luisLearnService = new LuisLearnService();
            luisLearnService.SetNameApp().GetAwaiter();

            HostingEnvironment.QueueBackgroundWorkItem(workItem =>
                luisLearnService?.LearnLatestMessageSended(intent, dialogContext));
        }

    }
}