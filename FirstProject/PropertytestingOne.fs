module PropertytestingOne

open Treerendering

// Each key represents the depth and its corresponding value is the absolute position of the children in that depth.
type TreeMap = Map<int,float list> 

let extractPosition (t:Tree<'a*float>) : float = match t with | Node((_, position), _) -> position
let extractSubtrees (t:Tree<'a*float>) = match t with | Node((_, _), subtrees) -> subtrees

// Check that all positions x satisfies = x_i + 1 <= x_(i+1)
let rec allNodesNoLessThanOne (i:int) (t:float list) : bool =
    match t with
    | [] -> true                            // Different from paper <= because they are saved in reversed order. 
    | head::tail when not tail.IsEmpty -> (((head) + 1.0) >= (tail.Head)) && allNodesNoLessThanOne i tail
    | _ -> true

// Checks for dephts in the map.
let AllNodesDistancedAtleastOneApart (t:TreeMap) = Map.forall allNodesNoLessThanOne t

// Organizing every node in the treemap.
let ComputeTreeMap (root:Tree<'a*float>) : TreeMap = 
    let rec ComputeTreeMap_rec (node:Tree<'a*float>) (acc:float) (t:TreeMap) (d:int) : TreeMap =
        
        let relativePos = extractPosition node
        let absolutePos = acc+relativePos

        // Finds nodes currently saved in TreeMap at the same depth i
        let currentList = match (t.TryFind d) with | Some v -> absolutePos::v | None -> [absolutePos]
        // Adds this node to the TreeMap
        let updatedTreeMap = Map.add d currentList t
        let subtrees = extractSubtrees node
        
        // Recursively add all subtrees to TreeMap
        let rec ComputeSubTrees (subt: Tree<'a*float> list) (acc:TreeMap) : TreeMap =
            match subt with
            | [] -> acc
            | head::tail -> let treeMap = ComputeTreeMap_rec head absolutePos acc (d+1)
                            ComputeSubTrees tail treeMap
        ComputeSubTrees subtrees updatedTreeMap

    ComputeTreeMap_rec root 0.0 Map.empty 0

let ConstructTreeMap (t:Tree<'a>) : TreeMap = 
    let (root, _) = design_tree t
    ComputeTreeMap root

let rec fitInvariant (t:Tree<'a>) : bool = 
    let treeapender = ConstructTreeMap t
    AllNodesDistancedAtleastOneApart treeapender
       
let rec fitInvariantFromDesigned (t:Tree<'a*float>) : bool =
    AllNodesDistancedAtleastOneApart (ComputeTreeMap t)

let testFitInvariant =
    FsCheck.Check.Quick fitInvariant