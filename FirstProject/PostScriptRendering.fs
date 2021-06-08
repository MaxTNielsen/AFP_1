module PostScriptRendering

open AST
open Treerendering

type position = 
    {
        x : float
        y : float
    }

let labelHeight = 15.0

let toPSslow (t : Tree<'a> ) : string =
    let initialString = "%!
1 1 scale
<</PageSize[595 842]/ImagingBBox null>> setpagedevice
297 421 translate
newpath
/Times-Roman findfont 14 scalefont setfont
0 -10 moveto\n"
   
    let createLabel (s : string) (pos : position) : (string*position) = 
        let posIn = 
            sprintf "%f %f moveto\n" pos.x (pos.y - labelHeight)
        
        let printLabel =
            sprintf "(%s) dup stringwidth pop 2 div neg 0 rmoveto show\n" s
        
        let moveDown =   
            sprintf "%f %f moveto\n" pos.x (pos.y - labelHeight*1.5)
        
        let positionTo = {x = pos.x ; y = pos.y - labelHeight*1.5}
        
        (posIn + printLabel + moveDown, positionTo)

    let createLine (relativePos : float) (pos : position) : (string*position) = 
        let newPos = {x = pos.x + relativePos * 50.0 ; y = pos.y - 50.0}

        let line = 
            sprintf "%f %f lineto\n" newPos.x newPos.y
        
        let moveLine = 
            sprintf "%f %f moveto\n" pos.x pos.y

        (moveLine + line, newPos)
    
    let concatenateString (a : string) (b : string) : string = a + b


    let rec rendersubTree (pos : position) (t : Tree<'a*float>) : string =
        let ((label,relativePos), subtree1) = match t with | Node(node, subtree) -> (node, subtree) | _ -> failwith "smth"
        let (labelString1, newPos1) = createLine relativePos pos
        let (labelString2, newPos2) = createLabel label newPos1
        let mapped = (List.map (rendersubTree newPos2) subtree1)
        let subString = List.fold concatenateString "" mapped

        labelString1 + labelString2 + subString
    
    let renderTree (t: Tree<'a*float>) : string = 
        let ((label,relativePos), subtree1) = match t with | Node(node, subtree) -> (node, subtree) | _ -> failwith "smth"
        let (labelString, newPos) = createLabel label {x= 0.0 ; y= -10.0}
        
        let mapped = (List.map (rendersubTree newPos) subtree1)
        let subString = List.fold concatenateString "" mapped

        labelString + subString

    let showString = "showpage\n"
    let designedTree = fst (design_tree t)

    let postscriptString = initialString + (renderTree designedTree) + "stroke\n"  + showString

    System.IO.File.WriteAllText("test.ps", postscriptString)

    postscriptString

let toPSfast (t:StringIntTree) : string =
    "Not implemented";;



//let postScript

