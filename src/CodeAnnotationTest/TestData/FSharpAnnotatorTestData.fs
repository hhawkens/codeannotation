module public CodeAnnotation.Tests.FSharpAnnotatorTestData

[<Literal>]
let public fsharpCode = """
module internal TestSpace.TestModule

open System
open System .  Buffers

///
/// This is a so called "xml" comment then
///
[<Literal>]
let PI: float32 = 3.1f

[<Struct>]
type internal PositiveNumber = One | Two | Three of int | More of value: uint32

type public Clock = private Minutes of int

type private Container<'atype, 'b> = {
    Value: 'atype
    Checksum: 'b * 'atype list
}

type Allergen =
    | Eggs = 0b1
    | Peanuts = 0o2
    | Shellfish = 0x4

let let_be_1 = 1

let getMins (Minutes minutes) = minutes

// let this be 'a' simple comment
let private consonants =
    ["thr";"sch";"th";"qu";"ch";"b";"c";"d";"f";"g";"h";"j";
     "k";"l";"m";"n";"p";"q";"r";"s";"t";"v";"w";"x";"y";"//z";];

(**
 * and let this be a "multi line" comment
 *)
let private vowels = [|0,'x';1,'y';2,'a';3,'e';4,'i';5,'\u0061';6,'\n'|];

let result : Result<int,string * int> = Ok 1

let private boolToOption v b = if b then Some v else None // let this be an inline comment

let private containsAny<'t> (substrings: (string * string) list) (input: Option<string * int * 't> * float) =
    substrings |> List.exists (fun _ -> true)

let private (|ActiveOne| ActiveTwo| ActiveThree |) (txt: string) = (* let this be an "inline"
    multiline: comment (placed nonsensically)*)
    if txt.Trim() = "" then ActiveOne "wow" else ActiveTwo ""

let private (|ActiveSingle|_|) txt =
#if DEBUG
    if txt = "" then None else Some txt
#endif

let private giveStuff x =
    match x with
    | 0 -> 1
    | 1 -> 1
    | x -> x * 2uy

let translate (input: string) =
    input.Split("This is not a module X.X or namespace Y.Y")
    |> Array.map (fun s -> s)
    |> String.concat " "

let mlst = @"
module multiline string test
"

let accumulate (func: ('a) -> 'b) (input: 'a list): 'b list =
    let rec accumulate func state = function
        | [] -> state
        | h::t -> accumulate func ((func h)::state) t
    List.rev <| accumulate func [] input

"""

// ================================================================================================

