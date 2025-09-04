# TeamChallenge
# Web API Documentation

## Static Domain
[https://impala-still-pika.ngrok-free.app/](https://impala-still-pika.ngrok-free.app/)

## Swagger UI
[https://impala-still-pika.ngrok-free.app/swagger/index.html](https://impala-still-pika.ngrok-free.app/swagger/index.html)

---

## Authentication

### **POST** `/api/auth/signup`

#### Request Body
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "clientUrl": "string",
  "cartItems": [
    {
      "productId": 0,
      "quantity": 0
    }
  ]
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": "User registered successfully.Please check your email to confirm your account."
}
```

#### Notes
If the cart is empty, send `cartItems` as `null`:

```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "clientUrl": "string",
  "cartItems": null
}
```

### **POST** `/api/auth/login`

#### Request Body
```json
{
  "usernameOrEmail": "string",
  "password": "string",
  "rememberMe": true
}
```

#### Response Body
```json
{
  "statusCode": 401,
  "isSuccess": false,
  "message": "Confirm Email"
}
```

### **POST** `/api/auth/logout`

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

---

### **POST** `/api/auth/resend-email-confirmation`

#### Request Body
```json
{
  "email": "string",
  "clientUrl": "string"
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": "If an account exists for that email, a confirmation link has been resent."
}
```


## Products

### **GET** `/api/products`

#### Response Body
```json
{
  "data": [
    {
      "name": "Prod 1",
      "description": "Description for product 1",
      "price": 10.99,
      "stockQuantity": 100,
      "categoryId": 1,
      "category": null,
      "reviews": null,
      "id": 1
    }
  ],
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **POST** `/api/products`

#### Request Body
```json
{
  "name": "string",
  "description": "string",
  "price": 1,
  "categoryId": 1
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **GET** `/api/products/{id}`

Example: `/api/products/1`

#### Response Body
```json
{
  "data": {
    "name": "Prod 1",
    "description": "Description for product 1",
    "price": 10.99,
    "stockQuantity": 100,
    "categoryId": 1,
    "category": null,
    "reviews": null,
    "id": 1
  },
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

---

### **PUT** `/api/products/{id}`

Example: `/api/products/1`

#### Request Body
```json
{
  "name": "string",
  "description": "string",
  "price": 2,
  "categoryId": 1
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **DELETE** `/api/products/{id}`

Example: `/api/products/1`

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

## Reviews

### **GET** `/api/reviews`

#### Response Body
```json
{
  "data": [],
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

---

### **POST** `/api/reviews`

#### Request Body
```json
{
  "rating": 5,
  "comment": "string",
  "productId": 1,
  "userId": "string"
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **GET** `/api/reviews/{id}`

Example: `/api/reviews/5`

#### Response Body
```json
{
  "data": {
    "userId": "5e6f1df4-75a3-4cc6-914d-20dc56a30888",
    "user": null,
    "productId": 2,
    "product": null,
    "rating": 5,
    "comment": "string",
    "id": 5
  },
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

---

### **PUT** `/api/reviews/{id}`

Example: `/api/reviews/5`

#### Request Body
```json
{
  "rating": 5,
  "comment": "new comment",
  "productId": 2,
  "userId": "5e6f1df4-75a3-4cc6-914d-20dc56a30888"
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **DELETE** `/api/reviews/{id}`

Example: `/api/reviews/5`

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

## Cart Items

### **GET** `/api/cart-items/{id}`

Example: `/api/cart-items/1`

#### Response Body
```json
{
  "statusCode": 404,
  "isSuccess": false,
  "message": "Cart item not found ID : 1"
}
```

---

### **POST** `/api/cart-items/create`

#### Request Body
```json
{
  "productId": 1,
  "cartId": 1,
  "quantity": 1
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **PUT** `/api/cart-items/update`

#### Request Body
```json
{
  "quantity": 5
}
```

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

---

### **DELETE** `/api/cart-items/delete/{id}`

#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```
