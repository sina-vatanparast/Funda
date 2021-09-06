# Funda
Funda Technical Assignment 2021

### Welcome To My Solution

#### Notes:
- Page size = 25. However, the optimal number of the page size of each request in the real world can be gained by monitoring different requests in diffrent sizes.
- In the real world applications, different modules (Service & Http Consumer) should be present in different services and we can benefit from async calls and message brokers like RabbitMq.
- Config variables are defined as constants in this project, while in the real world, especially in Microservices Architecture, configuration information is moved out of the microservice and externalized through a configuration management tool outside of the code. The same deployment can propagate across environments with the correct configuration applied.
