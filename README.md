## AStar
A* Pathfinding Algorithm Test Demo

### Core Data Structure：
openList:Store the node to be compared

closeList:Store nodes that are already in the path

### Core idea of algorithm:
1.Add start node to openList

2.Repeat the following operation While openList.Count > 0 :
+ Make the first node in openList as current node
+ Remove current node from openList and add it to closeList
+ Check if current node equal to end node
   + if Yes, Generate Path
+ Traverses the nodes around the current node
   + Calculate the cost of the current point to the beginning(g) and end(h)
   + If node is not in openList, add it to openList;Else Update its cost value
+ Sort openList according to value "f" of nodes

3.if find no path,output prompt
