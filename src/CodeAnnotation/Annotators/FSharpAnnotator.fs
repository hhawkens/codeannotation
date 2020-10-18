module internal CodeAnnotation.FSharpAnnotator

open System.Text.RegularExpressions

let private keywords =
    [|"abstract";"and";"as";"assert";"base";"begin";"class";"default";"delegate";"do";"done";"downcast";
      "downto";"elif";"else";"end";"exception";"extern";"false";"finally";"fixed";"for";"fun";"function";
      "global";"if";"in";"inherit";"inline";"interface";"internal";"lazy";"let";"let!";"match";"match!";
      "member";"module";"mutable";"namespace";"new";"not";"null";"of";"open";"or";"override";"private";
      "public";"rec";"return";"return!";"select";"static";"struct";"then";"to";"true";"try";"type";"upcast";
      "use";"use!";"val";"void";"when";"while";"with";"yield";"yield!"|]
    |> Set.ofArray

let private patterns = [|
    {Block = Comment; Regex = @"(\(\*[\s\S]*?\*\))"} // Multi line comment
    {Block = Comment; Regex = @"(\/\/.*)"} // Single line comment
    {Block = String; Regex = @"(""""""[\s\S]*?"""""")"} // String (triple quote)
    {Block = String; Regex = @"(@?""[\s\S]*?"")"} // String (possibly multiline, possibly verbatim)
    {Block = String; Regex = @"('\\?[\w\s\\]+')"} // Char
    {Block = Namespace; Regex = @"\bnamespace\s+([\w\.\s]+?)\n"} // Namespace definition
    {Block = Namespace; Regex = @"\bopen\s+([\w\.\s]+?)\n"} // "open" directive
    {Block = Attribute; Regex = @"\[<(.+)>\]"} // Attribute definition
    {Block = Type; Regex = @"\btype\s+(?:private\s+|internal\s+|public\s+)?(\w+)"} // Type definition
    {Block = Type; Regex = @"('\w+)(?!')"} // Generic
    {Block = Type; Regex = @"\bmodule\s+(?:private\s+|internal\s+|public\s+)?([\w\.\s]+?)\n"} // Module
    {Block = Type; Regex = @"(\bint\b|\bbigint\b|\bbyte\b|\bsbyte\b|\bfloat\b|\bdouble\b|\bchar\b|\bstring\b|\bbool\b|\bResult\b|\bOption\b)"} // Type keyword
    {Block = Literal; Regex = @"\b(\d+\.?\w*)\b"} // Literal
    {Block = Function; Regex = @"\.\s*(\w+)\("} // Method
    {Block = Function; Regex = @"\(\|([\s\S]+?)\|\)"} // Active Pattern
    {Block = Function; Regex = @"\B(\|>|<\|)\B"} // Pipe operator
|]

let private annotationFunction =
    GenericAnnotator.createAnnotator
        (patterns |> Seq.map (fun p -> {ValidRegex = Regex(p.Regex); Block = p.Block}))
        keywords

let internal annotate sourceCode = annotationFunction sourceCode
