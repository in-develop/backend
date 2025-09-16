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

### **GET** `/api/auth/logout`
- **Authentication:** Requires JWT Bearer token.
#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **POST** `/api/auth/resend-email-confirmation`

#### Request Body
```json
{
  "email": "string",
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

---

## Products

### **GET** `/api/products`
#### Response Body
```json
{
  "data": [
    {
      "id": 1,
      "name": "Prod 1",
      "description": "Description for product 1",
      "price": 10.99,
      "discountPrice": 0,
      "stockQuantity": 100,
      "categoryId": 1,
      "productBundleId": 1
    },
  ],
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **POST** `/api/products`
- **Authorization:** Requires JWT Bearer token with **admin** role.
#### Request Body
```json
{
  "name": "string",
  "description": "string",
  "stockQuantity": 0,
  "price": 0,
  "discountPrice": 0,
  "categoryId": 0
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
  "data": 
    {
      "id": 1,
      "name": "Prod 1",
      "description": "Description for product 1",
      "price": 10.99,
      "discountPrice": 0,
      "stockQuantity": 100,
      "categoryId": 1,
      "productBundleId": 1
    },
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **PUT** `/api/products/{id}`

Example: `/api/products/1`
- **Authorization:** Requires JWT Bearer token with **admin** role.
#### Request Body
```json
{
  "name": "string",
  "description": "string",
  "stockQuantity": 0,
  "price": 0,
  "discountPrice": 0,
  "categoryId": 0
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
- **Authorization:** Requires JWT Bearer token with **admin** role.
#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

---

## Reviews

### **GET** `/api/reviews`
- **Authorization:** Requires JWT Bearer token.
#### Response Body
```json
{
  "data": [
    {
      "userId": "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
      "user": null,
      "productId": 1,
      "product": null,
      "rating": 5,
      "comment": "Great product!",
      "id": 1
    }
  ],
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **POST** `/api/reviews`

- **Authorization:** Requires JWT Bearer token with **admin** role.
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

- **Authorization:** Requires JWT Bearer token.
#### Response Body
```json
{
  "data": {
      "userId": "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
      "user": null,
      "productId": 1,
      "product": null,
      "rating": 5,
      "comment": "Great product!",
      "id": 1
  },
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **PUT** `/api/reviews/{id}`

Example: `/api/reviews/5`
- **Authorization:** Requires JWT Bearer token with **admin** role.
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
- **Authorization:** Requires JWT Bearer token with **admin** role.
#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```
---
## Cart

### **GET** `/api/cart`
- **Description:** Get user cart with all cart items.
- **Authorization:** Requires JWT Bearer token.
#### Response Body
```json
{
    "data": [
        {
            "id": 1,
            "cartId": 1,
            "productId": 1,
            "productName": "Prod 1",
            "price": 10.99,
            "quantity": 2
        }
    ],
    "statusCode": 200,
    "isSuccess": true,
    "message": ""
}
```

---
## Cart Items

### **POST** `/api/cart-items/create`

- **Authorization:** Requires JWT Bearer token.
#### Request Body
```json
{
  "productId": 0,
  "quantity": 0
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

### **PUT** `/api/cart-items/{id}`

Example: `/api/cart-items/1`
- **Authorization:** Requires JWT Bearer token.
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

### **DELETE** `/api/cart-items/{id}`

Example: `/api/cart-items/1`
- **Authorization:** Requires JWT Bearer token.
#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```
---
## Product Bundles

### **GET** `/api/bundles`
#### Response Body
```json
{
  "data": [
    {
      "id": 1,
      "name": "Prod bundle 1",
      "description": "Description for product bundle 1",
      "price": 90.99,
      "discountPrice": 0,
      "stockQuantity": 10,
      "products": [
        {
          "id": 1,
          "name": "Prod 1",
          "description": "Description for product 1",
          "price": 10.99,
          "discountPrice": 0,
          "stockQuantity": 100,
          "categoryId": 1,
          "productBundleId": 1
        }
      ]
    }
  ],
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **POST** `/api/bundles`

- **Authorization:** Requires JWT Bearer token with **admin** role.
#### Request Body
```json
{
  "name": "string",
  "description": "string",
  "stockQuantity": 0,
  "price": 0,
  "discountPrice": 0,
  "productIds": [
    0,
    0
  ]
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

### **GET** `/api/bundles/{id}`

Example: `/api/reviews/5`

- **Authorization:** Requires JWT Bearer token.
#### Response Body
```json
{
  "data": {
      "id": 1,
      "name": "Prod bundle 1",
      "description": "Description for product bundle 1",
      "price": 90.99,
      "discountPrice": 0,
      "stockQuantity": 10,
      "products": [
        {
          "id": 1,
          "name": "Prod 1",
          "description": "Description for product 1",
          "price": 10.99,
          "discountPrice": 0,
          "stockQuantity": 100,
          "categoryId": 1,
          "productBundleId": 1
        }
      ]
    }
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```

### **PUT** `/api/bundles/{id}`

Example: `/api/reviews/5`
- **Authorization:** Requires JWT Bearer token with **admin** role.
#### Request Body
```json
{
  "name": "string",
  "description": "string",
  "stockQuantity": 0,
  "price": 0,
  "discountPrice": 0,
  "productIds": [
    0,
    0
  ]
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

### **DELETE** `/api/bundles/{id}`

Example: `/api/reviews/5`
- **Authorization:** Requires JWT Bearer token with **admin** role.
#### Response Body
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "message": ""
}
```
