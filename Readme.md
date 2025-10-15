# Project description

This project is a meal calorie counter where users can CRUD meals and track calories while having a expected calories limit.
Also User managers can CRUD users and Admins can CRUD meals and users.

## Installation

To set up the project please do the following:

-   Clone the repository `git clone https://github.com/StefDjuric/Calories-Project.git`
-   Open solution in Visual Studio
-   Right click on Calories.API project and set it as startup project
-   Run `dotnet ef database update --project Calories.Entities --startup-project Calories.API` in terminal to setup the db
-   Go to Calories.Application/client with `cd Calories.Application/client` and run `npm install`

## Usage

To run the project you just have to run the Calories.API project in Visual Studio and the whole project will run, first the backend then after a couple of seconds the frontend.
When running the project will seed every role in database for testing with this data:

-   username: user, password: Pa$$w0rd,
-   username: usermanager, password: Pa$$w0rd,
-   username: admin. password: Pa$$w0rd
