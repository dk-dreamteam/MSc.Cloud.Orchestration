\echo 'Running 7_insert_sample_data.sql'

SELECT "Events".create_event(
  'Casino Royale in Concert',
  'Celebrating its 20th anniversary, Casino Royale is back on the big screen, accompanied by a full symphony orchestra.

Experience David Arnold''s thrilling score performed live by the Royal Philharmonic Concert Orchestra conducted by Anthony Gabriele.

Directed by Martin Campbell, Casino Royale is the 21st Bond film in the series, and the first starring Daniel Craig, with an international cast that includes Eva Green, Mads Mikkelsen, Jeffrey Wright and Dame Judi Dench.',
  '2026-12-30 13:00:00Z',
  'https://d117kfg112vbe4.cloudfront.net/public/Royal-Albert-Hall-DAMS/Promoter-Imagery/2026/12-December/Casino-Royale-in-Concert/39939_csnor_stl_3_h-v3.jpg?type=image&id=6269&token=6d3688e5&mode=fill&width=1410&height=744&format=webp'
);

SELECT "Events".create_event(
  'Interstellar Live',
  'Interstellar Live sees Christopher Nolan’s Oscar-winning sci-fi epic back on the big screen, with Hans Zimmer’s revelatory score performed live by the Royal Philharmonic Concert Orchestra, conducted by Ben Palmer, and acclaimed organist Roger Sayer.',
  '2026-04-05T17:00:00Z',
  'https://d117kfg112vbe4.cloudfront.net/public/Royal-Albert-Hall-DAMS/Promoter-Imagery/2026/4-April/Interstellar-Live/lead-image.jpg?type=image&id=5637&token=9c81c49b&mode=fill&top=818&width=1410&height=744&format=webp'
);


SELECT "Reservations".create_reservation(
  (
    SELECT "Id"
    FROM "Events"."Event"
    WHERE "Name" = 'Casino Royale in Concert'
    LIMIT 1
  ),
  'Kostantinos Dimitriou',
  3
);