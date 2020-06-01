using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Helpers
{
    public static class ChannelHelper
    {
        public static async Task<IActionResult> Trigger(object data, string channelName, string eventName, IConfiguration pusherSection)
        {
            var options = new PusherOptions
            {
                Cluster = pusherSection["Pusher:CLUSTER"] ,
                Encrypted = true
            };
            var pusher = new Pusher(
              pusherSection["Pusher:APP_ID"],
              pusherSection["Pusher:APP_KEY"],
              pusherSection["Pusher:SECRET"],
              options
            );

            var result = await pusher.TriggerAsync(
              channelName,
              eventName,
              data
            );
            return new OkObjectResult(data);
        }
    }
}
