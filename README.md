# Depth Charts Microservice
This microservice allows users to add and manage team weekly depth charts, supporting multiple depth charts for multiple teams. Currently, it only supports NFL, but it can be easily extended to other sports.

## Technologies Used
.NET 8
SQLite Database
Minimum API

## Software Patterns Used
* Clean Architecture
* CQRS with Mediator
* Factory Pattern

## Assumptions
* A player plays only one sport.
* Players' ranks start from 1, not a 0-based index, as it is easier to understand for non-technical stakeholders.
* Depth charts are created for each week. If the week is not provided during chart creation, the application will assume it is for the current week and generate the week number accordingly.
* Week number calculations are specific to each sport.

## Service Explanations
* To extend this for another sport, configure the creation logic in DepthChartServiceFactory and implement the service class from IDepthChartService.
* The GlobalExceptionHandler is used to handle all application errors, preventing the need to wrap try-catch blocks in most places.
* The ProblemDetails class is used to convert HTTP responses to human-readable responses.
* The Mediator pipeline is used to handle Command/Query validation.
* The SQLite database is located in the APP_Data folder in the API project.

## Endpoint Details
* /Player: Create a player for a sport.
* /Sport: Create a sport.
* /Team: Create a team for a sport.
*/NFL/CreateDepthChart: Create a depth chart for a given team and week.
* /NFL/AddPlayerToDepthChart: Add a player to the given chart.
* /NFL/RemovePlayerFromDepthChart: Remove a player from a given chart.
* /NFL/GetBackups: Get all the backup players for a given player of a chart.
* /NFL/GetFullDepthChart: Get all the player ranks for the given chart.

## How to Run
* Clone the repository: https://github.com/asprna/FanDuel.DepthChart.git.
* Restore the NuGet packages.
* Use the attached SQLite database. If it's not available, create a new database using the command Update-Database from the Package Manager Console under the API project.
* Start the FanDuel.DepthChart.MinApi API. The Swagger page contains all the above endpoints, allowing you to test the application.
