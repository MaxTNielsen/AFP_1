module Treerendering

type Tree<'a> = Node of 'a * ('a Tree list)
type Extent = (float * float) list

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

////// Optimization ///////////////////////////
////// Made the function tail recursive ///////
let fitlistl es =
    let rec fitlistl_rec acc es accu =
        match (acc, es) with
        | (_, []) -> List.rev accu
        | (acc, (e :: es)) ->
            let x = fit acc e
            let newAcc = (merge acc (moveextent (e, x)))
            let newAccu = x :: accu
            (fitlistl_rec newAcc es newAccu)

    fitlistl_rec [] es []

let flipextent (e:Extent) : Extent = List.map (fun (p,q) -> (-q,-p)) e
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

let rec design' (Node (label, subtrees): 'a Tree) =
    let (trees, extents) = List.unzip (List.map design' subtrees)
    // Represents the displacements of trees.
    let positions = fitlist extents
    let ptrees = List.map movetree (List.zip trees positions)
    let pextents = List.map moveextent (List.zip extents positions)
    let resultextent = (0.0, 0.0) :: mergelist pextents
    let resulttree = Node((label, 0.0), ptrees)
    (resulttree, resultextent)

let design_tree tree = 
    design' tree