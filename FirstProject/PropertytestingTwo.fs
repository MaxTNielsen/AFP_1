module PropertytestingTwo
open Treerendering

let symmetricInvariant (t:Tree<'a>) : bool =
    let subt = match t with | Node(label, subtrees) -> subtrees
    let (_, extents) = List.unzip (List.map design' subt)

    if List.isEmpty extents then true else
        let fitlistL = fitlistl extents
        let fitlistLMin = List.min fitlistL
        let fitlistLSatisfied = fitlistLMin >= 0.0

        let fitlistR = fitlistr extents
        let fitlistRMax = List.max fitlistR
        let fitlistRSatisfied = fitlistRMax <= 0.0

        fitlistLSatisfied && fitlistRSatisfied

let testSymmetricInvariant =
    FsCheck.Check.Quick symmetricInvariant