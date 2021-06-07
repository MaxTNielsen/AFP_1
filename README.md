# AFP_1

- [ ] Design of aesthetic pleasant renderings of trees  
  - [x] Make F# version of program presented in research paper. 
  - [ ] List and discuss key observations concerning the translation of SML programs to F#programs
- [ ] Property based testing
  - [x] Property 1 (Nodes should be atleast 1 unit apart)
  - [x] Property 2 (Parrent should be centered over its offspring)
  - [x] Property 3 (Symmetric with respect to reflection)
  - [ ] Property 4 (Identical subtrees are rendered identically)
  - [ ] Discuss briefly you interpretation of each of the four rules and your the results of your tests.
- [ ] Translation to PostScript
  - [ ] toPSslow - Slow concatination version of postscript generation
  - [ ] toPSfast - Faster stringbuilder version of postscript generation
  - [ ] treeToFile - Save tree to file
  - [ ] posTreeToFile - Save designed position tree to file
  - [ ] Explain your considerations concerning avoidance of repeated code, and present the ideas behind your solution.
  - [ ] Analyse the run time of toPSslow and toPSfast
  - [ ] Conduct timing experiments with positioned trees of varying sized and relate the results to you analyses
- [ ] Rendering Abstract Syntax Tree
  - [ ] toGeneralTree
  - [ ] Make a brief discussion of the principles you used in your transformation from programsto trees of typeTree<string>, and your considerations concerning that transformation
- [ ] Extensions
  - [ ] Translation to Scalable Vector Graphics
- [ ] Evaluation
