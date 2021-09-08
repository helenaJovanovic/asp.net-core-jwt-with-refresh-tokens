# asp.net-core-jwt-with-refresh-tokens

Wep API for saving cooking recipes.

Users are authenticated using JWT tokens and only have authorization to view, save and change their own recipes.

For now it has an ability to register a new user and to provide login. 

</br>

## Must be logged in:

#### GET [url]/api/recipe
- get recipes the current user has created. Must be logged in

#### GET [url]/api/recipe/{id}
- get recipe that the current user created by id

#### POST [url]/api/recipe
- make a new recipe for the current user

Add text field in the  body of the request (json):
{
  "text": "The text of the recipe"
}

### TODO: 
- Add refresh tokens and logout
- Manage ingredients in the controller
- Chane existing recipe
