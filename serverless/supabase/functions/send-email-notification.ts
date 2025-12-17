// Setup type definitions for built-in Supabase Runtime APIs
import "jsr:@supabase/functions-js/edge-runtime.d.ts";

interface reqPayload {
  emailAddress: string,
  venueName: string,
  datetime: Date
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

  const dateFormatted:string = new Date(reqPayload.datetime)
    .toLocaleString("en-GB", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    })
    .replace(",", "");

  // prepare the request body to resend api.
  const myBody = {
    from: "me25052 & me25067 <onboarding@resend.dev>",
    to: reqPayload.emailAddress,
    subject: "New Reservation",
    html: `<p><strong>Congratulations!</strong></p><p>You made a new reservation for ${reqPayload.venueName} at ${dateFormatted}! <br/><br/> Thanks,<br/>The me25052 & me25067 team`
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
    .then((response) => response.text().then(value => responseTxt = value))
    .then((result) => console.log(result))
    .catch((error) => console.error(error));

  // return response.
  return new Response(
    JSON.stringify(responseTxt),
    { headers: { 'Content-Type': 'application/json', 'Connection': 'keep-alive' }}
  );
});