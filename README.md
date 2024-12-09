# From Zero to Hero: SOLID Principles for C# Developers Example code
Example code for the course "From Zero to Hero: SOLID Principles for C# Developers".

## Setting up the database
Navigate to the db folder using `cd db`

Spin up database using docker:
```bash
docker compose up --build -d
```

Run database migration scripts.
```bash
dotnet tool restore
dotnet dbup upgrade local.yml -e local.env --ensure
```

## Run the API
The example code is split into folders for each chapter of the course. The final folder contains the final code with all the modifications made throughout the course. Each folder is self contained with it's own solution folder so you can open each solution individually.
Navigate to the `Store.Api` folder inside each chapter folder and run `dotnet run`.

You can use the [Swagger UI](http://localhost:5010/swagger/index.html) to use the endpoints, there is also a Postman collection at the root of the project.

Use the `/auth` endpoint to get a token. You can use the following credentials:

- Admin User
  - Email: `admin.user@solid.com`
  - Password: `solidrocks`
- Normal User
  - Email: `normal.user@example.com`
  - Password: `solidrocks`