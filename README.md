# Ani-Dex: the mobile digital encyclopedia
## Project So far
### Frontend

### Backend
> Consist of Ai models made from tensorflow, a Directory of infomation on each animal and A python code to run as the server.

> The frontend can send one of two things to the backend: an image or an animal name to search for.
>
> If the image is sent the server will convert the image to an image array and then hand it to the models to predit what animal it is.
> Then the server looks through the directories to find information on that animal and sends that infromation (as strings with headers) to the frontend.

> Currently there isn't anything in place to for how the server and front end communicate.
>
> Also sending an image maybe too large to send as a single message so the image maybe compressed if there is a reliable way to decompress it. Though if send the image ins't taxing, then just send it.
> The image could also be sent in multiple messages.

> The models are being refined and will be uplaoded later.
