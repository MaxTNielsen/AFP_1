module GCLRenderer


open Treerendering
open AST
open PostScriptRendering

let rec generateExp (e:Exp) : Tree<string> =
    match e with
    | N(i) -> Node("N", [Node(string(i),[])])
    | B(b) -> Node("B", [Node(string(b),[])])
    | Access(a) -> Node("Access", [generateAccess a])
    | Addr(a) -> Node("Addr", [generateAccess a])
    | Apply(s,el) -> Node("Apply", [Node(s, [])]@List.map generateExp el)

and generateAccess (a:Access) : Tree<string> =
    match a with
    | AVar(s) -> Node("AVar", [Node(s, [])])
    | AIndex(a,e) -> Node("AIndex", [generateAccess a;generateExp e])
    | ADeref(e) -> Node("ADeref", [generateExp e])


and generateExpStmList (e:Exp*Stm list) =
    let (e,stl) = e
    [generateExp e]@List.map generateStm stl

and generateGuardedCommand (g:GuardedCommand) : Tree<string> =
    match g with
    | GC(expStmList) -> Node("GC", List.concat (List.map generateExpStmList expStmList))

and generateTyp (t:Typ) : Tree<string> =
    match t with
    | ITyp -> Node("ITyp", [])
    | BTyp -> Node("BTyp", [])                 
    | ATyp(t,iop) when iop.IsSome -> Node("ATyp", [generateTyp t; Node(string(iop.Value), [])]) 
    | ATyp(t,_) -> Node("ATyp", [generateTyp t])
    | PTyp(t) -> Node("PTyp", [generateTyp t])    
    | FTyp(tl, top) when top.IsSome -> Node("FTyp", (List.map generateTyp tl)@[(generateTyp top.Value)])
    | FTyp(tl, _) ->  Node("FTyp", (List.map generateTyp tl))


and generateDec (d:Dec) : Tree<string> =
    match d with
    | VarDec(t, s) -> Node("VarDec", [generateTyp t; Node(s, [])])      
    | FunDec(top,s,dl,stm) -> let subtrees = if top.IsSome then [generateTyp top.Value] else []
                              let declarations = List.map generateDec dl
                              Node("FunDec", subtrees@[Node(s, [])]@declarations@[generateStm stm])

and generateStm (s:Stm) : Tree<string> =
    match s with
    | PrintLn(e) -> Node("PrintLn", [generateExp e])
    | Ass(a,e) -> Node("Ass", [generateAccess a; generateExp e])
    | Return(eop) -> Node("Return", if eop.IsSome then [generateExp eop.Value] else [])
    | Alt(gcm) -> Node("Alt", [generateGuardedCommand gcm])
    | Do(gcm) -> Node("Do", [generateGuardedCommand gcm])
    | Block(dl,sl) -> Node("Block", (List.map generateDec dl)@(List.map generateStm sl))
    | Call(s, el) -> Node("Call", Node(s, [])::List.map generateExp el)

let toGeneralTree (p:Program) : Tree<string> =
    let (a,b) = match p with | P(a,b) -> (a,b)
    let declarationsList = List.map generateDec a
    let statementsList = List.map generateStm b
    let declarationsAndStatements = declarationsList@statementsList
    Node("P", declarationsAndStatements)

let TestGCLRendering =
    let sampleProgram = P ([VarDec (ITyp,"x")],
                    [Ass (AVar "x",N 1);
                     Do
                       (GC
                          [(Apply ("=",[Access (AVar "x"); N 1]),
                            [PrintLn (Access (AVar "x"));
                             Ass (AVar "x",Apply ("+",[Access (AVar "x"); N 1]))]);
                           (Apply ("=",[Access (AVar "x"); N 2]),
                            [PrintLn (Access (AVar "x"));
                             Ass (AVar "x",Apply ("+",[Access (AVar "x"); N 1]))]);
                           (Apply ("=",[Access (AVar "x"); N 3]),
                            [PrintLn (Access (AVar "x"));
                             Ass (AVar "x",Apply ("+",[Access (AVar "x"); N 1]))])]);
                     PrintLn (Access (AVar "x"))])
    let tree = toGeneralTree sampleProgram 
    treeToFile "GCprogram.ps" tree

