
// source program and corresponding abstract syntax tree MRH 08-06-2021
// Ex4.gc
(*
begin
  x : int;
  x := 1; 
  do  x=1 -> print x; x:= x+1
   |  x=2 -> print x; x:= x+1 
   |  x=3 -> print x; x:= x+1 
  od;
  print x
end
*)


(*
val it : Program =
  P ([VarDec (ITyp,"x")],
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
*)