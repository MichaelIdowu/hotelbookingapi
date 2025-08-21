# DemoApps-HotelBookingApi

Backend Developer: Michael Idowu (15/08/2025)

HotelBookingApi Project
Attached is a clean .vscode/launch.json with profiles in the following order: consumer, reviewer, dev, and docker. 

It assumes that the project lives in HotelBookingApi/ and uses port 8080/5031.
The HotelBookingApi project is a RESTful API designed to manage hotel bookings, reviews, and consumer interactions. It provides endpoints for creating, retrieving, updating, and deleting bookings and reviews.
The API is built using ASP.NET Core and follows best practices for RESTful design. It includes features such as authentication, authorization, and data validation.
The API is documented using Swagger, which provides an interactive interface for exploring the available endpoints and their parameters.
The API is hosted on GitHub and can be accessed at the following URL:
https://urban-potato-pjq6495qx6jcxwj-5031.app.github.dev/swagger/index.html

The API is designed to be used by both consumers and reviewers, with separate endpoints for each role. 
Consumers can create bookings, view their bookings, and leave reviews. 
Reviewers can manage reviews and view consumer bookings.
The API is built with scalability and performance in mind, using asynchronous programming patterns and efficient data access techniques.
The project includes unit tests to ensure the functionality of the API endpoints and business logic.
The API is designed to be easily extensible, allowing for future enhancements and additional features.
The project follows a clean architecture pattern, separating concerns into different layers such as presentation, application, domain, and infrastructure. 
This promotes maintainability and testability of the codebase.
The API is configured to run on port 8080, and it can be run locally using the provided launch configurations in the .vscode/launch.json file.
The API uses Entity Framework Core for data access, with a SQLite database as the backend.
The project includes a Dockerfile for containerization, allowing the API to be easily deployed in a Docker environment.
The API supports JSON as the primary data format for requests and responses, making it easy to integrate with various clients and platforms.
The API is designed to handle errors gracefully, returning appropriate HTTP status codes and error messages for invalid requests or server errors.

The API includes comprehensive logging to track requests, responses, and errors, aiding in debugging and monitoring the application.
The project follows coding standards and best practices, ensuring code quality and maintainability. 

The API is designed to be compatible with various client applications, including web, mobile, and desktop applications.

The documentation files for this project are placed in the HotelBookingApi/docs/ directory.

Thank you for your interest in this project.