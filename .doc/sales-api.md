[Back to README](../README.md)

### Sales

#### GET /sales
- Description: Retrieve a paginated list of sales
- Query Parameters:
  - `_page` (optional): Page number (default: 1)
  - `_size` (optional): Items per page (default: 10)
- Response:
  ```json
  {
    "success": true,
    "message": "Sales retrieved successfully",
    "data": [
      {
        "id": "string (guid)",
        "date": "string (ISO 8601)",
        "customer": "string",
        "branch": "string",
        "items": [
          {
            "product": "string",
            "quantity": "number",
            "unitPrice": "number",
            "totalPrice": "number"
          }
        ],
        "totalAmount": "number",
        "isCancelled": "boolean"
      }
    ],
    "currentPage": "number",
    "totalPages": "number",
    "totalCount": "number"
  }
  ```

#### POST /sales
- Description: Create a new sale
- Request Body:
  ```json
  {
    "date": "string (ISO 8601)",
    "customer": "string",
    "branch": "string",
    "items": [
      { "product": "string", "quantity": 1, "unitPrice": 10.0 }
    ]
  }
  ```
- Response:
  ```json
  {
    "success": true,
    "message": "Sale created successfully",
    "data": {
      "id": "string (guid)"
    }
  }
  ```

#### GET /sales/{id}
- Description: Retrieve a sale by ID
- Path Parameters:
  - `id`: Sale ID (guid)
- Response:
  ```json
  {
    "success": true,
    "message": "Sale retrieved successfully",
    "data": {
      "id": "string (guid)",
      "date": "string (ISO 8601)",
      "customer": "string",
      "branch": "string",
      "items": [
        {
          "product": "string",
          "quantity": "number",
          "unitPrice": "number",
          "totalPrice": "number"
        }
      ],
      "totalAmount": "number",
      "isCancelled": "boolean"
    }
  }
  ```

#### PUT /sales/{id}
- Description: Update a sale
- Path Parameters:
  - `id`: Sale ID (guid)
- Request Body:
  ```json
  {
    "date": "string (ISO 8601)",
    "customer": "string",
    "branch": "string",
    "items": [
      { "product": "string", "quantity": 2, "unitPrice": 9.5 }
    ],
    "isCancelled": false
  }
  ```
- Response:
  ```json
  {
    "success": true,
    "message": "Sale updated successfully",
    "data": {
      "id": "string (guid)"
    }
  }
  ```

#### DELETE /sales/{id}
- Description: Delete a sale
- Path Parameters:
  - `id`: Sale ID (guid)
- Response:
  ```json
  {
    "success": true,
    "message": "Sale deleted successfully"
  }
  ```

<br/>
<div style="display: flex; justify-content: space-between;">
  <a href="./products-api.md">Previous: Products API</a>
  <a href="./users-api.md">Next: Users API</a>
</div>
