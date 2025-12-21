// Setup type definitions for built-in Supabase Runtime APIs
import "jsr:@supabase/functions-js/edge-runtime.d.ts";

interface reqPayload {
  emailAddress: string,
  venueName: string,
  dateTime: Date,
  gifCode: string
};

console.info('server started');

Deno.serve(async (req: Request) => {

  // we added this to support healthchecks towards this function.
  if (req.method === "HEAD") {
    console.info('Healthcheck requested. Healthy.');
    return new Response(null, { status: 200 });
  }

  // get request payload.
  const reqPayload = await req.json();

  // add api key.
  const myHeaders = new Headers();
  myHeaders.append("Authorization", `Bearer ${Deno.env.get("RESEND_API_KEY")}`);

  const dateFormatted:string = new Date(reqPayload.dateTime)
    .toLocaleString("en-GB", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
    })
    .replace(",", "");

  // prepare the request body to resend api.
  const myBody = {
    from: "me25052 me25067 Reservations <msc.cloud.orchestration.reservations.no-reply@me25052-me25067.ink>",
    to: reqPayload.emailAddress,
    subject: "New Reservation",
    html: `<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Reservation Confirmation</title></head><body style='margin:0; padding:0; background-color:#f4f4f4; font-family:Verdana, Geneva, sans-serif; color:#333;'><table width='100%' cellpadding='0' cellspacing='0' role='presentation'><tr><td align='center' style='padding:40px 10px;'><table width='100%' cellpadding='0' cellspacing='0' role='presentation' style='max-width:600px; background-color:#ffffff; border-radius:12px; overflow:hidden; box-shadow:0 4px 12px rgba(0,0,0,0.1);'><tr><td style='background-color:#222; color:#ffffff; text-align:center; padding:40px;'><h1 style='margin:0; font-size:28px; font-weight:600;'>✨ Reservation Confirmed</h1></td></tr><tr><td style='padding:30px; font-size:16px; line-height:1.6; text-align:center; color:#333;'><p style='margin:0 0 20px; font-size:20px; font-weight:600;'>Congratulations! 🎉</p><table width='100%' cellpadding='0' cellspacing='0' role='presentation' style='margin:0 auto 24px; text-align:center;'><tr><td style='padding:6px 0;'><strong>Venue:</strong> ${reqPayload.venueName}</td></tr><tr><td style='padding:6px 0;'><strong>Starts At:</strong> ${dateFormatted}</td></tr></table><p style='margin:0 0 24px;'>We look forward to hosting you! 🥂 <br>Here is the countdown until your reservation:</p><center><div style='height:88px; overflow:hidden;'><img src='https://i.countdownmail.com/${reqPayload.gifCode}.gif' alt='Countdown'></div></center></td></tr><tr><td style='background-color:#f0f0f0; color:#666666; text-align:center; padding:20px; font-size:12px;'>Thank you, the <strong>me25052 & me25067</strong> team</td></tr></table></td></tr></table></body></html>`
  }

  const bulkerReq = new Request(
    'https://api.resend.com/emails', {
      method: "POST",
      headers: myHeaders,
      body: JSON.stringify(myBody)
    });

  // send request to bulker.
  let responseTxt;
  fetch(bulkerReq)
    .then((response) => responseTxt = response)
    .then((result) => console.log(result))
    .catch((error) => console.error(error));

  console.info(responseTxt);

  // return response.
  return new Response(
    JSON.stringify(responseTxt),
    { headers: { 'Content-Type': 'application/json', 'Connection': 'keep-alive' }}
  );
});