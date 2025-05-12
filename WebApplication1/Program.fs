namespace WebApplication1

#nowarn "20"

open System
open System.IO
open System.Threading.Tasks
open System.Reflection
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open FSharp.Json

type UserJson =
    { id: int; name: string; email: string }

type ProductJson =
    { sku: string
      title: string
      price: int }

[<JsonUnion(Mode = UnionMode.CaseKeyAsFieldValue, CaseKeyField = "kind", CaseValueField = "data")>]
type RequestJson =
    | [<JsonUnionCase(Case = "user")>] User of UserJson
    | [<JsonUnionCase(Case = "product")>] Product of ProductJson

    static member BindAsync(ctx: HttpContext, _param: ParameterInfo) : ValueTask<RequestJson> =
        task {
            use reader = new StreamReader(ctx.Request.Body)
            let! body = reader.ReadToEndAsync()

            try
                let req = Json.deserialize<RequestJson> body
                return req
            with ex ->
                return raise (BadHttpRequestException("Invalid request body", ex))
        }
        |> ValueTask<RequestJson>

module Program =
    let builder = WebApplication.CreateBuilder()
    let app = builder.Build()

    app.MapPost(
        "/api/request",
        Func<RequestJson, IResult>(fun req ->
            match req with
            | User user -> Results.Ok($"User: {user.id}, {user.name}, {user.email}")
            | Product product -> Results.Ok($"Product: {product.sku}, {product.title}, {product.price}"))
    )
    |> ignore

    app.Run()

    0
