## Use Cases Project

In Clean Architecture, the Use Cases (or Application Services) project is a relatively thin layer that wraps the domain model.

Use Cases are typically organized by feature. These may be simple CRUD operations or much more complex activities.

Use Cases should not depend directly on infrastructure concerns, making them simple to unit test in most cases.

Use Cases are often grouped into Commands and Queries, following CQRS.

Queries fetch data but do not mutate state. Commands mutate state but do not return data (other than perhaps an acknowledgment or ID).

Having Use Cases as a separate project can reduce the amount of logic in UI and Infrastructure projects.

Using Use Case handlers combined with behavior can consolidate cross-cutting concerns like logging, validation, and error handling.
See: https://www.youtube.com/watch?v=vagyJdrWLr0

Need help? Check out the sample here:
https://github.com/ardalis/CleanArchitecture/tree/main/sample

Still need help?
Contact us at https://nimblepros.com
