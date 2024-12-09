# From Zero to Hero: SOLID Principles for C# Developers - Course code
This is all the code for my course ["From Zero to Hero: SOLID Principles for C# Developers"](https://dometrain.com/course/from-zero-to-hero-solid-principles-for-csharp-developers/).

## Setting up the database
Navigate to the db folder using `cd db`

Spin up database using docker:
```bash
docker compose up --build -d
```

Note: if you are using an older version of docker compose you may need to use the command `docker-compose up --build -d`.

Run the database migration scripts. This will set up all the database tables and seed the database with some test data.
```bash
dotnet tool restore
dotnet dbup upgrade local.yml -e local.env --ensure
```

## Run the API
The example code is split into folders for each chapter of the course. The `final` folder contains the final code with all the modifications made throughout the course. The code requires [.NET 9 SDK (or newer)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) to run.

Each folder is self contained with it's own solution folder so you can open each solution individually.

To run the API navigate to the `Store.Api` folder inside each chapter folder and run `dotnet run`.

You can use the [Swagger UI](http://localhost:5010/swagger/index.html) to use the endpoints, there is also a Postman collection at the root of the project.

Use the `/auth` endpoint to get a token. You can use the following credentials:

- Admin User
  - Email: `admin.user@solid.com`
  - Password: `solidrocks`
- Normal User
  - Email: `normal.user@example.com`
  - Password: `solidrocks`