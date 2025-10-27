# General API
This is a general API that I use for some of my personal coding projects. It's built using .NET 8 with an MVC (Mode-View-Controller) architecture.<br/> 

### Technologies
![.NET 8](https://img.shields.io/badge/.NET%208-275779) 

## Table of Contents
- [Description](#description)
- [Usage](#usage)
- [Credits](#credits)

## Description
This API is built with .NET 8 MVC and is live on [Railway](https://railway.com). It connects to a managed Postgres SQL database hosted on [Supabase](https://supabase.com). 

## Usage
This project is a .NET Core MVC web API. You could run it locally from the code on my github repository, but it won't do you much good without the connection string to connect to my Postgres database. As an alternative that will still allow you to review the functionality of this API, you can use a tool like [Postman](https://www.postman.com) or [Insomnia](https://insomnia.rest) to hit some of the endpoints on the live API that is at [https://generalapi-production-b0ac.up.railway.app](https://generalapi-production-b0ac.up.railway.app). <br/><br/> For example, the following endpoints can be accessed with a **GET** request:
- `https://generalapi-production-b0ac.up.railway.app/api/home/framework` - Tech Frameworks I've used before
- `https://generalapi-production-b0ac.up.railway.app/api/home/education` - My educational history data
`https://generalapi-production-b0ac.up.railway.app/api/home/projects` - Coding projects I've recently worked on

## Credits
This project was created by Evan Malherbe - October 2025 - [GitHub profile](https://github.com/evanmalherbe)
