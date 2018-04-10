using Cognitive.LUIS.Programmatic;
using Cognitive.LUIS.Programmatic.Models;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveLearningBot
{
    [Serializable]
    public class LuisLearnService
    {
        private LuisApp luisApp;
        private LuisProgClient luisProgClient;

        public LuisLearnService()
        {
            var subscriptionKey = ConfigurationManager.AppSettings.Get("LuisSubscriptionKey");
            luisProgClient = new LuisProgClient(subscriptionKey, Location.WestUS);
        }

        public async Task SetNameApp() => luisApp = await luisProgClient.GetAppByNameAsync(ConfigurationManager.AppSettings.Get("LuisAppName"));

        public async Task LearnLatestMessageSended(string intent, IDialogContext dialogContext)
        {
            var message = dialogContext.UserData.GetValue<string>("MessageId");
            if (string.IsNullOrEmpty(message)) return;

            await Learn(message, intent);
            dialogContext.UserData.RemoveValue("MessageId");
        }

        public async Task Learn(string message, string intent)
        {
            var example = CreateExample(message, intent);

            var app = await luisProgClient.GetAppByNameAsync("PresentationBot");
            await luisProgClient.AddExampleAsync(app.Id, app.Endpoints.Production.VersionId, example);

            await luisProgClient.TrainAsync(app.Id, app.Endpoints.Production.VersionId);
            await VerifyStatusTraining(luisProgClient, app);

            await luisProgClient.PublishAsync(app.Id, app.Endpoints.Production.VersionId, false, "westus");
        }

        private async Task VerifyStatusTraining(LuisProgClient client, LuisApp app)
        {
            IEnumerable<Training> trainingList;
            do
            {
                trainingList = await client.GetTrainingStatusListAsync(app.Id, app.Endpoints.Production.VersionId);
            }
            while (!trainingList.All(x => x.Details.Status.Equals("Success")));
        }

        private Example CreateExample(string message, string intent) => new Example
        {
            Text = message,
            IntentName = intent
        };

    }
}