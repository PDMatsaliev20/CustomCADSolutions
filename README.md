# CustomCADs: The Land of 3D Models.
Here, you can enjoy our insightful home page and browse our inclusive gallery of 3D Models. 

If you're interested, you could participate too - you have a few options:
- *Registering as a Client* would allow you to make and track orders for a Custom 3D Model from our team of 3D Designers, or straight up buy models from the previously mentioned gallery. 
- *Registering as a Contributor* on the other hand gives you the opportunity to share your 3D Designs to the world by uploading them to our gallery, and you could show a price for which the model would be sold in the gallery - you'd reap a part of the benefit!
- If you're a Client and your order seems to be handled too slowly or if you're a Contributor and don't see your 3D Model in our gallery, please keep in mind that our team of Designers are constantly working on answering orders and validating your 3D models so they show up in the gallery. If you're up to the task, you could contact us and *apply to become a ***paid member*** of our team of Designers*. 
- If you're more into the web development side of this and you have an interest in one of these:
**React.js with TailwindCSS**
**ASP.NET Core Web API**
**Entity Framework Core with SQL Server**
then you're welcome to *apply for a ***paid position*** in our team of Developers* too.

## Project overview
This project:
- Deals with large and small files and data;
- Loads and renders 3D Models for the user to interact with;
- Supports purchases and delivery;
- Encourages communication between the demanding and the supplying parts of 3D Models;
- Includes a range of categories to describe your Order or 3D Model;
- Keeps you constantly updated on your Order/Model's status;

