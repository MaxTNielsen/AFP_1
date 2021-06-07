module PropertytestingOne

open Treerendering

type TreeAppender = Map<int,float list> 

let extractPosition (t:Tree<'a*float>) : float = match t with | Node((_, position), _) -> position | _ -> failwith "wtf"
let extractSubtrees (t:Tree<'a*float>) = match t with | Node((_, _), subtrees) -> subtrees | _ -> failwith "wtf"

let rec allNodesNoLessThanOne (i:int) (t:float list) : bool =
    match t with
    | [] -> true                                  // Different from paper <= because they are saved in reversed order. 
    | head::tail when not tail.IsEmpty -> (((head) + 1.0) >= (tail.Head)) && allNodesNoLessThanOne i tail
    | [_] -> true

let AllNodesDistancedAtleastOneApart (t:TreeAppender) = Map.forall allNodesNoLessThanOne t

let OrganizeIntoTreeAppender (root:Tree<'a*float>) : TreeAppender = 
    let rec OrganizeIntoTreeAppender_rec (node:Tree<'a*float>) (acc:float) (t:TreeAppender) (d:int) : TreeAppender =
        let relativePos = extractPosition node
        let absolutePos = acc+relativePos
        let currentList = match (t.TryFind d) with | Some v -> absolutePos::v | None -> [absolutePos]
        let newTreeAppender = Map.add d currentList t
        let subtreees = extractSubtrees node
        
        let rec OrganizeAllSubtrees (subt: Tree<'a*float> list) (acc:TreeAppender) : TreeAppender =
            match subt with
            | [] -> acc
            | head::tail -> let appender = OrganizeIntoTreeAppender_rec head absolutePos acc (d+1)
                            OrganizeAllSubtrees tail appender
        OrganizeAllSubtrees subtreees newTreeAppender

    OrganizeIntoTreeAppender_rec root 0.0 Map.empty 0

let CalculateTreeAppender (t:Tree<'a>) : TreeAppender = 
    let (root, _) = design_tree t
    OrganizeIntoTreeAppender root

let rec fitInvariant (t:Tree<'a>) : bool = 
    let treeapender = CalculateTreeAppender t
    AllNodesDistancedAtleastOneApart treeapender
        