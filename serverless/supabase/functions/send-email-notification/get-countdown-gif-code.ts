// used to fetch the countdown code for the gif that will be added to the email.
export default async function getCountDownGifCode(timeEnd: Date): Promise<string>{
  // prepare headers.
  const myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");
  myHeaders.append("Authorization", Deno.env.get("COUNTDOWNMAIL_API_KEY"));

  // prepare body.
  const raw = JSON.stringify({
    "skin_id": 4,
    "time_end": formatDateTime(timeEnd),
    "time_zone": "Europe/Athens",
    "font_family": "Roboto-Bold",
    "color_primary": "080808",
    "color_text": "F7FAFA",
    "color_bg": "FFFFFF",
    "transparent": "1"
  });
  
  // make http request.
  const response = await fetch("https://countdownmail.com/api/create", {
      method: "POST",
      headers: myHeaders,
      body: raw,
    });

  // if response is successful return the correct code.
  if (response.ok){
    var gifResponse = await response.json();
    return gifResponse.message.code;
  }
  else{
    console.error(await response.text())
    return "null";
  }
}

// used to format date time to specific format that counddownmail needs.
function formatDateTime(date: Date): string {
  const pad = (n: number) => n.toString().padStart(2, '0');

  const year = date.getFullYear();
  const month = pad(date.getMonth() + 1);
  const day = pad(date.getDate());

  const hours = pad(date.getHours());
  const minutes = pad(date.getMinutes());
  const seconds = pad(date.getSeconds());

  return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}