[<Literal>]
let public annotatedFsharpCode = """
{~Keyword:module~} {~Keyword:internal~} {~Type:TestSpace.TestModule~}

{~Keyword:open~} {~Namespace:System~}
{~Keyword:open~} {~Namespace:System .  Buffers~}

{~Comment:///~}
{~Comment:/// This is a so called "xml" comment then~}
{~Comment:///~}
[<{~Attribute:Literal~}>]
{~Keyword:let~} PI: float32 = {~Literal:3.1f~}

[<{~Attribute:Struct~}>]
{~Keyword:type~} {~Keyword:internal~} {~Type:PositiveNumber~} = One | Two | Three {~Keyword:of~} {~Type:int~} | More {~Keyword:of~} value: uint32

{~Keyword:type~} {~Keyword:public~} {~Type:Clock~} = {~Keyword:private~} Minutes {~Keyword:of~} {~Type:int~}

{~Keyword:type~} {~Keyword:private~} {~Type:Container~}<{~Type:'atype~}, {~Type:'b~}> = {
    Value: {~Type:'atype~}
    Checksum: {~Type:'b~} * {~Type:'atype~} list
}

{~Keyword:type~} {~Type:Allergen~} =
    | Eggs = {~Literal:0b1~}
    | Peanuts = {~Literal:0o2~}
    | Shellfish = {~Literal:0x4~}

{~Keyword:let~} let_be_1 = {~Literal:1~}

{~Keyword:let~} getMins (Minutes minutes) = minutes

{~Comment:// let this be 'a' simple comment~}
{~Keyword:let~} {~Keyword:private~} consonants =
    [{~String:"thr"~};{~String:"sch"~};{~String:"th"~};{~String:"qu"~};{~String:"ch"~};{~String:"b"~};{~String:"c"~};{~String:"d"~};{~String:"f"~};{~String:"g"~};{~String:"h"~};{~String:"j"~};
     {~String:"k"~};{~String:"l"~};{~String:"m"~};{~String:"n"~};{~String:"p"~};{~String:"q"~};{~String:"r"~};{~String:"s"~};{~String:"t"~};{~String:"v"~};{~String:"w"~};{~String:"x"~};{~String:"y"~};{~String:"//z"~};];

{~Comment:(**
 * and let this be a "multi line" comment
 *)~}
{~Keyword:let~} {~Keyword:private~} vowels = [|{~Literal:0~},{~String:'x'~};{~Literal:1~},{~String:'y'~};{~Literal:2~},{~String:'a'~};{~Literal:3~},{~String:'e'~};{~Literal:4~},{~String:'i'~};{~Literal:5~},{~String:'\u0061'~};{~Literal:6~},{~String:'\n'~}|];

{~Keyword:let~} result : {~Type:Result~}<{~Type:int~},{~Type:string~} * {~Type:int~}> = Ok {~Literal:1~}

{~Keyword:let~} {~Keyword:private~} boolToOption v b = {~Keyword:if~} b {~Keyword:then~} Some v {~Keyword:else~} None {~Comment:// let this be an inline comment~}

{~Keyword:let~} {~Keyword:private~} containsAny<{~Type:'t~}> (substrings: ({~Type:string~} * {~Type:string~}) list) (input: {~Type:Option~}<{~Type:string~} * {~Type:int~} * {~Type:'t~}> * {~Type:float~}) =
    substrings {~Function:|>~} List.exists ({~Keyword:fun~} _ -> {~Keyword:true~})

{~Keyword:let~} {~Keyword:private~} (|{~Function:ActiveOne| ActiveTwo| ActiveThree~} |) (txt: {~Type:string~}) = {~Comment:(* let this be an "inline"
    multiline: comment (placed nonsensically)*)~}
    {~Keyword:if~} txt.{~Function:Trim~}() = {~String:""~} {~Keyword:then~} ActiveOne {~String:"wow"~} {~Keyword:else~} ActiveTwo {~String:""~}

{~Keyword:let~} {~Keyword:private~} (|{~Function:ActiveSingle|_~}|) txt =
#if DEBUG
    {~Keyword:if~} txt = {~String:""~} {~Keyword:then~} None {~Keyword:else~} Some txt
#endif

{~Keyword:let~} {~Keyword:private~} giveStuff x =
    {~Keyword:match~} x {~Keyword:with~}
    | {~Literal:0~} -> {~Literal:1~}
    | {~Literal:1~} -> {~Literal:1~}
    | x -> x * {~Literal:2uy~}

{~Keyword:let~} translate (input: {~Type:string~}) =
    input.{~Function:Split~}({~String:"This is not a module X.X or namespace Y.Y"~})
    {~Function:|>~} Array.map ({~Keyword:fun~} s -> s)
    {~Function:|>~} String.concat {~String:" "~}

{~Keyword:let~} mlst = {~String:@"
module multiline string test
"~}

{~Keyword:let~} accumulate (func: ({~Type:'a~}) -> {~Type:'b~}) (input: {~Type:'a~} list): {~Type:'b~} list =
    {~Keyword:let~} {~Keyword:rec~} accumulate func state = {~Keyword:function~}
        | [] -> state
        | h::t -> accumulate func ((func h)::state) t
    List.rev {~Function:<|~} accumulate func [] input

"""
