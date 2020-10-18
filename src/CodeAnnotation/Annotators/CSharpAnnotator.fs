module internal CodeAnnotation.CSharpAnnotator

open System.Text.RegularExpressions

let private keywords =
    [|"abstract";"add";"as";"alias";"ascending";"async";"await";"base";"bool";"break";"by";"byte";
      "case";"catch";"char";"checked";"class";"const";"continue";"decimal";"default";"descending";"delegate";"do";
      "double";"dynamic";"else";"enum";"event";"equals";"explicit";"extern";"false";"finally";"fixed";"float";
      "for";"foreach";"from";"get";"global";"goto";"group";"if";"implicit";"in";"int";"interface";
      "internal";"into";"is";"join";"let";"lock";"long";"nameof";"namespace";"new";"null";"object";"on";
      "orderby";"operator";"out";"override";"params";"partial";"params";"private";"protected";"public";"readonly";"ref";
      "remove";"return";"sbyte";"sealed";"select";"set";"short";"sizeof";"stackalloc";"static";"string";"struct";
      "switch";"this";"throw";"true";"try";"typeof";"uint";"unmanaged";"ulong";"unchecked";"unsafe";"ushort";"using";
      "value";"var";"virtual";"void";"volatile";"when";"where";"while";"yield";|]
    |> Set.ofArray

let private patterns = [|
    {Block = Comment; Regex = @"(\/\*[\s\S]*?\*\/)"} // Multi line comment
    {Block = Comment; Regex = @"(\/\/.*)"} // Single line comment
    {Block = String; Regex = @"([@$]?[@$]?""[\s\S]*?"")"} // String (possibly interpolated, possibly verbatim)
    {Block = String; Regex = @"('\\?[\w\s\\]+')"} // Char
    {Block = Interface; Regex = @"(?:public|private|internal)?[^\S\r\n]+interface[\s]+([\w]+)"} // Interface definition
    {Block = Attribute; Regex = @"\[([a-zA-Z]+[\w]*)"} // Attribute definition
    {Block = Namespace; Regex = @"\bnamespace\s+([\s\S]*?)\s*{"} // Namespace
    {Block = Namespace; Regex = @"\busing\s+(?:static\s+)?([\s\S]*?)\s*;"} // Using
    {Block = Event; Regex = @"delegate\s+\w[\w\d@<>\s]+\s+([\w\d@]+)"} // Delegate definition
    {Block = Type; Regex = @"(?:public|private|internal)?[^\S\r\n]+(?:class|struct|enum)[\s]+([\w]+)"} // Type definition
    {Block = Type; Regex = @"new\s+([\w.]+)[\s<>\w]*\("} // New object
    {Block = Type; Regex = @"<\s*([a-zA-Z]+[\w\s,]*)\s*>"} // Generic
    {Block = Type; Regex = @"(?:class|interface)[\s\w\d<>]+?:([\s\w\d,<>]+)"} // Inherited type
    {Block = TypeCase; Regex = @"enum\s+?[\w\d@]+?\s*{([\s\w\d,@]+?)}"} // Enum case
    {Block = Literal; Regex = @"\b(\d+[\w.]*)"} // Literal
    {Block = Function; Regex = @"(\w+[\w\d]*)(?:<[\s\w\d,]+?>)?\s*\("} // Method
    {Block = Property; Regex = @"(\w[\w\d@]*)\s*=>"} // Property with expression body
    {Block = Property; Regex = @"(\w[\w\d@]*)\s*{\s*(?:get|set)"} // Property with get/set
    {Block = Constant; Regex = @"const\s+\w+\s+(\w+)"} // Constant
|]

let private annotateCSharp =
    GenericAnnotator.createAnnotator
        (patterns |> Seq.map (fun p -> {ValidRegex = Regex(p.Regex); Block = p.Block}))
        keywords

let internal annotate sourceCode = annotateCSharp sourceCode
