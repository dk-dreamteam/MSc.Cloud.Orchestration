// Setup type definitions for built-in Supabase Runtime APIs
import "jsr:@supabase/functions-js/edge-runtime.d.ts";

interface reqPayload {
  mobilePhone: number,
  venueName: string,
};

console.info('server started');

Deno.serve(async (req: Request) => {
  // get request.
  const reqPayload = await req.json();

  // prepare data and request to send to bulker api.
  const data = {
    countryCode: "30",
    baseUrl: "http://api.bulker.gr",
    apiKey: Deno.env.get("BULKER_API_KEY"),
    mockId: Date.now(),
    fromName: "Test SMS",
    templateText: `Thank for your reservation at ${reqPayload.venueName}`
  };

  const myHeaders = new Headers();
  myHeaders.append("Cookie", "ws_locale=el_GR");

  const bulkerReq = new Request(
    `${data.baseUrl}/http/sms.php?
    auth_key=${data.apiKey}&
    id=${data.mockId}&
    from=${data.fromName}&
    to=${data.countryCode}${reqPayload.mobilePhone}&
    text=${data.templateText}&
    coding=1`, {
      method: "GET",
      headers: myHeaders,
    });

fetch(bulkerReq)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.error(error));

  // return response.
  return new Response(
    JSON.stringify(data),
    { headers: { 'Content-Type': 'application/json', 'Connection': 'keep-alive' }}
  );
});