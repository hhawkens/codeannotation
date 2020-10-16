module internal CodeAnnotation.Tests.StringFuzz

open System

[<Literal>]
let private Chars =
    "     ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!§$%&/()=?'#,;.:-_{[]}ß~µ\n\n\n\n\n"

let internal generateRandomString length seed =
    let rand = Random(seed)
    let generateChar _ = Chars.[rand.Next(0, Chars.Length - 1)]
    seq {1 .. length}
    |> Seq.map generateChar
    |> String.Concat
