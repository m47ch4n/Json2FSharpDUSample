# Json2FSharpDU Sample

A minimal F# ASP.NET Core API sample using FSharp.Json to deserialize JSON into discriminated unions.

## How to run

```bash
dotnet run --project WebApplication1
```

The API will listen on `http://localhost:5235`.

## Example requests

### User

```bash
curl http://localhost:5235/api/request \
  --json '{
    "kind": "user",
    "data": {
      "id": 1,
      "name": "John Smith",
      "email": "john.smith@example.com"
    }
  }'
```

_Response_

```bash
"User: 1, John Smith, john.smith@example.com"
```

### Product

```bash
curl http://localhost:5235/api/request \
  --json '{
    "kind": "product",
    "data": {
      "sku": "ABC123",
      "title": "F# in Action",
      "price": 2999
    }
  }'
```

_Response_

```bash
"Product: ABC123, F# in Action, 2999"
```

### Invalid Request (returns 400)

_Unknown kind_

```bash
curl http://localhost:5235/api/request \
  --json '{
    "kind": "unknown",
    "data": {}
  }'
```

_Invalid user data_

```bash
 curl http://localhost:5235/api/request \
  -d '{
    "kind": "user",
    "data": {
      "sku": "ABC123",
      "title": "F# in Action",
      "price": 2999
    }
  }'
```
