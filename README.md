# asp.net-core-jwt-with-refresh-tokens

Recipes manager in .NET Core Web Api

Authorization and authentication practice project using ASP.NET Core Identity and JWT tokens. Only registered users can use the Web Api and they can create, edit and delete only their own entries.

</br>

## Authorization management:

### Register: [url]/api/AuthManagement/Login
- Body of the request
```
{
    "name": "hex@gmail.com",
    "email": "email@email.com",
    "password": "Password123!"
}
```

### Logging in: [url]/api/AuthManagement/Login
- Body of the request
```
{
    "email": "email@email.com",
    "password": "Password123!"
}
```

## Must be logged in:

#### GET [url]/api/recipe
- get recipes the current user has created. Must be logged in

#### GET [url]/api/recipe/{id}
- get recipe that the current user created by id

#### POST [url]/api/recipe
- make a new recipe for the current user

Add text field in the  body of the request (json):
```
{
  "text": "The text of the recipe"
}
```

Id is autogenerated

#### PUT [url]/api/recipe
- change existing recipe

Body of the request(json):
```
{
	"id": {intIdOfTheEntry}
	"text": "changed recipe text"
}
```
#### GET [url]/api/recipe/ingr/{id}
- Get ingridients that the recipe with id has, if the current user has created the recipe

#### POST [url]/api/recipe/ingr/{id}
- Create a new ingredient and form a relationship with a recipe that has primarykey of id
- Ingridient has to be provided in the body of the request in json form

Body of the request(json):
```
{
	"name": "The name of the ingridient"
}
```

#### PUT [url]/api/recipe/ingr/{RecipeId}/{IngrId}
- Add an existing ingrident to a recipe

#### DELETE [url]/api/recipe/ingr/{RecipeId}/{IngrId}
- Remove an ingridient from a recipe

### TODO: 
- Add refresh tokens and logout
- Manage ingredients in the controller
- Add delete functionality
