# General API
This is a general API that I use for some of my personal coding projects. It's built using .NET 8 with an MVC (Mode-View-Controller) architecture.<br/> 

### Technologies
![.NET 8](https://img.shields.io/badge/.NET%208-275779) 

## Table of Contents
- [Description](#description)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Credits](#credits)

## Description
This API is built with .NET 8 MVC and is live on [Railway](https://railway.com). It connects to a managed Postgres SQL database hosted on [Supabase](https://supabase.com). 

## Getting Started
This project is a .NET Core MVC web API. To run it outside of Visual Studio, you'll need the **.NET 8 SDK** installed on your machine (the version that is compatible with this project - 8.0). 

1. **Get the code (cloning the repository)** - You'll first need to get the code from Github. Follow these steps from your command line interface (CLI), such as Command Prompt, Powershell or Bash:<br/>
`git clone https://github.com/evanmalherbe/GeneralAPI.git`
2. **Navigate to project directory** - Now use the `cd` command to move into the directory that contains the project's `.csproj` file.<br/>
`cd GeneralAPI`
3. **Restore dependencies (optional but recommended):** Run the following command to download any necessary packages and dependencies. This is often done automatically, but this makes sure everything is in place.<br/>
`dotnet restore`
4. **Run the API:** Execute the project using the `dotnet run` command. <br/>`dotnet run`
5. **Testing/Verification:** The server will start and make the endpoints available on a specific local port (e.g. at `http://localhost:5001` - check the console message for the exact URL). **You do not need to open a browser window for the API.** To test the API directly, use a tool like [Postman](https://www.postman.com) or [Insomnia](https://insomnia.rest) to hit the local endpoints (e.g. `http://localhost:5001/api/home/framework` - a GET endpoint).

## Usage
As mentioned above, you can test the API endpoints using a tool like [Postman](https://www.postman.com) to try hitting some of the endpoints such as:<br />
*- replace the base url below with the specific URL that your machine chooses to run the local API on as seen in your console/CLI*
- `http://localhost:5001/api/home/framework` - **GET** - frameworks I've used before
- `http://localhost:5001/api/home/education` - **GET** - my education history data
`http://localhost:5001/api/home/projects` - **GET** - coding projects I've recently worked on

## Credits
This project was created by Evan Malherbe - October 2025 - [GitHub profile](https://github.com/evanmalherbe)
