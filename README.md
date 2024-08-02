# Ani-Dex: the mobile digital encyclopedia
## Project So far
### Frontend
> The frontend was made on Unity AndroidSDK with C# language.
> 
> It is the face of all the operations of Ani-Dex offering the user the ability to:
> 1) Take pictures with there camera and receive info about the animal infront of them.
> 2) See previously scaned images.
> 3) And just search of the animal by name
>
> Connected through the backend over Http connection. Though currently it wouldn't be fit for android devices.
> For some reason it's not able to talk to the backend when on mobile. It's most likly that I didn't use the right code, but it could also be a permissions
> issue with android devices.
>
> For action 1 images are resized and sent over to the backend where they are processed and the results plus additional information is sent to the frontend of the program.
> For action 2 the server is uninvolved as the frontend just take from the local storage.
> For action 3 the client makes a request for the information by name. This is similar to acion 1 but it skips over the image rec part.


### Backend
> Consist of Ai models made from tensorflow, a Directory of infomation on each animal and A python code to run as the server.

> The frontend can send one of two things to the backend: an image or an animal name to search for.
>
> If the image is sent the server will convert the image to an image array and then hand it to the models to predit what animal it is.
> Then the server looks through the directories to find information on that animal and sends that infromation (as strings with headers) to the frontend.
>
> If an animal name is sent over then the server will skip the model prediction step. 

> The server is runs and connects to the frontend using Flask API. HTTP url allow the client to send request an the server replies.

### Extra's
> There are also the Juypter Notebooks used to make the model in this repositiory. And the links to the datasets will be included shortly.
>
> In the backend and frontend, there is a feature that never got implemented fully. It would allow the app the add more information about creatures by
> stating potential mixed breeds.
> But this could not be finish in time. Maybe later it can be finialised.
