Book Management API — RESTful web API developed using ASP.NET Core for managing books. 

Technical Requirements:
1. Programming language: C#
2. Framework: .NET 8 or .NET 9
3. Database: SQL Server (with EF Core or Dapper) or MongoDB (MongoDB C# Driver)
4. 3-layered architecture: models, data access, API (ASP.NET Core Web API)
5. Design API according to REST: provide correct URIs for endpoints, use proper HTTP status codes for responses
6. Use Swagger for API documentation
7. Optional: implement JWT-based authentication to secure all endpoints, so that only authorized users can access them

Functional requirements:
1. Books model:
  - Title
  - Publication year
  - Author name
  - Views count (number of retrieving book details)
2. The API should allow CRUD operations:
  - Adding (single and bulk)
  - Updating (single)
  - Soft deleting (single and bulk) books
  - Retrieving a list of books (only titles) from the most popular to less popular with pagination
  - Retrieving details of a book (all book data)
3. Validation:
  - A book cannot be added if the book with the same name exists
  - Optional: implement other necessary validations as well, so that incorrect data cannot be submitted
4. Optional: When retrieving book details, calculate its popularity score on the fly (don’t save in database), taking into account following points:
  - Number of times the book was accessed (BookViews) - tracked in the database - how many times an endpoint “Get book details” is accessed
  - Age of the book (YearsSincePublished) - the older books the smaller bonus
  - Formula example (can be modified): Popularity Score = BookViews * 0.5 + YearsSincePublished * 2