### Frontend architecture
The React project is built with Vite and so has a public, tests and src folders. 
Inside the src folder there's:
- `main.jsx` - The starting point of the application and the place to add libraries like fontawesome, tailwind and react router.
- `index.css` - Some basic global CSS to avoid excessive repetitiveness.
- `app.jsx` - The component that holds all other components, as well as the context's provider and other configurations.
- `assets` - My logo and other images.
- `layout` - Where I keep the header, navbar and footer UI components.
- `guest` - The guest pages and their components (there's /home, /login, /register, /info).
- `public` - The public pages and their components (there's /gallery, /about, /policy, /info).
- `private` - The private pages and their components (there's /orders, /cads and /designer).
- `hooks` - The place where my custom hooks reside.
- `routing` - The routes and loaders for all pages and the auth-guard.
- `requests` - Here I configure axios and predefine all the requests to my RESTful API, which I import and use in the other pages.
- `languages` - Basically the place of all the languages' resources my app supports (just _bg_ and _en_ for now).
- `contexts` - I keep the contexts I create with the Context API here.
- `constants/data` - Where I basically keep a copy of my backend data validation but as a JS object so I can access and use it in React too.
- `components` - The ones I reuse across the whole app, such as fields or the CAD component containing all my THREE.js logic.

### Backend architecture
For the backend I have a few other .NET projects, including:
- `CustomCADs.API` - the ASP.NET Core Web API where I keep my controllers, DTOs, AutoMapper profiles for Service Models/DTO conversion, wwwroot with all the 3D Models and Images, and other helper classes here, as well as the configurations (like for the Identity framework and Stripe). I also have Spa Proxy configured so the React project runs whenever the API project compiles.
- `CustomCADs.Core` - the Application/Business Logic layer of the app, here reside the Service contracts (or interfaces, they get injected into the controllers) and their implementations, Service Models with all the validation logic and rules, AutoMapper profiles for Entity/Service Model conversion.
- `CustomCADs.Domain` - the place I keep the Data Constants and my Entity models with all the attributes necessary to configure Entity Framework Core, as well as the Repository's interface (which is later injected into the services).
- `CustomCADs.Infrastructure` - the project used for configuring External services (just Stripe for now) and the Database logic, such as DbContext, configurations and migrations. 
- `CustomCADs.Tests` - all the backend testing is done here (for now it's only unit tests but I should have integration and e2e tests covered by the end of the year).

## Setup

To run this project locally (best suited for **Visual Studio 2022**), you need:
- .NET 8
- Node.js v20.12.2 or up
- SQL Server 2022.

### Installation
You must only install the necessary dependencies for the frontend by:
1. `going to the terminal`
2. typing `cd customcads`
3. then `npm i`

### Configurations
There's also sample configurations for the backend in the `appsetings.json`, like username and password for default account for each of the roles and stripe public and secret keys, but most importantly **you must pass your connection string** here, preferably in the format suggested. Alternatively, you could avoid modifying the appsetings.json and make use of Visual Studio's `user-secrets.json` file (right-click on the API project and look for Manage User Secrets) - this file doesn't get noticed by git and it always stays the same no matter where you move the project or how many times you delete and reclone it.

### Running the app
To view the app, just start the API `(f5 or ctrl+f5)` - I have **Spa Proxy** configured, so the React project runs whenever the API compiles.


## Use cases and Role accessibilities
To unlock the **full potential** of the app, use the accounts I mentioned in the above section to log in and check out each role's capabilities. I've provided sample usernames and passwords, but you can change them to whatever you wish. 
______________________________________________________________________________________________
| Role        | Access Rights                                                                 |
|-------------|-------------------------------------------------------------------------------|
| Client      | Access to Orders controller. Can buy from gallery and make and track orders.  |
|_____________________________________________________________________________________________|
| Contributor | Access to Cads controller. Can upload 3D models and set prices.               |
|_____________________________________________________________________________________________|
| Designer    | Same as Contributor, but doesn't need validation for uploaded 3D models.      |
|             | Access to Designer controller. Validates 3D Models and answers Orders.        |
|_____________|_______________________________________________________________________________|
| Admin       | Access to all controllers in the Controllers/Admin folder. Can do anything    |
|_____________|_______________________________________________________________________________|
All roles have access to their own Home Page and the Gallery, as well as other pages like Privacy Policy and About us. 
If you want to view the Guest Home Page, Register or Login, you must, naturally, be logged out.

## Aditional info

### Unit Tests
If you want to check the **100 unit tests** I've designed in the CustomCADs.Test project, you can run them by using Visual Studio's Test Explorer (`ctrl+e, t`  or  `View -> Test Explorer`). /*I'll add here info about frontend tests, I don't know what since I haven't implemented them yet*/

### Known Issues 
You may see a few typescript compile bugs in the Error List tab, but they can be **safely ignored**. They're from a SignalR library I used in a project I no longer need but nevertheless keep in this .sln for certain reasons and I've tried to fix them - there **used to be 1500** of them, I've tried my best to limit them to 5.

### Future Improvements
I may have mentioned it above, but I'll expand my tests to **integration** and **e2e** once I get the chance. I'm planning to implement **CQRS** by configuring **Dapper** to take care of Queries (GET requests) and leaving Entity Framework Core to deal with Commands (POST, PUT, DELETE...). I'm also thinking about using a **OpenAI API key** to make calls to a mini ChatGPT that chats with Clients for a bit and makes their order for them instead of having to fill out a form, and it also provides the email of the best matched Designer. I'll also probably modify the **theme and design** of the whole site when I'm finished with the more important frontend/backend functionalities, the current one is just really comfortable to develop pages quickly and hopefully it's good enough to allow me to pass a certain exam :D.

### Backstory and Motivation
I first started developing this project for **the 2024 Bulgarian IT Olympiad**, where I gave up right before getting national place because I didn't find it worth to give the money needed to host it on Azure (I used up my free trial for the previous round).
The Olympiad I'm really interested in is this year's - I'll be **graduating** in less than a year, and if I **win a laureate** in this olympiad I'll have a place in the **university I'm striving for**. 
I also used this project for the project defense at the end of **SoftUni**'s ASP.NET Advanced course from the C# Web module, where I got **97/100** points. 
I'm developing it by myself, but I have a **partner** (also friend and classmate) who gave the idea to make a site for ordering 3D Models, and we're thinking of making this an **actual business** in the future, so I hope to **get as criticized** as possible in other professional events like the **React.js course project defense** I'll be presenting at this Sunday.

## Contacts
For questions, feedback or interests, feel free to can contact me at:
- **GMAIL**: ivanangelov414@gmail.com
- **PHONE** NUM: +359440400
- **INSTAGRAM**: @ivan_ivchos_angelov
