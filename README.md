# TwoBucket API

## Overview

The TwoBucket API is a .NET 8 application that solves the two bucket problem. 
The problem involves finding the steps to get a desired amount of water using two buckets with given capacities. 
The solution is cached to improve performance for repeated operations.

## Project Structure

- **Domain**: Contains domain models and contracts.
- **Service**: Contains service abstractions and implementations.
- **Application**: Contains the `TwoBucketController`, action filters and other presentation layer components as it holds the main API.

## How It Works

### Solve Algorithm

The `Solve` algorithm in the `IBucketChallengeService` interface is responsible for solving the two bucket problem. 
It takes three parameters: the capacities of the two buckets and the target volume. 
The algorithm returns a solution containing the steps to achieve the target volume.

#### Algorithm Explanation

The algorithm uses a breadth-first search (BFS) approach to explore all possible states of the two buckets until it finds a solution or determines that no solution is possible. 
Here is a detailed explanation of the algorithm:

1. **Initialization**: 
   - A `HashSet` is used to keep track of visited states to avoid processing the same state multiple times.
   - A `Queue` is used to manage the states to be processed, starting with the initial state where both buckets are empty.

2. **Processing States**:
   - The algorithm dequeues a state from the queue and checks if it meets the target volume in either bucket.
   - If the target volume is achieved, the solution steps are returned.
   - If the state has already been visited, it is skipped.

3. **Generating Next States**:
   - For each state, the algorithm generates all possible next states by performing actions such as filling, emptying, and transferring water between the buckets.
   - These next states are enqueued for further processing if they have not been visited.

4. **No Solution**:
   - If the queue is exhausted without finding a solution, an exception is thrown indicating that no solution is available.

### Caching

The solution is cached using `IMemoryCache` to improve performance for repeated operations. 
The cache key is generated based on the input parameters (bucket capacities and target volume). 
cached solution is returned if available; otherwise, the solution is computed and stored in the cache with a sliding expiration of 5 minutes.

### Validation Filter

The `ValidationFilterAttribute` is an action filter that validates the incoming request data. It ensures that the input parameters meet the required constraints (e.g., non-negative values). If the validation fails, a `422 Unprocessable Entity` response is returned with the validation errors.

### Architecture

The solution follows a layered architecture:

- **Presentation Layer**: Handles HTTP requests and responses. Contains controllers and action filters.
- **Service Layer**: Contains business logic and service abstractions.
- **Domain Layer**: Contains domain models and contracts.

## Accessing Swagger UI

The API documentation is available through Swagger UI. To access it, run the application and navigate to the following URL in your browser:

https://localhost:7221/swagger/index.html

## API Endpoints

### Solve Two Bucket Problem

- **Endpoint**: `POST /api/TwoBucket`
- **Description**: Solves the two bucket problem and returns the solution steps.
- **Content Type**: `application/json`
- **Request Body**:
```json
{
  "XCapacity": 1,
  "YCapacity": 3,
  "ZAmountWanted": 2
}
```
  
- **Responses**:
  - `200 OK`: Returns the solution steps.
  - `400 Bad Request`: Invalid input or no available solution.
  - `422 Unprocessable Entity`: Validation errors.

### Example Request Using `curl`

```sh
curl -X 'POST' \
  'https://localhost:7221/api/TwoBucket' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -H "Accept-Encoding: gzip, br" \
  -d '{
  "x_capacity": 1,
  "y_capacity": 3,
  "z_amount_wanted": 2
}'
```

#### Example Response

```json
{
  "solution": [
    {
      "stepCount": 1,
      "bucketX": 0,
      "bucketY": 3,
      "action": "Fill bucket Y",
      "status": null
    },
    {
      "stepCount": 2,
      "bucketX": 1,
      "bucketY": 2,
      "action": "Transfer from bucket Y to X",
      "status": "Solved"
    }
  ]
}
```

### OPTIONS Method

- **Endpoint**: `OPTIONS /api/TwoBucket`
- **Description**: Provides information about the communication options for the TwoBucket endpoint.
- **Responses**:
  - `200 OK`: Returns the allowed methods and supported headers.

### Example Request Using `curl`

```sh
curl -X OPTIONS https://localhost:7221/api/TwoBucket
```

## Running the Application

1. Clone the repository.
2. Navigate to the project directory.
3. Run the application using the following command:
  
```sh
dotnet run --project ChicksGold.Test.Application/ChicksGold.Test.Application.csproj
```

4. Access the Swagger UI at `https://localhost:7221/swagger/index.html`.

## Conclusion

The TwoBucket API provides a solution to the two bucket problem with caching and validation. 
The API is documented using Swagger, and the endpoints can be tested using tools like `curl` or the Swagger UI.
