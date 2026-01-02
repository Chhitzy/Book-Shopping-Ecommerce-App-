# Book-Shopping-Ecommerce-App-
This project is a full-stack Book Shopping E-Commerce application developed using ASP.NET Core, designed to simulate real-world online book purchasing workflows. The application focuses on secure authentication, payment processing, and role-based access control, following practical backend development practices.

The project was built as part of hands-on learning after completing industrial training, with emphasis on understanding how production-level eCommerce systems are structured and managed.

Project Overview

The Book Shopping E-Commerce App allows users to browse books, manage their shopping cart, securely authenticate, and complete purchases online. The system supports user authentication via Google OAuth, payment processing through Stripe, and authentication/verification services using Twilio. Administrative features are protected using authorization and role management.

This project demonstrates how multiple third-party services can be integrated into an ASP.NET Core application while maintaining security and scalability.

Technologies Used

ASP.NET Core
ASP.NET Core MVC
Entity Framework Core
SQL Server
Stripe Payment Gateway
Google OAuth
Twilio API
HTML, CSS, JavaScript
Bootstrap

Key Features Include:

User registration and login using Google OAuth
Secure authentication and authorization
Role-based access control (Admin / User)
Online payment processing using Stripe
Phone/SMS verification and communication using Twilio
Book listing, browsing, and cart management
Order placement and payment confirmation
Secure handling of user and payment data

Authentication And Authorization
The application implements secure authentication using Google OAuth, allowing users to log in with their Google accounts. Authorization is handled using role-based access control to restrict administrative features such as managing books and orders.

Twilio is integrated to support additional authentication/verification mechanisms, improving overall application security. Email sending is also supported but limited to the local network during 2Factor Authentication.

Payment Integration

Stripe is used for handling online payments securely. The integration ensures:
Secure transaction processing
Payment validation
Order confirmation after successful payment
No sensitive payment information is stored directly in the application.

Application Architecture

The project follows a layered architecture, separating concerns between:
Controllers
Business logic
Data access layer
Models and views
This structure improves maintainability, scalability, and readability of the codebase.

Purpose of the Project

The primary goal of this project was to:
Gain hands-on experience with ASP.NET Core
Understand real-world eCommerce application workflows
Learn third-party service integration (Stripe, Google OAuth, Twilio)
Practice secure authentication and authorization
Build a project suitable for portfolio and recruiter evaluation

Status

The project is functional and actively used for learning and improvement. Enhancements and refactoring are ongoing as part of continuous skill development.
Made Using ASP.NET CORE.


