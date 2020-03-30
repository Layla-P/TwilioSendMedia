using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace TwilioSendMedia.Controllers
{
    [ApiController]
    [Route("/")]
    
    public class WebhookController : ControllerBase
    {
        private string _twilioNumber = "whatsapp:+14155238886";
        private string _mediaUrl = "https://twilio-cms-prod.s3.amazonaws.com/images/01.CongressBot.FileNewProj.width-800.png";
        
        //if sending from WhatsApp the phone numbers must be prefixed with "whatsapp:"
        
        
        [Route("/hello")]
        [HttpGet]
        public IActionResult Hello()
        {
            return Content("Hey there");
        }
        
        [Route("/Option1")]
        [HttpPost]
        public async Task<IActionResult> Option1()
        {
            var twiml = $@"<Response>
                               <Message>
                                   <Media>{_mediaUrl}</Media>
                               </Message>
                           </Response>";
            
            return Content(twiml, "text/xml");
        }
        
        [Route("/Option2")]
        [HttpPost]
        public async Task<IActionResult> Option2([FromBody]MessageResource inboundMessage)
        {
            TwilioClient.Init(
                Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"),
                Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN")
            );
            
            var outboundMessage = await MessageResource.CreateAsync(
                from: new PhoneNumber(_twilioNumber),
                to: inboundMessage.From,
                body: "Ahoy from Twilio!",
                mediaUrl: new List<Uri>{new Uri(_mediaUrl)}
            );
            
            return Content("<Response></Response>", "text/xml");
        }

        
    }
}
