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
/Times-Roman findfont 14 scalefont setfont
0 -10 moveto\n"

let showString = "stroke\nshowpage\n"

let toPSslow (t : Tree<'a*float> ) : string =

    let createLabel (s : 'a) (pos : position) : (string*position) = 
        let posIn = sprintf "%f %f moveto\n" pos.x (pos.y - labelHeight)
        let printLabel = sprintf "(%A) dup stringwidth pop 2 div neg 0 rmoveto show\n" s
        let moveDown = sprintf "%f %f moveto\n" pos.x (pos.y - labelHeight*1.5)
        let positionTo = {x = pos.x ; y = pos.y - labelHeight*1.5}

        (posIn + printLabel + moveDown, positionTo)

    let createLine (relativePos : float) (pos : position) : (string*position) = 
        let newPos = {x = pos.x + relativePos * 50.0 ; y = pos.y - 50.0}
        let line = sprintf "%f %f lineto\n" newPos.x newPos.y
        let moveLine = sprintf "%f %f moveto\n" pos.x pos.y

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
        let ((label,_), subtree1) = match t with | Node(node, subtree) -> (node, subtree) | _ -> failwith "smth"
        let (labelString, newPos) = createLabel label {x= 0.0 ; y= -10.0}
        
        let mapped = (List.map (rendersubTree newPos) subtree1)
        let subString = List.fold concatenateString "" mapped

        labelString + subString

    let postscriptString = initialString + (renderTree t) + showString
    System.IO.File.WriteAllText("test.ps", postscriptString)

    postscriptString

let toPSfast (t :Tree<'a*float>) : string =
   
    let createLabel (s : 'a) (pos : position) (sb:StringBuilder) : (StringBuilder*position) = 
        let posIn = sprintf "%f %f moveto\n" pos.x (pos.y - labelHeight)
        let printLabel = sprintf "(%A) dup stringwidth pop 2 div neg 0 rmoveto show\n" s
        let moveDown = sprintf "%f %f moveto\n" pos.x (pos.y - labelHeight*1.5)
        let positionTo = {x = pos.x ; y = pos.y - labelHeight*1.5}
        
        // When we do time things try with a binding aswell and see if it makes a difference.
        (sb.Append(posIn).Append(printLabel).Append(moveDown), positionTo)

    let createLine (relativePos : float) (pos : position) (sb:StringBuilder) : (StringBuilder*position) = 
        let newPos = {x = pos.x + relativePos * 50.0 ; y = pos.y - 50.0}
        let line = sprintf "%f %f lineto\n" newPos.x newPos.y
        let moveLine = sprintf "%f %f moveto\n" pos.x pos.y

        // let newStringerBuilder = sb.Append(moveLine).Append(line)
        (sb.Append(moveLine).Append(line), newPos)
   

    let rec rendersubTree (pos : position) (sb:StringBuilder) (t : Tree<'a*float>)  : StringBuilder =
        let ((label,relativePos), subtree1) = match t with | Node(node, subtree) -> (node, subtree) | _ -> failwith "smth"
        let (newSb1, newPos1) = createLine relativePos pos sb
        let (newSb2, newPos2) = createLabel label newPos1 newSb1

        let rec rendersubTree_allchilden (sb:StringBuilder) (remaniningSubTrees: Tree<'a*float> list)  = 
            match remaniningSubTrees with
            | [] -> sb
            | head::tail -> let newSb3 = rendersubTree newPos2 newSb2 head
                            rendersubTree_allchilden newSb3 tail
        rendersubTree_allchilden newSb2 subtree1
    
    let renderTree (t: Tree<'a*float>) (sb:StringBuilder) : StringBuilder = 
        let ((label,_), subtree1) = match t with | Node(node, subtree) -> (node, subtree) | _ -> failwith "smth"
        let (labelString, newPos) = createLabel label {x= 0.0 ; y= -10.0} sb
        
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