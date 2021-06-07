// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System

// Define a function to construct a message to print

(* type Tree<'a> = | Node of 'a * ('a Tree list);;
type IntTree = Tree<int>;;
type StringTree = Tree<string>;;
type StringIntTree = Tree<string*float>;;
type Program = int;;
type Extent = (float*float) list *)

module treerendering =

    type Tree<'a> = Node of 'a * ('a Tree list)
    type IntTree = Tree<int>
    type StringTree = Tree<string>
    type StringIntTree = Tree<string * float>
    type Program = int
    type Extent = (float * float) list

    //(Node(label,subtrees) : 'a Tree) (x : float) : 'a Tree
    let movetree ((Node ((label, x), subtrees)), (x': float)) = Node((label, x + x'), subtrees)

    let moveextent ((e: Extent), (x: float)) = List.map (fun (p, q) -> (p + x, q + x)) e

    let rec merge (ps: Extent) (qs: Extent) : Extent =
        match (ps, qs) with
        | (ps, []) -> ps
        | ([], qs) -> qs
        | ((p, _) :: ps, (_, q) :: qs) -> (p, q) :: merge ps qs

    let mergelist es = List.fold merge [] es

    let rmax (p:float) (q:float) = if p > q then p else q

    let rec fit (ps : Extent) (qs : Extent) : float =
        match (ps,qs) with
        | (((_,p)::ps),((q,_)::qs)) -> rmax (fit ps qs) (p-q+1.0)
        | (_,_)                     -> 0.0


    //Optimization idea -> make the function tail recursive
    let fitlistl es =
        let rec fitlistl_rec acc es =
            match (acc, es) with
            | (acc, []) -> []
            | (acc, (e :: es)) ->
                let x = fit acc e
                let newAcc = (merge acc (moveextent (e, x)))
                x :: (fitlistl_rec newAcc es)

        fitlistl_rec [] es

    //Optimization idea -> make the function tail recursive
    let fitlistr_old es =
        let rec fitlistr_rec acc es =
            match (acc, es) with
            | (acc, []) -> []
            | (acc, (e :: es)) ->
                let x = -(fit e acc)
                let newAcc = (merge (moveextent (e, x)) acc)
                x :: (fitlistr_rec newAcc es)

        fitlistr_rec [] (List.rev es)

    let flipextent (e:Extent) : Extent = List.map (fun (p,q) -> (-p,-q)) e
    let fitlistr es =   let flippedExtents = (List.map flipextent (List.rev es))
                        let fittedExtends = fitlistl flippedExtents
                        let negatedExtends = List.map (fun a -> -a) fittedExtends
                        let revertedExtends = List.rev negatedExtends
                        revertedExtends

    let mean ((x, y): float * float) = (x + y) / 2.0

    let fitlist es =    let fitlistL = fitlistl es
                        let fitlistR = fitlistr es
                        let zippedLists = List.zip fitlistR fitlistL
                        let meanList = List.map mean zippedLists
                        meanList

    let design_tree tree =
        let rec design' (Node (label, subtrees): 'a Tree) =
            let (trees, extents) = List.unzip (List.map design' subtrees)
            // Represents the displacements of trees.
            let positions = fitlist extents
            let ptrees = List.map movetree (List.zip trees positions)
            let pextents = List.map moveextent (List.zip extents positions)
            let resultextent = (0.0, 0.0) :: mergelist pextents
            let resulttree = Node((label, 0.0), ptrees)
            (resulttree, resultextent)

        design' tree


    // 2 Property-based testing

    (*     // 3
    let toPSslow (t:StringIntTree) : string =
        "Not implemented";;

    // 4
    let toPSfast (t:StringIntTree) : string =
        "Not implemented";;

    // 5
    let treeTofile (s:string) (t:StringTree) =
        failwith "not implemented";;

    // 6
    let posTreeTofile (s:string) (t:StringTree) =
        failwith "not implemented";;

    // 10
    let toGeneralTree (p:Program) : StringTree =
        Node("Not implemented",[]) *)

    

    [<EntryPoint>]
    let main argv =
        printfn "Running stuff"
        let sleaf s = Node(s, [])
        let leaf = sleaf "leaf"
        let lowBranch = Node("lowBranch", [leaf;leaf;leaf])
        let branch = Node("branch", [leaf;lowBranch;leaf])
        let upperbranch = Node("upper", [branch;leaf;lowBranch])
        let complexTree = Node("complex root", [branch; upperbranch])

        let simpleTree = Node("root", 
                                [   sleaf("first leaf");
                                    Node("fst branch", [
                                        sleaf("fst 1. leaf");
                                        sleaf("fst 2. leaf")]);
                                    Node("snd branch", [
                                        sleaf("snd 1. leaf");
                                        sleaf("snd 2. leaf")]);
                                    sleaf("second leaf")])

        let designedTree = design_tree simpleTree
        let tree = fst designedTree
        let extents = snd designedTree
        
        printfn "designtree with input:\n %A \n\n results in:\n %A \n\n And the extents were: \n%A" simpleTree tree extents
        0 // return an integer exit code
