# btc-node

## API

**/user/create**
Allows to create a new user by sending a User-type object, containing fields 'name' and 'password.

**/user/login**
Allows to get a valid token if a user exists. Requires a User-type object, containing fields 'name' and 'password.

**/btcRate**
Fetches a latest BTC/UAH rate. Requires a valid token to proceed.

## Application

- All settings are in appsettings.json.
- Users are stored in the Users.csv file. If this file doesn't exit, the app ensures it will be created after the first launch.
- Uses JWT token for authentication.

## What should've been added :(
- Logging
- Unit tests
- Salt and hashing for passwords
- Error handling in middleware
- ...