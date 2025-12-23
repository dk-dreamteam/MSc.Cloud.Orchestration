// Setup type definitions for built-in Supabase Runtime APIs
import "jsr:@supabase/functions-js/edge-runtime.d.ts";
import getCountDownGifCode from './get-countdown-gif-code.ts';

console.info('server started');

Deno.serve(async (req: Request) => {
  // we added this to support healthchecks towards this function.
  if (req.method === "HEAD") {
    console.info('Healthcheck requested. Healthy.');
    return new Response(null, { status: 200 });
  }

  // get request payload.
  const reqPayload = await req.json();

  const gifCode = await getCountDownGifCode(new Date(reqPayload.dateTime));

  console.log(`the gif code is this ${gifCode}`);

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

  // read the email html template file and replace with our values.
  const template = await Deno.readTextFile('./template.html');
  const fomattedTemplate = template
    .replace('{{venueName}}', reqPayload.venueName)
    .replace('{{date}}', dateFormatted)
    .replace('{{gifCode}}', gifCode)

  // prepare the request body to resend api.
  const myBody = {
    from: "me25052 me25067 Reservations <msc.cloud.orchestration.reservations.no-reply@me25052-me25067.ink>",
    to: reqPayload.emailAddress,
    subject: "New Reservation",
    html: fomattedTemplate
  }

  const resendReq = new Request(
    'https://api.resend.com/emails', {
      method: "POST",
      headers: myHeaders,
      body: JSON.stringify(myBody)
    });


  // send request to resend api.
  const response = await fetch(resendReq);
  const responseTxt = await response.text();

  if (response.ok){
    console.info(responseTxt);
    console.info(`Email sent to: ${reqPayload.emailAddress}`);
    
    // return response.
    return new Response(
      responseTxt,
      { headers: { 'Content-Type': 'application/json', 'Connection': 'keep-alive' }}
    );
  }
  else{
    console.error(responseTxt);
  }

});