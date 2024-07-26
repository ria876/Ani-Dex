# Ani-Dex: the mobile digital encyclopedia
## Project So far
### Frontend
> Thre fontend is the image of the program, meant to run on mobile devices.

> The user has three main ways of using the app.
> 1) Taking a picture ("scanning") an animal to find out what it is.
> 2) Searching for a desired animal in the search bar.
> 3) Viewing previously scanned animals
>
> Additionaly the user may want to submit images and other feedback or updates about the animals in the app.

> 

### Backend
> Consist of Ai models made from tensorflow, a Directory of infomation on each animal and A python code to run as the server.

> The frontend can send one of two things to the backend: an image or an animal name to search for.
>
> If the image is sent the server will convert the image to an image array and then hand it to the models to predit what animal it is.
> Then the server looks through the directories to find information on that animal and sends that infromation (as strings with headers) to the frontend.
>
> If an animal name is sent over then the server will skip the model prediction step. 

> Currently there isn't anything in place to for how the server and front end communicate.
>
> Also sending an image maybe too large to send as a single message so the image maybe compressed if there is a reliable way to decompress it. Though if send the image ins't taxing, then just send it.
> The image could also be sent in multiple messages.

> The models are being refined and will be uplaoded later.
