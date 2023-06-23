## LoanApi


##### About 
Project is written in Microsofts latest technologies for 17/05/2021.


[About](#About)  
[Installation](#Installation)  
[Testing](#Testing) 


## About
- NET 5
- MSSQL Server
- Nswag

## Installation
- Clone github repository, or download and unzip it.
- Create migrations
- Create User
```json
{
  "firstName": "string",
  "lastName": "string",
  "username": "string",
  "password": "string",
  "dateOfBirth": "2021-05-17T12:23:54.303Z",
  "personalNumber": "string"
}
```
- Login 
```json
{
  "username": "string",
  "password": "string"
}
```
- Use Generated Token for working with data
## Testing

Basic Flow

First of all you need to login. Get generated token. 
Token format is "Bearer accessToken". Example : "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI..." 
To Acces data you need to pass generated token in Authorization header.
