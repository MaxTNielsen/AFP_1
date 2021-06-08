module PostScriptRendering

open AST
open Treerendering
open System.Text

type position = 
    {
        x : float
        y : float
    }

let labelHeight = 15.0

let initialString = "%!
1 1 scale
<</PageSize[595 842]/ImagingBBox null>> setpagedevice
1500 1200 translate
newpath
/Times-Roman findfont 18 scalefont setfont
0 -10 moveto\n"

let showString = "stroke\nshowpage\n"

let moveTo = sprintf "%f %f moveto\n"
let label = sprintf "(%s) dup stringwidth pop 2 div neg 0 rmoveto show\n"
let lineTo = sprintf "%f %f lineto\n"

let createLabel (s : 'a) (pos : position) : string*string*string*position = 
    let posIn = moveTo pos.x (pos.y - labelHeight)
    let printLabel = label s
    let moveDown = moveTo pos.x (pos.y - labelHeight*1.5)
    let positionTo = {x = pos.x ; y = pos.y - labelHeight*1.5}
    (posIn, printLabel, moveDown, positionTo)

let createLine (relativePos : float) (pos : position) : position*string*string =
    let newPos = {x = pos.x + relativePos * 50.0 ; y = pos.y - 50.0}
    let line = lineTo newPos.x newPos.y
    let moveLine = moveTo pos.x pos.y
    (newPos, line, moveLine)

let toPSslow (t : Tree<'a*float> ) : string =

    let createLabel_slow (s : 'a) (pos : position) : (string*position) = 
        let (posIn, printLabel, moveDown, positionTo) = createLabel s pos
        (posIn + printLabel + moveDown, positionTo)

    let createLine_slow (relativePos : float) (pos : position) : (string*position) = 
        let (newPos,line,moveLine) = createLine relativePos pos
        (moveLine + line, newPos)
    
    let concatenateString (a : string) (b : string) : string = a + b


    let rec rendersubTree (pos : position) (t : Tree<'a*float>) : string =
        let ((label,relativePos), subtree1) = match t with | Node(node, subtree) -> (node, subtree)
        let (labelString1, newPos1) = createLine_slow relativePos pos
        let (labelString2, newPos2) = createLabel_slow label newPos1
        let mapped = (List.map (rendersubTree newPos2) subtree1)
        let subString = List.fold concatenateString "" mapped

        labelString1 + labelString2 + subString
    
    let renderTree (t: Tree<'a*float>) : string = 
        let ((label,_), subtree1) = match t with | Node(node, subtree) -> (node, subtree)
        let (labelString, newPos) = createLabel_slow label {x= 0.0 ; y= -10.0}
        
        let mapped = (List.map (rendersubTree newPos) subtree1)
        let subString = List.fold concatenateString "" mapped

        labelString + subString

    let postscriptString = initialString + (renderTree t) + showString
    System.IO.File.WriteAllText("test.ps", postscriptString)

    postscriptString

let toPSfast (t :Tree<'a*float>) : string =
   
    let createLabel_fast (s : 'a) (pos : position) (sb:StringBuilder) : (StringBuilder*position) = 
        let (posIn, printLabel, moveDown, positionTo) = createLabel s pos
        (sb.Append(posIn).Append(printLabel).Append(moveDown), positionTo)

    let createLine_fast (relativePos : float) (pos : position) (sb:StringBuilder) : (StringBuilder*position) = 
        let (newPos, line,moveLine) = createLine relativePos pos
        // let newStringerBuilder = sb.Append(moveLine).Append(line)
        (sb.Append(moveLine).Append(line), newPos)
   

    let rec rendersubTree (pos : position) (sb:StringBuilder) (t : Tree<'a*float>)  : StringBuilder =
        let ((label,relativePos), subtree1) = match t with | Node(node, subtree) -> (node, subtree)
        let (newSb1, newPos1) = createLine_fast relativePos pos sb
        let (newSb2, newPos2) = createLabel_fast label newPos1 newSb1

        let rec rendersubTree_allchilden (sb:StringBuilder) (remaniningSubTrees: Tree<'a*float> list)  = 
            match remaniningSubTrees with
            | [] -> sb
            | head::tail -> let newSb3 = rendersubTree newPos2 newSb2 head
                            rendersubTree_allchilden newSb3 tail
        rendersubTree_allchilden newSb2 subtree1
    
    let renderTree (t: Tree<'a*float>) (sb:StringBuilder) : StringBuilder = 
        let ((label,_), subtree1) = match t with | Node(node, subtree) -> (node, subtree)
        let (labelString, newPos) = createLabel_fast label {x= 0.0 ; y= -10.0} sb
        
        let rec rendersubTree_allchilden (sb:StringBuilder) (remaniningSubTrees: Tree<'a*float> list)  = 
            match remaniningSubTrees with
            | [] -> sb
            | head::tail -> let newSb3 = rendersubTree newPos labelString head
                            rendersubTree_allchilden newSb3 tail
        rendersubTree_allchilden labelString subtree1

    let stringBuilder = new StringBuilder(initialString)
    let postscriptString = (renderTree t stringBuilder).Append(showString).ToString()
    postscriptString


    
let posTreeToFile (filename:string) (t:Tree<'a*float>) =
    System.IO.File.WriteAllText(filename, (toPSfast t))

let treeToFile (filename:string) (t:Tree<'a>) =
    posTreeToFile filename (fst (design_tree t))